using CostingApp.Module.Win.BO.Masters;
using CostingApp.Module.Win.BO.Masters.Period;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib;

namespace CostingApp.Module.Win.BO.Items {
    [NavigationItem("Transactions")]
    [ImageName("BO_Sale_v92")]
    public class SalesRecord : InventoryTransaction {
        const string NumberFormat = "Concat('SR-', PadLeft(ToStr(SequentialNumber), 6, '0'))";
        [PersistentAlias(NumberFormat)]
        public string RecordNumber {
            get { return Convert.ToString(EvaluateAlias(nameof(RecordNumber))); }
        }
        DateTime fFromDate;
        [RuleRequiredField("SalesRecord_RecordFromDate_RuleRequiredField", DefaultContexts.Save)]
        public DateTime FromDate {
            get { return fFromDate; }
            set { SetPropertyValue<DateTime>(nameof(FromDate), ref fFromDate, value); }
        }
        DateTime fToDate;
        [RuleRequiredField("SalesRecord_RecordToDate_RuleRequiredField", DefaultContexts.Save)]
        public DateTime ToDate {
            get { return fToDate; }
            set { SetPropertyValue<DateTime>(nameof(ToDate), ref fToDate, value); }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("SalesRecord_RecordDateRange_IsValid", DefaultContexts.Save, "from date should be less than or eqaul end date", UsedProperties = "FromDate, ToDate")]
        public bool IsDateRangeIsValid {
            get { return FromDate <= ToDate; }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("SalesRecord_RecordDateRange_IsExist", DefaultContexts.Save, "The date range is overlaping", UsedProperties = "FromDate, ToDate")]
        public bool IsDateRangeIsExist {
            get {
                var co = CriteriaOperator.And(new BinaryOperator(nameof(FromDate), ToDate, BinaryOperatorType.LessOrEqual),
                                              new BinaryOperator(nameof(ToDate), FromDate, BinaryOperatorType.GreaterOrEqual));
                if (Session.IsNewObject(this))
                    return ObjectSpace.GetObjects<SalesRecord>(co).Count == 0;
                else {
                    object oldValue = null;
                    if (!(WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(FromDate), out oldValue) &&
                        WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(ToDate), out oldValue)))
                        return true;
                    else
                        return ObjectSpace.GetObjects<SalesRecord>(co).Count == 0;
                }
            }
        }

        [Association("SalesRecord-SalesReportItem"), Aggregated]
        [RuleRequiredField("SalesRecord_Items_RuleRequiredField", DefaultContexts.Save)]
        public XPCollection<SalesReportItem> Items { get { return GetCollection<SalesReportItem>(nameof(Items)); } }

        public SalesRecord(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TransactionType = EnumInventoryTransactionType.Out;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(TransactionDate) && oldValue != newValue)
                    Period = BasePeriod.GetOpenedPeriodForDate(ObjectSpace, TransactionDate);
            }
        }
        protected override void OnSaving() {
            base.OnSaving();
            onSaving();
        }
        protected override string GetSequenceName() {
            return string.Concat(ClassInfo.FullName, ".SalesRecord");
        }

        private void onSaving() {
            updateItemData();
            updateItemsCards();
        }
        private void updateItemData() {
            foreach (var item in Items)
                foreach(var component in item.Components) {
                    if (component.ExpenseDate != TransactionDate)
                        component.ExpenseDate = TransactionDate;
                    if (component.Period != Period)
                        component.Period = Period;
                    if (component.Shop != Shop)
                        component.Shop = Shop;
                }
        }
        private void updateItemsCards() {
            if (Session.IsNewObject(this))
                foreach (var item in Items)
                    item.CreateItemComponent();
        }
    }
}
