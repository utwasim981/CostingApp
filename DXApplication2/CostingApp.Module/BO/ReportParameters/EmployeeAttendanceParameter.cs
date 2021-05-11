using CostingApp.Module.BO.Employees;
using CostingApp.Module.BO.Masters;
using CostingApp.Module.CommnLibrary.ReportParameters;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.Base;
using System;
using System.ComponentModel;
using DevExpress.Xpo;

namespace CostingApp.Module.BO.ReportParameters {
    //[DomainComponent]
    //public class EmployeeAttendanceParameter : DateRangeParameterObject {
    //    public EmployeeAttendanceParameter(IObjectSpaceCreator objectSpaceCreator) : base(objectSpaceCreator) {
    //    }

    //    private Shop fShop;
    //    [ImmediatePostData(true)]
    //    public Shop Shop {
    //        get { return fShop; }
    //        set {
    //            if (value != fShop) {
    //                fShop = value;
    //            }
    //        }
    //    }

    //    private Employee fEmployee;
    //    [ImmediatePostData(true)]
    //    public Employee Employee {
    //        get { return fEmployee; }
    //        set {
    //            if (value != fEmployee) {
    //                fEmployee = value;
    //            }
    //        }
    //    }

    //    public override CriteriaOperator GetCriteria() {
    //        var co = base.GetCriteria();
    //        if (Shop != null)
    //            co = CriteriaOperator.And(co, new BinaryOperator("Shop", Shop, BinaryOperatorType.Equal));
    //        if (Employee != null)
    //            co = CriteriaOperator.And(co, new BinaryOperator("Employee", Employee, BinaryOperatorType.Equal));
    //        return co;
    //    }
    //}

    [DomainComponent]
    public class AttendanceSummeryParameter : DateRangeParameterObject {

        public City City { get; set; }
        [DataSourceProperty("City.Shops",
            DataSourcePropertyIsNullMode.SelectAll)]
        public Shop Shop { get; set; }

        public override CriteriaOperator GetCriteria() {
            CriteriaOperator co = base.GetCriteria();
            if (City != null)
                co = CriteriaOperator.And(co, new BinaryOperator("Shop.City", City, BinaryOperatorType.Equal));
            if (Shop != null)
                co = CriteriaOperator.And(co, new BinaryOperator("Shop", Shop, BinaryOperatorType.Equal));
            return co;
        }

        public AttendanceSummeryParameter(IObjectSpaceCreator objectSpaceCreator) : base(objectSpaceCreator) {
        }
    }
    [DomainComponent]
    public class AttendanceDetailParamerter : DateRangeParameterObject {
        public Shop Shop { get; set; }
        public Employee Employee { get; set; }
        public override CriteriaOperator GetCriteria() {
            CriteriaOperator co = base.GetCriteria();
            if (Shop != null)
                co = CriteriaOperator.And(co, new BinaryOperator("Shop", Shop, BinaryOperatorType.Equal));
            if (Employee != null)
                co = CriteriaOperator.And(co, new BinaryOperator("Employee", Employee, BinaryOperatorType.Equal));
            return co;
        }
        public AttendanceDetailParamerter(IObjectSpaceCreator objectSpaceCreator) : base(objectSpaceCreator) {
        }
    }

}
