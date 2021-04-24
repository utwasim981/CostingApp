using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Kpi;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib;
using WXafLib.General.Model;
using WXafLib.General.Security;

namespace CostingApp.Module.Win.BO.Masters {
    [XafDefaultProperty(nameof(CityName))]
    [NavigationItem("Setup")]
    public class City : WXafSequenceObject {
        string fCityCode;
        [Appearance("City_CityCode.Enable", Enabled = false, Criteria = "GetBoolean('CityCodeA') = True")]
        [RuleRequiredField("City_CityCode_RuleRequiredField", DefaultContexts.Save, TargetCriteria = "GetBoolean('CityCodeM') = True")]
        [RuleUniqueValue("City_CityCode_RuleUniqueValue", DefaultContexts.Save)]
        public string CityCode {
            get { return fCityCode; }
            set { SetPropertyValue<string>(nameof(CityCode), ref fCityCode, value); }
        }
        string fCityName;
        [Size(150)]
        [RuleRequiredField("City_CityName_RuleRequiredField", DefaultContexts.Save)]
        [RuleUniqueValue("City_CityName_RuleUniqueValue", DefaultContexts.Save)]
        public string CityName {
            get { return fCityName; }
            set { SetPropertyValue<string>(nameof(CityName), ref fCityName, value); }
        }
        private bool fIsActive;
        [ImmediatePostData(true)]
        [XafDisplayName(@"Active")]
        [VisibleInDetailView(false)]
        [Association(@"Shop-City"), DevExpress.Xpo.Aggregated]
        public XPCollection<Shop> Shops { get { return GetCollection<Shop>(nameof(Shops)); } }

        public City(Session session) : base(session) { }
        protected override void OnSaving() {
            base.OnSaving();            
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(SequentialNumber) && oldValue != newValue &&
                    (bool)ValueManager.GetValueManager<Dictionary<string, object>>("Values").Value["CityCodeA"])
                    CityCode = SequentialNumber.ToString().PadLeft(4, '0');
            }
        }
    }
}
