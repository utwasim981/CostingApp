namespace CostingApp.Module.Controllers {
    partial class InventoryTransactionController {
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
            this.actPost = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // actPost
            // 
            this.actPost.Caption = "Post";
            this.actPost.ConfirmationMessage = "After posting a Transaction you can\'t change it, do you want to continue?";
            this.actPost.Id = "InventoryTransaction.Post";
            this.actPost.ToolTip = null;
            this.actPost.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.actPost_Execute);
            // 
            // InventoryTransactionController
            // 
            this.Actions.Add(this.actPost);
            this.TargetObjectType = typeof(CostingApp.Module.BO.ItemTransactions.Abstraction.InventoryTransaction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction actPost;
    }
}
