using CostingApp.Module.Win.BO.Expenses;
using DevExpress.ExpressApp;
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
using WXafLib.General;

namespace CostingApp.Module.Win.BO {
    [NavigationItem("Administration")]
    [IsSystemConfigration(true)]    
    public class SystemConfigration : XPObject {
        bool fCityCodeM;
        [XafDisplayName("Mandatory City Code")]
        [DefaultValue(true)]
        public bool CityCodeM {
            get { return fCityCodeM; }
            set { SetPropertyValue<bool>(nameof(CityCodeM), ref fCityCodeM, value); }
        }
        bool fCityCodeA;
        [XafDisplayName("Auto Generate City Code")]
        [DefaultValue(true)]
        public bool CityCodeA {
            get { return fCityCodeA; }
            set { SetPropertyValue<bool>(nameof(CityCodeA), ref fCityCodeA, value); }
        }
        bool fShopCodeM;
        [XafDisplayName("Mandatory Shop Code")]
        [DefaultValue(true)]
        public bool ShopCodeM {
            get { return fShopCodeM; }
            set { SetPropertyValue<bool>(nameof(ShopCodeM), ref fShopCodeM, value); }
        }
        bool fShopCodeA;
        [XafDisplayName("Auto Generated Shop Code")]
        [DefaultValue(true)]
        public bool ShopCodeA {
            get { return fShopCodeA; }
            set { SetPropertyValue<bool>(nameof(ShopCodeA), ref fShopCodeA, value); }
        }
        bool fEmployeeCodeM;
        [XafDisplayName("Mandatory Employee Code")]
        [DefaultValue(true)]
        public bool EmployeeCodeM {
            get { return fEmployeeCodeM; }
            set { SetPropertyValue<bool>(nameof(EmployeeCodeM), ref fEmployeeCodeM, value); }
        }
        bool fEmployeeCodeA;
        [XafDisplayName("Auto Generated Employee Code")]
        [DefaultValue(true)]
        public bool EmployeeCodeA {
            get { return fEmployeeCodeA; }
            set { SetPropertyValue<bool>(nameof(EmployeeCodeA), ref fEmployeeCodeA, value); }
        }
        bool fItemCodeM;
        [XafDisplayName("Mandatory Item Code")]
        [DefaultValue(true)]
        public bool ItemCodeM {
            get { return fItemCodeM; }
            set { SetPropertyValue<bool>(nameof(ItemCodeM), ref fItemCodeM, value); }
        }
        bool fItemCodeA;
        [XafDisplayName("Auto Generated Item Code")]
        [DefaultValue(true)]
        public bool ItemCodeA {
            get { return fItemCodeA; }
            set { SetPropertyValue<bool>(nameof(ItemCodeA), ref fItemCodeA, value); }
        }
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

        public override void AfterConstruction() {
            base.AfterConstruction();
            CityCodeA = false;
            CityCodeM = true;
            ShopCodeA = false;
            ShopCodeM = true;
            EmployeeCodeA = false;
            EmployeeCodeM = true;
        }
        protected override void OnSaved() {
            base.OnSaved();
        }
    }
    public class SystemConfigrationController : ViewController {
        public SystemConfigrationController() {
            TargetObjectType = typeof(SystemConfigration);
            TargetViewType = ViewType.DetailView;
        }
        protected override void OnActivated() {
            base.OnActivated();
            ObjectSpace.Committed += ObjectSpace_Committed;
        }

        private void ObjectSpace_Committed(object sender, EventArgs e) {
            SystemConfigrationHelper.LoggedOnHandeler(Application);
        }
    }
}
