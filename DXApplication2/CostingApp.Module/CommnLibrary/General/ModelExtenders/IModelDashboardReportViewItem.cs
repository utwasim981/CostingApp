using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.CommonLibrary.General.ModelExtenders {
    public interface IModelDashboardReportViewItem : IModelDashboardViewItem {
        string ReportName { get; set; }
        bool CreateDocumentOnLoad { get; set; }
        [Browsable(false)]
        new IModelView View { get; set; }
        [Browsable(false)]
        new string Criteria { get; set; }
        [Browsable(false)]
        [DefaultValue(ActionsToolbarVisibility.Hide)]
        new ActionsToolbarVisibility ActionsToolbarVisibility { get; set; }
        [DefaultValue(ViewItemVisibility.Show)]
        [Browsable(false)]
        ViewItemVisibility Visibility { get; set; }
        [Browsable(false)]
        MasterDetailMode? MasterDetailMode { get; set; }
    }
}
