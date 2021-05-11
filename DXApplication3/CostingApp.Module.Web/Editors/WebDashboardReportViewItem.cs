using CostingApp.Module.CommonLibrary.General.ModelExtenders;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.Web.Editors {
    [ViewItem(typeof(IModelDashboardReportViewItem))]
    public class WebDashboardReportViewItem : DashboardViewItem, IComplexViewItem {
        XtraReport report;
        readonly IModelDashboardReportViewItem _model;
        private IReportDataV2 _reportData;
        private ReportsModuleV2 _reportsModuleV2;
        XafApplication _application;

        public WebDashboardReportViewItem(IModelDashboardReportViewItem model, Type objectType)
            : base(model, objectType) {
            _model = model;
        }
        public ASPxWebDocumentViewer ReportViewer {
            get { return Control as ASPxWebDocumentViewer; }
        }
        public new IModelDashboardReportViewItem Model {
            get { return _model; }
        }
        public IReportDataV2 ReportData {
            get { return _reportData; }
        }
        public XtraReport Report {
            get { return report; }
        }

        protected override object CreateControlCore() {
            _reportsModuleV2 = ReportsModuleV2.FindReportsModule(_application.Modules);
            var reportDataType = _reportsModuleV2.ReportDataType;
            _reportData = (IReportDataV2)View.ObjectSpace.FindObject(reportDataType, CriteriaOperator.Parse("DisplayName=?", Model.ReportName));
            string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(_reportData);

            if (_reportData == null)
                throw new NullReferenceException(string.Format("Report {0} not found", Model.ReportName));
            report = ReportDataProvider.ReportsStorage.LoadReport(_reportData);
            report.DataSourceDemanded += report_DataSourceDemanded;
            ASPxWebDocumentViewer viewer = CreateASPxWebDocumentViewer();
            viewer.OpenReport(report);
            return viewer;
        }
        void report_DataSourceDemanded(object sender, EventArgs e) {
            ((XtraReport)sender).DataSourceDemanded -= report_DataSourceDemanded;
            InitReportParameterObject(sender);
        }
        private void InitReportParameterObject(object sender) {
            if (_reportData.ParametersObjectType != null) {
                var param = ((XtraReport)sender).Parameters[ReportDataSourceHelper.XafReportParametersObjectName];
                //if (param != null && param.Value == null) {
                //    param.Value = parametersObject;
                //}
            }
        }
        private ASPxWebDocumentViewer CreateASPxWebDocumentViewer() {
            ASPxWebDocumentViewer viewer = new ASPxWebDocumentViewer();
            viewer.ClientInstanceName = "xafReportViewer";
            //if (PopupMode) {
            //    viewer.Height = PopupWindow.GetWindowHeight();
            //}
            viewer.ClientSideEvents.Init = "viewerNewInit";
            ApplyCurrentTheme(viewer);
            return viewer;
        }
        protected virtual void ApplyCurrentTheme(ASPxWebDocumentViewer viewer) {
            if (BaseXafPage.CurrentTheme != null && BaseXafPage.CurrentTheme.ToLower().Contains("black")) {
                viewer.ColorScheme = "dark";
            }
        }
        #region Implementation of IComplexViewItem
        void IComplexViewItem.Setup(IObjectSpace objectSpace, XafApplication application) {
            _application = application;
            base.Setup(objectSpace, application);
        }
        #endregion
    }
}
