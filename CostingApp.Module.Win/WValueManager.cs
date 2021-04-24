using CostingApp.Module.Win.BO;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.Win {
    public class WValueManager {
        public static bool CityCodeM {
            get {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CityCodeM");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CityCodeM");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public static bool CityCodeA {
            get {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CityCodeA");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("CityCodeA");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public static bool ShopCodeM {
            get {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ShopCodeM");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ShopCodeM");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public static bool ShopCodeA {
            get {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ShopCodeA");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("ShopCodeA");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public static bool EmployeeCodeM {
            get {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("EmployeeCodeM");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("EmployeeCodeM");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
        public static bool EmployeeCodeA {
            get {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("EmployeeCodeA");
                if (valueManager.CanManageValue)
                    return valueManager.Value;
                else return true;
            }
            set {
                IValueManager<bool> valueManager = ValueManager.GetValueManager<bool>("EmployeeCodeA");
                if (valueManager.CanManageValue)
                    valueManager.Value = value;
            }
        }
    }
}
