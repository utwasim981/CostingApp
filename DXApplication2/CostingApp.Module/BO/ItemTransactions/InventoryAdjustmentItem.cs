using CostingApp.Module.CommonLibrary;
using CostingApp.Module.BO.Masters;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using DevExpress.ExpressApp.DC;
using CostingApp.Module.BO.Items;
using CostingApp.Module.BO.ItemTransactions.Abstraction;

namespace CostingApp.Module.BO.ItemTransactions {
    [AddItemClass(EnumInventoryTransactionType.InventoryAdjustment)]
    public class InventoryAdjustmentItem : InputInventoryRecord, IAddItemList {
        InventoryAdjustment fInventoryAdjustment;
        [Association("InventoryAdjustment-InventoryAdjustmentItem")]
        public InventoryAdjustment InventoryAdjustment {
            get { return fInventoryAdjustment; }
            set { SetPropertyValue<InventoryAdjustment>(nameof(InventoryAdjustment), ref fInventoryAdjustment, value); }
        }
        double fQuantityOnHand;
        [ModelDefault("AllowEdit", "False"),
            ImmediatePostData(true),
            XafDisplayName("Onhand")]
        public double QuantityOnHand {
            get { return fQuantityOnHand; }
            set { SetPropertyValue<double>(nameof(QuantityOnHand), ref fQuantityOnHand, value); }
        }
        double fActualQuantity;
        [ImmediatePostData(true),
            XafDisplayName("Actual")]
        public double ActualQuantity {
            get { return fActualQuantity; }
            set { SetPropertyValue<double>(nameof(ActualQuantity), ref fActualQuantity, value); }
        }
        public InventoryAdjustmentItem(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(InventoryAdjustment) && oldValue != newValue) {
                    Transaction = InventoryAdjustment;
                }
                if (propertyName == nameof(Item) && oldValue != newValue)
                    onItemValueChange();
                if (propertyName == nameof(TransactionUnit) && oldValue != newValue)
                    if (Session.IsNewObject(this))
                        QuantityOnHand = Item.GetQuantityOnHand(Shop, (Unit)newValue);
                    else
                        QuantityOnHand = Math.Round((QuantityOnHand * ((Unit)oldValue).ConversionRate) / ((Unit)newValue).ConversionRate, 3);
                if (propertyName == nameof(ActualQuantity) && oldValue != newValue)
                    onActualQuantityValueChange();
            }
        }
        protected override void OnSavingRecord() {
            
        }
        protected override void OnDeletingRecord() {            
        }
        private void onActualQuantityValueChange() {
            if (ActualQuantity > QuantityOnHand) {
                RecordType = EnumInventoryRecordType.In;
                Quantity = ActualQuantity - QuantityOnHand;
            }
            else if (ActualQuantity < QuantityOnHand) {
                RecordType = EnumInventoryRecordType.Out;
                Quantity = QuantityOnHand - ActualQuantity;
            }
        }
        private void onItemValueChange() {
            if (Item != null) {
                QuantityOnHand = Item.GetQuantityOnHand(Shop, TransactionUnit);
            }
        }
        public void ApproveItem() {
            base.OnSavingRecord();
        }

    }
}
