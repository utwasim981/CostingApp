using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.BO.Items {
    [XafDisplayName("Menu Item")]
    [XafDefaultProperty(nameof(ItemName))]
    public class MenuItem : ItemCard {
        [RuleRequiredField("AssemblyItem_Components_RuleRequiredField", DefaultContexts.Save)]
        [Association("MenuItem-MenuItemComponent"), DevExpress.Xpo.Aggregated]
        public XPCollection<MenuItemComponent> Components {
            get {
                return GetCollection<MenuItemComponent>(nameof(Components));
            }
        }
        public MenuItem(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            ItemType = EnumItemCard.MenuItem;
        }
    }
}
