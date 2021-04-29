using CostingApp.Module.BO.Expenses;
using CostingApp.Module.BO.Masters.Period;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
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

namespace CostingApp.Module.BO.Employees {
    [NavigationItem("Transactions")]
    [ImageName("BO_Appointment")]
    public class EmployeeAttendance : ExpenseRecord {
        public string Number {
            get {
                return string.Format("EA-{0}", SequentialNumber.ToString().PadLeft(6, '0'));
            }
        }
        string fStartAt;
        [Size(5)]
        [ImmediatePostData(true)]
        public string StartAt {
            get { return fStartAt; }
            set {
                var _value = Convert.ToDateTime(value).ToString("dd/MM/yyyy HH:mm").Substring(11, 5);
                SetPropertyValue<string>(nameof(StartAt), ref fStartAt, _value);
                calculateNumberOfHours();
                calculateTotals();
            }
        }
        string fEndAt;
        [Size(5)]
        [ImmediatePostData(true)]
        public string EndAt {
            get { return fEndAt; }
            set {
                var _value = Convert.ToDateTime(value).ToString("dd/MM/yyyy HH:mm").Substring(11, 5);
                SetPropertyValue<string>(nameof(EndAt), ref fEndAt, _value);
                    calculateNumberOfHours();
                    calculateTotals();
            }
        }
        double fCalculatedHours;
        [ImmediatePostData(true)]
        [ModelDefault("AllowEdit", "False")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public double CalculatedHours {
            get { return fCalculatedHours; }
            set {
                SetPropertyValue<double>(nameof(CalculatedHours), ref fCalculatedHours, value);
                calculateTotals();
            }
        }
        double fNumberOfHours;
        [ImmediatePostData(true)]
        public double NumberOfHours {
            get { return fNumberOfHours; }
            set {
                SetPropertyValue(nameof(NumberOfHours), ref fNumberOfHours, value);
                calculateTotals();
            }
        }
        double fCardHourPrice;
        [ImmediatePostData(true)]
        [ModelDefault("AllowEdit", "False")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public double CardHourPrice {
            get { return fCardHourPrice; }
            set {
                SetPropertyValue<double>(nameof(CardHourPrice), ref fCardHourPrice, value);
                calculateTotals();
            }
        }
        double fHourPrice;
        [ImmediatePostData(true)]
        public double HourPrice {
            get { return fHourPrice; }
            set {
                SetPropertyValue<double>(nameof(HourPrice), ref fHourPrice, value);
                calculateTotals();
            }
        }
        double fCalculatedTotal;
        [ModelDefault("AllowEdit", "False")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public double CalculatedTotal {
            get { return fCalculatedTotal; }
            set { SetPropertyValue(nameof(CalculatedTotal), ref fCalculatedTotal, value); }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("EmployeeAttendance_Employee_IsValid", DefaultContexts.Save, "Employee must not be empty")]
        public bool IsEmployeeIsValid {
            get {
                return Employee != null;
            }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("EmployeeAttendance_Amount_IsValid", DefaultContexts.Save, "Amount should be greater than 0")]
        public bool IsAmountValid {
            get {
                return Amount != 0;
            }
        }

        public EmployeeAttendance(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            StartAt = "00:00";
            EndAt = "00:00";
            ExpenseType = Session.GetObjectByKey<ExpenseType>((((ExpenseType)ValueManager.GetValueManager<Dictionary<string, object>>("Values").Value["SalaryExpense"])).Oid);
        }
        protected override void OnSaving() {
            base.OnSaving();
            calculateTotals();
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(Employee) && oldValue != newValue) {
                    CardHourPrice = Employee != null && Employee.ContractType.SalaryType == EnumSalaryType.Hourly ? Employee.SalaryPerHour : 0;
                    HourPrice = Employee != null && Employee.ContractType.SalaryType == EnumSalaryType.Hourly ? Employee.SalaryPerHour : 0;
                }
                if (propertyName == nameof(ExpenseDate) && oldValue != newValue)
                    Period = BasePeriod.GetOpenedPeriodForDate(ObjectSpace, ExpenseDate);
            }
        }
        protected override string GetSequenceName() {
            return string.Concat(ClassInfo.FullName, ".EmployeeAttendance");
        }

        private void calculateNumberOfHours() {
            if (!IsLoading && StartAt != null && EndAt != null) {
                var timeArray = fStartAt.Split(':');
                var startAt = ExpenseDate + new TimeSpan(Convert.ToInt32(timeArray[0]), Convert.ToInt32(timeArray[1]), 0);
                timeArray = fEndAt.Split(':');
                var endAt = ExpenseDate + new TimeSpan(Convert.ToInt32(timeArray[0]), Convert.ToInt32(timeArray[1]), 0);
                endAt = endAt < startAt ? endAt.AddDays(1) : endAt;
                var difference = endAt - startAt;
                CalculatedHours = difference.Hours;
                if (difference.Minutes >= 15 && difference.Minutes <= 44)
                    CalculatedHours += 0.5;
                else if (difference.Minutes >= 45)
                    CalculatedHours++;
                NumberOfHours = CalculatedHours;
            }
        }
        private void calculateTotals() {
            if (!IsLoading) {
                CalculatedTotal = CalculatedHours * CardHourPrice;
                Amount = NumberOfHours * HourPrice;
            }
        }
    }

    public class EmployeeAttendanceCheckExpenseTypeController : ViewController {
        public EmployeeAttendanceCheckExpenseTypeController() {
            TargetObjectType = typeof(EmployeeAttendance);
        }
        protected override void OnActivated() {
            base.OnActivated();
            NewObjectViewController controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
                controller.NewObjectAction.Executing += NewObjectAction_Executing;
        }
        private void NewObjectAction_Executing(object sender, CancelEventArgs e) {
            if (ValueManager.GetValueManager<Dictionary<string, object>>("Values").Value["SalaryExpense"] == null) {
                MessageOptions options = new MessageOptions();
                options.Duration = 2000;
                options.Message = "Salries expense type not added to system setup";
                options.Type = InformationType.Error;
                options.Web.Position = InformationPosition.Top;
                options.Win.Caption = "Error";
                options.Win.Type = WinMessageType.Flyout;
                Application.ShowViewStrategy.ShowMessage(options);
                e.Cancel = true;
            }
        }
    }
}
