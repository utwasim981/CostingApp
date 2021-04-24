using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXafLib.General.CriteriaOperators {
    public class IsBoolValueFunctionOperator : ICustomFunctionOperator {
        public string Name {
            get {
                return "GetBoolean";
            }
        }
        public object Evaluate(params object[] operands) {
            var value = ValueManager.GetValueManager<Dictionary<string, object>>("Values");
            return (bool)value.Value[operands[0].ToString()];
        }

        public Type ResultType(params Type[] operands) {
            return typeof(bool);
        }
        static IsBoolValueFunctionOperator() {
            IsBoolValueFunctionOperator instance = new IsBoolValueFunctionOperator();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null) {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }
        public static void Register() { }
    }
    public class IsStringValueFunctionOperator : ICustomFunctionOperator {
        public string Name {
            get {
                return "GetString";
            }
        }
        public object Evaluate(params object[] operands) {
            var value = ValueManager.GetValueManager<Dictionary<string, object>>("Values");
            return value.Value[operands[0].ToString()].ToString();
        }

        public Type ResultType(params Type[] operands) {
            return typeof(string);
        }
        static IsStringValueFunctionOperator() {
            IsStringValueFunctionOperator instance = new IsStringValueFunctionOperator();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null) {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }
        public static void Register() { }
    }
    public class IsNumberValueFunctionOperator : ICustomFunctionOperator {
        public string Name {
            get {
                return "GetNumeric";
            }
        }
        public object Evaluate(params object[] operands) {
            var value = ValueManager.GetValueManager<Dictionary<string, object>>("Values");
            return Convert.ToDouble(value.Value[operands[0].ToString()]);
        }

        public Type ResultType(params Type[] operands) {
            return typeof(double);
        }
        static IsNumberValueFunctionOperator() {
            IsNumberValueFunctionOperator instance = new IsNumberValueFunctionOperator();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null) {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }
        public static void Register() { }
    }
    public class IsDateValueFunctionOperator : ICustomFunctionOperator {
        public string Name {
        get {
            return "GetDate";
        }
    }
    public object Evaluate(params object[] operands) {
            var value = ValueManager.GetValueManager<Dictionary<string, object>>("Values");
            return Convert.ToDateTime(value.Value[operands[0].ToString()]);
        }

    public Type ResultType(params Type[] operands) {
        return typeof(DateTime);
    }
    static IsDateValueFunctionOperator() {
            IsDateValueFunctionOperator instance = new IsDateValueFunctionOperator();
        if (CriteriaOperator.GetCustomFunction(instance.Name) == null) {
            CriteriaOperator.RegisterCustomFunction(instance);
        }
    }
    public static void Register() { }
}
    public class ISEnumValueFunctionOperator : ICustomFunctionOperator {
        public string Name {
            get {
                return "GetEnum";
            }
        }
        public object Evaluate(params object[] operands) {
            var value = ValueManager.GetValueManager<Dictionary<string, object>>("Values");
            return Convert.ToInt32(value.Value[operands[0].ToString()]);
        }

        public Type ResultType(params Type[] operands) {
            return typeof(int);
        }
        static ISEnumValueFunctionOperator() {
            ISEnumValueFunctionOperator instance = new ISEnumValueFunctionOperator();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null) {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }
        public static void Register() { }
    }
}
