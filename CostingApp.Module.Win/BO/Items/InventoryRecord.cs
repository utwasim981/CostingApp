using CostingApp.Module.Win.BO.Expenses;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib.General.ModelExtenders;

namespace CostingApp.Module.Win.BO.Items {
    [NavigationItem("Testing")]
    public abstract class InventoryRecord : ExpenseRecord {
        InventoryTransaction fTransaction;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public InventoryTransaction Transaction {
            get { return fTransaction; }
            set { SetPropertyValue<InventoryTransaction>(nameof(Transaction), ref fTransaction, value); }
        }
        Unit fTransactionUnit;
        [RuleRequiredField("InventoryRecord_TransactionUnit_RuleRequiredField", DefaultContexts.Save)]
        public Unit TransactionUnit {
            get { return fTransactionUnit; }
            set { SetPropertyValue<Unit>(nameof(TransactionUnit), ref fTransactionUnit, value); }
        }
        Unit fBaseUnit;
        [ModelDefault("AllowEdit", "False")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public Unit BaseUnit {
            get { return fBaseUnit; }
            set { SetPropertyValue<Unit>(nameof(BaseUnit), ref fBaseUnit, value); }
        }
        Unit fStockUnit;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [ModelDefault("AllowEdit", "False")]
        public Unit StockUnit {
            get { return fStockUnit; }
            set { SetPropertyValue<Unit>(nameof(StockUnit), ref fStockUnit, value); }
        }
        double fQuantity;
        [ImmediatePostData(true)]
        public double Quantity {
            get { return fQuantity; }
            set { SetPropertyValue<double>(nameof(Quantity), ref fQuantity, value); }
        }
        public double QuantityIN {
            get { return Transaction.TransactionType == EnumInventoryTransactionType.In ? Quantity : 0; }
        }
        public double QuantityOut {
            get { return Transaction.TransactionType == EnumInventoryTransactionType.Out ? Quantity : 0; }
        }
        double fBaseQuantity;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public double BaseQuantity {
            get { return fBaseQuantity; }
            set { SetPropertyValue<double>(nameof(BaseQuantity), ref fBaseQuantity, value); }
        }
        double fStockQuantity;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public double StockQuantity {
            get { return fStockQuantity; }
            set { SetPropertyValue<double>(nameof(StockQuantity), ref fStockQuantity, value); }
        }
        double fPrice;
        //[RuleValueComparison("InventoryRecord_Price_GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        [ImmediatePostData(true)]
        public double Price {
            get { return fPrice; }
            set { SetPropertyValue<double>(nameof(Price), ref fPrice, value); }
        }
        EnumInventoryTransactionType fTransactionType;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public EnumInventoryTransactionType TransactionType {
            get { return fTransactionType; }
            set { SetPropertyValue<EnumInventoryTransactionType>(nameof(TransactionType), ref fTransactionType, value); }
        }

        public InventoryRecord(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(Item) && oldValue != newValue)
                    onItemChangeValue();
                if (propertyName == nameof(TransactionUnit) && oldValue != newValue)
                    calculateQuantity();
                if (propertyName == nameof(Quantity) && oldValue != newValue)
                    calculateQuantity();
            }
        }

        private void onItemChangeValue() {
            if (Item != null) {
                BaseUnit = Item != null ? Item.BaseUnit : null;
                StockUnit = Item == null ? null : Item.StockUnit != null ? Item.StockUnit : Item.BaseUnit;
            }
            else {
                BaseUnit = null;
                StockUnit = null;
            }
        }        
        private void calculateQuantity() {
            BaseQuantity = TransactionUnit == null ? 0 : Quantity * TransactionUnit.ConversionRate;
            StockQuantity = StockUnit == null ? BaseQuantity : Math.Round(BaseQuantity / StockUnit.ConversionRate, 2);
        }
    }
}
