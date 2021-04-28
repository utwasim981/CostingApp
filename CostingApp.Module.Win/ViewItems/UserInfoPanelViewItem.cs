using CostingApp.Module.CommonLibrary;
using CostingApp.Module.CommonLibrary.General.Model;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Utils.Frames;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.Win.ViewItems {
    [ViewItem(typeof(IModelUserPanel))]
    public class UserInfoPanelViewItem : ViewItem {
        readonly IModelUserPanel _model;
        public UserInfoPanelViewItem(IModelUserPanel modelUserPanel, Type objectType)
            : base(objectType, modelUserPanel.Id) {
            _model = modelUserPanel;
        }

        #region Overrides of ViewItem
        protected override object CreateControlCore() {
            if (CurrentObject != null && CurrentObject is IWXafObject) {
                View.ObjectSpace.Committed += ObjectSpace_Committed;
                return new LabelControl { Text = CreateText(), AllowHtmlString = true };
            }
            else
                return new LabelControl { Text = "", AllowHtmlString = true };
        }

        private void ObjectSpace_Committed(object sender, EventArgs e) {
            if (CurrentObject != null && Control != null)
                ((LabelControl)Control).Text = CreateText();
        }

        protected override void OnCurrentObjectChanged() {
            base.OnCurrentObjectChanged();
            if (CurrentObject != null && Control != null && CurrentObject is IWXafObject)
                ((LabelControl)Control).Text = CreateText();
        }
        #endregion
        public IModelUserPanel Model {
            get { return _model; }
        }
        string CreateText() {
            var current = (IWXafObject)CurrentObject;
            string text = string.Format("{0}: <b>{1}</b> {2}: <b>{3}</b>     {4}: <b>{5} </b>{6}: <b>{7}</b>",
                "Create By",
                current.CreatedBy != null ? current.CreatedBy.FullName : "",
                "Create On",
                current.CreatedOn != DateTime.MinValue ? current.CreatedOn.ToString("dd/MM/yyyy HH:mm") : "",
                "Last Modified By",
                current.UpdatedBy != null ? current.UpdatedBy.FullName : "",
                "Last Modified On",
                current.UpdatedOn != DateTime.MinValue ? current.UpdatedOn.ToString("dd/MM/yyyy HH:mm") : "");
            return text;
        }
    }

    public interface IModelUserPanel : IModelViewItem {
    }
    public class UserPanelItemUpdater : ModelNodesGeneratorUpdater<ModelDetailViewItemsNodesGenerator> {
        public override void UpdateNode(ModelNode node) {
            var Parent = (IModelDetailView)node.Parent;
            var type = XafTypesInfo.Instance.FindTypeInfo(Parent.ModelClass.Name);
            if (WXafHelper.CheckIfWXafBase(type)) {
                //var type = Type.GetType(Parent.ModelClass.Name);
                //if (type != null && type is IWXafObject) {
                string Name = "UserPanel";
                IModelUserPanel Item = node.AddNode<IModelUserPanel>(Name);
                var layoutItem = Parent.Layout.AddNode<IModelLayoutViewItem>(Name + "Layout");
                layoutItem.ShowCaption = false;
                layoutItem.ViewItem = Item;
            }
            // }
        }
    }
}
