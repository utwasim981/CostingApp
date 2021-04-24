using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXafLib.General.Security {
    [PropertyEditor(typeof(String), false)]
    public class ActionPropertyEditor : StringPropertyEditor, IComplexViewItem {
        private IObjectSpace objectSpace;
        private XafApplication application;
        public ActionPropertyEditor(Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
        protected override object CreateControlCore() { return new ComboBoxEdit(); }
        protected override RepositoryItem CreateRepositoryItem() { return new RepositoryItemComboBox(); }
        protected override void SetupRepositoryItem(DevExpress.XtraEditors.Repository.RepositoryItem item) {
            base.SetupRepositoryItem(item);
            foreach (IModelAction action in application.Model.ActionDesign.Actions) {
                if (action != null) {
                    IModelActionExtender modelAction = action as IModelActionExtender;
                    if (modelAction != null && modelAction.CutomAction)
                        ((RepositoryItemComboBox)item).Items.Add(action.Id);
                }
            }
        }
        void IComplexViewItem.Setup(IObjectSpace objectSpace, XafApplication application) {
            this.objectSpace = objectSpace;
            this.application = application;
        }
    }
}
