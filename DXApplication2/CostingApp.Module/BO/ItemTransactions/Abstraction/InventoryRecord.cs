using CostingApp.Module.BO.Expenses;
using CostingApp.Module.BO.Items;
using CostingApp.Module.BO.Masters;
using CostingApp.Module.BO.Masters.Period;
using CostingApp.Module.CommonLibrary.General.Model;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
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

namespace CostingApp.Module.BO.ItemTransactions.Abstraction {
    [NavigationItem("Testing"),
        RuleCombinationOfPropertiesIsUnique("InventoryRecord_RuleUniqueField", DefaultContexts.Save, "Transaction, Item, TransactionUnit"),
        VisibleInReports(true)]
    public abstract class InventoryRecord : WXafBaseObject {
        #region Virtual
        protected virtual void OnDeletingRecord() { }
        protected virtual void OnSavingRecord() { }
        #endregion
        #region Properties
        Shop fShop;
        [DataSourceCriteria("IsActive = True"),
            RuleRequiredField("InventoryRecord_Shop_RuleRequiredField", DefaultContexts.Save),
            VisibleInDetailView(false),
            VisibleInListView(false), ModelDefault("AllowEdit", "False"),
            ImmediatePostData(true)]
        public Shop Shop {
            get { return fShop; }
            set { SetPropertyValue<Shop>(nameof(Shop), ref fShop, value); }
        }

        DateTime fDate;
        [VisibleInDetailView(false),
            VisibleInListView(false),
            //RuleRequiredField("InventoryRecord_Date_RuleRequiredField", DefaultContexts.Save),
            ImmediatePostData(true),
            ModelDefault("AllowEdit", "False")]
        public DateTime Date {
            get { return fDate; }
            set { SetPropertyValue<DateTime>(nameof(Date), ref fDate, value); }
        }

        BasePeriod fPeriod;
        [ModelDefault("AllowEdit", "False"),
            //RuleRequiredField("InventoryRecord_Period_RuleRequiredField", DefaultContexts.Save),
            VisibleInDetailView(false),
            VisibleInListView(false),
            ImmediatePostData(true)]
        public BasePeriod Period {
            get { return fPeriod; }
            set { SetPropertyValue<BasePeriod>(nameof(Period), ref fPeriod, value); }
        }

        ItemCard fItem;
        [RuleRequiredField("InventoryRecord_Item_RuleRequiredField", DefaultContexts.Save),
            DataSourceCriteria("IsActive = True"),
            ImmediatePostData(true)]
        public ItemCard Item {
            get { return fItem; }
            set { SetPropertyValue<ItemCard>(nameof(Item), ref fItem, value); }
        }

        ExpenseType fExpenseType;
        [DataSourceCriteria("IsActive = True"),
            RuleRequiredField("InventoryRecord_ExpenseType_RuleRequiredField", DefaultContexts.Save),
            VisibleInDetailView(false),
            VisibleInListView(false),
            ImmediatePostData(true),
            ModelDefault("AllowEdit", "False")]
        public ExpenseType ExpenseType {
            get { return fExpenseType; }
            set { SetPropertyValue<ExpenseType>(nameof(ExpenseType), ref fExpenseType, value); }
        }

        Unit fTransactionUnit;
        [RuleRequiredField("InventoryRecord_TransactionUnit_RuleRequiredField", DefaultContexts.Save),
            ImmediatePostData(true),
            XafDisplayName("Unit"),
            DataSourceProperty("Item.UnitType.Units")]
        public Unit TransactionUnit {
            get { return fTransactionUnit; }
            set { SetPropertyValue<Unit>(nameof(TransactionUnit), ref fTransactionUnit, value); }
        }

