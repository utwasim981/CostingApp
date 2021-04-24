using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXafLib.General.Security {
    public class ActionPermission : BaseObject {
        public ActionPermission(Session session) : base(session) { }

        public string ActionId {
            get { return GetPropertyValue<string>("ActionId"); }
            set { SetPropertyValue<string>("ActionId", value); }
        }

        [Association("WXafRole-ActionPermission")]
        public WXafRole Role {
            get { return GetPropertyValue<WXafRole>("Role"); }
            set { SetPropertyValue<WXafRole>("Role", value); }
        }
    }

    public interface IModelActionExtender {
        [DefaultValue(false)]
        [Category("WXaf")]
        bool CutomAction { get; set; }
    }

    public class ActionPermissionController : ViewController, IModelExtender {
        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            extenders.Add<IModelAction, IModelActionExtender>();
        }
        protected override void OnActivated() {
            base.OnActivated();
            foreach (Controller controller in Frame.Controllers)
                foreach (ActionBase action in controller.Actions) {
                    IModelActionExtender modelAction = Application.Model.ActionDesign.Actions[action.Id] as IModelActionExtender;
                    if (modelAction != null) {
                        if (modelAction != null && modelAction.CutomAction) {
                            action.Active["Security"] = SecuritySystem.IsGranted(new ExecuteActionPermissionRequest(action.Id));
                        }
                    }
                    else
                        action.Active["Security"] = true;
                }
        }

    }
}
