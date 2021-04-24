using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using CostingApp.Module.Win.BO.Expenses;

namespace CostingApp.Module.Win.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            CreateExpenseCategory();
            ObjectSpace.CommitChanges();
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
        }

        void CreateExpenseCategory() {
            var category = ObjectSpace.FindObject<ExpenseCategory>(CriteriaOperator.Parse("ExpenseCategoryName = ?", "Food Cost"));
            if (category == null) {
                category = ObjectSpace.CreateObject<ExpenseCategory>();
                category.ExpenseCategoryName = "Food Cost";
            }
            category = ObjectSpace.FindObject<ExpenseCategory>(CriteriaOperator.Parse("ExpenseCategoryName = ?", "Labor Cost"));
            if (category == null) {
                category = ObjectSpace.CreateObject<ExpenseCategory>();
                category.ExpenseCategoryName = "Labor Cost";
            }
            category = ObjectSpace.FindObject<ExpenseCategory>(CriteriaOperator.Parse("ExpenseCategoryName = ?", "Ovrehead Cost"));
            if (category == null) {
                category = ObjectSpace.CreateObject<ExpenseCategory>();
                category.ExpenseCategoryName = "Ovrehead Cost";
            }
            category = ObjectSpace.FindObject<ExpenseCategory>(CriteriaOperator.Parse("ExpenseCategoryName = ?", "Other Costs"));
            if (category == null) {
                category = ObjectSpace.CreateObject<ExpenseCategory>();
                category.ExpenseCategoryName = "Other Costs";
            }
        }
    }
}
