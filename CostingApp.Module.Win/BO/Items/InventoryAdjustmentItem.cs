using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using WXafLib;

namespace CostingApp.Module.Win.BO.Items {
    [ImageName("")]
    public class InventoryAdjustmentItem : InventoryRecord {
        InventoryAdjustment fInventoryAdjustment;
        [Association("InventoryAdjustment-InventoryAdjustmentItem")]
        public InventoryAdjustment InventoryAdjustment {
            get { return fInventoryAdjustment; }
            set { SetPropertyValue<InventoryAdjustment>(nameof(InventoryAdjustment), ref fInventoryAdjustment, value); }
        }
        double fQuantityOnHand;
        [ModelDefault("AllowEdit", "False")]
        public double QuantityOnHand {
            get { return fQuantityOnHand; }
            set { SetPropertyValue<double>(nameof(QuantityOnHand), ref fQuantityOnHand, value); }
        }
        double fActualQuantity;
        public double ActualQuantity {
            get { return fActualQuantity; }
            set { SetPropertyValue<double>(nameof(ActualQuantity), ref fActualQuantity, value); }
        }

        public InventoryAdjustmentItem(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(InventoryAdjustment) && oldValue != newValue)
                    onInventoryAdjustmentValueChange();
                if (propertyName == nameof(Item) && oldValue != newValue)
                    onItemValueChange();
                if (propertyName == nameof(ActualQuantity) && oldValue != newValue)
                    onActualQuantityValueChange();
            }
        }

        private void onInventoryAdjustmentValueChange() {
            if (InventoryAdjustment != null) {
                Shop = InventoryAdjustment.Shop;
                Transaction = InventoryAdjustment;
                ExpenseDate = InventoryAdjustment.EffectedDate;
                Period = InventoryAdjustment.Period;
            }
        }
        private void onActualQuantityValueChange() {
            if (ActualQuantity > QuantityOnHand) {
                TransactionType = EnumInventoryTransactionType.In;
                Quantity = ActualQuantity - QuantityOnHand;
            }
            else if (ActualQuantity < QuantityOnHand) {
                TransactionType = EnumInventoryTransactionType.Out;
                Quantity = QuantityOnHand - ActualQuantity;
            }
        }
        private void onItemValueChange() {
            if (Item != null) {
                TransactionUnit = Item.StockUnit;
                QuantityOnHand = Item.GetQuantityOnHand(Shop);
            }
        }

        public void UpdateItemCard() {
            object quantityOldValue = null;
            if (WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(Quantity), out quantityOldValue))
                if (TransactionType == EnumInventoryTransactionType.In)
                    Item.UpdateQuantityOnHand(Shop, TransactionUnit, (Quantity - Convert.ToDouble(quantityOldValue)));
                else
                    Item.UpdateQuantityOnHand(Shop, TransactionUnit, (Quantity - Convert.ToDouble(quantityOldValue)) * -1);
            else {
                if (TransactionType == EnumInventoryTransactionType.In)
                    Item.UpdateQuantityOnHand(Shop, TransactionUnit, Quantity);
                else
                    Item.UpdateQuantityOnHand(Shop, TransactionUnit, Quantity * -1);
            }
        }
    }
}
