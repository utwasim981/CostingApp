using CostingApp.Module.BO;
using CostingApp.Module.CommonLibrary.General.Model;
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


namespace CostingApp.Module.BO.Employees {
    [XafDefaultProperty(nameof(ContractTypeName))]
    [NavigationItem("Employees Setup")]
    [ImageName("BO_Contract")]
    public class ContractType : WXafBaseObject {
        string fContractTypeName;
        [Size(150)]
        [RuleRequiredField("ContractType_ContractTypeName_RuleUniqueValue", DefaultContexts.Save)]
        [RuleRequiredField("ContractTypeName_ContractTypeNameName_RuleUniqueValue", DefaultContexts.Save)]
        public string ContractTypeName {
            get { return fContractTypeName; }
            set { SetPropertyValue<string>(nameof(ContractTypeName), ref fContractTypeName, value); }
        }
        EnumSalaryType fSalaryType;
        public EnumSalaryType SalaryType {
            get { return fSalaryType; }
            set { SetPropertyValue<EnumSalaryType>(nameof(SalaryType), ref fSalaryType, value); }
        }
        public ContractType(Session session) : base(session) { }
    }

}
