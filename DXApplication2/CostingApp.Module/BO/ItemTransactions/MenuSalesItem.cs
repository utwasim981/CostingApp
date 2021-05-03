using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.Xpo.Metadata;
using System;
using DevExpress.ExpressApp.Model;
using CostingApp.Module.CommonLibrary.General.Model;
using CostingApp.Module.CommonLibrary;
using CostingApp.Module.BO.Items;

namespace CostingApp.Module.BO.ItemTransactions {
    public class MenuSalesItem : WXafBase {
        MenuSales fMenuSales;
        [Association("MenuSales-MenuSalesItem")]
        public MenuSales MenuSales {
            get { return fMenuSales; }
            set { SetPropertyValue<MenuSales>(nameof(MenuSales), ref fMenuSales, value); }
        }
        ItemCard fItem;
        [DataSourceCriteria("IsActive = True"),
            RuleRequiredField("MenuSalesItem_Item_RuleRequiredField", DefaultContexts.Save),
            ImmediatePostData(true)]
        public ItemCard Item {
            get { return fItem; }
            set { SetPropertyValue<ItemCard>(nameof(Item), ref fItem, value); }
        }
        double fQuantity;
        [ImmediatePostData(true),
            RuleValueComparison("MenuSalesItem_Quantity.GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public double Quantity {
            get { return fQuantity; }
            set { SetPropertyValue<double>(nameof(Quantity), ref fQuantity, value); }
        }
        [VisibleInDetailView(false),
            VisibleInListView(false),
            PersistentAlias("Components.Sum(Amount)")]
        public double Amount {
            get { return Convert.ToDouble(EvaluateAlias(nameof(Amount))); }
        }


        [Association("MenuSalesItem-MenuSalesComponent"), Aggregated,
            ModelDefault("AllowEdit", "False")]
        public XPCollection<MenuSalesComponent> Components { get { return GetCollection<MenuSalesComponent>(nameof(Components)); } }

        public MenuSalesItem(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Quantity = 1;
        }
        public void CreateItemComponent() {
            MenuSalesComponent salesComponent = null;
            if (this.Item is InventoryItem) {
                salesComponent = Components.FirstOrDefault(x => x.Item == Item);
                if (salesComponent == null) {
                    salesComponent = ObjectSpace.CreateObject<MenuSalesComponent>();
                    salesComponent.MenuSalesItem = this;
                    salesComponent.Transaction = MenuSales;
                    salesComponent.Item = Item;
                    Components.Add(salesComponent);
                }
                salesComponent.Shop = MenuSales.Shop;
                salesComponent.Date = MenuSales.TransactionDate;
                salesComponent.Period = MenuSales.Period;                
                salesComponent.Quantity = Quantity;
            }
            else
                foreach (var itemComponent in ((MenuItem)Item).Components) {
                    salesComponent = Components.FirstOrDefault(x => x.Item == itemComponent.Item);
                    if (salesComponent == null) {
                        salesComponent = ObjectSpace.CreateObject<MenuSalesComponent>();
                        salesComponent.MenuSalesItem = this;
                        salesComponent.Transaction = MenuSales;
                        salesComponent.Item = itemComponent.Component;
                        salesComponent.TransactionUnit = itemComponent.Unit;
                        Components.Add(salesComponent);
                    }
                    salesComponent.Shop = MenuSales.Shop;
                    salesComponent.Date = MenuSales.TransactionDate;
                    salesComponent.Period = MenuSales.Period;
                    salesComponent.Quantity = Quantity * itemComponent.Quantity;
                }
            OnChanged(nameof(Components));
        }
    }
}
