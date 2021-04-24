using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
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
using WXafLib.General.Security;

namespace CostingApp.Module.Win.BO.Masters.Period {
    [DomainComponent]
    public class OpenFullYearPeriods : IObjectSpaceLink, INotifyPropertyChanged {
        int fYearEnd;
        [Size(4)]
        [RuleValueComparison("OpenFullYearPeriods_YearEnd_NotEquals1000", DefaultContexts.Save, ValueComparisonType.GreaterThan, 1000)]
        public int YearEnd {
            get { return fYearEnd; }
            set {
                fYearEnd = value;
                if (StartingMonth == 0)
                    StartDate = new DateTime(fYearEnd, (int)StartingMonth + 1, 1);
                else
                    StartDate = new DateTime(fYearEnd, (int)StartingMonth, 1);
                var tempDate = StartDate.AddMonths(11);
                new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
                OnPropertyChanged(nameof(YearEnd));
            }
        }
        EnumMonth fStartingMonth;

        [RuleValueComparison("OpenFullYearPeriods_YearEnd_NotEquals0", DefaultContexts.Save, ValueComparisonType.NotEquals, EnumMonth.None)]
        public EnumMonth StartingMonth {
            get { return fStartingMonth; }
            set {
                fStartingMonth = value;
                StartDate = new DateTime(fYearEnd, (int)StartingMonth, 1);
                var tempDate = StartDate.AddMonths(11);
                new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
                OnPropertyChanged(nameof(StartingMonth));
            }
        }
        public OpenFullYearPeriods() {
            YearEnd = DateTime.Now.Year;
            StartingMonth = EnumMonth.January;
        }
        DateTime fStartDate;
        [VisibleInDetailView(false)]
        public DateTime StartDate {
            get {
                return fStartDate; 
                    //new DateTime(Year, (int)StartingMonth, 1);
            }
            set {
                fStartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        DateTime fEndDate;
        [VisibleInDetailView(false)]
        public DateTime EndDate {

            get {
                //var startDate = new DateTime(Year, (int)StartingMonth, 1);
                //var tempDate = startDate.AddMonths(11);
                return fEndDate; 
                    //new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
            }
            set {
                fEndDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("Period_PeriodDateRange_IsValid1", "The date range is overlaping", TargetContextIDs = "DialogOK", UsedProperties = "StartDate, EndDate")]
        public bool IsDateRangeIsValid {
            get {
                string criteria = $"({nameof(StartDate)} <= ?) And ({nameof(EndDate)} >= ?)";
                var periods = ObjectSpace.GetObjects<Period>(CriteriaOperator.Parse(criteria, EndDate, StartDate));
                return periods.Count == 0;
            }
        }

        public IObjectSpace ObjectSpace { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    //public class OpenFullYearPeriodsController : ViewController<ListView> {
    //    public OpenFullYearPeriodsController() {
    //        TargetObjectType = typeof(Period);
    //        createPopupWindowShowAction();
    //    }
    //    void createFullYearPeriod(OpenFullYearPeriods OpenFullYearPeriods) {
    //        var yStartDate = new DateTime(OpenFullYearPeriods.YearEnd, (int)OpenFullYearPeriods.StartingMonth, 1);
    //        var tempDate = yStartDate.AddMonths(11);
    //        var yEndDate = new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
    //        var objectSpace = Application.CreateObjectSpace();
    //        var year = createYearPeriod(OpenFullYearPeriods.YearEnd.ToString(), yStartDate, yEndDate, objectSpace);
    //        var qStartDate = yStartDate;
    //        for (int q = 1; q <= 4; q++) {
    //            tempDate = qStartDate.AddMonths(2);
    //            var qEndDate = new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
    //            var quarter = createQuarterPeriod(year, $"Q{q} {year.PeriodName}", qStartDate, qEndDate, objectSpace);
    //            var pStartDate = qStartDate;
    //            for (int m = 1; m <= 3; m++) {
    //                var pEndDate = new DateTime(pStartDate.Year, pStartDate.Month, DateTime.DaysInMonth(pStartDate.Year, pStartDate.Month));
    //                createBasePeriod(quarter, $"{pStartDate.ToString("MMM")} {year.PeriodName}", pStartDate, pEndDate, objectSpace);
    //                pStartDate = pEndDate.AddDays(1);
    //            }
    //            qStartDate = qEndDate.AddDays(1);
    //        }
    //        try {
    //            objectSpace.CommitChanges();
    //            ObjectSpace.Refresh();
    //            MessageOptions options = new MessageOptions();
    //            options.Duration = 2000;
    //            options.Message = "task(s) have been successfully updated!";
    //            options.Type = InformationType.Success;
    //            options.Web.Position = InformationPosition.Top;
    //            options.Win.Caption = "Success";
    //            options.Win.Type = WinMessageType.Toast;
    //            Application.ShowViewStrategy.ShowMessage(options);
    //        }
    //        catch (Exception ex) {
    //            ObjectSpace.Rollback();
    //        }
    //    }
    //    YearPeriod createYearPeriod(string Name, DateTime StratDate, DateTime EndDate, IObjectSpace objectSpace) {
    //        var year = objectSpace.CreateObject<YearPeriod>();
    //        year.PeriodName = Name;
    //        year.StartDate = StratDate;
    //        year.EndDate = EndDate;
    //        return year;
    //    }
    //    QuarterPeriod createQuarterPeriod(YearPeriod Year, string Name, DateTime StratDate, DateTime EndDate, IObjectSpace objectSpace) {
    //        var quarter = objectSpace.CreateObject<QuarterPeriod>();
    //        quarter.YearPeriod = Year;
    //        quarter.PeriodName = Name;
    //        quarter.StartDate = StratDate;
    //        quarter.EndDate = EndDate;
    //        return quarter;
    //    }
    //    void createBasePeriod(QuarterPeriod Quarter, string Name, DateTime StratDate, DateTime EndDate, IObjectSpace objectSpace) {
    //        var period = objectSpace.CreateObject<BasePeriod>();
    //        period.QuarterPeriod = Quarter;
    //        period.PeriodName = Name;
    //        period.StartDate = StratDate;
    //        period.EndDate = EndDate;
    //    }
    //    private void Action_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
    //        createFullYearPeriod((OpenFullYearPeriods)e.PopupWindowView.CurrentObject);
    //    }
    //    private void Action_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
    //        var os = Application.CreateObjectSpace(typeof(OpenFullYearPeriods));
    //        var openFullYearPeriods = os.CreateObject<OpenFullYearPeriods>();
    //        e.View = Application.CreateDetailView(os, openFullYearPeriods);
    //        ((DetailView)e.View).ViewEditMode = ViewEditMode.Edit;
    //    }

    //}
}
