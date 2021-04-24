using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.Win.BO.Items {
    [NavigationItem("Inventory")]
    public class TestItemTransaction : InventoryRecord {
        public TestItemTransaction(Session session) : base(session) { }
    }
}
