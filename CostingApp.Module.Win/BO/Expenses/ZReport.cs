using CostingApp.Module.Win.BO;
using CostingApp.Module.Win.BO.Expenses;
using CostingApp.Module.Win.BO.Masters;
using CostingApp.Module.Win.BO.Masters.Period;
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
using WXafLib.General.Model;
using WXafLib.General.Security;

namespace CostingApp.Module.Win.BO.Expenses {
    [NavigationItem("Transactions")]
    [ImageName("BO_Sale_Item")]
    public class ZReport : WXafSequenceObject {
        public string Number {
            get {
                return string.Format("ZR-{0}", SequentialNumber.ToString().PadLeft(6, '0'));
            }
        }
        Shop fShop;
        [DataSourceCriteria("IsActive = True")]
        [RuleRequiredField("ZReport_Shop_RuleRequiredField", DefaultContexts.Save)]
        public Shop Shop {
            get { return fShop; }
            set {
                SetPropertyValue<Shop>(nameof(Shop), ref fShop, value);
                if (!IsLoading && Details.Count != 0)
                    foreach (var detail in Details)
                        detail.Shop = value;
            }
        }
        DateTime fReportDate;
        [RuleRequiredField("ZReport_ReportDate_RuleRequiredField", DefaultContexts.Save)]
        public DateTime ReportDate {
            get { return fReportDate; }
            set {
                SetPropertyValue<DateTime>(nameof(ReportDate), ref fReportDate, value);
                if (!IsLoading) {
                    Period = BasePeriod.GetOpenedPeriodForDate(ObjectSpace, ReportDate);
                }
            }
        }
        BasePeriod fPeriod;
        [ModelDefault("AllowEdit", "False")]
        public BasePeriod Period {
            get { return fPeriod; }
            set { SetPropertyValue<BasePeriod>(nameof(Period), ref fPeriod, value); }
        }
        double fSales;
        [RuleValueComparison("ZReport_Sales.GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public double Sales {
            get { return fSales; }
            set { SetPropertyValue<double>(nameof(Sales), ref fSales, value); }
        }
        [Association("ZReport-ZReportComponent)"), DevExpress.Xpo.Aggregated]
        public XPCollection<ZReportDetail> Details {
            get { return GetCollection<ZReportDetail>(nameof(Details)); }
        }
        public ZReport(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            ReportDate = DateTime.Now;
            var components = new XPCollection<ZReportComponent>(Session);
            foreach (var component in components) {
                if (component.IsActive) {
                    var detail = new ZReportDetail(Session);
                    detail.ZReport = this;
                    detail.Component = component;
                    detail.ExpenseType = component.ExpenseType;
                    Details.Add(detail);
                }
            }
        }
        protected override void OnSaved() {
            base.OnSaved();
            updateDetailsData();
        }
        private void updateDetailsData() {
            if (Details.Count != 0)
                foreach(var detail in Details) {
                    detail.ExpenseDate = ReportDate;
                    detail.Period = Period;
                }
        }
    }
    [RuleCombinationOfPropertiesIsUnique("ZReportDetail_ZReport_Component.RuleUniqueField", DefaultContexts.Save, "ZReport, Component")]
    public class ZReportDetail : ExpenseRecord {
        ZReport fZReport;
        [Association("ZReport-ZReportComponent)")]
        public ZReport ZReport {
            get { return fZReport; }
            set {
                SetPropertyValue<ZReport>(nameof(ZReport), ref fZReport, value);
                if (!IsLoading)
                    onZReportValueChange();
            }
        }
        ZReportComponent fComponent;
        [RuleRequiredField("ZReportDetail_Component_RuleRequiredField", DefaultContexts.Save)]
        public ZReportComponent Component {
            get { return fComponent; }
            set { SetPropertyValue<ZReportComponent>(nameof(Component), ref fComponent, value); }
        }
        double fReportAmount;
        public double ReportAmount {
            get { return fReportAmount; }
            set {
                SetPropertyValue<double>(nameof(ReportAmount), ref fReportAmount, value);
                if (!IsLoading && Component != null) {
                    Amount = Component.Calculation == EmumCalculationType.Value ? ReportAmount * Component.Value : Math.Round((ReportAmount * Component.Value) / 100, 2);
                }
            }
        }
        public ZReportDetail(Session session) : base(session) { }

        private void onZReportValueChange() {
            if (ZReport != null) {
                Shop = ZReport.Shop;
                ExpenseDate = ZReport.ReportDate;
                Period = ZReport.Period;
            }
        }
    }

    public class ZReportCheckExpenseTypeController : ViewController {
        public ZReportCheckExpenseTypeController() {
            TargetObjectType = typeof(ZReport);
        }
        protected override void OnActivated() {
            base.OnActivated();
            NewObjectViewController controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
                controller.NewObjectAction.Executing += NewObjectAction_Executing;
        }

        private void NewObjectAction_Executing(object sender, CancelEventArgs e) {
            if (ValueManager.GetValueManager<Dictionary<string, object>>("Values").Value["VatExpense"] == null) {
                MessageOptions options = new MessageOptions();
                options.Duration = 2000;
                options.Message = "VAT expense type not added to system setup";
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
