using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CostingApp.Module.Win.BO.Masters.Period {
    public class BasePeriod : Period {
        protected override ITreeNode Parent {
            get { return QuarterPeriod; }
        }
        protected override IBindingList Children {
            get {
                return new BindingList<object>();
            }
        }
        QuarterPeriod fQuarterPeriod;
        [RuleRequiredField("BasePeriod_QuarterPeriod_RuleRequiredField", DefaultContexts.Save)]
        [Association("QuarterPeriod-BasePeriod")]
        public QuarterPeriod QuarterPeriod {
            get { return fQuarterPeriod; }
            set { SetPropertyValue<QuarterPeriod>(nameof(QuarterPeriod), ref fQuarterPeriod, value); }
        }
        public BasePeriod(Session session) : base(session) { }
        public BasePeriod(Session session, string name) : base(session) {
            PeriodName = name;
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            PeriodType = EnumPersiodType.Base;
        }
        protected override string GetSequenceName() {
            return string.Concat(ClassInfo.FullName, QuarterPeriod.SequentialNumber.ToString());
        }
    }
}

