using CostingApp.Module.BO.ItemTransactions.Abstraction;
using CostingApp.Module.CommonLibrary;
using DevExpress.ExpressApp.DC;
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
        ImageName("BO_Invoice"),
        XafDefaultProperty(nameof(Number))]
    public class SalesInvoice : InventoryTransaction {
        public override EnumInventoryTransactionType TransactionType { get { return EnumInventoryTransactionType.SalesInvoice; } }
        const string NumberFormat = "Concat('INV-', PadLeft(ToStr(SequentialNumber), 6, '0'))";
        [PersistentAlias(NumberFormat)]
        public string Number {
            get { return Convert.ToString(EvaluateAlias(nameof(Number))); }
        }
        string fSellTo;
        [RuleRequiredField("SalesInvoice_SellTo_RuleRequiredField", DefaultContexts.Save)]
        public string SellTo {
            get { return fSellTo; }
            set { SetPropertyValue<string>(nameof(SellTo), ref fSellTo, value); }
        }
        [RuleRequiredField("SalesInvoice__Items_RuleRequiredField", DefaultContexts.Save)]
        [Association("SalesInvoice-SalesInvoiceDetail"), DevExpress.Xpo.Aggregated]
        [ImmediatePostData(false)]
        public XPCollection<SalesInvoiceDetail> Items {
            get {
                return GetCollection<SalesInvoiceDetail>(nameof(Items));
            }
        }
        public SalesInvoice(Session session) : base(session) { }
        protected override string GetSequenceName() {
            return ClassInfo.FullName;
        }
    }
}
