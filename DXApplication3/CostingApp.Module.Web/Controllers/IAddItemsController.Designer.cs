namespace CostingApp.Module.Web.Controllers {
    partial class AdjustmentAddItemsController {
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
            this.actClose = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // actClose
            // 
            this.actClose.Caption = "Cancel";
            this.actClose.ConfirmationMessage = null;
            this.actClose.Id = "AdjustmentAddItems.Close";
            this.actClose.ToolTip = null;
            this.actClose.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.actClose_Execute);
            // 
            // AdjustmentAddItemsController
            // 
            this.Actions.Add(this.actClose);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction actClose;
    }
}
