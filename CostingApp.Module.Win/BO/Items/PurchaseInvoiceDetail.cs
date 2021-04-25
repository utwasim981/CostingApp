using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using System.Linq;

namespace CostingApp.Module.Win.BO.Items {
    public class PurchaseInvoiceDetail : InventoryRecord {
        PurchaseInvoice fInvoice;
        [Association("PurchaseInvoice-PurchaseInvoiceDetail")]
        public PurchaseInvoice Invoice {
            get { return fInvoice; }
            set {
                SetPropertyValue<PurchaseInvoice>(nameof(Invoice), ref fInvoice, value);
                if (!IsLoading)
                    onInvoiceValueChange();
            }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PurchaseInvoiceDetail_Item_IsValid", DefaultContexts.Save, "Item must not be empty")]
        public bool IsItemIsValid {
            get { return Item != null; }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PurchaseInvoiceDetail_QuantityIn_IsValid", DefaultContexts.Save, "Item must not be empty")]
        public bool IsQuantityInIsValid {
            get { return Quantity != 0; }
        }
        public PurchaseInvoiceDetail(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Quantity = 1;
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(Item) && oldValue != newValue)
                    onItemValueChange();
                if ((propertyName == nameof(Price) || propertyName == nameof(Quantity)) && oldValue != newValue)
                    Amount = Quantity * Price;
                if (propertyName == nameof(Amount) && oldValue != newValue)
                    updateInvoiceTotal();
            }
        }
        protected override void OnSaving() {
            base.OnSaving();
            updateInvoiceTotal();
        }
        protected override void OnDeleting() {
            base.OnDeleting();
            updateIemCard();
        }

        private void onInvoiceValueChange() {
            if (Invoice != null) {
                Shop = Invoice.Shop;
                Transaction = Invoice;
                ExpenseDate = Invoice.TransactionDate;
                Period = Invoice.Period;
            }
        }
        private void onItemValueChange() {

            if (Item != null) {
                TransactionUnit = Item.PurchaseUnit;
                Price = Item.PurchasePrice;
            }
            else {
                TransactionUnit = null;
                Price = 0;
            }
        }
        private void updateInvoiceTotal() {
            if (Invoice != null)
                Invoice.Total = Invoice.Items.Sum(x => x.Amount);
        }
        private void updateIemCard() {
            if (Session.IsObjectToSave(this))
                Item.UpdateQuantityOnHand(Quantity * -1);
        }
    }
}
