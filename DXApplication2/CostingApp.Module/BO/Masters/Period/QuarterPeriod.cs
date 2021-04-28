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

namespace CostingApp.Module.BO.Masters.Period {
    public class QuarterPeriod : Period {
        protected override ITreeNode Parent {
            get { return YearPeriod; }
        }
        protected override IBindingList Children {
            get { return Periods; }
        }
        private YearPeriod fYearPeriod;
        [Association("YearPeriod-QuarterPeriod")]
        [RuleRequiredField("QuarterPeriod_YearPeriod_RuleRequiredField", DefaultContexts.Save)]
        public YearPeriod YearPeriod {
            get {
                return fYearPeriod;
            }
            set {
                SetPropertyValue(nameof(YearPeriod), ref fYearPeriod, value);
            }
        }
        [Association("QuarterPeriod-BasePeriod"), Aggregated]
        public XPCollection<BasePeriod> Periods {
            get {
                return GetCollection<BasePeriod>(nameof(Periods));
            }
        }
        public QuarterPeriod(Session session) : base(session) { }
        public QuarterPeriod(Session session, string name) : base(session) {
            PeriodName = name;
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            PeriodType = EnumPersiodType.Quarter;
        }
        protected override string GetSequenceName() {
            return string.Concat(ClassInfo.FullName, YearPeriod.SequentialNumber.ToString());
        }
    }
}
