using CostingApp.Module.BO.ItemTransactions.Abstraction;
using CostingApp.Module.BO.Masters;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.BO.ItemTransactions {
    [NavigationItem("Transactions"),
        XafDefaultProperty(nameof(Number)),
        ImageName("BO_Order_Item")]
    [AddItemClass(EnumInventoryTransactionType.InventoryTransfer)]
    public class InventoryTransfer : InventoryTransaction {
        public override EnumInventoryTransactionType TransactionType { get { return EnumInventoryTransactionType.InventoryTransfer; } }
        const string NumberFormat = "Concat('IT-', PadLeft(ToStr(SequentialNumber), 6, '0'))";
        [PersistentAlias(NumberFormat)]
        public string Number {
            get { return Convert.ToString(EvaluateAlias(nameof(Number))); }
        }
        Shop fDestination;
        [DataSourceCriteria("IsActive = True"),
            RuleRequiredField("InventoryTransfer_Destination_RuleRequiredField", DefaultContexts.Save),
            ImmediatePostData(true),
            ]
        public Shop Destination {
            get { return fDestination; }
            set { SetPropertyValue<Shop>(nameof(Destination), ref fDestination, value); }
        }
        [Association("InventoryTransfer-InventoryTransferSourceItem"), DevExpress.Xpo.Aggregated,
            RuleRequiredField("InventoryTransfer_Items_RuleRequiredField", DefaultContexts.Save)]
        public XPCollection<InventoryTransferSourceItem> Items {
            get { return GetCollection<InventoryTransferSourceItem>(nameof(Items)); }
        }    
        [Association("InventoryTransfer-InventoryTransferDestinationItem"), DevExpress.Xpo.Aggregated,
            VisibleInDetailView(false),
            ModelDefault("AllowEdit", "False")]
        public XPCollection<InventoryTransferDestinationItem> DestinationItems {
            get { return GetCollection<InventoryTransferDestinationItem>(nameof(DestinationItems)); }
        }
        [Browsable(false),
            NonPersistent,
            RuleFromBoolProperty("InventoryTransfer_ShopDestination_IsValid", DefaultContexts.Save, "Destination must no equal to shop", UsedProperties = "Shop, Destination")]
        public bool IsShopDestinationValid {
            get { return Destination != Shop; }
        }
        public InventoryTransfer(Session session) : base(session) { }
        protected override void OnSaving() {
            foreach(var item in Items) {
                var destnation = DestinationItems.FirstOrDefault(x => x.Item == item.Item);
                if (destnation == null) {
                    destnation = ObjectSpace.CreateObject<InventoryTransferDestinationItem>();
                    destnation.InventoryTransfer = item.InventoryTransfer;
                    destnation.Item = item.Item;

                    DestinationItems.Add(destnation);
                }
                destnation.TransactionUnit = item.TransactionUnit;
                destnation.Quantity = item.Quantity;
                item.Price = item.Price;
            }
            base.OnSaving();
        }
        protected override string GetSequenceName() {
            return ClassInfo.FullName;
        }
        protected override void OnMasterDataChanged() {
            foreach (var item in DestinationItems) {
                if (item.Shop != Destination)
                    item.Shop = Destination;
                if (item.Date != TransactionDate)
                    item.Date = TransactionDate;
                if (item.Period != Period)
                    item.Period = Period;
            }
        }
    }
}
