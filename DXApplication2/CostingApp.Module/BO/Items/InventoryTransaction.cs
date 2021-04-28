
using CostingApp.Module.BO.Masters;
using CostingApp.Module.BO.Masters.Period;
using CostingApp.Module.CommonLibrary.General.Model;
using DevExpress.ExpressApp;
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

namespace CostingApp.Module.BO.Items {
    public abstract class InventoryTransaction : WXafSequenceObject {
        Shop fShop;
        [DataSourceCriteria("IsActive = True")]
        [RuleRequiredField("PurchaseInvoice_Shop_RuleRequiredField", DefaultContexts.Save)]
        [ImmediatePostData(false)]
        public Shop Shop {
            get { return fShop; }
            set { SetPropertyValue<Shop>(nameof(Shop), ref fShop, value); }
        }
        BasePeriod fPeriod;
        [ModelDefault("AllowEdit", "False")]
        [ImmediatePostData(false)]
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
            set { SetPropertyValue<DateTime>(nameof(TransactionDate), ref fTransactionDate, value); }
        }
        EnumStatus fStatus;
        [ModelDefault("AllowEdit", "False")]
        public EnumStatus Status {
            get { return fStatus; }
            set { SetPropertyValue<EnumStatus>(nameof(Status), ref fStatus, value); }
        }
        EnumInventoryTransactionType fTransactionType;
        [Browsable(false)]
        [ImmediatePostData(false)]
        public EnumInventoryTransactionType TransactionType {
            get { return fTransactionType; }
            set { SetPropertyValue<EnumInventoryTransactionType>(nameof(TransactionType), ref fTransactionType, value); }
        }
        
        public InventoryTransaction(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TransactionDate = DateTime.Now;
            Status = EnumStatus.Opened;
        }

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("InventoryTransaction_Period_IsValid", DefaultContexts.Save, "There is no period oppened for this date")]
        public bool IsPeriodIsValid {
            get { return Period != null && Period.Status == EnumStatus.Opened; }
        }
    }

    public class PurchaseInvoiceCheckShop : ViewController {
        public PurchaseInvoiceCheckShop() {
            TargetObjectType = typeof(InventoryTransaction);
            TargetViewType = ViewType.DetailView;
        }
        protected override void OnActivated() {
            base.OnActivated();
            ListPropertyEditor itemListPropertyEditor = itemListPropertyEditor = ((DetailView)View).FindItem("Items") as ListPropertyEditor;
            //if (View.CurrentObject is PurchaseInvoice)
                
            //else if (View.CurrentObject is SalesRecord)
            //    itemListPropertyEditor = ((DetailView)View).FindItem("Items") as ListPropertyEditor;
            //else if (View.CurrentObject is InventoryAdjustment)
            if (itemListPropertyEditor != null)
                itemListPropertyEditor.ControlCreated += ItemListPropertyEditor_ControlCreated;

        }

        private void ItemListPropertyEditor_ControlCreated(object sender, EventArgs e) {
            ListPropertyEditor itemListPropertyEditor = (ListPropertyEditor)sender;
            Frame listViewFrame = itemListPropertyEditor.Frame;
            ListView nestedListView = itemListPropertyEditor.ListView;
            var controller = listViewFrame.GetController<NewObjectViewController>();
            controller.ObjectCreating += Controller_ObjectCreating;
        }

        private void Controller_ObjectCreating(object sender, ObjectCreatingEventArgs e) {
            if (((InventoryTransaction)View.CurrentObject).Shop == null || ((InventoryTransaction)View.CurrentObject).Period == null) {
                e.Cancel = true;
                MessageOptions options = new MessageOptions();
                options.Duration = 4000;
                options.Message = "Please cheose the shop and date before adding any item";
                options.Type = InformationType.Warning;
                options.Web.Position = InformationPosition.Top;
                options.Win.Caption = "Warrning";
                options.Win.Type = WinMessageType.Flyout;
                Application.ShowViewStrategy.ShowMessage(options);
            }
        }
    }
}
