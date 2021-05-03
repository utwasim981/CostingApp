using CostingApp.Module.BO.ItemTransactions.Abstraction;
using CostingApp.Module.BO.Masters.Period;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.BO.ItemTransactions {
    [NavigationItem("Transactions"),
        ImageName("invoice"),
        XafDefaultProperty(nameof(Number))]

    public class PurchaseInvoice : InventoryTransaction {
        public override EnumInventoryTransactionType TransactionType { get { return EnumInventoryTransactionType.PurchaseInvoice; } }
        const string NumberFormat = "Concat('PI-', PadLeft(ToStr(SequentialNumber), 6, '0'))";
        [PersistentAlias(NumberFormat)]
        public string Number {
            get { return Convert.ToString(EvaluateAlias(nameof(Number))); }
        }
        string fVendorBill;
        public string VendorBill {
            get { return fVendorBill; }
            set { SetPropertyValue<string>(nameof(VendorBill), ref fVendorBill, value); }
        }
        [RuleRequiredField("PurchaseInvoice_Items_RuleRequiredField", DefaultContexts.Save)]
        [Association("PurchaseInvoice-PurchaseInvoiceDetail"), DevExpress.Xpo.Aggregated]
        [ImmediatePostData(false)]
        public XPCollection<PurchaseInvoiceDetail> Items {
            get {
                return GetCollection<PurchaseInvoiceDetail>(nameof(Items));
            }
        }


        public PurchaseInvoice(Session session) : base(session) {
        }
        protected override string GetSequenceName() {
            return ClassInfo.FullName;
        }
    }
}
