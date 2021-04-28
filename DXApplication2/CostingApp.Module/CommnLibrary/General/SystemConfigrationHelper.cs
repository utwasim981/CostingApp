using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.CommonLibrary.General {
    [AttributeUsage(AttributeTargets.Class)]
    public class IsSystemConfigration : Attribute {
        public bool Value;
        public IsSystemConfigration(bool value) {
            Value = value;
        }
    }
    public class SystemConfigrationHelper {
        public static void LoggedOnHandeler(XafApplication application) {
            var persistentTypes = XafTypesInfo.Instance.PersistentTypes.Where(x => x.IsPersistent);
            foreach (var persistentType in persistentTypes) {
                var value = persistentType.FindAttribute<IsSystemConfigration>();

                if (value != null && value.Value) {
                    AddSystemConfigurationToValueManager(application, persistentType);
                }

            }
        }
        static void AddSystemConfigurationToValueManager(XafApplication application, ITypeInfo type) {
            var os = application.CreateObjectSpace();
            IValueManager<Dictionary<string, object>> valueManager = ValueManager.GetValueManager<Dictionary<string, object>>("Values");
            if (valueManager.CanManageValue)
                valueManager.Value = null;
            var systemConfig = os.FindObject(type.Type, null);
            if (systemConfig != null) {
                Dictionary<string, object> values = new Dictionary<string, object>();
                foreach (var property in type.Members.Where(x => x.IsProperty)) {
                    values.Add(property.Name, property.GetValue(systemConfig));
                }
                valueManager = ValueManager.GetValueManager<Dictionary<string, object>>("Values");
                if (valueManager.CanManageValue)
                    valueManager.Value = values;
            }
        }
    }    
}
