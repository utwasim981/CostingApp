using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using CostingApp.Module.BO;
using CostingApp.Module.BO.Expenses;
using CostingApp.Module.CommonLibrary.General.Security;
using DevExpress.ExpressApp.Security;
using CostingApp.Module.CommonLibrary.General;
using CostingApp.Module.CommonLibrary.General.Model;

namespace CostingApp.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            SequenceGenerator.RegisterSequences(XafTypesInfo.Instance.PersistentTypes.Where(x => x.IsPersistent && x.Base.Name == "WXafSequenceObject"));
            CreateDefaultRole();
            CreateAdminUser();
            CreateExpenseCategory();
            CreateSystemConfigration();
            ObjectSpace.CommitChanges();
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
        }

        private void CreateSystemConfigration() {
            var systemconfig = ObjectSpace.FindObject<SystemConfigration>(null);
            if (systemconfig == null)
                systemconfig = ObjectSpace.CreateObject<SystemConfigration>();
        }
        void CreateExpenseCategory() {
            var category = ObjectSpace.FindObject<ExpenseCategory>(new BinaryOperator("ExpenseCategoryName", "Food Cost", BinaryOperatorType.Equal));// CriteriaOperator.Parse("ExpenseCategoryName = ?", "Food Cost"));
            if (category == null) {
                category = ObjectSpace.CreateObject<ExpenseCategory>();
                category.ExpenseCategoryName = "Food Cost";
            }
            category = ObjectSpace.FindObject<ExpenseCategory>(new BinaryOperator("ExpenseCategoryName", "Employees Cost", BinaryOperatorType.Equal)); //CriteriaOperator.Parse("ExpenseCategoryName = ?", "Labor Cost"));
            if (category == null) {
                category = ObjectSpace.CreateObject<ExpenseCategory>();
                category.ExpenseCategoryName = "Employees Cost";
            }
            category = ObjectSpace.FindObject<ExpenseCategory>(new BinaryOperator("ExpenseCategoryName", "Overhead Cost", BinaryOperatorType.Equal));// CriteriaOperator.Parse("ExpenseCategoryName = ?", "Ovrehead Cost"));
            if (category == null) {
                category = ObjectSpace.CreateObject<ExpenseCategory>();
                category.ExpenseCategoryName = "Overhead Cost";
            }
            category = ObjectSpace.FindObject<ExpenseCategory>(new BinaryOperator("ExpenseCategoryName", "Other Costs", BinaryOperatorType.Equal));// CriteriaOperator.Parse("ExpenseCategoryName = ?", "Other Costs"));
            if (category == null) {
                category = ObjectSpace.CreateObject<ExpenseCategory>();
                category.ExpenseCategoryName = "Other Costs";
            }
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
