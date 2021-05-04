using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CostingApp.Module.BO;
using CostingApp.Module.BO.Items;
using CostingApp.Module.BO.ItemTransactions;
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
    public partial class InventoryAdjustmentController : ViewController {
        public InventoryAdjustmentController() {
            InitializeComponent();
            TargetObjectType = typeof(InventoryAdjustment);
        }
        protected override void OnActivated() {
            base.OnActivated();
            ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
            View.CurrentObjectChanged += View_CurrentObjectChanged;
        }

        private void View_CurrentObjectChanged(object sender, EventArgs e) {
            enableAction();
        }

        private void ObjectSpace_ModifiedChanged(object sender, EventArgs e) {
            enableAction();
        }
        protected override void OnDeactivated() {
            ObjectSpace.ModifiedChanged -= ObjectSpace_ModifiedChanged;
            base.OnDeactivated();
        }

        private void enableAction() {
            actApprove.Enabled["ObjectSpaceIsModified"] = !ObjectSpace.IsModified && CurrentObject != null && CurrentObject.Step != EnumInventorySteps.Approced && CurrentObject.Status != EnumStatus.Closed;
        }

        InventoryAdjustment CurrentObject {
            get { return (InventoryAdjustment)View.CurrentObject; }
        }

        private void actApprove_Execute(object sender, SimpleActionExecuteEventArgs e) {
            try {
                CurrentObject.ApproveTransaction();               
                ObjectSpace.CommitChanges();
            }
            catch(Exception ex) {
                ObjectSpace.Rollback();
            }
        }        
    }
}
