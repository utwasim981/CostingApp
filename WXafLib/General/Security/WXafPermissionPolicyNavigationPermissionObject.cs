using System;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace WXafLib.General.Security {
    [System.ComponentModel.DisplayName("Navigation Item Permissions")]
    [ImageName("BO_Security_Permission_Navigate")]
    [Persistent("SecurityNavigationPermissions")]
    [DefaultListViewOptions(true, NewItemRowPosition.Top)]
    public class WXafPermissionPolicyNavigationPermissionObject : XPObject, IPermissionPolicyNavigationPermissionObject {
        public WXafPermissionPolicyNavigationPermissionObject(Session session) : base(session) {
        }
        IPermissionPolicyRole IPermissionPolicyNavigationPermissionObject.Role {
            get {
                return Role;
            }
        }
        [RuleRequiredField]
        [ImmediatePostData]
        [VisibleInListView(true)]
        [Size(SizeAttribute.Unlimited)]
        [DisplayName("Navigation Item")]
        public string ItemPath {
            get {
                return GetPropertyValue<string>("ItemPath");
            }
            set {
                SetPropertyValue("ItemPath", value);
            }
        }
        [System.ComponentModel.DisplayName("Navigate")]
        public SecurityPermissionState? NavigateState {
            get { return GetPropertyValue<SecurityPermissionState?>("NavigateState"); }
            set { SetPropertyValue("NavigateState", value); }
        }
        [Association]
        [VisibleInListView(false), VisibleInDetailView(false)]
        public WXafPermissionPolicyRoleBase Role {
            get { return GetPropertyValue<WXafPermissionPolicyRoleBase>("Role"); }
            set { SetPropertyValue("Role", value); }
        }
    }
}
