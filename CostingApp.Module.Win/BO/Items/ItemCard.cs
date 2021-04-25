using CostingApp.Module.Win.BO.Expenses;
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
using WXafLib.General.Model;
using WXafLib.General.Security;

namespace CostingApp.Module.Win.BO.Items {
    [NavigationItem("Inventory Setup")]
    [ImageName("BO_Product")]
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
        public ItemCategory Category {
            get { return fCategory; }
            set { SetPropertyValue<ItemCategory>(nameof(Category), ref fCategory, value); }
        }
        UnitType fUnitType;
        [DataSourceCriteria("IsActive = True")]
        [Association("ItemCard-UnitType")]
        [RuleRequiredField("ItemCard_UnitType_RuleRequiredField", DefaultContexts.Save)]
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
        DateTime fLastPurchaseDate;
        [ModelDefault("AllowEdit", "False")]
        public DateTime LastPurchaseDate {
            get { return fLastPurchaseDate; }
            set { SetPropertyValue<DateTime>(nameof(LastPurchaseDate), ref fLastPurchaseDate, value); }
        }
        double fLastPurchasePrice;
        [ModelDefault("AllowEdit", "False")]
        public double LastPurchasePrice {
            get { return fLastPurchasePrice; }
            set { SetPropertyValue<double>(nameof(LastPurchasePrice), ref fLastPurchasePrice, value); }
        }
        double fQuantityOnHand;
        [ModelDefault("AllowEdit", "False")]
        public double QuantityOnHand {
            get { return fQuantityOnHand; }
            set { SetPropertyValue<double>(nameof(QuantityOnHand), ref fQuantityOnHand, value); }
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
        public void UpdateLasPurchasePrice(InventoryRecord record) {
            if (record.ExpenseDate > LastPurchaseDate) {
                LastPurchaseDate = record.ExpenseDate;
                if (PurchaseUnit == null)
                    LastPurchasePrice = Math.Round(record.Price / record.TransactionUnit.ConversionRate, 2);
                else
                    LastPurchasePrice = Math.Round((record.Price / record.TransactionUnit.ConversionRate) * PurchaseUnit.ConversionRate, 2);
            }
        }
        public void UpdateQuantityOnHand(double quantity) {
            QuantityOnHand += quantity;
        }
    }
}
