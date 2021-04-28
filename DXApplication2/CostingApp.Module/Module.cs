using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;
using CostingApp.Module.CommonLibrary.General.CriteriaOperators;
using CostingApp.Module.CommonLibrary.General.Security;
using CostingApp.Module.CommonLibrary.General;
using CostingApp.Module.BO.Expenses;

namespace CostingApp.Module {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class CostingAppModule : ModuleBase {
        public CostingAppModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
            CriteriaOperatorRegistration.Register();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            ExecuteActionPermissionRequest.Register(Application);
            application.LoggedOn += Application_LoggedOn;
        }
        private void Application_LoggedOn(object sender, LogonEventArgs e) {
            SystemConfigrationHelper.LoggedOnHandeler(Application);
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
            IValueManager<string> valueManager = ValueManager.GetValueManager<string>("FullName");
            if (valueManager.CanManageValue)
                valueManager.Value = "concat(FirstName, ' ', LastName)";
        }
    }
}
