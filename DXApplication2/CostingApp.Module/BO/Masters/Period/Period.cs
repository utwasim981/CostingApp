using System;
using System.ComponentModel;
using CostingApp.Module.CommonLibrary.General.Model;
using CostingApp.Module.CommonLibrary.General.Security;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;


namespace CostingApp.Module.BO.Masters.Period {
    [XafDefaultProperty(nameof(PeriodName))]
    [NavigationItem("Administration")]
    [ImageName("Calendar")]
    public abstract class Period : WXafBaseObject, ITreeNode {
        private string fPeriodName;
        [RuleRequiredField("Period_PeriodName_RuleRequiredField", DefaultContexts.Save)]
        [RuleUniqueValue("Period_PeriodName_RuleUniqueValue", DefaultContexts.Save)]
        public string PeriodName {
            get { return fPeriodName; }
            set { SetPropertyValue<string>(nameof(PeriodName), ref fPeriodName, value); }
        }
        DateTime fStartDate;
        public DateTime StartDate {
            get { return fStartDate; }
            set { SetPropertyValue<DateTime>(nameof(StartDate), ref fStartDate, value); }
        }
        DateTime dateTime;
        public DateTime EndDate {
            get { return dateTime; }
            set { SetPropertyValue<DateTime>(nameof(EndDate), ref dateTime, value); }
        }
        [Browsable(false)]
        public EnumPersiodType PeriodType {
            get { return fPeriodType; }
            set { SetPropertyValue<EnumPersiodType>(nameof(PeriodType), ref fPeriodType, value); }
        }
        EnumStatus fStatus;
        public EnumStatus Status {
            get { return fStatus; }
            set { SetPropertyValue<EnumStatus>(nameof(Status), ref fStatus, value); }
        }
        DateTime fOpenDate;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public DateTime OpenDate {
            get { return fOpenDate; }
            set { SetPropertyValue<DateTime>(nameof(OpenDate), ref fOpenDate, value); }
        }
        WXafUser fOpenedBy;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public WXafUser OpenedBy {
            get { return fOpenedBy; }
            set { SetPropertyValue<WXafUser>(nameof(OpenedBy), ref fOpenedBy, value); }
        }
        DateTime fCloseDate;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public DateTime CloseDate {
            get { return fCloseDate; }
            set { SetPropertyValue<DateTime>(nameof(CloseDate), ref fCloseDate, value); }
        }
        WXafUser fClosedBy;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public WXafUser ClosedBy {
            get { return fClosedBy; }
            set { SetPropertyValue<WXafUser>(nameof(ClosedBy), ref fClosedBy, value); }
        }
        EnumPersiodType fPeriodType;
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("Period_StartDateGreaterThanEndDate_IsValid", DefaultContexts.Save, "The start date must be less than the end date", UsedProperties = "StartDate, EndDate")]
        public bool IsStartDateEndDateIsValid {
            get { return StartDate < EndDate; }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("Period_PeriodDateRange_IsValid", DefaultContexts.Save, "The date range is overlaping", UsedProperties = "StartDate, EndDate")]
        public bool IsDateRangeIsValid {
            get {
                var co = CriteriaOperator.And(new BinaryOperator(nameof(StartDate), EndDate, BinaryOperatorType.LessOrEqual),
                                              new BinaryOperator(nameof(EndDate), StartDate, BinaryOperatorType.GreaterOrEqual),
                                              new BinaryOperator(nameof(PeriodType), PeriodType, BinaryOperatorType.Equal));

                return ObjectSpace.GetObjects<Period>(co).Count == 0;
            }
        }

        public Period(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Status = EnumStatus.None;
        }
        protected override void OnSaving() {
            base.OnSaving();
            OpenDate = DateTime.Now;
            OpenedBy = ObjectSpace.GetObject<WXafUser>((WXafUser)SecuritySystem.CurrentUser);
        }

        public static BasePeriod GetOpenedPeriodForDate(IObjectSpace objectSpace, DateTime date) {
            var co = CriteriaOperator.And(CriteriaOperator.And(new BinaryOperator(nameof(StartDate), date.Date, BinaryOperatorType.LessOrEqual),
                new BinaryOperator(nameof(EndDate), date.Date, BinaryOperatorType.GreaterOrEqual)),
                new BinaryOperator(nameof(Status), EnumStatus.Opened, BinaryOperatorType.Equal));
            //var co = new BetweenOperator(date, new OperandProperty("StartDate"), new OperandProperty("EndDate"));
            return objectSpace.FindObject<BasePeriod>(co);
        }
        public static BasePeriod GetClosedPeriodForDate(IObjectSpace objectSpace, DateTime date) {
            var co = CriteriaOperator.And(CriteriaOperator.And(new BinaryOperator(nameof(StartDate), date, BinaryOperatorType.LessOrEqual),
                new BinaryOperator(nameof(EndDate), date, BinaryOperatorType.GreaterOrEqual)),
                new BinaryOperator(nameof(Status), EnumStatus.Closed, BinaryOperatorType.Equal));
            return objectSpace.FindObject<BasePeriod>(co);
        }


        #region ITreeNode
        protected abstract ITreeNode Parent {
            get;
        }
        protected abstract IBindingList Children {
            get;
        }
        IBindingList ITreeNode.Children {
            get {
                return Children;
            }
        }
        string ITreeNode.Name {
            get {
                return PeriodName;
            }
        }
        ITreeNode ITreeNode.Parent {
            get {
                return Parent;
            }
        }
        #endregion
    }

}
