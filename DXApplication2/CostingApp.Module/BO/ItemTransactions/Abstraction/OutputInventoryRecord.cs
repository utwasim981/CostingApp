using CostingApp.Module.BO.Items;
using CostingApp.Module.CommonLibrary;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.BO.ItemTransactions.Abstraction {
    [NonPersistent]
    public abstract class OutputInventoryRecord : InventoryRecord {
        public OutputInventoryRecord(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            RecordType = EnumInventoryRecordType.Out;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(TransactionUnit) && oldValue != newValue)
                    onTranscationUnitChange();
            }
        }
        protected override void OnDeletingRecord() {
            base.OnDeletingRecord();
            Item.UpdateQuantityOnHand(NotRecordType, Shop, TransactionUnit, Quantity);
        }
        protected override void OnSavingRecord() {
            base.OnSavingRecord();
            object quantityOldValue = null;
            WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(Quantity), out quantityOldValue);
            object unitOldValue = null;
            WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(TransactionUnit), out unitOldValue);
            if (quantityOldValue != null || unitOldValue != null) {
                if (quantityOldValue != null)
                    if (unitOldValue != null) {
                        Item.UpdateQuantityOnHand(NotRecordType, Shop, (Unit)unitOldValue, Convert.ToDouble(quantityOldValue));
                        Item.UpdateQuantityOnHand(RecordType, Shop, TransactionUnit, Quantity);
                    }
                    else {
                        if (Quantity - Convert.ToDouble(quantityOldValue) > 0)
                            Item.UpdateQuantityOnHand(RecordType, Shop, TransactionUnit, Math.Abs((Quantity - Convert.ToDouble(quantityOldValue))));
                        else
                            Item.UpdateQuantityOnHand(NotRecordType, Shop, TransactionUnit, Math.Abs((Quantity - Convert.ToDouble(quantityOldValue))));
                    }
                else
                    Item.UpdateQuantityOnHand(RecordType, Shop, TransactionUnit, Quantity);
            }
        }

        private void onTranscationUnitChange() {
            Quantity = Math.Round(BaseQuantity / TransactionUnit.ConversionRate, 3);
            Price = Math.Round(BasePrice * TransactionUnit.ConversionRate, 3);
        }
    }
}
