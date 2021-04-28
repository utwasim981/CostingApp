using CostingApp.Module.BO.Employees;
using CostingApp.Module.CommonLibrary.General.Model;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
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


namespace CostingApp.Module.BO.Masters {
    [XafDefaultProperty(nameof(ShopName))]
    [ImageName("company")]
    [NavigationItem("Administration")]
    public class Shop : WXafSequenceObject {
        City fCity;
        [RuleRequiredField("Shop_City.RuleRequiredField", DefaultContexts.Save)]
        [Association(@"Shop-City")]
        [DataSourceCriteria("IsActive = True")]
        public City City {
            get { return fCity; }
            set { SetPropertyValue<City>(nameof(City), ref fCity, value); }
        }
        private Employee fShopLeader;
        [DataSourceCriteria("IsActive = True")]
        public Employee ShopLeader {
            get { return fShopLeader; }
            set { SetPropertyValue<Employee>(nameof(ShopLeader), ref fShopLeader, value); }
        }
        string fShopCode;
        [Appearance("Shop_ShopCode.Enable", Enabled = false, Criteria = "GetBoolean('ShopCodeA') = True")]
        [RuleRequiredField("Shop_ShopCode_RuleRequiredField", DefaultContexts.Save, TargetCriteria = "GetBoolean('ShopCodeM') = True")]
        [RuleUniqueValue("Shop_ShopCode_RuleUniqueValue", DefaultContexts.Save)]
        public string ShopCode {
            get { return fShopCode; }
            set { SetPropertyValue<string>(nameof(ShopCode), ref fShopCode, value); }
        }
        string fShopName;
        [Size(150)]
        [RuleRequiredField("Shop_ShopName.RuleRequiredField", DefaultContexts.Save)]
        public string ShopName {
            get { return fShopName; }
            set { SetPropertyValue<string>(nameof(ShopName), ref fShopName, value); }
        }
        string fAddress;
        [Size(150)]
        public string Address {
            get { return fAddress; }
            set { SetPropertyValue<string>(nameof(Address), ref fAddress, value); }
        }
        string fPhoneNumber;
        [Size(25)]
        public string PhoneNumber {
            get { return fPhoneNumber; }
            set { SetPropertyValue<string>(nameof(PhoneNumber), ref fPhoneNumber, value); }
        }
        bool fSunOn;
        [ImmediatePostData(true)]
        [XafDisplayName(@"Sunday")]
        [VisibleInListView(false)]
        public bool SunOn {
            get { return fSunOn; }
            set { SetPropertyValue<bool>(nameof(SunOn), ref fSunOn, value); }
        }
        string fSunFrom;
        [Appearance("Shop.SunFrom.Enable", Enabled = false, Criteria = "SunOn = False")]
        [VisibleInListView(false)]
        public string SunFrom {
            get { return fSunFrom; }
            set { SetPropertyValue<string>(nameof(SunFrom), ref fSunFrom, value); }
        }
        string fSunTo;
        [Appearance("Shop.SunTo.Enable", Enabled = false, Criteria = "SunOn = False")]
        [VisibleInListView(false)]
        public string SunTo {
            get { return fSunTo; }
            set { SetPropertyValue<string>(nameof(SunTo), ref fSunTo, value); }
        }
        bool fMonOn;
        [ImmediatePostData(true)]
        [XafDisplayName(@"Monday")]
        [VisibleInListView(false)]
        public bool MonOn {
            get { return fMonOn; }
            set { SetPropertyValue<bool>(nameof(MonOn), ref fMonOn, value); }
        }
        string fMonFrom;
        [Appearance("Shop.MonFrom.Enable", Enabled = false, Criteria = "MonOn = False")]
        [VisibleInListView(false)]
        public string MonFrom {
            get { return fMonFrom; }
            set { SetPropertyValue<string>(nameof(MonFrom), ref fMonFrom, value); }
        }
        string fMonTo;
        [Appearance("Shop.MonTo.Enable", Enabled = false, Criteria = "MonOn = False")]
        [VisibleInListView(false)]
        public string MonTo {
            get { return fMonTo; }
            set { SetPropertyValue<string>(nameof(MonTo), ref fMonTo, value); }
        }
        bool fTusOn;
        [ImmediatePostData(true)]
        [XafDisplayName(@"Tuesday")]
        [VisibleInListView(false)]
        public bool TusOn {
            get { return fTusOn; }
            set { SetPropertyValue<bool>(nameof(TusOn), ref fTusOn, value); }
        }
        string fTusFrom;
        [Appearance("Shop.TusFrom.Enable", Enabled = false, Criteria = "TusOn = False")]
        [VisibleInListView(false)]
        public string TusFrom {
            get { return fTusFrom; }
            set { SetPropertyValue<string>(nameof(TusFrom), ref fTusFrom, value); }
        }
        string fTusTo;
        [Appearance("Shop.TusTo.Enable", Enabled = false, Criteria = "TusOn = False")]
        [VisibleInListView(false)]
        public string TusTo {
            get { return fTusTo; }
            set { SetPropertyValue<string>(nameof(TusTo), ref fTusTo, value); }
        }
        bool fWedOn;
        [ImmediatePostData(true)]
        [XafDisplayName(@"Wednesday")]
        [VisibleInListView(false)]
        public bool WedOn {
            get { return fWedOn; }
            set { SetPropertyValue<bool>(nameof(WedOn), ref fWedOn, value); }
        }
        string fWedFrom;
        [Appearance("Shop.WedFrom.Enable", Enabled = false, Criteria = "WedOn = False")]
        [VisibleInListView(false)]
        public string WedFrom {
            get { return fWedFrom; }
            set { SetPropertyValue<string>(nameof(WedFrom), ref fWedFrom, value); }
        }
        string fWedTo;
        [Appearance("Shop.WedTo.Enable", Enabled = false, Criteria = "WedOn = False")]
        [VisibleInListView(false)]
        public string WedTo {
            get { return fWedTo; }
            set { SetPropertyValue<string>(nameof(WedTo), ref fWedTo, value); }
        }
        bool fThuOn;
        [ImmediatePostData(true)]
        [XafDisplayName(@"Thursday")]
        [VisibleInListView(false)]
        public bool ThuOn {
            get { return fThuOn; }
            set { SetPropertyValue<bool>(nameof(ThuOn), ref fThuOn, value); }
        }
        string fThuFrom;
        [Appearance("Shop.ThuFrom.Enable", Enabled = false, Criteria = "ThuOn = False")]
        [VisibleInListView(false)]
        public string ThuFrom {
            get { return fThuFrom; }
            set { SetPropertyValue<string>(nameof(ThuFrom), ref fThuFrom, value); }
        }
        string fThuTo;
        [Appearance("Shop.ThuTo.Enable", Enabled = false, Criteria = "ThuOn = False")]
        [VisibleInListView(false)]
        public string ThuTo {
            get { return fThuTo; }
            set { SetPropertyValue<string>(nameof(ThuTo), ref fThuTo, value); }
        }
        bool fFriOn;
        [ImmediatePostData(true)]
        [XafDisplayName(@"Friday")]
        [VisibleInListView(false)]
        public bool FriOn {
            get { return fFriOn; }
            set { SetPropertyValue<bool>(nameof(FriOn), ref fFriOn, value); }
        }
        string fFriFrom;
        [Appearance("Shop.FriFrom.Enable", Enabled = false, Criteria = "FriOn = False")]
        [VisibleInListView(false)]
        public string FriFrom {
            get { return fFriFrom; }
            set { SetPropertyValue<string>(nameof(FriFrom), ref fFriFrom, value); }
        }
        string fFriTo;
        [Appearance("Shop.FriTo.Enable", Enabled = false, Criteria = "FriOn = False")]
        [VisibleInListView(false)]
        public string FriTo {
            get { return fFriTo; }
            set { SetPropertyValue<string>(nameof(FriTo), ref fFriTo, value); }
        }
        bool fSatOn;
        [ImmediatePostData(true)]
        [XafDisplayName(@"Saturday")]
        [VisibleInListView(false)]
        public bool SatOn {
            get { return fSatOn; }
            set { SetPropertyValue<bool>(nameof(SatOn), ref fSatOn, value); }
        }
        string fSatFrom;
        [Appearance("Shop.SatFrom.Enable", Enabled = false, Criteria = "SatOn = False")]
        [VisibleInListView(false)]
        public string SatFrom {
            get { return fSatFrom; }
            set { SetPropertyValue<string>(nameof(SatFrom), ref fSatFrom, value); }
        }
        string fSatTo;
        [Appearance("Shop.SatTo.Enable", Enabled = false, Criteria = "SatOn = False")]
        [VisibleInListView(false)]
        public string SatTo {
            get { return fSatTo; }
            set { SetPropertyValue<string>(nameof(SatTo), ref fSatTo, value); }
        }
        public Shop(Session session) : base(session) { }
        protected override string GetSequenceName() {            
            return string.Concat(ClassInfo.FullName, "-", City.CityCode.Replace(" ", "_"));
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(SequentialNumber))
                    onSequentialNumberValueChange(oldValue, newValue);                    
            }
        }

        private void onSequentialNumberValueChange(object oldValue, object newValue) {
            if (oldValue != newValue &&
                (bool)ValueManager.GetValueManager<Dictionary<string, object>>("Values").Value["ShopCodeA"])
                ShopCode = string.Format("SH-{0}-{1}", City.CityCode, SequentialNumber.ToString().PadLeft(4, '0'));
        }
    }
}
