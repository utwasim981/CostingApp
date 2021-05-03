using CostingApp.Module.BO.ItemTransactions.Abstraction;
using CostingApp.Module.BO.Masters.Period;
using CostingApp.Module.CommonLibrary;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
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

namespace CostingApp.Module.BO.ItemTransactions {
    [NavigationItem("Transactions"),
        ImageName("BO_Sale_v92"),
        XafDefaultProperty(nameof(Number))]
    public class MenuSales : InventoryTransaction {
        public override EnumInventoryTransactionType TransactionType { get { return EnumInventoryTransactionType.MenuSales; } }
        const string NumberFormat = "Concat('MS-', PadLeft(ToStr(SequentialNumber), 6, '0'))";
        [PersistentAlias(NumberFormat)]
        public string Number {
            get { return Convert.ToString(EvaluateAlias(nameof(Number))); }
        }
        DateTime fFromDate;
        [RuleRequiredField("MenuSales_RecordFromDate_RuleRequiredField", DefaultContexts.Save)]
        public DateTime FromDate {
            get { return fFromDate; }
            set { SetPropertyValue<DateTime>(nameof(FromDate), ref fFromDate, value); }
        }
        DateTime fToDate;
        [RuleRequiredField("MenuSales_RecordToDate_RuleRequiredField", DefaultContexts.Save)]
        public DateTime ToDate {
            get { return fToDate; }
            set { SetPropertyValue<DateTime>(nameof(ToDate), ref fToDate, value); }
        }
        [Association("MenuSales-MenuSalesItem"), DevExpress.Xpo.Aggregated]
        [RuleRequiredField("MenuSales_Items_RuleRequiredField", DefaultContexts.Save)]
        public XPCollection<MenuSalesItem> Items { get { return GetCollection<MenuSalesItem>(nameof(Items)); } }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("MenuSales_RecordDateRange_IsValid", DefaultContexts.Save, "from date should be less than or eqaul end date", UsedProperties = "FromDate, ToDate")]
        public bool IsDateRangeIsValid {
            get { return FromDate <= ToDate; }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("MenuSales_RecordDateRange_IsExist", DefaultContexts.Save, "The date range is overlaping", UsedProperties = "FromDate, ToDate")]
        public bool IsDateRangeIsExist {
            get {
                var co = CriteriaOperator.And(new BinaryOperator(nameof(FromDate), ToDate, BinaryOperatorType.LessOrEqual),
                                              new BinaryOperator(nameof(ToDate), FromDate, BinaryOperatorType.GreaterOrEqual));
                if (Session.IsNewObject(this))
                    return ObjectSpace.GetObjects<MenuSales>(co).Count == 0;
                else {
                    object oldValue = null;
                    if (!(WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(FromDate), out oldValue) &&
                        WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(ToDate), out oldValue)))
                        return true;
                    else
                        return ObjectSpace.GetObjects<MenuSales>(co).Count == 0;
                }
            }
        }
        [Browsable(false),
            NonPersistent,
            RuleFromBoolProperty("MenuSales_Item_RuleUniqueField", DefaultContexts.Save, "\"Item\" must be unique", UsedProperties = "Items")]
        public bool IsItemsUnique {
            get {
                if (Items.Count == 0)
                    return true;
                else
                    return Items.GroupBy(x => x.Item).Count() == 1;
            }
        }

        public MenuSales(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if ((propertyName == nameof(Shop) ||
                    propertyName == nameof(TransactionDate) ||
                    propertyName == nameof(Period)) &&
                    oldValue != newValue)
                    onMasterValueChange();
            }
        }
        protected override void OnSaving() {
            base.OnSaving();
            if (!(Session is NestedUnitOfWork))
                onSaving();
        }
        protected void onMasterValueChange() {
            foreach (var item in Items) {
                foreach (var component in item.Components) {
                    if (component.Shop != Shop)
                        component.Shop = Shop;
                    if (component.Date != TransactionDate)
                        component.Date = TransactionDate;
                    if (component.Period != Period)
                        component.Period = Period;
                }
            }
        }
        protected override string GetSequenceName() {
            return ClassInfo.FullName;
        }

        private void onSaving() {
            foreach(var item in Items) {
                item.CreateItemComponent();
            }
        }
    }
}
