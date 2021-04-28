using CostingApp.Module.BO.Employees;
using CostingApp.Module.BO.Items;
using CostingApp.Module.BO.Masters;
using CostingApp.Module.BO.Masters.Period;
using CostingApp.Module.CommonLibrary.General.Model;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.BO.Expenses {
    [NavigationItem("Testing")]
    public abstract class ExpenseRecord : WXafSequenceObject {
        Shop fShop;
        [DataSourceCriteria("IsActive = True")]
        [RuleRequiredField("ExpenseRecord_Shop_RuleRequiredField", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        public Shop Shop {
            get { return fShop; }
            set { SetPropertyValue<Shop>(nameof(Shop), ref fShop, value); }
        }
        BasePeriod fPeriod;
        [RuleRequiredField("ExpenseRecord_Period_RuleRequiredField", DefaultContexts.Save)]
        [ModelDefault("AllowEdit", "False")]
        [ImmediatePostData(true)]
        public BasePeriod Period {
            get { return fPeriod; }
            set { SetPropertyValue<BasePeriod>(nameof(Period), ref fPeriod, value); }
        }
        ExpenseType fExpenseType;
        [DataSourceCriteria("IsActive = True")]
        [RuleRequiredField("ExpenseRecord_ExpenseType_RuleRequiredField", DefaultContexts.Save)]
        [Appearance("ExpenseRecord_ExpenseType.Enable", Enabled = false, Criteria = "Not IsNull(Item)")]
        [ImmediatePostData(true)]
        public ExpenseType ExpenseType {
            get { return fExpenseType; }
            set { SetPropertyValue<ExpenseType>(nameof(ExpenseType), ref fExpenseType, value); }
        }
        DateTime fExpenseDate;
        [RuleRequiredField("ExpenseRecord_ExpenseDate_RuleRequiredField", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        public DateTime ExpenseDate {
            get { return fExpenseDate; }
            set { SetPropertyValue<DateTime>(nameof(ExpenseDate), ref fExpenseDate, value); }
        }
        double fAmount;
        //[RuleValueComparison("ExpenseRecord_Amount.GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public double Amount {
            get { return fAmount; }
            set { SetPropertyValue<double>(nameof(Amount), ref fAmount, value); }
        }
        string fVendorBill;
        public string VendorBill {
            get { return fVendorBill; }
            set { SetPropertyValue<string>(nameof(VendorBill), ref fVendorBill, value); }
        }
        string fNotes;
        public string Notes {
            get { return fNotes; }
            set { SetPropertyValue<string>(nameof(Notes), ref fNotes, value); }
        }
        EnumStatus fStatus;
        [ModelDefault("AllowEdit", "False")]
        [ImmediatePostData(true)]
        public EnumStatus Status {
            get { return fStatus; }
            set { SetPropertyValue<EnumStatus>(nameof(Status), ref fStatus, value); }
        }
        Employee fEmployee;
        [ImmediatePostData(true)]
        public Employee Employee {
            get { return fEmployee; }
            set { SetPropertyValue<Employee>(nameof(Employee), ref fEmployee, value); }
        }
        ItemCard fItem;
        [DataSourceCriteria("IsActive = True")]
        [ImmediatePostData(true)]
        public ItemCard Item {
            get { return fItem; }
            set {
                SetPropertyValue<ItemCard>(nameof(Item), ref fItem, value);
                if (!IsLoading)
                    ExpenseType = Item != null ? Item.ExpenseType : null;
            }
        }

        //[NonPersistent]
        //[Browsable(false)]
        //[RuleFromBoolProperty("ExpenseRecord_Period_IsValid", DefaultContexts.Save, "There is no period oppened for this date")]
        //public bool IsPeriodIsValid {
        //    get { return Period != null && Period.Status == EnumStatus.Opened; }
        //}               
        public ExpenseRecord(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            ExpenseDate = DateTime.Now;
            Status = EnumStatus.Opened;
        }
    }
}
