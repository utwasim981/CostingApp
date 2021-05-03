using CostingApp.Module.BO.ItemTransactions.Abstraction;
using DevExpress.Xpo;

namespace CostingApp.Module.BO.ItemTransactions {
    public class InventoryTransferDestinationItem : InputInventoryRecord {
        InventoryTransfer fInventoryTransfer;
        [Association("InventoryTransfer-InventoryTransferDestinationItem")]
        public InventoryTransfer InventoryTransfer {
            get { return fInventoryTransfer; }
            set { SetPropertyValue<InventoryTransfer>(nameof(InventoryTransfer), ref fInventoryTransfer, value); }
        }
        public InventoryTransferDestinationItem(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(InventoryTransfer) && oldValue != newValue)
                    Transaction = InventoryTransfer;
                // TODO Inventory Transfer
                if (propertyName == nameof(InventoryTransfer.Destination) && oldValue != newValue)
                    Shop = InventoryTransfer.Shop;
            }
        }
    }
}
