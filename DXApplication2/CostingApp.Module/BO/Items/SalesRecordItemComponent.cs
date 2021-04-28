using CostingApp.Module.CommonLibrary;
using DevExpress.Xpo;
using System;

namespace CostingApp.Module.BO.Items {
    public class SalesRecordItemComponent : InventoryRecord {
        SalesReportItem fSalesReportItem;
        [Association("SalesReportItem-SalesRecordItemComponent")]
        public SalesReportItem SalesReportItem {
            get { return fSalesReportItem; }
            set { SetPropertyValue<SalesReportItem>(nameof(SalesReportItem), ref fSalesReportItem, value); }
        }

        public SalesRecordItemComponent(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TransactionType = EnumInventoryTransactionType.Out;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && propertyName == nameof(SalesReportItem) && oldValue != newValue)
                onSalesReportItemValueChanged();
        }
        protected override void OnDeleting() {
            base.OnDeleting();
            if (Session.IsObjectToSave(this))
                Item.UpdateQuantityOnHand(Shop, TransactionUnit, Quantity);
        }

        public void UpdateComponentItemCard() {
            object oldValue = null;
            if (WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(Quantity), out oldValue))                
                Item.UpdateQuantityOnHand(Shop, TransactionUnit, (Quantity - Convert.ToDouble(oldValue)) * -1);
            else
                Item.UpdateQuantityOnHand(Shop, TransactionUnit, Quantity * -1);
        }
        private void onSalesReportItemValueChanged() {
            if (SalesReportItem != null && SalesReportItem.SalesRecord != null)
                Transaction = SalesReportItem.SalesRecord;
        }

    }
}
