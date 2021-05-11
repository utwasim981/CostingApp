using CostingApp.Module.BO.ItemTransactions.Abstraction;
using CostingApp.Module.BO.Masters.Period;
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

namespace CostingApp.Module.BO.ItemTransactions {
    [NavigationItem("Transactions"),
        ImageName("BO_Order_Item"),
        XafDefaultProperty(nameof(Number)),
        VisibleInReports(true)]
    [Appearance("InventoryAdjustment_Approved.Enabled", Enabled = false, TargetItems = "*", Criteria = "Step = 1")]
    [AddItemClass(EnumInventoryTransactionType.InventoryAdjustment)]
    public class InventoryAdjustment : InventoryTransaction {
        public override EnumInventoryTransactionType TransactionType { get { return EnumInventoryTransactionType.InventoryAdjustment; } }       
        const string NumberFormat = "Concat('IA-', PadLeft(ToStr(SequentialNumber), 6, '0'))";
        [PersistentAlias(NumberFormat)]
        public string Number {
            get { return Convert.ToString(EvaluateAlias(nameof(Number))); }
        }
        EnumInventorySteps fStep;
        [ModelDefault("AllowEdit", "False")]
        public EnumInventorySteps Step {
            get { return fStep; }
            set { SetPropertyValue<EnumInventorySteps>(nameof(Step), ref fStep, value); }
        }
        [Association("InventoryAdjustment-InventoryAdjustmentItem"), DevExpress.Xpo.Aggregated]
        [RuleRequiredField("InventoryAdjustment_Items_RuleRequiredField", DefaultContexts.Save)]
        public XPCollection<InventoryAdjustmentItem> Items { get { return GetCollection<InventoryAdjustmentItem>(nameof(Items)); } }

        public InventoryAdjustment(Session session) : base(session) { }
        protected override void OnSaving() {
            base.OnSaving();
        }
        protected override string GetSequenceName() {
            return ClassInfo.FullName;
        }
        public void ApproveTransaction() {            
            foreach (var item in Items) {
                item.ApproveItem();
            }
            Step = EnumInventorySteps.Approced;
        }        
    }
}
