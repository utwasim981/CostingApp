namespace CostingApp.Module.Controllers {
    partial class InventoryAdjustmentController {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.actApprove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // actApprove
            // 
            this.actApprove.Caption = "Approve";
            this.actApprove.ConfirmationMessage = "Do you want to approve the transaction?";
            this.actApprove.Id = "InventoryAdjustment.Approve";
            this.actApprove.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.actApprove.TargetObjectsCriteria = "Step == 0";
            this.actApprove.TargetObjectType = typeof(CostingApp.Module.BO.ItemTransactions.InventoryAdjustment);
            this.actApprove.ToolTip = null;
            this.actApprove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.actApprove_Execute);
            // 
            // InventoryAdjustmentController
            // 
            this.Actions.Add(this.actApprove);
            this.TargetObjectType = typeof(CostingApp.Module.BO.ItemTransactions.InventoryAdjustment);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction actApprove;
    }
}
