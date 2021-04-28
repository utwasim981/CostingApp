using DevExpress.ExpressApp;
using System.Collections.Generic;
using DevExpress.ExpressApp.Editors;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;

namespace CostingApp.Module.CommonLibrary.General.CriteriaOperators {
    //public class ShowInternalIDControllers : ViewController {
    //    private DevExpress.ExpressApp.ConditionalAppearance.AppearanceController appearanceController;

    //    protected override void OnActivated() {
    //        base.OnActivated();
    //        if (WXafHelper.CheckIfWXafBase(View.ObjectTypeInfo)) {
    //            appearanceController = Frame.GetController<DevExpress.ExpressApp.ConditionalAppearance.AppearanceController>();
    //            if (appearanceController != null) {
    //                appearanceController.CustomApplyAppearance += appearanceController_CustomApplyAppearance;
    //            }
    //        }
    //        if (View is DetailView) {
    //            foreach (ViewItem item in ((DetailView)View).Items) {
    //                if (item is DecimalPropertyEditor || item is DoublePropertyEditor || item is FloatPropertyEditor ||
    //                    item is IntegerPropertyEditor || item is LongPropertyEditor) {
    //                    item.ControlCreated += new EventHandler<EventArgs>(item_ControlCreated);
    //                }
    //            }
    //        }
    //        if (View is ListView) {
    //        }
    //    }
    //    void appearanceController_CustomApplyAppearance(object sender, ApplyAppearanceEventArgs e) {
    //        var keyName = View.ObjectTypeInfo.KeyMember.Name;
    //        var shoInterlID = ((WXafUser)SecuritySystem.CurrentUser).ShowInternalID;
    //        if (View is ListView) {
    //            if (e.Item is ColumnWrapper && ((ColumnWrapper)e.Item).PropertyName == keyName) {
    //                ((ColumnWrapper)e.Item).Caption = "Internal ID";
    //                if (shoInterlID)
    //                    e.AppearanceObject.Visibility = ViewItemVisibility.Show;
    //                else
    //                    e.AppearanceObject.Visibility = ViewItemVisibility.Hide;
    //            }
    //        }
    //        else if (View is DetailView) {
    //            if (e.Item is PropertyEditor) {
    //                // object targetObject = e.ContextObjects.Length > 0 ? e.ContextObjects[0] : null;
    //                if (((PropertyEditor)e.Item).PropertyName == keyName) { //targetObject != null && 
    //                    ((PropertyEditor)e.Item).Caption = "Internal ID";
    //                    if (shoInterlID)
    //                        e.AppearanceObject.Visibility = ViewItemVisibility.Show;
    //                    else
    //                        e.AppearanceObject.Visibility = ViewItemVisibility.Hide;
    //                }
    //            }
    //        }

    //    }

    //    protected override void OnViewControlsCreated() {
    //        base.OnViewControlsCreated();
    //        //if (View is DetailView) {
    //        //    var view = (DetailView)View;
    //        //    foreach (ViewItem item in view.Items) {
    //        //        if (item is DecimalPropertyEditor || item is DoublePropertyEditor || item is FloatPropertyEditor ||
    //        //            item is IntegerPropertyEditor || item is LongPropertyEditor) {
    //        //            item.ControlCreated += new EventHandler<EventArgs>(item_ControlCreated);
    //        //        }
    //        //    }
    //        //}
    //        //else
    //        if (View is ListView) {
    //            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
    //            if (listEditor != null) {
    //                GridView gridView = listEditor.GridView;
    //                for (int i = 0; i < gridView.Columns.Count; i++)
    //                    gridView.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
    //            }
    //            var treeListEditor = ((ListView)View).Editor as TreeListEditor;
    //            if (treeListEditor != null) {
    //                treeListEditor.TreeList.HandleCreated += TreeList_HandleCreated;
    //            }
    //        }
    //    }        
    //    private void TreeList_HandleCreated(object sender, EventArgs e) {
    //        //((TreeListEditor)((ListView)View).Editor).TreeList.ExpandAll();
    //    }

    //    private void item_ControlCreated(object sender, EventArgs e) {
    //        if (((PropertyEditor)sender).Control is SpinEdit) {
    //            ((SpinEdit)((PropertyEditor)sender).Control).RightToLeft = System.Windows.Forms.RightToLeft.Yes;
    //            for (int i = 0; i < ((SpinEdit)((PropertyEditor)sender).Control).Properties.Buttons.Count; i++)
    //                ((SpinEdit)((PropertyEditor)sender).Control).Properties.Buttons[i].Visible = false;
    //        }

    //        if (sender is DecimalPropertyEditor) {
    //            DecimalPropertyEditor dpe = sender as DecimalPropertyEditor;
    //            ((SpinEdit)dpe.Control).RightToLeft = System.Windows.Forms.RightToLeft.Yes;
    //        }
    //        else if (sender is DoublePropertyEditor) {
    //            DoublePropertyEditor dpe = sender as DoublePropertyEditor;
    //            ((SpinEdit)dpe.Control).RightToLeft = System.Windows.Forms.RightToLeft.Yes;
    //        }
    //        else if (sender is FloatPropertyEditor) {
    //            FloatPropertyEditor dpe = sender as FloatPropertyEditor;
    //            ((SpinEdit)dpe.Control).RightToLeft = System.Windows.Forms.RightToLeft.Yes;
    //        }
    //        else if (sender is IntegerPropertyEditor) {
    //            IntegerPropertyEditor dpe = sender as IntegerPropertyEditor;
    //            ((SpinEdit)dpe.Control).RightToLeft = System.Windows.Forms.RightToLeft.Yes;
    //        }
    //        else if (sender is DoublePropertyEditor) {
    //            LongPropertyEditor dpe = sender as LongPropertyEditor;
    //            ((SpinEdit)dpe.Control).RightToLeft = System.Windows.Forms.RightToLeft.Yes;
    //        }
    //    }

    //    protected override void OnDeactivated() {
    //        if (appearanceController != null) {
    //            appearanceController.CustomApplyAppearance -= appearanceController_CustomApplyAppearance;
    //        }
    //        if (View is DetailView) {
    //            DetailView view = (DetailView)View;
    //            foreach (ViewItem item in view.Items) {
    //                if (item is DecimalPropertyEditor || item is DoublePropertyEditor || item is FloatPropertyEditor ||
    //                    item is IntegerPropertyEditor || item is LongPropertyEditor) {
    //                    item.ControlCreated -= new EventHandler<EventArgs>(item_ControlCreated);
    //                }
    //            }
    //        }
    //        base.OnDeactivated();
    //    }

        
    //}
}
