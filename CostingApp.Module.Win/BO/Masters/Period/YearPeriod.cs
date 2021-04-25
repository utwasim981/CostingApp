using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.Win.BO.Masters.Period {
    public class YearPeriod : Period {
        protected override ITreeNode Parent {
            get { return null; }
        }
        protected override IBindingList Children {
            get { return Quarters; }
        }
        [Association("YearPeriod-QuarterPeriod"), Aggregated]
        public XPCollection<QuarterPeriod> Quarters {
            get {
                return GetCollection<QuarterPeriod>(nameof(Quarters));
            }
        }
        public YearPeriod(Session session) : base(session) { }
        public YearPeriod(Session session, string name) : base(session) {
            PeriodName = name;
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            PeriodType = EnumPersiodType.Year;            
        }
    }
}
