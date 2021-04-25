using CostingApp.Module.Win.BO.Expenses;
using CostingApp.Module.Win.BO.Masters;
using CostingApp.Module.Win.BO.Masters.Period;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib.General.Model;
using WXafLib.General.Security;

namespace CostingApp.Module.Win.BO.Items {
    [NavigationItem("Transactions")]
    [ImageName("invoice")]
    public class PurchaseInvoice : InventoryTransaction {
        const string NumberFormat = "Concat('PI-', PadLeft(ToStr(SequentialNumber), 6, '0'))";
        [PersistentAlias(NumberFormat)]
        public string InvoiceNumber {
            get { return Convert.ToString(EvaluateAlias(nameof(InvoiceNumber))); }
        }
        string fBillNumber;
        public string BillNumber {
            get { return fBillNumber; }
            set { SetPropertyValue<string>(nameof(BillNumber), ref fBillNumber, value); }
        }
        string fNotes;
        public string Notes {
            get { return fNotes; }
            set { SetPropertyValue<string>(nameof(Notes), ref fNotes, value); }
        }
        double fSubTotal;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public double SubTotal {
            get { return fSubTotal; }
            set { SetPropertyValue<double>(nameof(SubTotal), ref fSubTotal, value); }
        }
        double fVAT;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public double VAT {
            get { return fVAT; }
            set { SetPropertyValue<double>(nameof(VAT), ref fVAT, value); }
        }
        double fTotal;
        [ModelDefault("AllowEdit", "False")]
        public double Total {
            get { return fTotal; }
            set { SetPropertyValue<double>(nameof(Total), ref fTotal, value); }
        }
        [RuleRequiredField("PurchaseInvoice_Items_RuleRequiredField", DefaultContexts.Save)]
        [Association("PurchaseInvoice-PurchaseInvoiceDetail"), Aggregated]
        public XPCollection<PurchaseInvoiceDetail> Items {
            get {
                return GetCollection<PurchaseInvoiceDetail>(nameof(Items));
            }
        }

        public PurchaseInvoice(Session session) : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TransactionDate = DateTime.Now;
            TransactionType = EnumInventoryTransactionType.In;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
        }
        protected override void OnSaving() {
            base.OnSaving();
            Total = Items.Sum(x => x.Amount);
            updateItemsData();            
            updateItemsCards();
        }
        protected override string GetSequenceName() {
            return string.Concat(ClassInfo.FullName, ".PurchaseInvoice");
        }

        private void updateItemsData() {
            if (Items.Count != 0) {
                foreach (var item in Items) {
                    if (item.Shop != Shop)
                        item.Shop = Shop;
                    if (item.ExpenseDate != TransactionDate)
                        item.ExpenseDate = TransactionDate;
                    if (item.Period != Period)
                        item.Period = Period;
                }
            }
        }
        private void updateItemsCards() {
            foreach (var item in Items) {
                item.Item.UpdateLasPurchasePrice(item);
                if (Session.IsNewObject(item))
                    item.Item.UpdateQuantityOnHand(Math.Round((item.Quantity * item.TransactionUnit.ConversionRate) / item.StockUnit.ConversionRate, 2));
                else if (Session.IsObjectToSave(item)) {
                    XPMemberInfo qunatityInfo = item.ClassInfo.GetMember(nameof(item.Quantity));
                    var oldValue = PersistentBase.GetModificationsStore(item).GetPropertyOldValue(qunatityInfo);
                    if (oldValue != null)
                        item.Item.UpdateQuantityOnHand(item.Quantity - Convert.ToDouble(oldValue));
                }
            }
        }
    }
}
