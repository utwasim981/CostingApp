using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.CommonLibrary.General.CriteriaOperators {
    public class CriteriaOperatorRegistration {
        public static void Register() {
            //IshowInternalIDFunctionOperator.Register();
            IsBoolValueFunctionOperator.Register();
            IsStringValueFunctionOperator.Register();
            IsNumberValueFunctionOperator.Register();
            IsDateValueFunctionOperator.Register();
            //ISEnumValueFunctionOperator.Register();
        }
    }
}
