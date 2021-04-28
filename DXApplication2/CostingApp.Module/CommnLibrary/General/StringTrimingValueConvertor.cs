using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.CommnLibrary.General {
    public class StringTrimingValueConvertor : ValueConverter {
        public override Type StorageType { get { return typeof(string); } }

        public override object ConvertFromStorageType(object value) {
            return value.ToString();
        }

        public override object ConvertToStorageType(object value) {
            return value.ToString().TrimStart().TrimEnd();
        }
    }
}
