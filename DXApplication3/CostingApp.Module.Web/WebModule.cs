﻿using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Persistent.BaseImpl;
using CostingApp.Module.BO.ItemTransactions.Abstraction;
using CostingApp.Module.BO.Items;
using DevExpress.Data.Filtering;
using CostingApp.Module.BO;

namespace CostingApp.Module.Web {
    [ToolboxItemFilter("Xaf.Platform.Web")]
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class CostingAppAspNetModule : ModuleBase {
        public object[] EnumItemType { get; private set; }

        //private void Application_CreateCustomModelDifferenceStore(Object sender, CreateCustomModelDifferenceStoreEventArgs e) {
        //    e.Store = new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), true, "Web");
        //    e.Handled = true;
        //}
        private void Application_CreateCustomUserModelDifferenceStore(Object sender, CreateCustomModelDifferenceStoreEventArgs e) {
            e.Store = new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), false, "Web");
            e.Handled = true;
        }
        public CostingAppAspNetModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            return ModuleUpdater.EmptyModuleUpdaters;
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            application.SetupComplete += Application_SetupComplete;
        }

        private void Application_SetupComplete(object sender, EventArgs e) {
            Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;

        }
        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e) {
            var nonPersistentObjectSpace = e.ObjectSpace as NonPersistentObjectSpace;
            if (nonPersistentObjectSpace != null) {
                nonPersistentObjectSpace.ObjectsGetting += NonPersistentObjectSpace_ObjectsGetting;
            }
        }

        private void NonPersistentObjectSpace_ObjectsGetting(object sender, ObjectsGettingEventArgs e) {
            if (e.ObjectType == typeof(AddInventoryItems))
                e.Objects = FillAddInventoryItems();
            else if (e.ObjectType == typeof(AddPurchaseItems))
                e.Objects = FillAddPurchaseItems();
            else if (e.ObjectType == typeof(AddSalesItems))
                e.Objects = FillAddSalesItems();
        }

        private BindingList<AddInventoryItems> FillAddInventoryItems() {
            BindingList<AddInventoryItems> objects = new BindingList<AddInventoryItems>();
            var os = Application.CreateObjectSpace();
            IList<ItemCard> items = null;
            int index = 0;
            try {
                items = os.GetObjects<ItemCard>(CriteriaOperator.Parse("ItemType = ? And IsActive = ?", EnumItemCard.InventoryItem, true));
                foreach (var item in items) {
                    var addItem = new AddInventoryItems();
                    addItem.ID = item.Oid;
                    addItem.Item = item;
                    addItem.Unit = item.StockUnit;
                    addItem.Quantity = 0;
                    objects.Add(addItem);
                    index++;
                }
                return objects;
            }
            catch(Exception ex) {
                return null;
            }
        }
        private BindingList<AddPurchaseItems> FillAddPurchaseItems() {
            BindingList<AddPurchaseItems> objects = new BindingList<AddPurchaseItems>();
            var os = Application.CreateObjectSpace();
            IList<ItemCard> items = null;
            int index = 0;
            try {
                items = os.GetObjects<ItemCard>(CriteriaOperator.Parse("ItemType = ? And IsActive = ?", EnumItemCard.InventoryItem, true));
                foreach (var item in items) {
                    var addItem = new AddPurchaseItems();
                    addItem.ID = item.Oid;
                    addItem.Item = item;
                    addItem.Unit = item.PurchaseUnit;
                    addItem.Quantity = 0;
                    objects.Add(addItem);
                    index++;
                }
                return objects;
            }
            catch (Exception ex) {
                return null;
            }
        }
        private BindingList<AddSalesItems> FillAddSalesItems() {
            BindingList<AddSalesItems> objects = new BindingList<AddSalesItems>();
            var os = Application.CreateObjectSpace();
            IList<ItemCard> items = null;
            int index = 0;
            try {
                items = os.GetObjects<ItemCard>(CriteriaOperator.Parse("ItemType = ? And IsActive = ?", EnumItemCard.InventoryItem, true));
                foreach (var item in items) {
                    var addItem = new AddSalesItems();
                    addItem.ID = item.Oid;
                    addItem.Item = item;
                    addItem.Unit = item.SalesUnit;
                    addItem.Quantity = 0;
                    objects.Add(addItem);
                    index++;
                }
                return objects;
            }
            catch (Exception ex) {
                return null;
            }
        }
    }
}
