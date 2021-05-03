using CostingApp.Module.BO.Items;
using CostingApp.Module.BO.ItemTransactions.Abstraction;
using CostingApp.Module.CommonLibrary;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace CostingApp.Module.BO.ItemTransactions {
    public class PurchaseInvoiceDetail : InputInventoryRecord {
        PurchaseInvoice fInvoice;
        [Association("PurchaseInvoice-PurchaseInvoiceDetail")]
        public PurchaseInvoice Invoice {
            get { return fInvoice; }
            set { SetPropertyValue<PurchaseInvoice>(nameof(Invoice), ref fInvoice, value); }
        }        

        public PurchaseInvoiceDetail(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(Invoice) && oldValue != newValue)
                    Transaction = Invoice;
                else if (propertyName == nameof(TransactionUnit) && oldValue != newValue)
                    Price = Item.GetLastPurchasePrice(Shop, TransactionUnit);
            }
        }
    }
}
