using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using WXafLib.General.Model;
using WXafLib.General.Security;
using DevExpress.ExpressApp.Security;
using WXafLib.General;

namespace WXafLib {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            SequenceGenerator.RegisterSequences(XafTypesInfo.Instance.PersistentTypes.Where(x => x.IsPersistent && x.Base.Name == "WXafSequenceObject"));
            CreateAdminUser();
            CreateDefaultRole();
            CreateSystemConfiguration();
            ObjectSpace.CommitChanges();
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }

        private void CreateAdminUser() {
            WXafUser userAdmin = ObjectSpace.FindObject<WXafUser>(new BinaryOperator("UserName", "Administrator"));
            if (userAdmin == null) {
                userAdmin = ObjectSpace.CreateObject<WXafUser>();
                userAdmin.UserName = "Administrator";
                userAdmin.SetPassword("");
                userAdmin.Roles.Add(CreateAdminRole());
            }
        }
        private WXafRole CreateAdminRole() {
            WXafRole adminRole = ObjectSpace.FindObject<WXafRole>(new BinaryOperator("Name", "Administrators"));
            if (adminRole == null) {
                adminRole = ObjectSpace.CreateObject<WXafRole>();
                adminRole.Name = "Administrators";
                adminRole.IsAdministrative = true;
            }
            return adminRole;
        }
        private void CreateDefaultRole() {
            WXafRole defaultRole = ObjectSpace.FindObject<WXafRole>(new BinaryOperator("Name", "Default"));
            if (defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<WXafRole>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectPermission<WXafUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<WXafUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<WXafUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<WXafRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
        }
        private void CreateSystemConfiguration() {
            var persistentTypes = XafTypesInfo.Instance.PersistentTypes.Where(x => x.IsPersistent);
            foreach (var persistentType in persistentTypes) {
                var value = persistentType.FindAttribute<IsSystemConfigration>();

                if (value != null && value.Value) {
                    if (ObjectSpace.GetObjects(persistentType.Type, null).Count == 0) {
                        ObjectSpace.CreateObject(persistentType.Type);
                    }
                }
            }
        }
    }
}
