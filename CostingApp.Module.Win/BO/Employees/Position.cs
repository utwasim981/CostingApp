using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib;
using WXafLib.General.Model;
using WXafLib.General.Security;

namespace CostTech.Module.Win.BO.Employees {
    [XafDefaultProperty(nameof(PositionName))]
    [NavigationItem("Employees Setup")]
    [ImageName("BO_Position")]
    public class Position : WXafBaseObject {
        string fPositionName;
        [Size(150)]
        [RuleRequiredField("Position_PositionName_RuleRequiredField", DefaultContexts.Save)]
        [RuleUniqueValue("Position_PositionName_RuleUniqueValue", DefaultContexts.Save)]
        public string PositionName {
            get { return fPositionName; }
            set { SetPropertyValue<string>(nameof(PositionName), ref fPositionName, value); }
        }
        public Position(Session session) : base(session) { }
    }
}
