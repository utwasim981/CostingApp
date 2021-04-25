using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using WXafLib.General.Model;
using WXafLib.General.ModelExtenders;

namespace CostingApp.Module.Win.BO.Items {
    [ImageName("itemcomponent")]
    [RuleCombinationOfPropertiesIsUnique("ItemComponent_Item_Component.RuleUniqueField", DefaultContexts.Save, "Item, Component")]
    public class MenuItemComponent : WXafBaseObject {        
        MenuItem fItem;
        [ListViewColumnOptions(false, false)]
        [Association("MenuItem-MenuItemComponent")]        
        public MenuItem Item {
            get { return fItem; }
            set { SetPropertyValue<MenuItem>(nameof(Item), ref fItem, value); }
        }
        ItemCard fComponent;
        [ListViewColumnOptions(false, false)]
        [RuleRequiredField("ItemComponent_Component_RuleRequiredField", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        [DataSourceCriteria("IsActive = True And ItemType <> 2")]
        public ItemCard Component {
            get { return fComponent; }
            set {
                SetPropertyValue<ItemCard>(nameof(Component), ref fComponent, value);
                onItemValueChanged();
            }
        }
        Unit fUnit;
        [ListViewColumnOptions(false, false)]
        [RuleRequiredField("ItemComponent_Unit_RuleRequiredField", DefaultContexts.Save)]
        [DataSourceProperty("Component.UnitType.Units")]
        public Unit Unit {
            get { return fUnit; }
            set { SetPropertyValue<Unit>(nameof(Unit), ref fUnit, value); }
        }
        double fQuantity;
        [ListViewColumnOptions(false, false)]
        [RuleValueComparison("ItemComponent_Quantity.GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public double Quantity {
            get { return fQuantity; }
            set { SetPropertyValue<double>(nameof(Quantity), ref fQuantity, value); }
        }
        public MenuItemComponent(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Quantity = 1;
        }

        private void onItemValueChanged() {
            if (!IsLoading)
                Unit = Component.StockUnit;
        }
    }
}
