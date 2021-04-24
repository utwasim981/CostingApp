using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.Win.BO {
    public enum EnumSalaryType {
        Hourly = 0,
        Monthly = 1
    }
    public enum EnumGender {
        Male = 0,
        Female = 1
    }
    public enum EnumStatus {
        [XafDisplayName("")]
        [ImageName("BO_Person")]
        Opened,
        [XafDisplayName("")]
        [ImageName("BO_Employee")]
        Closed
    }
    public enum EnumPersiodType {
        Base = 0,
        Quarter = 1,
        Year = 2
    }
    public enum EnumMonth {
        //[XafDisplayName("")]
        None = 0,
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
    public enum EnumItemCard {
        InventoryItem = 0,
        NonInventoryItem = 1,
        AssemblyItem = 2
    }
    public enum EmumCalculationType {
        Value = 0,
        Percentage = 1
    }

}
