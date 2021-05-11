using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.CommnLibrary.General.ModelExtenders {
    public interface IModelNavigationItemReport {
        [Category("WXaf"), ]
        string ReportName { get; set; }
    }

    public class ShowReportFromNavigation : ShowNavigationItemController, IModelExtender {
        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            extenders.Add<IModelNavigationItem, IModelNavigationItemReport>();
        }

        protected override void ShowNavigationItem(SingleChoiceActionExecuteEventArgs e) {
            base.ShowNavigationItem(e);
            if (e.SelectedChoiceActionItem != null) {
                var model = e.SelectedChoiceActionItem.Model as IModelNavigationItemReport;
                if (model != null && !string.IsNullOrEmpty(model.ReportName)) {
                    IObjectSpace objectSpace = ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
                    IReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(CriteriaOperator.Parse("DisplayName = ?", model.ReportName));
                    string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
                    ReportServiceController controller = Frame.GetController<ReportServiceController>();
                    if (controller != null)
                        controller.ShowPreview(handle);
                }
            }
        }
    }
}
