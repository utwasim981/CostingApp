using DevExpress.Xpo;

namespace CostingApp.Module.Win.BO.Items {
    public class SalesRecordItemComponent : InventoryRecord {
        SalesReportItem fSalesReportItem;
        [Association("SalesReportItem-SalesRecordItemComponent")]
        public SalesReportItem SalesReportItem {
            get { return fSalesReportItem; }
            set { SetPropertyValue<SalesReportItem>(nameof(SalesReportItem), ref fSalesReportItem, value); }
        }

        public SalesRecordItemComponent(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && propertyName == nameof(SalesReportItem) && oldValue != newValue)
                onSalesReportItemValueChanged();
        }
        protected override void OnDeleting() {
            base.OnDeleting();
            if (Session.IsObjectToSave(this))
                Item.UpdateQuantityOnHand(Quantity);
        }

        private void onSalesReportItemValueChanged() {
            if (SalesReportItem != null && SalesReportItem.SalesRecord != null)
                Transaction = SalesReportItem.SalesRecord;
        }
    }
}
