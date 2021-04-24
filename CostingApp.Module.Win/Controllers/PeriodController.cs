using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CostingApp.Module.Win.BO.Masters.Period;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace CostingApp.Module.Win.Controllers {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PeriodController : ViewController {
        public PeriodController() {
            InitializeComponent();
            TargetObjectType = typeof(Period);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        void createFullYearPeriod(OpenFullYearPeriods OpenFullYearPeriods) {
            var yStartDate = new DateTime(OpenFullYearPeriods.YearEnd, (int)OpenFullYearPeriods.StartingMonth, 1);
            var tempDate = yStartDate.AddMonths(11);
            var yEndDate = new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
            var objectSpace = Application.CreateObjectSpace();
            var year = createYearPeriod(OpenFullYearPeriods.YearEnd.ToString(), yStartDate, yEndDate, objectSpace);
            var qStartDate = yStartDate;
            for (int q = 1; q <= 4; q++) {
                tempDate = qStartDate.AddMonths(2);
                var qEndDate = new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
                var quarter = createQuarterPeriod(year, $"Q{q} {year.PeriodName}", qStartDate, qEndDate, objectSpace);
                var pStartDate = qStartDate;
                for (int m = 1; m <= 3; m++) {
                    var pEndDate = new DateTime(pStartDate.Year, pStartDate.Month, DateTime.DaysInMonth(pStartDate.Year, pStartDate.Month));
                    createBasePeriod(quarter, $"{pStartDate.ToString("MMM")} {year.PeriodName}", pStartDate, pEndDate, objectSpace);
                    pStartDate = pEndDate.AddDays(1);
                }
                qStartDate = qEndDate.AddDays(1);
            }
            try {
                objectSpace.CommitChanges();
                ObjectSpace.Refresh();
                MessageOptions options = new MessageOptions();
                options.Duration = 2000;
                options.Message = "task(s) have been successfully updated!";
                options.Type = InformationType.Success;
                options.Web.Position = InformationPosition.Top;
                options.Win.Caption = "Success";
                options.Win.Type = WinMessageType.Toast;
                Application.ShowViewStrategy.ShowMessage(options);
            }
            catch (Exception ex) {
                ObjectSpace.Rollback();
            }
        }
        YearPeriod createYearPeriod(string Name, DateTime StratDate, DateTime EndDate, IObjectSpace objectSpace) {
            var year = objectSpace.CreateObject<YearPeriod>();
            year.PeriodName = Name;
            year.StartDate = StratDate;
            year.EndDate = EndDate;
            return year;
        }
        QuarterPeriod createQuarterPeriod(YearPeriod Year, string Name, DateTime StratDate, DateTime EndDate, IObjectSpace objectSpace) {
            var quarter = objectSpace.CreateObject<QuarterPeriod>();
            quarter.YearPeriod = Year;
            quarter.PeriodName = Name;
            quarter.StartDate = StratDate;
            quarter.EndDate = EndDate;
            return quarter;
        }
        void createBasePeriod(QuarterPeriod Quarter, string Name, DateTime StratDate, DateTime EndDate, IObjectSpace objectSpace) {
            var period = objectSpace.CreateObject<BasePeriod>();
            period.QuarterPeriod = Quarter;
            period.PeriodName = Name;
            period.StartDate = StratDate;
            period.EndDate = EndDate;
        }

        private void actFullPeriod_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            var os = Application.CreateObjectSpace(typeof(OpenFullYearPeriods));
            var openFullYearPeriods = os.CreateObject<OpenFullYearPeriods>();
            e.View = Application.CreateDetailView(os, openFullYearPeriods);
            ((DetailView)e.View).ViewEditMode = ViewEditMode.Edit;
        }

        private void actFullPeriod_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
            createFullYearPeriod((OpenFullYearPeriods)e.PopupWindowView.CurrentObject);
        }
    }
}
