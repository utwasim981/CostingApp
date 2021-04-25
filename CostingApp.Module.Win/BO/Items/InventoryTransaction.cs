using CostingApp.Module.Win.BO.Masters;
using CostingApp.Module.Win.BO.Masters.Period;
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

namespace CostingApp.Module.Win.BO.Items {
    public abstract class InventoryTransaction : WXafSequenceObject {
        Shop fShop;
        [DataSourceCriteria("IsActive = True")]
        [RuleRequiredField("PurchaseInvoice_Shop_RuleRequiredField", DefaultContexts.Save)]
        public Shop Shop {
            get { return fShop; }
            set { SetPropertyValue<Shop>(nameof(Shop), ref fShop, value); }
        }
        BasePeriod fPeriod;
        [ModelDefault("AllowEdit", "False")]
        [RuleRequiredField("PurchaseInvoice_Period_RuleRequiredField", DefaultContexts.Save)]
        public BasePeriod Period {
            get { return fPeriod; }
            set { SetPropertyValue<BasePeriod>(nameof(Period), ref fPeriod, value); }
        }        
        DateTime fTransactionDate;
        [ImmediatePostData(true)]
        [RuleRequiredField("PurchaseInvoice_InvoiceDate_RuleRequiredField", DefaultContexts.Save)]
        public DateTime TransactionDate {
            get { return fTransactionDate; }
            set {
                SetPropertyValue<DateTime>(nameof(TransactionDate), ref fTransactionDate, value);
                if (!IsLoading) {
                    Period = BasePeriod.GetOpenedPeriodForDate(ObjectSpace, TransactionDate);
                }
            }
        }
        EnumStatus fStatus;
        [ModelDefault("AllowEdit", "False")]
        public EnumStatus Status {
            get { return fStatus; }
            set { SetPropertyValue<EnumStatus>(nameof(Status), ref fStatus, value); }
        }
        EnumInventoryTransactionType fTransactionType;
        [Browsable(false)]
        public EnumInventoryTransactionType TransactionType {
            get { return fTransactionType; }
            set { SetPropertyValue<EnumInventoryTransactionType>(nameof(TransactionType), ref fTransactionType, value); }
        }
        
        public InventoryTransaction(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TransactionDate = DateTime.Now;
        }

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("InventoryTransaction_Period_IsValid", DefaultContexts.Save, "There is no period oppened for this date")]
        public bool IsPeriodIsValid {
            get { return Period != null && Period.Status == EnumStatus.Opened; }
        }
    }
}
