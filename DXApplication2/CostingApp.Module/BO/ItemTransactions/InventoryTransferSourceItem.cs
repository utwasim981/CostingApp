using CostingApp.Module.BO.ItemTransactions.Abstraction;
using DevExpress.Xpo;
using System.Linq;

namespace CostingApp.Module.BO.ItemTransactions {
    [AddItemClass(EnumInventoryTransactionType.InventoryTransfer)]
    public class InventoryTransferSourceItem : OutputInventoryRecord, IAddItemList {
        InventoryTransfer fInventoryTransfer;
        [Association("InventoryTransfer-InventoryTransferSourceItem")]
        public InventoryTransfer InventoryTransfer {
            get { return fInventoryTransfer; }
            set { SetPropertyValue<InventoryTransfer>(nameof(InventoryTransfer), ref fInventoryTransfer, value); }
        }

        public InventoryTransferSourceItem(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(InventoryTransfer) && oldValue != newValue)
                    Transaction = InventoryTransfer;
            }
        }
        protected override void OnSaving() {
            if (!Session.IsNewObject(InventoryTransfer) &&
                !Session.IsObjectToSave(InventoryTransfer)) {
                var destnation = InventoryTransfer.DestinationItems.FirstOrDefault(x => x.Item == Item && x.TransactionUnit == TransactionUnit);
                if (destnation != null) {
                    destnation.Quantity = Quantity;
                    destnation.Price = Price;
                }
            }
            base.OnSaving();
        }
        protected override void OnDeleting() {
            var destnation = InventoryTransfer.DestinationItems.FirstOrDefault(x => x.Item == Item);
            if (destnation != null)
                InventoryTransfer.DestinationItems.Remove(destnation);
            base.OnDeleting();
        }
    }
}
