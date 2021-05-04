using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using CostingApp.Module.BO.Items;
using DevExpress.ExpressApp.Data;

namespace CostingApp.Module.BO.ItemTransactions.Abstraction {

    public interface IAddItems {}

    public interface IAddInventoryItem: IAddItems { }
    public interface IAddMenuItems : IAddItems { }
    public interface IAddPurchaseItems : IAddInventoryItem { }
    public interface IAddStockItems : IAddInventoryItem { }
    public interface IAddSalesItems : IAddInventoryItem { }

    [DomainComponent]
    public class AddItems : IAddPurchaseItems, IXafEntityObject, IObjectSpaceLink, INotifyPropertyChanged {
        private IObjectSpace objectSpace;
        private void OnPropertyChanged(String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public AddItems() {
        }
        [Browsable(false)]
        [Key]
        public long ID { get; set; }
        [Browsable(false)]
        private ItemCard fItem;
        public ItemCard Item {
            get { return fItem; }
            set {
                if (fItem != value) {
                    fItem = value;
                    OnPropertyChanged(nameof(Item));
                }
            }
        }
        public string ItemName { get { return Item.ItemName; } }
        [ModelDefault("AllowEdit", "False")]
        public string Category { get { return Item.Category.ItemCategoryName; } }
        public string StockUnit { get { return Item.StockUnit.UnitName; } }
        public string PurchaseUnit { get { return Item.PurchaseUnit.UnitName; } }
        public string SalesUnit { get { return Item.SalesUnit.UnitName; } }
        private Unit fUnit;
        [DataSourceProperty("Item.UnitType.Units")]
        public Unit Unit {
            get { return fUnit; }
            set {
                if (fUnit != value) {
                    fUnit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }
        private double fQuantity;
        public double Quantity {
            get { return fQuantity; }
            set {
                if (fQuantity != value) {
                    fQuantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }


        #region IXafEntityObject members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIXafEntityObjecttopic.aspx)
        void IXafEntityObject.OnCreated() {
            // Place the entity initialization code here.
            // You can initialize reference properties using Object Space methods; e.g.:
            // this.Address = objectSpace.CreateObject<Address>();
        }
        void IXafEntityObject.OnLoaded() {
            // Place the code that is executed each time the entity is loaded here.
        }
        void IXafEntityObject.OnSaving() {
            // Place the code that is executed each time the entity is saved here.
        }
        #endregion

        #region IObjectSpaceLink members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIObjectSpaceLinktopic.aspx)
        // Use the Object Space to access other entities from IXafEntityObject methods (see https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113707.aspx).
        IObjectSpace IObjectSpaceLink.ObjectSpace {
            get { return objectSpace; }
            set { objectSpace = value; }
        }
        #endregion

        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
    [DomainComponent]
    public class AddPurchaseItems : IAddPurchaseItems, IXafEntityObject, IObjectSpaceLink, INotifyPropertyChanged {
        private IObjectSpace objectSpace;
        private void OnPropertyChanged(String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public AddPurchaseItems() {
        }
        [Browsable(false)]
        [Key]
        public long ID { get; set; }
        private ItemCard fItem;
        [Browsable(false)]
        public ItemCard Item {
            get { return fItem; }
            set {
                if (fItem != value) {
                    fItem = value;
                    OnPropertyChanged(nameof(Item));
                }
            }
        }
        public string ItemName { get { return Item.ItemName; } }
        [ModelDefault("AllowEdit", "False")]
        public string Category { get { return Item.Category.ItemCategoryName; } }
        public string StockUnit { get { return Item.StockUnit != null ? Item.StockUnit.UnitName : null; } }
        public string PurchaseUnit { get { return Item.PurchaseUnit != null ? Item.PurchaseUnit.UnitName : null; } }
        public string SalesUnit { get { return Item.SalesUnit != null ? Item.SalesUnit.UnitName : null; } }
        private Unit fUnit;
        [DataSourceProperty("Item.UnitType.Units")]
        public Unit Unit {
            get { return fUnit; }
            set {
                if (fUnit != value) {
                    fUnit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }
        private double fQuantity;
        public double Quantity {
            get { return fQuantity; }
            set {
                if (fQuantity != value) {
                    fQuantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }


        #region IXafEntityObject members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIXafEntityObjecttopic.aspx)
        void IXafEntityObject.OnCreated() {
            // Place the entity initialization code here.
            // You can initialize reference properties using Object Space methods; e.g.:
            // this.Address = objectSpace.CreateObject<Address>();
        }
        void IXafEntityObject.OnLoaded() {
            // Place the code that is executed each time the entity is loaded here.
        }
        void IXafEntityObject.OnSaving() {
            // Place the code that is executed each time the entity is saved here.
        }
        #endregion

        #region IObjectSpaceLink members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIObjectSpaceLinktopic.aspx)
        // Use the Object Space to access other entities from IXafEntityObject methods (see https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113707.aspx).
        IObjectSpace IObjectSpaceLink.ObjectSpace {
            get { return objectSpace; }
            set { objectSpace = value; }
        }
        #endregion

        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
    [DomainComponent]
    public class AddSalesItems : IAddSalesItems, IXafEntityObject, IObjectSpaceLink, INotifyPropertyChanged {
        private IObjectSpace objectSpace;
        private void OnPropertyChanged(String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public AddSalesItems() {
        }
        [Browsable(false)]
        [Key]
        public long ID { get; set; }
        private ItemCard fItem;
        [Browsable(false)]
        public ItemCard Item {
            get { return fItem; }
            set {
                if (fItem != value) {
                    fItem = value;
                    OnPropertyChanged(nameof(Item));
                }
            }
        }
        public string ItemName { get { return Item.ItemName; } }
        [ModelDefault("AllowEdit", "False")]
        public string Category { get { return Item.Category.ItemCategoryName; } }
        public string StockUnit { get { return Item.StockUnit != null ? Item.StockUnit.UnitName : null; } }
        public string PurchaseUnit { get { return Item.PurchaseUnit != null ? Item.PurchaseUnit.UnitName : null; } }
        public string SalesUnit { get { return Item.SalesUnit != null ? Item.SalesUnit.UnitName : null; } }
        private Unit fUnit;
        [DataSourceProperty("Item.UnitType.Units")]
        public Unit Unit {
            get { return fUnit; }
            set {
                if (fUnit != value) {
                    fUnit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }
        private double fQuantity;
        public double Quantity {
            get { return fQuantity; }
            set {
                if (fQuantity != value) {
                    fQuantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }


        #region IXafEntityObject members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIXafEntityObjecttopic.aspx)
        void IXafEntityObject.OnCreated() {
            // Place the entity initialization code here.
            // You can initialize reference properties using Object Space methods; e.g.:
            // this.Address = objectSpace.CreateObject<Address>();
        }
        void IXafEntityObject.OnLoaded() {
            // Place the code that is executed each time the entity is loaded here.
        }
        void IXafEntityObject.OnSaving() {
            // Place the code that is executed each time the entity is saved here.
        }
        #endregion

        #region IObjectSpaceLink members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIObjectSpaceLinktopic.aspx)
        // Use the Object Space to access other entities from IXafEntityObject methods (see https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113707.aspx).
        IObjectSpace IObjectSpaceLink.ObjectSpace {
            get { return objectSpace; }
            set { objectSpace = value; }
        }
        #endregion

        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
    [DomainComponent]
    public class AddInventoryItems : IAddStockItems, IXafEntityObject, IObjectSpaceLink, INotifyPropertyChanged {
        private IObjectSpace objectSpace;
        private void OnPropertyChanged(String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public AddInventoryItems() {
        }
        [Browsable(false)]
        [Key]
        public long ID { get; set; }
        private ItemCard fItem;
        [Browsable(false)]
        public ItemCard Item {
            get { return fItem; }
            set {
                if (fItem != value) {
                    fItem = value;
                    OnPropertyChanged(nameof(Item));
                }
            }
        }
        public string ItemName { get { return Item.ItemName; } }
        [ModelDefault("AllowEdit", "False")]
        public string Category { get { return Item.Category.ItemCategoryName; } }
        public string StockUnit { get { return Item.StockUnit != null ? Item.StockUnit.UnitName : null; } }
        public string PurchaseUnit { get { return Item.PurchaseUnit != null ? Item.PurchaseUnit.UnitName : null; } }
        public string SalesUnit { get { return Item.SalesUnit != null ? Item.SalesUnit.UnitName : null; } }
        private Unit fUnit;
        [DataSourceProperty("Item.UnitType.Units")]
        public Unit Unit {
            get { return fUnit; }
            set {
                if (fUnit != value) {
                    fUnit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }
        private double fQuantity;
        public double Quantity {
            get { return fQuantity; }
            set {
                if (fQuantity != value) {
                    fQuantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }


        #region IXafEntityObject members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIXafEntityObjecttopic.aspx)
        void IXafEntityObject.OnCreated() {
            // Place the entity initialization code here.
            // You can initialize reference properties using Object Space methods; e.g.:
            // this.Address = objectSpace.CreateObject<Address>();
        }
        void IXafEntityObject.OnLoaded() {
            // Place the code that is executed each time the entity is loaded here.
        }
        void IXafEntityObject.OnSaving() {
            // Place the code that is executed each time the entity is saved here.
        }
        #endregion

        #region IObjectSpaceLink members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIObjectSpaceLinktopic.aspx)
        // Use the Object Space to access other entities from IXafEntityObject methods (see https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113707.aspx).
        IObjectSpace IObjectSpaceLink.ObjectSpace {
            get { return objectSpace; }
            set { objectSpace = value; }
        }
        #endregion

        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}