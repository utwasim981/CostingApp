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
        protected override string GetSequenceName() {
            return string.Concat(ClassInfo.FullName, ".ExpenseCard");
        }
        public ExpenseCard(Session session) : base(session) { }
    }
}
