using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System.Collections;

namespace CostingApp.Module.BO.ItemTransactions.Abstraction {
    public class InventoryRecordAddItemsController : ViewController<ListView> {
        private PopupWindowShowAction actAddItems;
        private InventoryTransaction MasterObject = null;
        PropertyCollectionSource collectionSource = null;
        public InventoryRecordAddItemsController() {
            TargetObjectType = typeof(InventoryRecord);
            TargetViewNesting = Nesting.Nested;
            actAddItems = new PopupWindowShowAction();
            actAddItems.Id = "InventoryRecord.AddItems";
            actAddItems.Caption = "Add Items";
            Actions.Add(actAddItems);
            actAddItems.CustomizePopupWindowParams += ActAddItems_CustomizePopupWindowParams;
            actAddItems.Execute += actAddItems_Execute;
        }
        protected override void OnActivated() {
            base.OnActivated();
            var attribute = View.ObjectTypeInfo.FindAttribute<AddItemClassAttribute>();// TargetObjectType.GetCustomAttributes(typeof(AddItemClassAttribute), false);
            if (attribute != null) {
            ///if (TargetObjectType.GetInterfaces().Contains(typeof(IAddItems))){
                collectionSource = View.CollectionSource as PropertyCollectionSource;
                if (collectionSource != null) {
                    collectionSource.MasterObjectChanged += OnMasterObjectChanged;
                    if (collectionSource.MasterObject != null) {
                        UpdateMasterObject(collectionSource.MasterObject);
                        actAddItems.Enabled["Test"] = MasterObject.Shop != null;
                    }
                }
            }
        }
        protected override void OnDeactivated() {
            PropertyCollectionSource collectionSource = View.CollectionSource as PropertyCollectionSource;
            if (collectionSource != null)
                collectionSource.MasterObjectChanged -= OnMasterObjectChanged;
            if (MasterObject != null && MasterObject.ObjectSpace != null)
                MasterObject.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged1;
            base.OnDeactivated();
        }
        private void OnMasterObjectChanged(object sender, System.EventArgs e) {
            UpdateMasterObject(((PropertyCollectionSource)sender).MasterObject);
        }
        private void UpdateMasterObject(object masterObject) {
            MasterObject = (InventoryTransaction)masterObject;
            MasterObject.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged1;
        }
        private void ObjectSpace_ObjectChanged1(object sender, ObjectChangedEventArgs e) {
            actAddItems.Enabled["Test"] = MasterObject.Shop != null;
        }
        private void ActAddItems_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            ListView view = null;
            var attribute = View.ObjectTypeInfo.FindAttribute<AddItemClassAttribute>();
            if (attribute != null) {
                switch (attribute.TransactionType) {
                    case EnumInventoryTransactionType.PurchaseInvoice:
                        view = Application.CreateListView(typeof(AddPurchaseItems), true);
                        break;
                    case EnumInventoryTransactionType.SalesInvoice:
                        view = Application.CreateListView(typeof(AddSalesItems), true);
                        break;
                    case EnumInventoryTransactionType.InventoryAdjustment:
                        view = Application.CreateListView(typeof(AddInventoryItems), true);
                        break;
                    case EnumInventoryTransactionType.InventoryTransfer:
                        view = Application.CreateListView(typeof(AddInventoryItems), true);
                        break;
                }
            }
            e.View = view;
            e.DialogController.SaveOnAccept = true;
            e.DialogController.CancelAction.Active["NothingToCancel"] = false;
        }
        private void actAddItems_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
            IList items = (IList)((ListView)e.PopupWindow.View).CollectionSource.Collection;
            var attribute = View.ObjectTypeInfo.FindAttribute<AddItemClassAttribute>();
            if (attribute.TransactionType == EnumInventoryTransactionType.PurchaseInvoice) {
                foreach (var item in items) {
                    if (((AddPurchaseItems)item).Quantity != 0) {
                        var newObj = ObjectSpace.CreateObject<PurchaseInvoiceDetail>();
                        newObj.Item = ObjectSpace.GetObject(((AddPurchaseItems)item).Item);
                        newObj.TransactionUnit = ObjectSpace.GetObject(((AddPurchaseItems)item).Unit);
                        newObj.Quantity = ((AddPurchaseItems)item).Quantity;
                        ((PurchaseInvoice)MasterObject).Items.Add(newObj);
                    }
                }
            }
            else if (attribute.TransactionType == EnumInventoryTransactionType.SalesInvoice) {
                foreach (var item in items) {
                    if (((AddSalesItems)item).Quantity != 0) {
                        var newObj = ObjectSpace.CreateObject<SalesInvoiceDetail>();
                        newObj.Item = ObjectSpace.GetObject(((AddSalesItems)item).Item);
                        newObj.TransactionUnit = ObjectSpace.GetObject(((AddSalesItems)item).Unit);
                        newObj.Quantity = ((AddPurchaseItems)item).Quantity;
                        ((SalesInvoice)MasterObject).Items.Add(newObj);
                    }
                }
            }
            else if (attribute.TransactionType == EnumInventoryTransactionType.InventoryTransfer) {
                foreach (var item in items) {
                    if (((AddInventoryItems)item).Quantity != 0) {
                        var newObj = ObjectSpace.CreateObject<InventoryTransferSourceItem>();
                        newObj.Item = ObjectSpace.GetObject(((AddInventoryItems)item).Item);
                        newObj.TransactionUnit = ObjectSpace.GetObject(((AddInventoryItems)item).Unit);
                        newObj.Quantity = ((AddInventoryItems)item).Quantity;
                        ((InventoryTransfer)MasterObject).Items.Add(newObj);
                    }
                }
            }
            else if (attribute.TransactionType == EnumInventoryTransactionType.InventoryAdjustment) {
                foreach (var item in items) {
                    if (((AddInventoryItems)item).Quantity != 0) {
                        var newObj = ObjectSpace.CreateObject<InventoryAdjustmentItem>();
                        newObj.Item = ObjectSpace.GetObject(((AddInventoryItems)item).Item);
                        newObj.TransactionUnit = ObjectSpace.GetObject(((AddInventoryItems)item).Unit);
                        newObj.Quantity = ((AddInventoryItems)item).Quantity;
                        ((InventoryAdjustment)MasterObject).Items.Add(newObj);
                    }
                }
            }
        }
    }
}
