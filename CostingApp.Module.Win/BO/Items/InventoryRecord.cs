using CostingApp.Module.Win.BO.Expenses;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib.General.ModelExtenders;

namespace CostingApp.Module.Win.BO.Items {
    public abstract class InventoryRecord : ExpenseRecord {
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
        [RuleValueComparison("InventoryRecord_Quantity_GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        [ImmediatePostData(true)]
        public double Quantity {
            get { return fQuantity; }
            set { SetPropertyValue<double>(nameof(Quantity), ref fQuantity, value); }
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
        [RuleValueComparison("InventoryRecord_Price_GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        [ImmediatePostData(true)]
        public double Price {
            get { return fPrice; }
            set { SetPropertyValue<double>(nameof(Price), ref fPrice, value); }
        }
        public InventoryRecord(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Quantity = 1;
        }
        protected override void OnSaving() {
            base.OnSaving();
            if (ExpenseDate > Item.LastPurchaseDate) {
                Item.LastPurchaseDate = ExpenseDate;
                Item.LastPurchasePrice = Price;
            }
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(Item) && oldValue != newValue) {
                    BaseUnit = Item != null ? Item.BaseUnit : null;
                    if (Item.ItemType == EnumItemCard.InventoryItem)
                        StockUnit = Item != null ? Item.StockUnit : null;
                    else
                        StockUnit = Item != null ? Item.BaseUnit : null;
                }
                if ((propertyName == nameof(Quantity) || propertyName == nameof(Price)) && oldValue != newValue)
                    Amount = Price * Quantity;
                if ((propertyName == nameof(Quantity) || propertyName == nameof(TransactionUnit)) && newValue != oldValue) {
                    BaseQuantity = TransactionUnit == null ? 0 : Quantity * TransactionUnit.ConversionRate;
                    StockQuantity = StockUnit == null ? 0 : BaseQuantity / StockUnit.ConversionRate;
                }                
            }
        }
    }
}
