﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WXafLib
{
    public class WXafHelper
    {
        public static string IsNull(string Value, string DefaultValue)
        {
            return !string.IsNullOrEmpty(Value) ? Value : DefaultValue;
        }
        public static string TrimStringValue(object Value) {
            if (Value != null)
                return Value.ToString().TrimStart().TrimEnd();
            else
                return string.Empty;
        }
        public static bool CheckIfWXafBase(object Object) {            
            var derived = XafTypesInfo.Instance.FindTypeInfo(typeof(Object));
            while (derived != null) {
                if (derived.Name is "WXafBase")
                    return true;
                derived = derived.Base;
            }
            return false;
        }
        public static bool CheckIfWXafBase(ITypeInfo type) {
            var derived = type;
            while (derived != null) {
                if (derived.Name is "WXafBase")
                    return true;
                derived = derived.Base;
            }
            return false;
        }
    }
}