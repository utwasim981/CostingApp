
using CostingApp.Module.BO.Expenses;
using CostingApp.Module.BO.Masters;
using CostingApp.Module.CommonLibrary.General.Model;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CostingApp.Module.BO.Items {
    [NavigationItem("Inventory Setup")]
//    [ImageName("BO_Product")]
    public abstract class ItemCard : WXafSequenceObject, ICategorizedItem {
        EnumItemCard fItemType;
        [VisibleInDetailView(false)]
        [ImmediatePostData(true)]
        public EnumItemCard ItemType {
            get { return fItemType; }
            set { SetPropertyValue<EnumItemCard>(nameof(ItemType), ref fItemType, value); }
        }
        string fItemCode;
        [RuleRequiredField("ItemCard_ItemCode_RuleRequiredField", DefaultContexts.Save, TargetCriteria = "GetBoolean('ItemCodeM') = True")]
        [Appearance("ItemCard_ItemCode.Enables", Enabled = false, Criteria = "GetBoolean('ItemCodeA') = True")]
        public string ItemCode {
            get { return fItemCode; }
            set { SetPropertyValue<string>(nameof(ItemCode), ref fItemCode, value); }
        }
        string fItemName;
        [RuleRequiredField("ItemCard_ItemName_RuleRequiredField", DefaultContexts.Save)]
        [RuleUniqueValue("ItemCard_ItemName_RuleUniqueValue", DefaultContexts.Save)]
        public string ItemName {
            get { return fItemName; }
            set { SetPropertyValue<string>(nameof(ItemName), ref fItemName, value); }
        }
        ItemCategory fCategory;
        [RuleRequiredField("ItemCard_Category_RuleRequiredField", DefaultContexts.Save)]
        [DataSourceCriteria("IsActive = True")]
        [ImmediatePostData(true)]
        public ItemCategory Category {
            get { return fCategory; }
            set { SetPropertyValue<ItemCategory>(nameof(Category), ref fCategory, value); }
        }
        UnitType fUnitType;
        [DataSourceCriteria("IsActive = True")]
        [Association("ItemCard-UnitType")]
        [RuleRequiredField("ItemCard_UnitType_RuleRequiredField", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        public UnitType UnitType {
            get { return fUnitType; }
            set {
                SetPropertyValue<UnitType>(nameof(UnitType), ref fUnitType, value);
                if (!IsLoading)
                    onUnitTypeChanged();
            }
        }
        Unit fBaseUnit;
        [ModelDefault("AllowEdit", "False")]
        public Unit BaseUnit {
            get { return fBaseUnit; }
            set { SetPropertyValue<Unit>(nameof(BaseUnit), ref fBaseUnit, value); }
        }
        Unit fPurchaseUnit;
        [DataSourceProperty("UnitType.Units")]
        [Appearance("ItemCard_PurchaseUnit_Enabled", Enabled = false, Criteria = "ItemType = 2")]
        public Unit PurchaseUnit {
            get { return fPurchaseUnit; }
            set { SetPropertyValue<Unit>(nameof(PurchaseUnit), ref fPurchaseUnit, value); }
        }
        Unit fSalesUnit;
        [DataSourceProperty("UnitType.Units")]
        [Appearance("ItemCard_SalesUnit_Enabled", Enabled = false, Criteria = "ItemType = 1")]
        public Unit SalesUnit {
            get { return fSalesUnit; }
            set { SetPropertyValue<Unit>(nameof(SalesUnit), ref fSalesUnit, value); }
        }
        Unit fStockUnit;
        [DataSourceProperty("UnitType.Units")]
        [Appearance("ItemCard_StockUnit_Enabled", Enabled = false, Criteria = "ItemType = 2")]
        public Unit StockUnit {
            get { return fStockUnit; }
            set { SetPropertyValue<Unit>(nameof(StockUnit), ref fStockUnit, value); }
        }
        ExpenseType fExpenseType;
        [RuleRequiredField("ItemCard_ExpenseType_RuleRequiredField", DefaultContexts.Save, TargetCriteria = "ItemType <> 2")]
        [Appearance("ItemCard_ExpenseType_Enabled", Enabled = false, Criteria = "ItemType = 2")]
        [DataSourceCriteria("IsActive = True")]
        public ExpenseType ExpenseType {
            get { return fExpenseType; }
            set { SetPropertyValue<ExpenseType>(nameof(ExpenseType), ref fExpenseType, value); }
        }
        double fPurchasePrice;
        public double PurchasePrice {
            get { return fPurchasePrice; }
            set { SetPropertyValue<double>(nameof(PurchasePrice), ref fPurchasePrice, value); }
        }
        double fSellPrice;
        public double SellPrice {
            get { return fSellPrice; }
            set { SetPropertyValue<double>(nameof(SellPrice), ref fSellPrice, value); }
        }       
        [Association("ItemCard-ItemQuantity"), DevExpress.Xpo.Aggregated]
        [ModelDefault("AllowEdit", "False")]
        public XPCollection<ItemQuantity> Inventory {
            get { return GetCollection<ItemQuantity>(nameof(Inventory)); }
        }

        ITreeNode ICategorizedItem.Category {
            get {
                return Category;
            }
            set {
                Category = (ItemCategory)value;
            }
        }

        public ItemCard(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(SequentialNumber))
                    onSequentialNumberValueChange(oldValue, newValue);
            }
        }

        private void onSequentialNumberValueChange(object oldValue, object newValue) {
            if (oldValue != newValue &&
                (bool)ValueManager.GetValueManager<Dictionary<string, object>>("Values").Value["CityCodeA"])
                ItemCode = $"ITM-{SequentialNumber.ToString().PadLeft(5, '0')}";
        }
        private void onUnitTypeChanged() {
            BaseUnit = UnitType.Units.FirstOrDefault(x => x.BaseUnit);
        }
        private void onUnitChange() {
            //TODO: when ItemUnit Updated.
            foreach(var itemQuantity in Inventory) {
            }
        }
        public void UpdateLasPurchasePrice(Shop Shop, Unit TransactionUnit, DateTime PurchaseDate, double PurchasePrice) {
            var itemQuantity = this.Inventory.FirstOrDefault(x => x.Shop == Shop);
            if (itemQuantity == null) {
                itemQuantity = ObjectSpace.CreateObject<ItemQuantity>();
                itemQuantity.Shop = Shop;
                itemQuantity.LastPurchaseDate = DateTime.MinValue;
                itemQuantity.ItemCard = this;
                Inventory.Add(itemQuantity);
            }
            if (PurchaseDate > itemQuantity.LastPurchaseDate) {
                itemQuantity.LastPurchaseDate = PurchaseDate;
                itemQuantity.LastPurchasePrice = PurchaseUnit != null ?
                                                Math.Round((PurchasePrice * TransactionUnit.ConversionRate) / PurchaseUnit.ConversionRate, 2) :
                                                PurchasePrice * TransactionUnit.ConversionRate;
            }
        }
        public void UpdateQuantityOnHand(Shop Shop, Unit TransactionUnit, double Quantity) {
            var itemQuantity = this.Inventory.FirstOrDefault(x => x.Shop == Shop);
            if (itemQuantity == null) {
                itemQuantity = ObjectSpace.CreateObject<ItemQuantity>();
                itemQuantity.Shop = Shop;
                itemQuantity.LastPurchaseDate = DateTime.MinValue;
                itemQuantity.ItemCard = this;
                Inventory.Add(itemQuantity);
            }
            itemQuantity.QuantityOnHand += Math.Round((Quantity * TransactionUnit.ConversionRate) / StockUnit.ConversionRate, 2);
        }
        public double GetQuantityOnHand(Shop Shop) {
            var itemQuantity = Inventory.FirstOrDefault(x => x.Shop == Shop);
            return itemQuantity == null ? 0 : itemQuantity.QuantityOnHand;
        }
        public double GetLastPurchasePrice(Shop Shop) {
            var itemQuantity = Inventory.FirstOrDefault(x => x.Shop == Shop);
            return itemQuantity == null ? 0 : itemQuantity.LastPurchasePrice;
        }
    }
}
