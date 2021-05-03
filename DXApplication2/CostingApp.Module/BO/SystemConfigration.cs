using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CostingApp.Module.CommonLibrary.General;
using CostingApp.Module.BO.Expenses;
using CostingApp.Module.BO.Items;
using DevExpress.ExpressApp.Editors;

namespace CostingApp.Module.BO {
    [NavigationItem("Administration")]
    [IsSystemConfigration(true)]    
    [ImageName("setup")]
    public class SystemConfigration : XPObject {
        ExpenseType fSalaryExpense;
        public ExpenseType SalaryExpense {
            get { return fSalaryExpense; }
            set { SetPropertyValue<ExpenseType>(nameof(SalaryExpense), ref fSalaryExpense, value); }
        }
        ExpenseType fVatExpense;
        public ExpenseType VatExpense {
            get { return fVatExpense; }
            set { SetPropertyValue<ExpenseType>(nameof(VatExpense), ref fVatExpense, value); }
        }

        public SystemConfigration(Session session) : base(session) { }
    }
    public class SystemConfigrationController : ViewController<DetailView> {
        public SystemConfigrationController() {
            TargetObjectType = typeof(SystemConfigration);
        }
        protected override void OnActivated() {
            base.OnActivated();
            if (View.ViewEditMode == ViewEditMode.View)
                View.ViewEditMode = ViewEditMode.Edit;
            ObjectSpace.Committed += ObjectSpace_Committed;
        }

        private void ObjectSpace_Committed(object sender, EventArgs e) {
            SystemConfigrationHelper.LoggedOnHandeler(Application);
        }
    }
}
