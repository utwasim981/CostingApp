using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.XtraBars;

namespace WXafLib.General {
    public interface IModelClassHideViewToolBar {
        [Category("eXpand")]
        [Description("Hides view toolbar")]
        bool? HideToolBar { get; set; }
    }
    [ModelInterfaceImplementor(typeof(IModelClassHideViewToolBar), "ModelClass")]
    public interface IModelViewHideViewToolBar : IModelClassHideViewToolBar {
    }

    public class HideToolBarController : ViewController<ObjectView>, IModelExtender {
        void IModelExtender.ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            extenders.Add<IModelClass, IModelClassHideViewToolBar>();
            extenders.Add<IModelObjectView, IModelViewHideViewToolBar>();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            var template = Frame.Template as IBarManagerHolder;
            if (template != null && template.BarManager != null && ((IModelViewHideViewToolBar)View.Model).HideToolBar.HasValue) {
                var hideToolBar = ((IModelViewHideViewToolBar)View.Model).HideToolBar;
                SetToolbarVisibility(template, hideToolBar != null && !hideToolBar.Value);
            }
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
            var template = Frame.Template as IBarManagerHolder;
            if (template != null && template.BarManager != null && ((IModelViewHideViewToolBar)View.Model).HideToolBar.HasValue) {
                var hideToolBar = ((IModelViewHideViewToolBar)View.Model).HideToolBar;
                SetToolbarVisibility(template, hideToolBar != null && hideToolBar.Value);
            }
        }
        void SetToolbarVisibility(IBarManagerHolder template, bool visible) {
            foreach (Bar bar in template.BarManager.Bars) {
                if (bar.BarName == "ListView Toolbar" || bar.BarName == "Main Toolbar") {
                    bar.Visible = visible;
                    break;
                }
            }

            var supportActionsToolbarVisibility = template as ISupportActionsToolbarVisibility;
            if (supportActionsToolbarVisibility != null) (supportActionsToolbarVisibility).SetVisible(visible);
        }
    }
}
