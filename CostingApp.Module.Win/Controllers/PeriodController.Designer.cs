namespace CostingApp.Module.Win.Controllers {
    partial class PeriodController {
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
            this.actFullPeriod = new DevExpress.ExpressApp.Actions.PopupWindowShowAction();
            // 
            // actFullPeriod
            // 
            this.actFullPeriod.AcceptButtonCaption = null;
            this.actFullPeriod.CancelButtonCaption = null;
            this.actFullPeriod.Caption = "Setup Full Year";
            this.actFullPeriod.ConfirmationMessage = "Do you want setup a full year periods?";
            this.actFullPeriod.Id = "Period.OpenFullYearPeriod";
            this.actFullPeriod.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.actFullPeriod.ToolTip = null;
            this.actFullPeriod.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.actFullPeriod.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.actFullPeriod_CustomizePopupWindowParams);
            this.actFullPeriod.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.actFullPeriod_Execute);
            // 
            // PeriodController
            // 
            this.Actions.Add(this.actFullPeriod);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction actFullPeriod;
    }
}
