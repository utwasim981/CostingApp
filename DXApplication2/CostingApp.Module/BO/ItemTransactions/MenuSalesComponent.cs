using CostingApp.Module.BO.ItemTransactions.Abstraction;
using CostingApp.Module.CommonLibrary;
using DevExpress.Xpo;
using System;

namespace CostingApp.Module.BO.ItemTransactions {
    public class MenuSalesComponent : OutputInventoryRecord {
        MenuSalesItem fMenuSalesItem;
        [Association("MenuSalesItem-MenuSalesComponent")]
        public MenuSalesItem MenuSalesItem {
            get { return fMenuSalesItem; }
            set { SetPropertyValue<MenuSalesItem>(nameof(MenuSalesItem), ref fMenuSalesItem, value); }
        }

        public MenuSalesComponent(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && propertyName == nameof(MenuSalesItem.MenuSales) && oldValue != newValue)
                onMenuSalesItemValueChanged();
        }

        private void onMenuSalesItemValueChanged() {
            Transaction = (MenuSalesItem != null && MenuSalesItem.MenuSales != null) ? MenuSalesItem.MenuSales : null;
        }

    }
}
