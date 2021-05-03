using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CostingApp.Module.BO.ItemTransactions;
using CostingApp.Module.BO.ItemTransactions.Abstraction;
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
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InventoryTransactionController : ViewController {
        public InventoryTransactionController() {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated() {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated() {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void actPost_Execute(object sender, SimpleActionExecuteEventArgs e) {
            try {
                if (CurrentObject != null)
                    CurrentObject.Status = BO.EnumStatus.Closed;
                ObjectSpace.CommitChanges();
            }
            catch(Exception ex) {
                ObjectSpace.Rollback();
            }
        }

        private void enableAction() {
            actPost.Enabled["ObjectSpaceIsModified"] = CurrentObject != null && !ObjectSpace.IsModified && CurrentObject.Status != BO.EnumStatus.Closed;
        }
        public InventoryTransaction CurrentObject {
            get { return (InventoryTransaction)View.CurrentObject; }
        }
    }
}
