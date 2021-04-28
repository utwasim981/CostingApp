using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CostingApp.Module.CommonLibrary.General.Security {
    [ImageName("BO_Role"), System.ComponentModel.DisplayName("Role")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class WXafRole : WXafPermissionPolicyRoleBase, IPermissionPolicyRoleWithUsers, ICanInitializeRole {
        public WXafRole(Session session) : base(session) { }

        [Association]
        public XPCollection<WXafUser> Users {
            get { return GetCollection<WXafUser>("Users"); }
        }
        IEnumerable<IPermissionPolicyUser> IPermissionPolicyRoleWithUsers.Users {
            get {
                return Users.OfType<IPermissionPolicyUser>();
            }
        }

        public bool AddUser(object user) {
            bool result = false;
            WXafUser permissionPolicyUser = user as WXafUser;
            if (permissionPolicyUser != null) {
                Users.Add(permissionPolicyUser);
                result = true;
            }
            return result;
        }

        [Association("WXafRole-ActionPermission"), DevExpress.Xpo.Aggregated]
        public XPCollection<ActionPermission> ActionPermissions {
            get { return GetCollection<ActionPermission>("ActionPermissions"); }
        }
    }
}
