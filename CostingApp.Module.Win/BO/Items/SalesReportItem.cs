using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using WXafLib.General.Model;
using System.Linq;

namespace CostingApp.Module.Win.BO.Items {
    public class SalesReportItem : WXafBase {
        SalesRecord fSalesRecord;
        [Association("SalesRecord-SalesReportItem")]
        public SalesRecord SalesRecord {
            get { return fSalesRecord; }
            set { SetPropertyValue<SalesRecord>(nameof(SalesRecord), ref fSalesRecord, value); }
        }
        MenuItem fItem;
        [DataSourceCriteria("IsActive = True")]
        [RuleRequiredField("SalesReportItem_Item_RuleRequiredField", DefaultContexts.Save)]
        public MenuItem Item {
            get { return fItem; }
            set { SetPropertyValue<MenuItem>(nameof(Item), ref fItem, value); }
        }
        double fQuantity;
        [RuleValueComparison("SalesReportItem_Quantity.GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public double Quantity {
            get { return fQuantity; }
            set { SetPropertyValue<double>(nameof(Quantity), ref fQuantity, value); }
        }
        //[VisibleInDetailView(false)]
        [Association("SalesReportItem-SalesRecordItemComponent"), Aggregated]
        public XPCollection<SalesRecordItemComponent> Components { get { return GetCollection<SalesRecordItemComponent>(nameof(Components)); } }

        public SalesReportItem(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Quantity = 1;
        }
        protected override void OnSaving() {
            base.OnSaving();
            createUpdateItemComponent();
        }

        private void createUpdateItemComponent() {

            if (Session.IsNewObject(this)) {
                foreach (var itemComponent1 in Item.Components) {
                    SalesRecordItemComponent salesComponent = null;
                    salesComponent = ObjectSpace.CreateObject<SalesRecordItemComponent>();
                    salesComponent.Shop = SalesRecord.Shop;
                    salesComponent.ExpenseDate = SalesRecord.TransactionDate;
                    salesComponent.Period = SalesRecord.Period;
                    salesComponent.SalesReportItem = this;
                    salesComponent.Item = itemComponent1.Component;
                    salesComponent.TransactionUnit = itemComponent1.Unit;
                    salesComponent.Quantity = Quantity * itemComponent1.Quantity;
                    Components.Add(salesComponent);
                }
            }
            else if (Session.IsObjectToSave(this)) {
                foreach (var itemcomponent in Item.Components) {
                    var test = Components.FirstOrDefault(x => x.Item.Oid == itemcomponent.Component.Oid);
                    test.Quantity = Quantity * itemcomponent.Quantity;
                }
            }
        }

    }
}
