
using CostingApp.Module.BO.Masters;
using CostingApp.Module.BO.Masters.Period;
using CostingApp.Module.CommonLibrary.General.Model;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
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
    [Appearance("InventoryTransaction_Posted.Enabled", Enabled = false, TargetItems ="*", Criteria = "Status = 2")]
    public abstract class InventoryTransaction : WXafSequenceObject {
        Shop fShop;
        [DataSourceCriteria("IsActive = True"),
            RuleRequiredField("InventoryTransaction_Shop_RuleRequiredField", DefaultContexts.Save),
            ImmediatePostData(false)]
        public Shop Shop {
            get { return fShop; }
            set { SetPropertyValue<Shop>(nameof(Shop), ref fShop, value); }
        }

        DateTime fTransactionDate;
        [ImmediatePostData(true),
            RuleRequiredField("InventoryTransaction_TransactionDate_RuleRequiredField", DefaultContexts.Save),
            XafDisplayName("Date")]            
        public DateTime TransactionDate {
            get { return fTransactionDate; }
            set { SetPropertyValue<DateTime>(nameof(TransactionDate), ref fTransactionDate, value); }
        }

        BasePeriod fPeriod;
        [ModelDefault("AllowEdit", "False"),
            ImmediatePostData(false),
            RuleRequiredField("InventoryTransaction_Period_RuleRequiredField", DefaultContexts.Save)]
        public BasePeriod Period {
            get { return fPeriod; }
            set { SetPropertyValue<BasePeriod>(nameof(Period), ref fPeriod, value); }
        }
        string fNotes;
        [Size(200)]
        public string Notes {
            get { return fNotes; }
            set { SetPropertyValue<string>(nameof(Notes), ref fNotes, value); }
        }
        EnumStatus fStatus;
        [ModelDefault("AllowEdit", "False")]
        public EnumStatus Status {
            get { return fStatus; }
            set { SetPropertyValue<EnumStatus>(nameof(Status), ref fStatus, value); }
        }

        EnumInventoryRecordType fTransactionType;
        [VisibleInListView(false),
            VisibleInDetailView(false),
            ImmediatePostData(false),
            ModelDefault("AllowEdit", "False")]
        public EnumInventoryRecordType TransactionType {
            get { return fTransactionType; }
            set { SetPropertyValue<EnumInventoryRecordType>(nameof(TransactionType), ref fTransactionType, value); }
        }

        [PersistentAlias("Items.Sum(Amount)")]
        public double Total {
            get { return Convert.ToDouble(EvaluateAlias(nameof(Total))); }
        }
        
        public InventoryTransaction(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TransactionDate = DateTime.Now;
            Status = EnumStatus.Opened;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if(!IsLoading) {
                if (propertyName == nameof(TransactionDate) && oldValue != newValue)
                    Period = BasePeriod.GetOpenedPeriodForDate(ObjectSpace, TransactionDate);
                if (propertyName == nameof(TransactionDate) ||
                    propertyName == nameof(Shop) ||
                    propertyName == nameof(Period) ||
                    (this is InventoryTransfer && propertyName == "Destination") &&
                    oldValue != newValue)
                    UpdateItems();
            }
        }

        

        protected abstract void UpdateItems();
    }    
}
