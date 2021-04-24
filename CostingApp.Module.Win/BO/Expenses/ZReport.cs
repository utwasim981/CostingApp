using CostingApp.Module.Win.BO.Masters;
using CostingApp.Module.Win.BO.Masters.Period;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib.General.Model;

namespace CostingApp.Module.Win.BO.Expenses {
    [NavigationItem("Setup")]
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
                    if (Details.Count != 0)
                        foreach (var detail in Details)
                            detail.ExpenseDate = value;
                    GetPeriod();
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
        [Association("ZReport-ZReportComponent)"), Aggregated]
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
        private void GetPeriod() {
            Period = Session.FindObject<BasePeriod>(CriteriaOperator.Parse("StartDate <= ? And EndDate >= ?", ReportDate, ReportDate));
        }
    }
    [RuleCombinationOfPropertiesIsUnique("ZReportDetail_ZReport_Component.RuleUniqueField", DefaultContexts.Save, "ZReport, Component")]
    public class ZReportDetail : ExpenseRecord {
        ZReport fZReport;
        [Association("ZReport-ZReportComponent)")]
        public ZReport ZReport {
            get { return fZReport; }
            set { SetPropertyValue<ZReport>(nameof(ZReport), ref fZReport, value); }
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
                    Amount = Component.Calculation == EmumCalculationType.Value ? ReportAmount * Component.Value : (ReportAmount * Component.Value) / 100;
                }
            }
        }

        public ZReportDetail(Session session) : base(session) { }
    }
}
