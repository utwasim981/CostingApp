using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.BO.Items {
    public class InventoryItem : ItemCard{
        public InventoryItem(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            ItemType = EnumItemCard.InventoryItem;
        }
    }
}
