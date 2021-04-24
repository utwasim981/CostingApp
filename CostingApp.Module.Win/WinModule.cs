using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Persistent.BaseImpl;
using CostingApp.Module.Win.BO;
using DevExpress.Persistent.Base;
//using WXafLib.Module;

namespace CostingApp.Module.Win {
    [ToolboxItemFilter("Xaf.Platform.Win")]
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class CostingAppWindowsFormsModule : ModuleBase {      
        //private void Application_CreateCustomModelDifferenceStore(Object sender, CreateCustomModelDifferenceStoreEventArgs e) {
        //    e.Store = new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), true, "Win");
        //    e.Handled = true;
        //}
        private void Application_CreateCustomUserModelDifferenceStore(Object sender, CreateCustomModelDifferenceStoreEventArgs e) {
            e.Store = new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), false, "Win");
            e.Handled = true;
        }
        public CostingAppWindowsFormsModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            //application.CreateCustomModelDifferenceStore += Application_CreateCustomModelDifferenceStore;
            application.CreateCustomUserModelDifferenceStore += Application_CreateCustomUserModelDifferenceStore;
            // Manage various aspects of the application UI and behavior at the module level.            
            application.LoggedOn += Application_LoggedOn;
        }
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);            
        }

        private void Application_LoggedOn(object sender, LogonEventArgs e) {
            //var persistentTypes = XafTypesInfo.Instance.PersistentTypes.Where(x => x.IsPersistent);
            //foreach (var persistentType in persistentTypes) {
            //    IModelClassExtender model = Application.Model.BOModel.GetClass(Type.GetType(persistentType.FullName)) as IModelClassExtender;
            //    if (model != null && model.IsSystemConfigration) {
            //        AddSystemConfigurationToValueManager(Application, persistentType);
            //    }
            //}
        }
        void AddSystemConfigurationToValueManager(XafApplication app, ITypeInfo Type) {

        }
        //private void AddValuesToValueManager(SystemConfigration systemConfig) {
        //    var t = XafTypesInfo.Instance.FindTypeInfo(Type.GetType("CostingApp.Module.Win.BO.SystemConfigration"));// typeof(SystemConfigration));
        //    var m = t.Members.Where(x=> x.IsProperty);
        //    //Dictionary<string, >
        //}
    }
}
