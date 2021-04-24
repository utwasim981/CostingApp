using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib;
using WXafLib.General.Model;
using WXafLib.General.Security;

namespace CostingApp.Module.Win.BO.Expenses {
    [XafDefaultProperty(nameof(ExpenseCategoryName))]
    [NavigationItem("Setup")]
    public class ExpenseCategory : WXafBaseObject {
        string fExpenseCategoryName;
        [Size(150)]
        [RuleRequiredField("ExpenseCategory_ExpenseCategoryName_RuleRequiredField", DefaultContexts.Save)]
        [RuleUniqueValue("ExpenseCategory_ExpenseCategoryName_RuleUniqueValue", DefaultContexts.Save)]
        public string ExpenseCategoryName {
            get { return fExpenseCategoryName; }
            set { SetPropertyValue<string>(nameof(ExpenseCategoryName), ref fExpenseCategoryName, value); }
        }
        [VisibleInDetailView(false)]
        [Association(@"ExpenseType-ExpenseCategory"), DevExpress.Xpo.Aggregated]
        public XPCollection<ExpenseType> ExpenseTypes { get { return GetCollection<ExpenseType>(nameof(ExpenseTypes)); } }

        public ExpenseCategory(Session session) : base(session) { }

    }
}
