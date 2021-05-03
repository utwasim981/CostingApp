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
    public abstract class InputInventoryRecord : InventoryRecord {
        public InputInventoryRecord(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            RecordType = EnumInventoryRecordType.In;
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
                if (Transaction.TransactionType == EnumInventoryTransactionType.PurchaseInvoice)
                    Item.UpdateLasPurchasePrice(Shop, TransactionUnit, Date, Price);
                if (quantityOldValue != null)
                    if (unitOldValue != null) {
                        Item.UpdateQuantityOnHand(NotRecordType, Shop, (Unit)unitOldValue, Convert.ToDouble(quantityOldValue));
                        Item.UpdateQuantityOnHand(RecordType, Shop, TransactionUnit, Quantity);
                    }
                    else {
                        if (Quantity - Convert.ToDouble(quantityOldValue) < 0)
                            Item.UpdateQuantityOnHand(RecordType, Shop, TransactionUnit, Math.Abs((Quantity - Convert.ToDouble(quantityOldValue))));
                        else
                            Item.UpdateQuantityOnHand(NotRecordType, Shop, TransactionUnit, Math.Abs((Quantity - Convert.ToDouble(quantityOldValue))));
                    }
                else
                    Item.UpdateQuantityOnHand(RecordType, Shop, TransactionUnit, Quantity);
            }
        }
    }
}
