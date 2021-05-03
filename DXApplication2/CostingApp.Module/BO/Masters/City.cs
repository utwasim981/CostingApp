using CostingApp.Module.CommonLibrary.General.Model;
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

namespace CostingApp.Module.BO.Masters {
    [XafDefaultProperty(nameof(CityName))]
    [ImageName("city")]
    [NavigationItem("Administration")]
    public class City : WXafBaseObject {
        string fCityCode;
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
        [VisibleInDetailView(false)]
        [Association(@"Shop-City"), DevExpress.Xpo.Aggregated]
        public XPCollection<Shop> Shops { get { return GetCollection<Shop>(nameof(Shops)); } }

        public City(Session session) : base(session) { }
        protected override void OnSaving() {
            base.OnSaving();            
        }
    }
}
