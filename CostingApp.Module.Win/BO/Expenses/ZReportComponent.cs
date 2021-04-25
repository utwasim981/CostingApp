using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib.General.Model;

namespace CostingApp.Module.Win.BO.Expenses {
    [NavigationItem("Expenses Setup")]
    public class ZReportComponent : WXafBaseObject {
        string fComponentName;
        [RuleRequiredField("ZReportComponent_ComponentName_RuleRequiredField", DefaultContexts.Save)]
        [RuleUniqueValue("ZReportComponent_ComponentName_RuleUniqueValue", DefaultContexts.Save)]
        public string ComponentName {
            get { return fComponentName; }
            set { SetPropertyValue<string>(nameof(ComponentName), ref fComponentName, value); }
        }
        EmumCalculationType fCalculation;
        public EmumCalculationType Calculation {
            get { return fCalculation; }
            set { SetPropertyValue<EmumCalculationType>(nameof(Calculation), ref fCalculation, value); }
        }
        double fValue;
        [RuleValueComparison("ZReportComponent_Value.GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public double Value {
            get { return fValue; }
            set { SetPropertyValue<double>(nameof(Value), ref fValue, value); }
        }
        ExpenseType fExpenseType;
        [RuleRequiredField("ZReportComponent_ExpenseType_RuleRequiredField", DefaultContexts.Save)]
        public ExpenseType ExpenseType {
            get { return fExpenseType; }
            set { SetPropertyValue<ExpenseType>(nameof(ExpenseType), ref fExpenseType, value); }
        }

        public ZReportComponent(Session session) : base(session) { }
    }
}
