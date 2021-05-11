using CostingApp.Module.BO.Masters;
using CostingApp.Module.CommnLibrary.General;
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

namespace CostingApp.Module.BO.Employees {
    [XafDefaultProperty(nameof(FullName))]
    [ImageName("employee")]
    [NavigationItem("Employees Setup")]
    public class Employee : WXafBaseObject {
        ContractType fContractType;
        [XafDisplayName("Contract Type")]
        [RuleRequiredField("Employee_ContractType_RuleRequiredField", DefaultContexts.Save)]
        [DataSourceCriteria("IsActive = True")]
        [ImmediatePostData(true)]
        public ContractType ContractType {
            get { return fContractType; }
            set { SetPropertyValue<ContractType>(nameof(ContractType), ref fContractType, value); }
        }
        Position fPosition;
        [XafDisplayName("Position")]
        [RuleRequiredField("Employee_Position_RuleRequiredField", DefaultContexts.Save)]
        [DataSourceCriteria("IsActive = True")]
        public Position Position {
            get { return fPosition; }
            set { SetPropertyValue<Position>(nameof(Position), ref fPosition, value); }
        }
        Shop fDefaultShop;
        [DataSourceCriteria("IsActive = True")]
        public Shop DefaultShop {
            get { return fDefaultShop; }
            set { SetPropertyValue<Shop>(nameof(DefaultShop), ref fDefaultShop, value); }
        }
        string fEmployeeCode;
        [RuleUniqueValue("Employee_EmployeeCode_RuleUniqueValue", DefaultContexts.Save)]
        public string EmployeeCode {
            get { return fEmployeeCode; }
            set { SetPropertyValue<string>(nameof(EmployeeCode), ref fEmployeeCode, value); }
        }
        string fFirstName;
        [Size(50)]
        [ImmediatePostData(true)]
        [RuleRequiredField("Employee_FirstName_RuleRequiredField", DefaultContexts.Save)]
        [ValueConverter(typeof(StringTrimingValueConvertor))]
        public string FirstName {
            get { return fFirstName; }
            set {
                SetPropertyValue<string>(nameof(FirstName), ref fFirstName, value);
                //FullName = calcFullName();
            }
        }
        string fLastName;
        [Size(50)]
        [ImmediatePostData(true)]
        [RuleRequiredField("Employee_LastName_RuleRequiredField", DefaultContexts.Save)]
        [ValueConverter(typeof(StringTrimingValueConvertor))]
        public string LastName {
            get { return fLastName; }
            set {
                SetPropertyValue<string>(nameof(LastName), ref fLastName, value);
                //FullName = calcFullName();
            }
        }        
        [XafDisplayName("Employee Name")]
        [PersistentAlias("concat(FirstName, ' ', LastName)")]
        public string FullName {
            get { return Convert.ToString(EvaluateAlias(nameof(FullName))); }
        }
        EnumGender fGender;
        public EnumGender Gender {
            get { return fGender; }
            set { SetPropertyValue<EnumGender>(nameof(Gender), ref fGender, value); }
        }
        double fSalaryPerHour;
        [Appearance("Employee_SalaryPerHour_Enable", Enabled = false, Context = "DetailView", Criteria = "IsNull(ContractType) Or ContractType.SalaryType = 1")]
        [RuleValueComparison("Employee_SalaryPerHour_GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "ContractType.SalaryType = 0")]
        public double SalaryPerHour {
            get { return fSalaryPerHour; }
            set { SetPropertyValue<double>(nameof(SalaryPerHour), ref fSalaryPerHour, value); }
        }
        double fMonthlySalary;
        [Appearance("Employee_MonthlySalary_Enable", Enabled = false, Context = "DetailView", Criteria = "IsNull(ContractType) Or ContractType.SalaryType = 0")]
        [RuleValueComparison(@"Employee_MonthlySalary_GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "ContractType.SalaryType = 1")]
        public double MonthlySalary {
            get { return fMonthlySalary; }
            set { SetPropertyValue<double>(nameof(MonthlySalary), ref fMonthlySalary, value); }
        }
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit, DetailViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorFixedHeight = 200, DetailViewImageEditorFixedWidth = 200,
            ListViewImageEditorCustomHeight = 100)]
        public byte[] EmployeePhoto {
            get { return GetPropertyValue<byte[]>(nameof(EmployeePhoto)); }
            set { SetPropertyValue<byte[]>(nameof(EmployeePhoto), value); }
        }
        public Employee(Session session) : base(session) { }
        private string calcFullName() {
            return ObjectFormatter.Format("{FirstName} {LastName}", this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
        }
    }
}
