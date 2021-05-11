using CostingApp.Module.BO.Items;
using CostingApp.Module.BO.ItemTransactions.Abstraction;
using CostingApp.Module.CommonLibrary;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace CostingApp.Module.BO.ItemTransactions {
    [AddItemClass(EnumInventoryTransactionType.SalesInvoice)]
    public class SalesInvoiceDetail : OutputInventoryRecord, IAddItemList {
        SalesInvoice fInvoice;
        [Association("SalesInvoice-SalesInvoiceDetail")]
        public SalesInvoice Invoice {
            get { return fInvoice; }
            set { SetPropertyValue<SalesInvoice>(nameof(Invoice), ref fInvoice, value); }
        }
        public SalesInvoiceDetail(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(Invoice) && oldValue != newValue)
                    Transaction = Invoice;
            }
        }
    }
}
