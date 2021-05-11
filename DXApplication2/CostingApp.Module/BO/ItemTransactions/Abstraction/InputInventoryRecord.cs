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
            if (Transaction.TransactionType == EnumInventoryTransactionType.PurchaseInvoice)
                Item.UpdateLasPurchasePrice(Shop, TransactionUnit, Date, Price);

            if (Session.IsNewObject(this)) {
                Item.UpdateQuantityOnHand(RecordType, Shop, TransactionUnit, Quantity);
                return;
            }
            double quantityOldValue = 0;
            Unit unitOldValue = null;
            if (WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(Quantity)) &&
                WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(TransactionUnit))){
                quantityOldValue = Convert.ToDouble(WXafHelper.GetOldValue(ClassInfo, this, nameof(Quantity)));
                unitOldValue = (Unit)WXafHelper.GetOldValue(ClassInfo, this, nameof(TransactionUnit));
                Item.UpdateQuantityOnHand(NotRecordType, Shop, unitOldValue, quantityOldValue);
                Item.UpdateQuantityOnHand(RecordType, Shop, TransactionUnit, Quantity);
            }
            else if (WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(TransactionUnit))) {
                unitOldValue = (Unit)WXafHelper.GetOldValue(ClassInfo, this, nameof(TransactionUnit));
                Item.UpdateQuantityOnHand(NotRecordType, Shop, unitOldValue, Quantity);
                Item.UpdateQuantityOnHand(RecordType, Shop, TransactionUnit, Quantity);
            }
            else if (WXafHelper.IsProrpotyChanged(ClassInfo, this, nameof(Quantity))) {
                quantityOldValue = Convert.ToDouble(WXafHelper.GetOldValue(ClassInfo, this, nameof(Quantity)));
                Item.UpdateQuantityOnHand(NotRecordType, Shop, TransactionUnit, quantityOldValue);
                Item.UpdateQuantityOnHand(RecordType, Shop, TransactionUnit, Quantity);
            }            
        }
    }
}
