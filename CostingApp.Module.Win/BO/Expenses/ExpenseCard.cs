using CostingApp.Module.Win.BO.Masters;
using CostingApp.Module.Win.BO.Expenses;
using CostingApp.Module.Win.BO.Masters.Period;
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
using WXafLib.General.Model;
using CostTech.Module.Win.BO.Employees;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Data.Filtering;


namespace CostingApp.Module.Win.BO.Expenses {
    [NavigationItem("Transactions")]
    [ImageName("money")]
    public class ExpenseCard : ExpenseRecord {
        const string NumberFormat = "Concat('EX-', PadLeft(ToStr(SequentialNumber), 6, '0'))";
        [PersistentAlias(NumberFormat)]
        public string Number {
            get {
                return Convert.ToString(EvaluateAlias(nameof(Number)));
            }
        }

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("ExpenseCard_Amount_IsValid", DefaultContexts.Save, "Amount should be greater than 0")]
        public bool IsAmountValid {
            get {
                return Amount != 0;
            }
        }
        protected override string GetSequenceName() {
            return string.Concat(ClassInfo.FullName, ".ExpenseCard");
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(ExpenseDate) && oldValue != newValue)
                    Period = BasePeriod.GetOpenedPeriodForDate(ObjectSpace, ExpenseDate);
            }
        }
        public ExpenseCard(Session session) : base(session) { }
    }
}
