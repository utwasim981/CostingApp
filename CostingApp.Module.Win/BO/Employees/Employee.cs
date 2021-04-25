using CostingApp.Module.Win.BO;
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
using WXafLib;
using WXafLib.General.Model;
using WXafLib.General.Security;

namespace CostTech.Module.Win.BO.Employees {
    [XafDefaultProperty(nameof(FullName))]
    [ImageName("employee")]
    [NavigationItem("Employees Setup")]
    public class Employee : WXafSequenceObject {
        ContractType fContractType;
        [XafDisplayName("Contract Type")]
        [RuleRequiredField("Employee_ContractType_RuleRequiredField", DefaultContexts.Save)]
        [DataSourceCriteria("IsActive = True")]
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
        string fEmployeeCode;
        [Appearance("Employee_EmployeeCode.Enable", Enabled = false, Criteria = "GetBoolean('EmployeeCodeA') = True")]
        [RuleRequiredField("Employee_EmployeeCode_RuleRequiredField", DefaultContexts.Save, TargetCriteria = "GetBoolean('EmployeeCodeM') = True")]
        [RuleUniqueValue("Employee_EmployeeCode_RuleUniqueValue", DefaultContexts.Save)]
        public string EmployeeCode {
            get { return fEmployeeCode; }
            set { SetPropertyValue<string>(nameof(EmployeeCode), ref fEmployeeCode, value); }
        }
        string fFirstName;
        [Size(50)]
        [ImmutableObject(true)]
        [RuleRequiredField("Employee_FirstName_RuleRequiredField", DefaultContexts.Save)]
        public string FirstName {
            get { return fFirstName; }
            set {
                SetPropertyValue<string>(nameof(FirstName), ref fFirstName, value);
                FullName = calcFullName();
            }
        }
        string fLastName;
        [Size(50)]
        [ImmutableObject(true)]
        [RuleRequiredField("Employee_LastName_RuleRequiredField", DefaultContexts.Save)]
        public string LastName {
            get { return fLastName; }
            set {
                SetPropertyValue<string>(nameof(LastName), ref fLastName, value);
                FullName = calcFullName();
            }
        }
        string fFullName;
        [Size(150)]
        [ModelDefault("AllowEdit", "False")]
        [RuleUniqueValue("Employee_FullName_RuleUniqueValue", DefaultContexts.Save)]
        public string FullName {
            get { return fFullName; }
            set {SetPropertyValue<string>(nameof(FullName), ref fFullName, value); }
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
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit, DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
        public byte[] EmployeePhoto {
            get { return GetPropertyValue<byte[]>(nameof(EmployeePhoto)); }
            set { SetPropertyValue<byte[]>(nameof(EmployeePhoto), value); }
        }
        public Employee(Session session) : base(session) { }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading) {
                if (propertyName == nameof(SequentialNumber))
                    onSequentialNumberValueChange(oldValue, newValue);
            }
        }

        private void onSequentialNumberValueChange(object oldValue, object newValue) {
            if (oldValue != newValue &&
                (bool)ValueManager.GetValueManager<Dictionary<string, object>>("Values").Value["EmployeeCodeA"])
                EmployeeCode = SequentialNumber.ToString().PadLeft(4, '0');
        }
        private string calcFullName() {
            return ObjectFormatter.Format("{FirstName} {LastName}", this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
        }
    }
}
