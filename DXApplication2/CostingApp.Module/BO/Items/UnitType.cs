using CostingApp.Module.CommonLibrary.General.Model;
using CostingApp.Module.CommonLibrary.General.ModelExtenders;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.BO.Items {
    [NavigationItem("Inventory Setup")]
    [ImageName("Unit")]
    public class UnitType : WXafBaseObject {
        string fTypeName;
        [RuleRequiredField("UnitType_TypeName_RuleRequiredField", DefaultContexts.Save)]
        [RuleUniqueValue("UnitType_TypeName_RuleUniqueValue", DefaultContexts.Save)]
        [Appearance("UnitType_TypeName.Enabled", Enabled = false, Criteria = "Items.Count <> 0")]
        public string TypeName {
            get { return fTypeName; }
            set { SetPropertyValue<string>(nameof(TypeName), ref fTypeName, value); }
        }
        [Association("Unit-UnitType"), Aggregated]
        public XPCollection<Unit> Units {
            get { return GetCollection<Unit>(nameof(Units)); }
        }
        [VisibleInDetailView(false)]
        [Association("ItemCard-UnitType"), Aggregated]
        XPCollection<ItemCard> Items {
            get { return GetCollection<ItemCard>(nameof(Items)); }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("UnitType_BasUnit_IsValid", DefaultContexts.Save, "One base unit is required")]
        public bool IsBaseItemIsValid {
            get { return Units.Count(x => x.BaseUnit) == 1; }
        }
        public UnitType(Session session) : base(session) { }
    }

    [RuleCombinationOfPropertiesIsUnique("Unit_UnitType_UnitName.RuleUniqueField", DefaultContexts.Save, "UnitType, UnitName")]
    public class Unit : WXafBaseObject {
        UnitType fUnitType;
        [RuleRequiredField("Unit_UnitType_RuleRequiredField", DefaultContexts.Save)]
        [Association("Unit-UnitType")]
        [ListViewColumnOptions(false, false)]
        public UnitType UnitType {
            get { return fUnitType; }
            set { SetPropertyValue<UnitType>(nameof(UnitType), ref fUnitType, value); }
        }
        string fUnitName;
        [RuleRequiredField("Unit_UnitName_RuleRequiredField", DefaultContexts.Save)]
        [Appearance("Unit_UnitName.Enabled", Enabled = false, Criteria = "UnitType.Items.Count <> 0 && Oid <> -1")]
        [ListViewColumnOptions(false, false)]
        public string UnitName {
            get { return fUnitName; }
            set { SetPropertyValue<string>(nameof(UnitName), ref fUnitName, value); }
        }
        bool isBase;
        [ImmediatePostData(true)]
        [Appearance("Unit_BaseUnit.Enabled", Enabled = false, Criteria = "UnitType.Items.Count <> 0 && Oid <> -1")]
        [ListViewColumnOptions(false, false)]
        public bool BaseUnit {
            get { return isBase; }
            set {
                SetPropertyValue<bool>(nameof(BaseUnit), ref isBase, value);
                if (!IsLoading)
                    ConversionRate = isBase ? 1 : 0;
            }
        }
        double fConversionRate;
        [RuleValueComparison("Unit_ConversionRate_GreaterThan0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        [Appearance("Unit_ConversionRate_Enable", Enabled = false, Criteria = "BaseUnit = True Or (UnitType.Items.Count <> 0 && Oid <> -1)")]
        [ListViewColumnOptions(false, false)]
        public double ConversionRate {
            get { return fConversionRate; }
            set { SetPropertyValue<double>(nameof(ConversionRate), ref fConversionRate, value); }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("Unit_HasItem_IsValid", DefaultContexts.Delete, "Unit type is used and can not updated any more")]
        public bool IsUnitItemsIsValid {
            get {
                if (Session.IsNewObject(this))
                    return true;
                else
                    return ObjectSpace.GetObjects<ItemCard>(CriteriaOperator.Parse("UnitType.Oid = ?", this.UnitType.Oid)).Count == 0;
            }
        }
        public Unit(Session session) : base(session) { }
    }
}