        double fQuantity;
        [ImmediatePostData(true),
            XafDisplayName("QTY"),
            RuleValueComparison("InventoryRecord_Quantity_GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public double Quantity {
            get { return fQuantity; }
            set { SetPropertyValue<double>(nameof(Quantity), ref fQuantity, value);}
        }

        double fPrice;
        //[RuleValueComparison("InventoryRecord_Price_GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0),
            [ImmediatePostData(true),
            Appearance("InventoryRecord_Price.Enabled", Enabled = false, Criteria = "RecordType = 1")]
        public double Price {
            get { return fPrice; }
            set { SetPropertyValue<double>(nameof(Price), ref fPrice, value); }
        }

        [XafDisplayName("AMT"),
            PersistentAlias("Price * Quantity")]
        public double Amount {
            get { return Convert.ToDouble(EvaluateAlias(nameof(Amount))); }
        }

        string fNotes;
        [Size(200)]
        public string Notes {
            get { return fNotes; }
            set { SetPropertyValue<string>(nameof(Notes), ref fNotes, value); }
        }

        Unit fBaseUnit;
        [ModelDefault("AllowEdit", "False"),
            VisibleInDetailView(false),
            VisibleInListView(false),
            ImmediatePostData(false)]
        public Unit BaseUnit {
            get { return fBaseUnit; }
            set { SetPropertyValue<Unit>(nameof(BaseUnit), ref fBaseUnit, value); }
        }

        double fFifoQuantity;
        [VisibleInDetailView(false),
            VisibleInListView(false),
            ModelDefault("AllowEdit", "False")]
        public double FifoQuantity {
            get { return fFifoQuantity; }
            set { SetPropertyValue<double>(nameof(FifoQuantity), ref fFifoQuantity, value); }
        }

        double fLifoQuantity;
        [VisibleInDetailView(false),
            VisibleInListView(false),
            ModelDefault("AllowEdit", "False")]
        public double LifoQuantity {
            get { return fLifoQuantity; }
            set { SetPropertyValue<double>(nameof(LifoQuantity), ref fLifoQuantity, value); }
        }

        EnumInventoryRecordType fRecordType;
        [VisibleInDetailView(false),
            VisibleInListView(false),
            ImmediatePostData(true),
            ModelDefault("AllowEdit", "False")]
        public EnumInventoryRecordType RecordType {
            get { return fRecordType; }
            set { SetPropertyValue<EnumInventoryRecordType>(nameof(RecordType), ref fRecordType, value); }
        }
        InventoryTransaction fTransaction;
        [VisibleInDetailView(false),
            VisibleInListView(false),
            ImmediatePostData(false),
            ModelDefault("AllowEdit", "False")]
        public InventoryTransaction Transaction {
            get { return fTransaction; }
            set { SetPropertyValue<InventoryTransaction>(nameof(Transaction), ref fTransaction, value); }
        }
        #endregion
        #region CalculatedPrerties
        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("iif(RecordType = 0, Quantity, 0)")]
        public double TransactionQuantityIn {
            get { return Convert.ToDouble(EvaluateAlias(nameof(TransactionQuantityIn))); }
        }

        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("iif(RecordType = 1, Quantity, 0)")]
        public double TransactionQuantityOut {
            get { return Convert.ToDouble(EvaluateAlias(nameof(TransactionQuantityOut))); }
        }

        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("Round(Quantity * TransactionUnit.ConversionRate, 3)")]
        public double BaseQuantity {
            get { return Convert.ToDouble(EvaluateAlias(nameof(BaseQuantity))); }
        }

        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("Round((Quantity * TransactionUnit.ConversionRate) / Item.StockUnit.ConversionRate, 3)")]
        public double StockQuantity {
            get { return Convert.ToDouble(EvaluateAlias(nameof(StockQuantity))); }
        }

        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("Round(Price / TransactionUnit.ConversionRate, 3)")]
        public double BasePrice {
            get { return Convert.ToDouble(EvaluateAlias(nameof(BasePrice))); }
        }

        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("Round((Price / TransactionUnit.ConversionRate) * Item.StockUnit.ConversionRate, 3)")]
        public double StockPrice {
            get { return Convert.ToDouble(EvaluateAlias(nameof(StockPrice))); }
        }

        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("Round(FifoQuantity * TransactionUnit.ConversionRate, 3)")]
        public double BaseFifoQuantity {
            get { return Convert.ToDouble(EvaluateAlias(nameof(BaseFifoQuantity))); }
        }

        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("Round((FifoQuantity * TransactionUnit.ConversionRate) / Item.StockUnit.ConversionRate, 3)")]
        public double StockFifoQuantity {
            get { return Convert.ToDouble(EvaluateAlias(nameof(StockFifoQuantity))); }
        }

        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("Round(LifoQuantity * TransactionUnit.ConversionRate, 3)")]
        public double BaseLifoQuantity {
            get { return Convert.ToDouble(EvaluateAlias(nameof(BaseLifoQuantity))); }
        }

        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("Round((LifoQuantity * TransactionUnit.ConversionRate) / Item.StockUnit.ConversionRate, 3)")]
        public double StockLifoQuantity {
            get { return Convert.ToDouble(EvaluateAlias(nameof(StockLifoQuantity))); }
        }
        [Browsable(false)]
        public EnumInventoryRecordType NotRecordType {
            get { return RecordType == EnumInventoryRecordType.In ? EnumInventoryRecordType.Out : EnumInventoryRecordType.In; }
        }
        #endregion
        #region Base Override

        public InventoryRecord(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Quantity = 1;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(Transaction) && oldValue != newValue)
                    onTransactionValueChange();
                else if (propertyName == nameof(Item) && oldValue != newValue)
                    onItemValueChange();
            }
        }
        protected override void OnDeleting() {
            base.OnDeleting();
            if (!Session.IsNewObject(Transaction))
                OnDeletingRecord();
        }
        protected override void OnSaving() {
            base.OnSaving();
            if (!(Session is NestedUnitOfWork) && !IsDeleted) {
                OnSavingRecord();
                if (RecordType == EnumInventoryRecordType.Out)
                    Item.GetFifoRecord(this);
            }                
        }
        #endregion
        #region Privite Methods
        private void onTransactionValueChange() {
            if (Transaction != null) {
                if (Shop != Transaction.Shop)
                    Shop = Transaction.Shop;
                if (Date != Transaction.TransactionDate)
                    Date = Transaction.TransactionDate;
                if (Period != Transaction.Period)
                    Period = Transaction.Period;
            }
            else {
                Shop = null;
                Period = null;
            }
        }
        private void onItemValueChange() {
            BaseUnit = Item == null ? null : Item.BaseUnit;
            ExpenseType = Item == null ? null : Item.ExpenseType;
            if (Transaction != null) {
                switch (Transaction.TransactionType) {
                    case EnumInventoryTransactionType.PurchaseInvoice:
                        TransactionUnit = Item.PurchaseUnit;
                        Price = Item.GetLastPurchasePrice(Shop, TransactionUnit);
                        break;
                    case EnumInventoryTransactionType.SalesInvoice:
                        TransactionUnit = Item.SalesUnit;
                        break;
                    case EnumInventoryTransactionType.InventoryTransfer:
                        TransactionUnit = Item.StockUnit;
                        break;
                    case EnumInventoryTransactionType.InventoryAdjustment:
                        TransactionUnit = Item.StockUnit;
                        break;
                }
            }
        }
        #endregion
    }
}
