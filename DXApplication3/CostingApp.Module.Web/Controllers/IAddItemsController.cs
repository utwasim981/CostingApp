using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CostingApp.Module.BO.Items;
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
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;


namespace CostingApp.Module.Web.Controllers {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AdjustmentAddItemsController : ViewController<ListView> {
        public AdjustmentAddItemsController() {
            InitializeComponent();
            TargetObjectType = typeof(IAddItems);
        }
        private DevExpress.ExpressApp.ConditionalAppearance.AppearanceController appearanceController;
        protected override void OnActivated() {
            base.OnActivated();
            appearanceController = Frame.GetController<DevExpress.ExpressApp.ConditionalAppearance.AppearanceController>();
            if (appearanceController != null) {
                appearanceController.CustomApplyAppearance += appearanceController_CustomApplyAppearance;
            }
        }

        void appearanceController_CustomApplyAppearance(object sender, ApplyAppearanceEventArgs e) {
            if (View is ListView) {
                if (e.Item is ColumnWrapper) {
                    if (View.ObjectTypeInfo.Implements<IAddPurchaseItems>() && ((ColumnWrapper)e.Item).PropertyName == "SalesUnit")
                        e.AppearanceObject.Visibility = ViewItemVisibility.Hide;
                }
            }
        }

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            #region EditModeByClick
            if (View.Model.AllowEdit) {
                editor = View.Editor as ASPxGridListEditor;
                if (editor != null) {
                    editor.Grid.SettingsSearchPanel.Visible = true;
                    editor.Grid.Load += Grid_Load;
                    processRecordController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    processRecordController.CustomProcessSelectedItem += processRecordController_CustomProcessSelectedItem;
                }
            }
            #endregion
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (editor != null) {
                editor.Grid.Load -= Grid_Load;
                processRecordController.CustomProcessSelectedItem -= processRecordController_CustomProcessSelectedItem;
            }
        }
        #region EditModeByClick
        private ASPxGridListEditor editor;
        private ListViewProcessCurrentObjectController processRecordController;
        private void processRecordController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e) {
            e.Handled = true;
        }
        private void Grid_Load(object sender, EventArgs e) {
            editor.Grid.ClientSideEvents.RowClick = "function(s, e){{s.UnselectRowOnPage(e.visibleIndex);s.StartEditRow(e.visibleIndex);}}";
        }
        #endregion


        private void actClose_Execute(object sender, SimpleActionExecuteEventArgs e) {
            View.Close();
        }
    }
}
