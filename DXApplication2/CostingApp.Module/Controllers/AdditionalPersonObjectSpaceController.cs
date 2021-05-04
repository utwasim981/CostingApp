using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CostingApp.Module.BO.Items;
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

namespace CostingApp.Module.Controllers {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class AdditionalPersonObjectSpaceController : WindowController {
        public AdditionalPersonObjectSpaceController()
            : base() {
            TargetWindowType = WindowType.Main;
        }
        private IObjectSpace additionalObjectSpace;
        protected override void OnActivated() {
            base.OnActivated();
            Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
            additionalObjectSpace = Application.CreateObjectSpace(typeof(Unit));
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
            Application.ObjectSpaceCreated -= Application_ObjectSpaceCreated;
            if (additionalObjectSpace != null) {
                additionalObjectSpace.Dispose();
                additionalObjectSpace = null;
            }
        }
        private void Application_ObjectSpaceCreated(Object sender, ObjectSpaceCreatedEventArgs e) {
            if (e.ObjectSpace is NonPersistentObjectSpace) {
                ((NonPersistentObjectSpace)e.ObjectSpace).AdditionalObjectSpaces.Add(additionalObjectSpace);
            }
        }
    }
}
