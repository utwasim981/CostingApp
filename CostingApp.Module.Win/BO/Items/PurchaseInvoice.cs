using CostingApp.Module.Win.BO.Expenses;
using CostingApp.Module.Win.BO.Masters;
using CostingApp.Module.Win.BO.Masters.Period;
using DevExpress.Data.Filtering;
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
using WXafLib.General.Model;
using WXafLib.General.Security;

namespace CostingApp.Module.Win.BO.Items {
    [NavigationItem("Inventory")]
    public class PurchaseInvoice : WXafSequenceObject {
        Shop fShop;
        [DataSourceCriteria("IsActive = True")]
        [RuleRequiredField("PurchaseInvoice_Shop_RuleRequiredField", DefaultContexts.Save)]
        public Shop Shop {
            get { return fShop; }
            set { SetPropertyValue<Shop>(nameof(Shop), ref fShop, value);
                if (!IsLoading && Items.Count != 0)
                    foreach (var item in Items)
                        item.Shop = value;
            }
        }
        BasePeriod fPeriod;
        [ModelDefault("AllowEdit", "False")]
        public BasePeriod Period {
            get { return fPeriod; }
            set { SetPropertyValue<BasePeriod>(nameof(Period), ref fPeriod, value); }
        }
        public string InvoiceNumber {
            get { return string.Format("PI-{0}", SequentialNumber.ToString().PadLeft(6, '0')); }
        }
        DateTime fInvoiceDate;
        [RuleRequiredField("PurchaseInvoice_InvoiceDate_RuleRequiredField", DefaultContexts.Save)]
        public DateTime InvoiceDate {
            get { return fInvoiceDate; }
            set {
                SetPropertyValue<DateTime>(nameof(InvoiceDate), ref fInvoiceDate, value);
                if (!IsLoading) {
                    if (Items.Count != 0)
                        foreach (var item in Items)
                            item.ExpenseDate = value;
                    GetPeriod();
                }
            }
        }
        string fBillNumber;
        public string BillNumber {
            get { return fBillNumber; }
            set { SetPropertyValue<string>(nameof(BillNumber), ref fBillNumber, value); }
        }
        string fNotes;
        public string Notes {
            get { return fNotes; }
            set { SetPropertyValue<string>(nameof(Notes), ref fNotes, value); }
        }
        double fSubTotal;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public double SubTotal {
            get { return fSubTotal; }
            set { SetPropertyValue<double>(nameof(SubTotal), ref fSubTotal, value); }
        }
        double fVAT;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public double VAT {
            get { return fVAT; }
            set { SetPropertyValue<double>(nameof(VAT), ref fVAT, value); }
        }
        double fTotal;
        [ModelDefault("AllowEdit", "False")]
        public double Total {
            get { return fTotal; }
            set { SetPropertyValue<double>(nameof(Total), ref fTotal, value); }
        }
        EnumStatus fStatus;
        public EnumStatus Status {
            get { return fStatus; }
            set { SetPropertyValue<EnumStatus>(nameof(Status), ref fStatus, value); }
        }
        [RuleRequiredField("PurchaseInvoice_Items_RuleRequiredField", DefaultContexts.Save)]
        [Association("PurchaseInvoice-PurchaseInvoiceDetail"), Aggregated]
        public XPCollection<PurchaseInvoiceDetail> Items {
            get {
                return GetCollection<PurchaseInvoiceDetail>(nameof(Items));
            }
        }

        public PurchaseInvoice(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            InvoiceDate = DateTime.Now;
        }
        protected override void OnSaving() {
            base.OnSaving();
            Total = Items.Sum(x => x.Amount);
        }
        private void GetPeriod() {
            Period = Session.FindObject<BasePeriod>(CriteriaOperator.Parse("StartDate <= ? And EndDate >= ?", InvoiceDate, InvoiceDate));
        }
    }
    public class PurchaseInvoiceDetail : InventoryRecord {
        PurchaseInvoice fInvoice;
        [Association("PurchaseInvoice-PurchaseInvoiceDetail")]
        public PurchaseInvoice Invoice {
            get { return fInvoice; }
            set { SetPropertyValue<PurchaseInvoice>(nameof(Invoice), ref fInvoice, value); }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PurchaseInvoiceDetail_Item_IsValid", DefaultContexts.Save, "Item must not be empty")]
        public bool IsItemIsValid {
            get {
                return Item != null;
            }
        }
        public PurchaseInvoiceDetail(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsDeleted) {
                if (propertyName == nameof(Invoice) && oldValue != newValue) {
                    Shop = Invoice != null ? Invoice.Shop : null;
                    Period = Invoice != null ? Invoice.Period : null;
                    ExpenseDate = Invoice.InvoiceDate;
                }
                if (propertyName == nameof(Item) && oldValue != newValue) {
                    if (Item != null) {
                        TransactionUnit = Item.PurchaseUnit;
                        Price = Item.PurchasePrice;
                    }
                    else {
                        TransactionUnit = null;
                        Price = 0;
                    }
                }
                if (propertyName == nameof(Amount) && oldValue != newValue)
                    Invoice.Total = Invoice != null ? Invoice.Items.Sum(x => x.Amount) : 0;
            }
        }
        protected override void OnSaving() {
            base.OnSaving();
            if (Invoice != null)
                Invoice.Total = Invoice.Items.Sum(x => x.Amount);
        }
    }
}
