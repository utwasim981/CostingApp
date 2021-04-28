using CostingApp.Module.BO.Masters.Period;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.BO.Items {
    [NavigationItem("Transactions")]
    [ImageName("BO_Order_Item")]
    public class InventoryAdjustment : InventoryTransaction {
        const string NumberFormat = "Concat('IA-', PadLeft(ToStr(SequentialNumber), 6, '0'))";
        [PersistentAlias(NumberFormat)]
        public string Number {
            get { return Convert.ToString(EvaluateAlias(nameof(Number))); }
        }
        EnumInventorySteps fStep;
        [ModelDefault("AllowEdit", "False")]
        public EnumInventorySteps Step {
            get { return fStep; }
            set { SetPropertyValue<EnumInventorySteps>(nameof(Step), ref fStep, value); }
        }
        //DateTime fEffectedDate;
        //[RuleRequiredField("InventoryAdjustment_EffectedDate_RuleRequiredField", DefaultContexts.Save)]
        //[ImmediatePostData(true)]
        //public DateTime EffectedDate {
        //    get { return fEffectedDate; }
        //    set {
        //        SetPropertyValue<DateTime>(nameof(EffectedDate), ref fEffectedDate, value);
        //        if (!IsLoading)
        //            Period = BasePeriod.GetOpenedPeriodForDate(ObjectSpace, fEffectedDate);
        //    }
        //}
        [Association("InventoryAdjustment-InventoryAdjustmentItem"), Aggregated]
        [RuleRequiredField("InventoryAdjustment_Items_RuleRequiredField", DefaultContexts.Save)]
        public XPCollection<InventoryAdjustmentItem> Items { get { return GetCollection<InventoryAdjustmentItem>(nameof(Items)); } }
        //[NonPersistent]
        //[Browsable(false)]
        //[RuleFromBoolProperty("InventoryAdjustment_EffectedDate_IsValid", DefaultContexts.Save, "Effected date must not be empty")]
        //public bool IsEffectedDatesValid {
        //    get { return Step == EnumInventorySteps.Approced && EffectedDate == DateTime.MinValue; }
        //}


        public InventoryAdjustment(Session session) : base(session) {}
        protected override void OnSaving() {
            base.OnSaving();
            updateItemsData();
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof (TransactionDate) && oldValue != newValue) {
                    Period = BasePeriod.GetOpenedPeriodForDate(ObjectSpace, TransactionDate);
                    updateItemsData();
                }
            }
        }
        protected override string GetSequenceName() {
            return string.Concat(ClassInfo.FullName, ".InventoryAdjustment");
        }

        private void updateItemsData() {
            if (Items.Count != 0) {
                foreach (var item in Items) {
                    if (item.Shop != Shop)
                        item.Shop = Shop;
                    if (item.ExpenseDate != TransactionDate)
                        item.ExpenseDate = TransactionDate;
                    if (item.Period != Period)
                        item.Period = Period;
                }
            }
        }
        public void updateItemCard() {
            foreach (var item in Items)
                item.UpdateItemCard();
        }
    }
}
