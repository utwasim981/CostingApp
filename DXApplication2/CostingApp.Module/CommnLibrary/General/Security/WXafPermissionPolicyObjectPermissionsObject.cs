using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CostingApp.Module.CommonLibrary.General.Security {
    [System.ComponentModel.DisplayName("Object Permissions")]
    [ImageName("BO_Security_Permission_Object")]
    [DefaultListViewOptions(true, NewItemRowPosition.Top)]
    [Persistent("SequrityObjectPermission")]
    public class WXafPermissionPolicyObjectPermissionsObject : XPObject, IPermissionPolicyObjectPermissionsObject {
        IPermissionPolicyTypePermissionObject IPermissionPolicyObjectPermissionsObject.TypePermissionObject {
            get {
                return TypePermissionObject;
            }
        }
        [Size(SizeAttribute.Unlimited)]
        [CriteriaOptions("TypePermissionObject.TargetType")]
        [VisibleInListView(true)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string Criteria {
            get { return GetPropertyValue<string>("Criteria"); }
            set { SetPropertyValue("Criteria", value); }
        }
        [System.ComponentModel.DisplayName("Read")]
        public SecurityPermissionState? ReadState {
            get { return GetPropertyValue<SecurityPermissionState?>("ReadState"); }
            set { SetPropertyValue("ReadState", value); }
        }
        [System.ComponentModel.DisplayName("Write")]
        public SecurityPermissionState? WriteState {
            get { return GetPropertyValue<SecurityPermissionState?>("WriteState"); }
            set { SetPropertyValue("WriteState", value); }
        }
        [System.ComponentModel.DisplayName("Delete")]
        public SecurityPermissionState? DeleteState {
            get { return GetPropertyValue<SecurityPermissionState?>("DeleteState"); }
            set { SetPropertyValue("DeleteState", value); }
        }
        [System.ComponentModel.DisplayName("Navigate")]
        public SecurityPermissionState? NavigateState {
            get { return GetPropertyValue<SecurityPermissionState?>("NavigateState"); }
            set { SetPropertyValue("NavigateState", value); }
        }
        [Association]
        [VisibleInListView(false), VisibleInDetailView(false)]
        public WXafPermissionPolicyTypePermissionObject TypePermissionObject {
            get { return GetPropertyValue<WXafPermissionPolicyTypePermissionObject>("TypePermissionObject"); }
            set { SetPropertyValue("TypePermissionObject", value); }
        }
        public WXafPermissionPolicyObjectPermissionsObject(Session session)
            : base(session) {
        }
    }
}
