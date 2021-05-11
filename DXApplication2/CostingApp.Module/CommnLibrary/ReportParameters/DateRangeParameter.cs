using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentDateTime;
using FluentDate;

namespace CostingApp.Module.CommnLibrary.ReportParameters {
    [DomainComponent]
    public class DateRangeParameterObject : ReportParametersObjectBase, INotifyPropertyChanged {
        public DateRangeParameterObject(IObjectSpaceCreator objectSpaceCreator) : base(objectSpaceCreator) {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Option = EnumDateRange.ThisMonth;
        }
        private void OnPropertyChanged(String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private EnumDateRange fOption;
        [ImmediatePostData(true)]
        public EnumDateRange Option {
            get { return fOption; }
            set {
                if (fOption != value) {
                    fOption = value;
                    OnPropertyChanged(nameof(Option));
                }
            }
        }

        //[Appearance("DateRangeParameterObject_StartDate", Visibility = ViewItemVisibility.Hide , Criteria = "Option != 8")]
        public DateTime StartDate { get; set; }
        //[Appearance("DateRangeParameterObject_EndDate", Visibility = ViewItemVisibility.Hide, Criteria = "Option != 8")]
        public DateTime EndDate { get; set; }

        public override CriteriaOperator GetCriteria() {
            switch (Option) {
                case EnumDateRange.Yesterday:
                    StartDate = DateTime.Now.AddDays(-1);
                    EndDate = DateTime.Now.AddDays(-1);
                    break;
                case EnumDateRange.Today:
                    StartDate = DateTime.Now;
                    EndDate = DateTime.Now;
                    break;
                case EnumDateRange.LastWeek:
                    StartDate = 1.Weeks().Ago().Previous(DayOfWeek.Monday);
                    EndDate = DateTime.Now.Previous(DayOfWeek.Sunday);
                    break;
                case EnumDateRange.ThisWeek:
                    StartDate = DateTime.Now.Previous(DayOfWeek.Monday);
                    EndDate = DateTime.Now.Next(DayOfWeek.Sunday);
                    break;
                case EnumDateRange.LastMonth:
                    StartDate = DateTime.Now.PreviousMonth().SetDay(1);
                    EndDate = DateTime.Now.PreviousMonth().EndOfMonth();
                    break;
                case EnumDateRange.ThisMonth:
                    StartDate = DateTime.Now.SetDay(1);
                    EndDate = DateTime.Now.EndOfMonth();
                    break;
                case EnumDateRange.LastYear:
                    StartDate = DateTime.Now.PreviousYear().SetMonth(1).SetDay(1);
                    EndDate = DateTime.Now.PreviousYear().EndOfYear();
                    break;
                case EnumDateRange.ThisYear:
                    StartDate = DateTime.Now.SetMonth(1).SetDay(1);
                    EndDate = DateTime.Now.EndOfYear();
                    break;
            }
            return CriteriaOperator.And(new BinaryOperator("Date", StartDate.Date + new TimeSpan(0,0,0), BinaryOperatorType.GreaterOrEqual),
                    new BinaryOperator("Date", EndDate.Date + new TimeSpan(23,59,59), BinaryOperatorType.LessOrEqual));
        }

        public override SortProperty[] GetSorting() {
            return null;
        }

        protected override IObjectSpace CreateObjectSpace() {
            return objectSpaceCreator.CreateObjectSpace(null);
        }

        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

    public enum EnumDateRange {
        Yesterday,
        Today,
        LastWeek,
        ThisWeek,
        LastMonth,
        ThisMonth,
        LastYear,
        ThisYear,
        Custom
    }
}
