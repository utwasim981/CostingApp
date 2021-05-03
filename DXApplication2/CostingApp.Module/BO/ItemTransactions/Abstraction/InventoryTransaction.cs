using CostingApp.Module.BO.Masters;
using CostingApp.Module.BO.Masters.Period;
using CostingApp.Module.CommonLibrary.General.Model;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;

namespace CostingApp.Module.BO.ItemTransactions.Abstraction {
    [Appearance("InventoryTransaction_Posted.Enabled", Enabled = false, TargetItems = "*", Criteria = "Status = 2")]
    public abstract class InventoryTransaction : WXafSequenceObject {

        public abstract EnumInventoryTransactionType TransactionType { get; }

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
        [PersistentAlias("Items.Sum(Amount)")]
        public double Total {
            get { return Convert.ToDouble(EvaluateAlias(nameof(Total))); }
        }

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("InventoryTransaction_Period_IsValid", DefaultContexts.Save, "There is no period oppened for this date")]
        public bool IsPeriodIsValid {
            get { return Period != null && Period.Status == EnumStatus.Opened; }
        }

        public InventoryTransaction(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            TransactionDate = DateTime.Now;
            Status = EnumStatus.Opened;
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(TransactionDate) && oldValue != newValue)
                    Period = BasePeriod.GetOpenedPeriodForDate(ObjectSpace, TransactionDate);
                if (propertyName == nameof(TransactionDate) ||
                    propertyName == nameof(Shop) ||
                    propertyName == nameof(Period) ||
                    oldValue != newValue)
                    OnMasterDataChanged();
            }
        }
        protected virtual void OnMasterDataChanged() {
            foreach(var member in ClassInfo.Members) {
                if (member.IsCollection && 
                    (member.CollectionElementType.BaseClass.ClassType == typeof(InputInventoryRecord) ||
                    (member.CollectionElementType.BaseClass.ClassType == typeof(OutputInventoryRecord) && TransactionType != EnumInventoryTransactionType.InventoryTransfer)))
                    foreach(var obj in member.GetValue(this) as IList) {
                        var item = (InventoryRecord)obj;
                        if (item.Date != TransactionDate)
                            item.Date = TransactionDate;
                        if (item.Shop != Shop)
                            item.Shop = Shop;
                        if (item.Period != Period)
                            item.Period = Period;
                    }
            }
        }
    }

    public class InventoryTransacrionCheckShop : ViewController {
        public InventoryTransacrionCheckShop() {
            TargetObjectType = typeof(InventoryTransaction);
            TargetViewType = ViewType.DetailView;
        }
        ListPropertyEditor itemListPropertyEditor;
        NewObjectViewController newController;
        protected override void OnActivated() {
            base.OnActivated();
            foreach (var member in ((InventoryTransaction)View.CurrentObject).ClassInfo.Members) {
                if (member.IsCollection &&
                    (member.CollectionElementType.BaseClass.ClassType == typeof(InputInventoryRecord) ||
                     member.CollectionElementType.BaseClass.ClassType == typeof(OutputInventoryRecord))) {
                    itemListPropertyEditor = ((DetailView)View).FindItem(member.Name) as ListPropertyEditor;
                    if (itemListPropertyEditor != null)
                        itemListPropertyEditor.ControlCreated += ItemListPropertyEditor_ControlCreated;
                }
            }
        }
        protected override void OnDeactivated() {
            if (newController != null)
                newController.ObjectCreating -= Controller_ObjectCreating;
            if (itemListPropertyEditor != null)
                itemListPropertyEditor.ControlCreated -= ItemListPropertyEditor_ControlCreated;
            base.OnDeactivated();
        }
        private void ItemListPropertyEditor_ControlCreated(object sender, EventArgs e) {
            ListPropertyEditor itemListPropertyEditor = (ListPropertyEditor)sender;
            Frame listViewFrame = itemListPropertyEditor.Frame;
            ListView nestedListView = itemListPropertyEditor.ListView;
            newController = listViewFrame.GetController<NewObjectViewController>();
            newController.ObjectCreating += Controller_ObjectCreating;
        }
        private void Controller_ObjectCreating(object sender, ObjectCreatingEventArgs e) {
            foreach (var member in ((InventoryTransaction)View.CurrentObject).ClassInfo.Members) {
                if (member.MemberType == typeof(Shop) && member.GetValue(View.CurrentObject) == null) {
                    e.Cancel = true;
                    MessageOptions options = new MessageOptions();
                    options.Duration = 4000;
                    options.Message = "Please cheose the shop and date before adding any item";
                    options.Type = InformationType.Warning;
                    options.Web.Position = InformationPosition.Top;
                    options.Win.Caption = "Warrning";
                    options.Win.Type = WinMessageType.Flyout;
                    Application.ShowViewStrategy.ShowMessage(options);
                    break;
                }
            }


                //if (((InventoryTransaction)View.CurrentObject).Shop == null || ((InventoryTransaction)View.CurrentObject).Period == null ||
                //(View.CurrentObject.GetType().GetProperty("Destination") != null && View.CurrentObject.GetType().GetProperty("Destination").GetValue(View.CurrentObject) == null)) {
                
        }
    }
}
