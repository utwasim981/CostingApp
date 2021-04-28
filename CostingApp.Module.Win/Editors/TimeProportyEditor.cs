using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.Win.Editors {
    [PropertyEditor(typeof(String), false)]
    public class TimeProportyEditor : DXPropertyEditor {
        public TimeProportyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) {
            this.ControlBindingProperty = "Time";
        }
        protected override object CreateControlCore() {
            return new TimeEdit();
        }
        protected override void SetupRepositoryItem(RepositoryItem item) {
            base.SetupRepositoryItem(item);
            ((RepositoryItemTimeEdit)item).Mask.EditMask = "HH:mm";
            ((RepositoryItemTimeEdit)item).Mask.UseMaskAsDisplayFormat = true;
        }
        public override bool CanFormatPropertyValue {
            get { return true; }
        }
        //public class TimeProportyEditor : PropertyEditor, IInplaceEditSupport, IAppearanceEnabled, IAppearanceVisibility, INotifyAppearanceVisibilityChanged, IDisposable {
        //private TimeEdit control = null;

        //public FontStyle FontStyle {
        //    get {
        //        if (control != null) {
        //            return control.Font.Style;
        //        }
        //        return DevExpress.Utils.AppearanceObject.DefaultFont.Style;
        //    }
        //    set {
        //        if (control != null) {
        //            control.Font = new System.Drawing.Font(control.Font, value);
        //        }
        //    }
        //}
        //public Color FontColor {
        //    get {
        //        if (control != null) {
        //            return control.ForeColor;
        //        }
        //        return Color.Empty;
        //    }
        //    set {
        //        if (control != null) {
        //            control.ForeColor = value;
        //        }
        //    }
        //}
        //public Color BackColor {
        //    get {
        //        if (control != null) {
        //            return control.BackColor;
        //        }
        //        return Color.Empty;
        //    }
        //    set {
        //        if (control != null) {
        //            control.BackColor = value;
        //        }
        //    }
        //}

        //protected override void ReadValueCore() {
        //    if (control != null) {
        //        if (CurrentObject != null) {
        //            control.ReadOnly = false;
        //            if (PropertyValue != null) {
        //                var timeArray = PropertyValue.ToString().Split(':');
        //                control.Time = DateTime.Now.Date + new TimeSpan(Convert.ToInt32(timeArray[0]), Convert.ToInt32(timeArray[1]), 0);
        //            }
        //        }
        //        else {
        //            control.ReadOnly = true;
        //            control.Time = DateTime.Now.Date + new TimeSpan(0, 0, 0);
        //        }
        //    }
        //}
        //private void control_ValueChanged(object sender, EventArgs e) {
        //    if (!IsValueReading) {
        //        OnControlValueChanged();
        //        WriteValueCore();
        //    }
        //}
        //protected override object CreateControlCore() {
        //    control = new TimeEdit();
        //    control.Properties.TimeEditStyle = TimeEditStyle.TouchUI;
        //    control.Properties.Mask.EditMask = "HH:mm";
        //    control.Properties.Mask.UseMaskAsDisplayFormat = true;
        //    control.TextChanged += control_ValueChanged;
        //    return control;
        //}
        //protected override void OnControlCreated() {
        //    base.OnControlCreated();
        //    ReadValue();
        //}
        //public TimeProportyEditor(Type objectType, IModelMemberViewItem info)
        //: base(objectType, info) {
        //}
        //protected override void Dispose(bool disposing) {
        //    if (control != null) {
        //        control.TextChanged -= control_ValueChanged;
        //        control = null;
        //    }
        //    base.Dispose(disposing);
        //}
        //RepositoryItem IInplaceEditSupport.CreateRepositoryItem() {
        //    RepositoryItemTimeEdit item = new RepositoryItemTimeEdit();
        //    item.TimeEditStyle = TimeEditStyle.TouchUI;
        //    item.Mask.EditMask = "HH:MM";
        //    item.Mask.UseMaskAsDisplayFormat = true;
        //    return item;
        //}
        //protected override object GetControlValueCore() {
        //    if (control != null) {
        //        return control.Text;
        //    }
        //    return null;
        //}

        //void IAppearanceFormat.ResetBackColor() {
        //    if (control != null) {
        //        control.Properties.Appearance.BackColor = Color.Empty;
        //        control.Properties.Appearance.BackColor2 = Color.Empty;

        //        control.Properties.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
        //        control.Properties.Appearance.Image = null;
        //        InitializeAppearance(control.Properties);
        //    }
        //}
        //void IAppearanceFormat.ResetFontColor() {
        //    if (control != null) {
        //        control.Properties.Appearance.ForeColor = Color.Empty;
        //        control.Properties.Appearance.BorderColor = Color.Empty;

        //        control.Properties.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
        //        control.Properties.Appearance.Image = null;
        //        InitializeAppearance(control.Properties);
        //    }
        //}
        //void IAppearanceFormat.ResetFontStyle() {
        //    if (control != null) {
        //        if (!DevExpress.Utils.AppearanceObject.DefaultFont.Equals(control.Properties.Appearance.Font)) {
        //            control.Properties.Appearance.Font = DevExpress.Utils.AppearanceObject.DefaultFont; // InnerDefaultFont - inaccessible  
        //        }

        //        control.Properties.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
        //        control.Properties.Appearance.Image = null;
        //        InitializeAppearance(control.Properties);
        //    }
        //}
    }
}
