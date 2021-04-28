using CostingApp.Module.CommonLibrary;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace CostingApp.Module.BO.Items {
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
        [RuleFromBoolProperty("PurchaseInvoiceDetail_Item_IsValid", DefaultContexts.Save, "Item must not be empty", UsedProperties = nameof(Item))]
        public bool IsItemIsValid {
            get { return Item != null; }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PurchaseInvoiceDetail_Quantity_IsValid", DefaultContexts.Save, "Quantity should be greater than 0", UsedProperties = nameof(Quantity))]
        public bool IsQuantityInIsValid {
            get { return Quantity != 0; }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PurchaseInvoiceDetail_Price_IsValid", DefaultContexts.Save, "Price should be greater than 0", UsedProperties = nameof(Price))]
        public bool IPriceInIsValid {
            get { return Price != 0; }
        }

        public PurchaseInvoiceDetail(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Quantity = 1;
            TransactionType = EnumInventoryTransactionType.In;
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
            Item.UpdateQuantityOnHand(Shop, TransactionUnit, Quantity * -1);
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
                Price = Item.GetLastPurchasePrice(Shop) != 0 ? Item.GetLastPurchasePrice(Shop) : Item.PurchasePrice;
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

        public void UpdateItemCard() {
            Item.UpdateLasPurchasePrice(Shop, TransactionUnit, ExpenseDate, Price);
            object quantityOldValue = null;
            object unitOldValue = null;
            if (WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(Quantity), out quantityOldValue))
                if (WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(TransactionUnit), out unitOldValue)) {
                    Item.UpdateQuantityOnHand(Shop, (Unit)unitOldValue, Convert.ToDouble(quantityOldValue));
                    Item.UpdateQuantityOnHand(Shop, TransactionUnit, Quantity);
                }
                else
                    Item.UpdateQuantityOnHand(Shop, TransactionUnit, (Quantity - Convert.ToDouble(quantityOldValue)));
            else
                Item.UpdateQuantityOnHand(Shop, TransactionUnit, Quantity);
        }
    }
}
