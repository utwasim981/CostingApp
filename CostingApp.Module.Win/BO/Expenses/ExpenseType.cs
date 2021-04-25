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
    [XafDefaultProperty(nameof(ExpenseTypeName))]
    [NavigationItem("Expenses Setup")]
    [ImageName("expensetype")]
    public class ExpenseType : WXafBaseObject {       
        ExpenseCategory fExpenseCategory;
        [Association("ExpenseType-ExpenseCategory")]
        [DataSourceCriteria("IsActive = True")]
        public ExpenseCategory ExpenseCategory {
            get { return fExpenseCategory; }
            set { SetPropertyValue<ExpenseCategory>(nameof(ExpenseCategory), ref fExpenseCategory, value); }
        }
        string fExpenseTypeName;
        [Size(150)]
        [RuleRequiredField("ExpenseType.ExpenseTypeName.Req", DefaultContexts.Save)]
        [RuleUniqueValue("ExpenseType.ExpenseTypeName.Unq", DefaultContexts.Save)]
        public string ExpenseTypeName {
            get { return fExpenseTypeName; }
            set { SetPropertyValue<string>(nameof(ExpenseTypeName), ref fExpenseTypeName, value); }
        }
        public ExpenseType(Session session) : base(session) { }
    }
}
