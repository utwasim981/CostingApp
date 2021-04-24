using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib.General.Model;
using WXafLib.General.ModelExtenders;
using WXafLib.General.Security;

namespace CostingApp.Module.Win.BO.Items {
    public class AssemblyItem : ItemCard {
        [RuleRequiredField("AssemblyItem_Components_RuleRequiredField", DefaultContexts.Save)]
        [Association("AssemblyItem-ItemComponent"), Aggregated]
        public XPCollection<ItemComponent> Components {
            get {
                return GetCollection<ItemComponent>(nameof(Components));
            }
        }
        public AssemblyItem(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            ItemType = EnumItemCard.AssemblyItem;
        }
    }
    [RuleCombinationOfPropertiesIsUnique("ItemComponent_Item_Component.RuleUniqueField", DefaultContexts.Save, "Item, Component")]
    public class ItemComponent : WXafBaseObject {        
        AssemblyItem fItem;
        [ListViewColumnOptions(false, false)]
        [Association("AssemblyItem-ItemComponent")]        
        public AssemblyItem Item {
            get { return fItem; }
            set { SetPropertyValue<AssemblyItem>(nameof(Item), ref fItem, value); }
        }
        ItemCard fComponent;
        [ListViewColumnOptions(false, false)]
        [RuleRequiredField("ItemComponent_Component_RuleRequiredField", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        [DataSourceCriteria("IsActive = True")]
        public ItemCard Component {
            get { return fComponent; }
            set {
                SetPropertyValue<ItemCard>(nameof(Component), ref fComponent, value);
                if (!IsLoading)
                    Unit = Component.StockUnit;
            }
        }
        Unit fUnit;
        [ListViewColumnOptions(false, false)]
        [RuleRequiredField("ItemComponent_Unit_RuleRequiredField", DefaultContexts.Save)]
        [DataSourceProperty("Item.UnitType.Units")]
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
        public ItemComponent(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Quantity = 1;
        }
    }
}
