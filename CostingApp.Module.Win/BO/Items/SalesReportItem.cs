using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using WXafLib.General.Model;
using System.Linq;
using DevExpress.Xpo.Metadata;
using System;
using WXafLib;
using DevExpress.ExpressApp.Model;

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
        [Association("SalesReportItem-SalesRecordItemComponent"), Aggregated]
        [ModelDefault("AllowEdit", "False")]
        public XPCollection<SalesRecordItemComponent> Components { get { return GetCollection<SalesRecordItemComponent>(nameof(Components)); } }

        public SalesReportItem(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Quantity = 1;
        }
        protected override void OnSaving() {
            base.OnSaving();
            onSaving();
            
        }
        private void onSaving() {
            if (SalesRecord != null && !Session.IsNewObject(SalesRecord))
                if (Session.IsNewObject(this))
                    this.CreateItemComponent();
                else if (Session.IsObjectToSave(this)) {
                    object oldValue = null;
                    if (WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(Quantity), out oldValue))
                        UpdateItemComonent();
                }
        }
        public void CreateItemComponent() {
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
                salesComponent.UpdateComponentItemCard();
                Components.Add(salesComponent);
            }
            OnChanged(nameof(Components));
        }
        public void UpdateItemComonent() {
            foreach (var itemcomponent in Item.Components) {
                var component = Components.FirstOrDefault(x => x.Item.Oid == itemcomponent.Component.Oid);
                component.Quantity = Quantity * itemcomponent.Quantity;
                component.UpdateComponentItemCard();
            }
            OnChanged(nameof(Components));
        }

    }
}
