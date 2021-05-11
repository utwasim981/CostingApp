using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Web.Editors;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Model;
using DevExpress.Web;

namespace CostingApp.Module.Web.Editors {
    [DevExpress.ExpressApp.Editors.PropertyEditor(typeof(Enum), true)]
    public class RadioButtonEnumPropertyEditor : WebPropertyEditor {
        private EnumDescriptor enumDescriptor;
        private List<object> controlsHash = new List<object>();

        public RadioButtonEnumPropertyEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info) {
            this.enumDescriptor = new EnumDescriptor(MemberInfo.MemberType);
        }
        ASPxRadioButtonList List;
        protected override WebControl CreateEditModeControlCore() {

            List = new ASPxRadioButtonList();
            List.RepeatDirection = RepeatDirection.Vertical;
            List.RepeatColumns = 2;
            foreach (object enumValue in enumDescriptor.Values) {
                List.Items.Add(enumDescriptor.GetCaption(enumValue));
                controlsHash.Add(enumValue);
            }
            List.SelectedIndexChanged += List_SelectedIndexChanged;



            //Panel placeHolder = new Panel();
            //controlsHash.Clear();
            //foreach (object enumValue in enumDescriptor.Values) {
            //    ASPxRadioButton radioButton = new ASPxRadioButton();
            //    radioButton.ID = "radioButton_" + enumValue.ToString();
            //    controlsHash.Add(radioButton, enumValue);
            //    radioButton.Text = enumDescriptor.GetCaption(enumValue);
            //    radioButton.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            //    radioButton.GroupName = propertyName;
            //    placeHolder.Controls.Add(radioButton);
            //}
            //return placeHolder;
            return List;
        }

        private void List_SelectedIndexChanged(object sender, EventArgs e) {
            EditValueChangedHandler(sender, e);
        }

        //void radioButton_CheckedChanged(object sender, EventArgs e) {
        //    EditValueChangedHandler(sender, e);
        //}
        protected override string GetPropertyDisplayValue() {
            return enumDescriptor.GetCaption(PropertyValue);
        }

        protected override void ReadEditModeValueCore() {
            //object value = PropertyValue;
            //if (value != null) {
            //    foreach (ASPxRadioButton radioButton in Editor.Controls) {
            //        radioButton.Checked = value.Equals(controlsHash[radioButton]);
            //    }
            //}
            ((ASPxRadioButtonList)Editor).SelectedIndex = (int)PropertyValue;
        }

        protected override object GetControlValueCore() {
            //object result = null;
            //foreach (ASPxRadioButton radioButton in Editor.Controls) {
            //    if (radioButton.Checked) {
            //        result = controlsHash[radioButton];
            //        break;
            //    }
            //}
            return controlsHash[((ASPxRadioButtonList)Editor).SelectedIndex];
        }
        //public override void BreakLinksToControl(bool unwireEventsOnly) {
        //    if (Editor != null) {
        //        foreach (ASPxRadioButton radioButton in Editor.Controls) {
        //            radioButton.CheckedChanged -= new EventHandler(radioButton_CheckedChanged);
        //        }
        //        if (!unwireEventsOnly) {
        //            controlsHash.Clear();
        //        }
        //    }
        //    base.BreakLinksToControl(unwireEventsOnly);
        //}
        protected override void SetImmediatePostDataScript(string script) {
            List.ClientSideEvents.SelectedIndexChanged = script;
            //foreach (ASPxRadioButton radioButton in controlsHash.Keys) {
            //    radioButton.ClientSideEvents.CheckedChanged = script;
            //}
        }
        //public new Panel Editor {
        //    get { return (Panel)base.Editor; }
        //}
    }
}
