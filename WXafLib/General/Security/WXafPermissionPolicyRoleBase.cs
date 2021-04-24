using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WXafLib.General.Security {
    [Persistent("SequrityRole")]
    [System.ComponentModel.DisplayName("Base Role")]
    public class WXafPermissionPolicyRoleBase : XPObject, IPermissionPolicyRole, ISecurityRole, ISecuritySystemRole, INavigationPermissions {
        [Association]
        [Aggregated]
        public XPCollection<WXafPermissionPolicyTypePermissionObject> TypePermissions {
            get { return GetCollection<WXafPermissionPolicyTypePermissionObject>("TypePermissions"); }
        }
        IEnumerable<IPermissionPolicyTypePermissionObject> IPermissionPolicyRole.TypePermissions {
            get {
                return TypePermissions.OfType<IPermissionPolicyTypePermissionObject>();
            }
        }
        public virtual IPermissionPolicyTypePermissionObject CreateTypePermissionObject(Type targetType) {
            WXafPermissionPolicyTypePermissionObject permissionPolicyTypePermissionObject = new WXafPermissionPolicyTypePermissionObject(Session);
            permissionPolicyTypePermissionObject.TargetType = targetType;
            permissionPolicyTypePermissionObject.Role = this;
            return permissionPolicyTypePermissionObject;
        }
        [Association]
        [Aggregated]
        public XPCollection<WXafPermissionPolicyNavigationPermissionObject> NavigationPermissions {
            get { return GetCollection<WXafPermissionPolicyNavigationPermissionObject>("NavigationPermissions"); }
        }
        IEnumerable<IPermissionPolicyNavigationPermissionObject> INavigationPermissions.NavigationPermissions {
            get {
                return NavigationPermissions.OfType<IPermissionPolicyNavigationPermissionObject>();
            }
        }
        public virtual IPermissionPolicyNavigationPermissionObject CreateNavigationPermissionObject(string itemPath) {
            WXafPermissionPolicyNavigationPermissionObject navigationPermissionObject = new WXafPermissionPolicyNavigationPermissionObject(Session);
            navigationPermissionObject.ItemPath = itemPath;
            navigationPermissionObject.Role = this;
            return navigationPermissionObject;
        }
        private string name;
        public WXafPermissionPolicyRoleBase(Session session)
            : base(session) {
        }
        [RuleRequiredField("SequrityRoleBase_Name_RuleRequiredField", DefaultContexts.Save)]
        [RuleUniqueValue("SequrityRoleBase_Name_RuleUniqueValue", DefaultContexts.Save, "The role with the entered Name was already registered within the system")]
        public string Name {
            get { return name; }
            set { SetPropertyValue("Name", ref name, value); }
        }
        public bool IsAdministrative {
            get { return GetPropertyValue<bool>("IsAdministrative"); }
            set { SetPropertyValue("IsAdministrative", value); }
        }
        public bool CanEditModel {
            get { return GetPropertyValue<bool>("CanEditModel"); }
            set { SetPropertyValue("CanEditModel", value); }
        }
        [VisibleInListView(false)]
        public SecurityPermissionPolicy PermissionPolicy {
            get { return GetPropertyValue<SecurityPermissionPolicy>("PermissionPolicy"); }
            set { SetPropertyValue("PermissionPolicy", value); }
        }
    }
}
