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

namespace CostingApp.Module.Win.BO.Items {
    [NavigationItem("Transactions")]
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
        [RuleFromBoolProperty("SalesRecord_RecordDateRange_IsValid", DefaultContexts.Save, "The date range is overlaping", UsedProperties = "FromDate, ToDate")]
        public bool IsDateRangeIsValid {
            get {
                var co = CriteriaOperator.And(new BinaryOperator(nameof(FromDate), ToDate, BinaryOperatorType.LessOrEqual),
                                              new BinaryOperator(nameof(ToDate), FromDate, BinaryOperatorType.GreaterOrEqual));
                if (Session.IsNewObject(this))
                    return ObjectSpace.GetObjects<SalesRecord>(co).Count == 0;
                else {
                    bool check = false;
                    XPMemberInfo fromDateInfo = ClassInfo.GetMember(nameof(FromDate));
                    check = PersistentBase.GetModificationsStore(this).GetPropertyOldValue(fromDateInfo) != null;
                    XPMemberInfo toDateInfo = ClassInfo.GetMember(nameof(ToDate));
                    check = PersistentBase.GetModificationsStore(this).GetPropertyOldValue(toDateInfo) != null;
                    if (!check)
                        return true;
                    else
                        return ObjectSpace.GetObjects<SalesRecord>(co).Count == 0;
                }
            }
        }

        [Association("SalesRecord-SalesReportItem"), Aggregated]
        public XPCollection<SalesReportItem> Items { get { return GetCollection<SalesReportItem>(nameof(Items)); } }

        public SalesRecord(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TransactionType = EnumInventoryTransactionType.Out;
        }
        protected override string GetSequenceName() {
            return string.Concat(ClassInfo.FullName, ".SalesRecord");
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
            foreach (var item in Items) {                
                if (Session.IsNewObject(item))
                    foreach (var component in item.Components)
                        component.Item.UpdateQuantityOnHand(Math.Round((component.Quantity * component.TransactionUnit.ConversionRate) / component.StockUnit.ConversionRate, 2) * -1);
                else if (Session.IsObjectToSave(item)) {
                    foreach (var component in item.Components) {
                        XPMemberInfo qunatityInfo = component.ClassInfo.GetMember(nameof(component.Quantity));
                        var oldValue = PersistentBase.GetModificationsStore(component).GetPropertyOldValue(qunatityInfo);
                        if (oldValue != null)
                            component.Item.UpdateQuantityOnHand((component.Quantity - Convert.ToDouble(oldValue)) * -1);
                    }
                }
            }
        }
    }
}
