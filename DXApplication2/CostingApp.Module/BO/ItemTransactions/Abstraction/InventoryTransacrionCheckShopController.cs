using CostingApp.Module.BO.Masters;
using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;

namespace CostingApp.Module.BO.ItemTransactions.Abstraction {
    public class InventoryTransacrionCheckShopController : ViewController {
        public InventoryTransacrionCheckShopController() {
            TargetObjectType = typeof(InventoryTransaction);
            TargetViewType = ViewType.DetailView;
        }
        ListPropertyEditor itemListPropertyEditor;
        NewObjectViewController newController;
        protected override void OnActivated() {
            base.OnActivated();
            foreach (var member in ((InventoryTransaction)View.CurrentObject).ClassInfo.Members) {
                if (member.IsCollection &&
                    (member.CollectionElementType.BaseClass.ClassType == typeof(InputInventoryRecord) ||
                     member.CollectionElementType.BaseClass.ClassType == typeof(OutputInventoryRecord))) {
                    itemListPropertyEditor = ((DetailView)View).FindItem(member.Name) as ListPropertyEditor;
                    if (itemListPropertyEditor != null)
                        itemListPropertyEditor.ControlCreated += ItemListPropertyEditor_ControlCreated;
                }
            }
        }
        protected override void OnDeactivated() {
            if (newController != null)
                newController.ObjectCreating -= Controller_ObjectCreating;
            if (itemListPropertyEditor != null)
                itemListPropertyEditor.ControlCreated -= ItemListPropertyEditor_ControlCreated;
            base.OnDeactivated();
        }
        private void ItemListPropertyEditor_ControlCreated(object sender, EventArgs e) {
            ListPropertyEditor itemListPropertyEditor = (ListPropertyEditor)sender;
            Frame listViewFrame = itemListPropertyEditor.Frame;
            ListView nestedListView = itemListPropertyEditor.ListView;
            newController = listViewFrame.GetController<NewObjectViewController>();
            newController.ObjectCreating += Controller_ObjectCreating;
        }
        private void Controller_ObjectCreating(object sender, ObjectCreatingEventArgs e) {
            foreach (var member in ((InventoryTransaction)View.CurrentObject).ClassInfo.Members) {
                if (member.MemberType == typeof(Shop) && member.GetValue(View.CurrentObject) == null) {
                    e.Cancel = true;
                    MessageOptions options = new MessageOptions();
                    options.Duration = 4000;
                    options.Message = "Please cheose the shop and date before adding any item";
                    options.Type = InformationType.Warning;
                    options.Web.Position = InformationPosition.Top;
                    options.Win.Caption = "Warrning";
                    options.Win.Type = WinMessageType.Flyout;
                    Application.ShowViewStrategy.ShowMessage(options);
                    break;
                }
            }


                //if (((InventoryTransaction)View.CurrentObject).Shop == null || ((InventoryTransaction)View.CurrentObject).Period == null ||
                //(View.CurrentObject.GetType().GetProperty("Destination") != null && View.CurrentObject.GetType().GetProperty("Destination").GetValue(View.CurrentObject) == null)) {
                
        }
    }
}
