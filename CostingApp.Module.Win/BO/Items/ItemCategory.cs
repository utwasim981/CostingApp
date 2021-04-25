using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib.General.Model;
using WXafLib.General.Security;

namespace CostingApp.Module.Win.BO.Items {
    [NavigationItem("Inventory Setup")]
    [RuleCombinationOfPropertiesIsUnique("ItemCategory_ItemCategoryName_Parentcategory.RuleUniqueField", DefaultContexts.Save, "ItemCategoryName, ParentCategory")]
    [ImageName("BO_Product_Group")]
    public class ItemCategory : WXafBaseObject, ITreeNode {

        string fItemCategoryName;
        [XafDisplayName("Category Name")]
        [RuleRequiredField("ItemCategory_ItemCategoryName_RuleRequiredField", DefaultContexts.Save)]
        public string ItemCategoryName {
            get { return fItemCategoryName; }
            set { SetPropertyValue<string>(nameof(ItemCategoryName), ref fItemCategoryName, value); }
        }
        private ItemCategory fParentCategory;
        [Association("ItemCategory-ItemCategory")]
        [XafDisplayName("Master")]
        public ItemCategory ParentCategory {
            get { return fParentCategory; }
            set { SetPropertyValue(nameof(ParentCategory), ref fParentCategory, value); }
        }
        [Association("ItemCategory-ItemCategory"), DevExpress.Xpo.Aggregated]
        public XPCollection<ItemCategory> Categories {
            get {
                return GetCollection<ItemCategory>(nameof(Categories));
            }
        }
        #region ITreeNode
        public string Name {
            get { return ItemCategoryName; }
        }
        public ITreeNode Parent {
            get { return ParentCategory; }
        }
        public IBindingList Children {
            get { return Categories; }
        }
        public ItemCategory(Session session) : base(session) { }
        #endregion
    }
}
