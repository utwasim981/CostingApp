using DevExpress.ExpressApp;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.CommonLibrary.General.Model {   
    [NonPersistent]
    public abstract class WXafBaseObject : WXafBase {
        public WXafBaseObject(Session session) : base(session) { }        
    }
}
