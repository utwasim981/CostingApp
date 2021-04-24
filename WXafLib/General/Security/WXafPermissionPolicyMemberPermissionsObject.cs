using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;

namespace WXafLib.General.Security {
    [System.ComponentModel.DisplayName("Member Permissions")]
    [ImageName("BO_Security_Permission_Member")]
    [DefaultListViewOptions(true, NewItemRowPosition.Top)]
    [Persistent("SequrityMemberPermission")]
    public class WXafPermissionPolicyMemberPermissionsObject : XPObject, ICheckedListBoxItemsProvider, IPermissionPolicyMemberPermissionsObject {
        protected virtual void OnItemsChanged() {
            if (ItemsChanged != null) {
                ItemsChanged(this, EventArgs.Empty);
            }
        }
        IPermissionPolicyTypePermissionObject IPermissionPolicyMemberPermissionsObject.TypePermissionObject {
            get { return TypePermissionObject; }
        }
        Dictionary<object, string> ICheckedListBoxItemsProvider.GetCheckedListBoxItems(string targetMemberName) {
            if (TypePermissionObject == null || !(TypePermissionObject is ICheckedListBoxItemsProvider)) {
                return new Dictionary<object, string>();
            }
            return ((ICheckedListBoxItemsProvider)TypePermissionObject).GetCheckedListBoxItems(targetMemberName);
        }
        public event EventHandler ItemsChanged;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(true)]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        public string Members {
            get { return GetPropertyValue<string>("Members"); }
            set { SetPropertyValue("Members", value); }
        }
        [NonPersistent]
        [Browsable(false)]
        public bool IsMemberExists {
            get {
                if (string.IsNullOrEmpty(Members)) {
                    return false;
                }
                ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(TypePermissionObject.TargetType);
                string[] membersArray = Members.Split(';');
                if (membersArray.Length == 0) {
                    return false;
                }
                foreach (string member in membersArray) {
                    if (typeInfo.FindMember(member.Trim()) == null) {
                        return false;
                    }
                }
                return true;
            }
        }
        [System.ComponentModel.DisplayName("Read")]
        public SecurityPermissionState? ReadState {
            get {
                return GetPropertyValue<SecurityPermissionState?>("ReadState");
            }
            set {
                SetPropertyValue("ReadState", value);
            }
        }
        [System.ComponentModel.DisplayName("Write")]
        public SecurityPermissionState? WriteState {
            get {
                return GetPropertyValue<SecurityPermissionState?>("WriteState");
            }
            set {
                SetPropertyValue("WriteState", value);
            }
        }
        [CriteriaOptions("TypePermissionObject.TargetType")]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        [Size(SizeAttribute.Unlimited)]
        [DevExpress.ExpressApp.Model.ModelDefault("RowCount", "0")]
        [VisibleInListView(true), VisibleInDetailView(true)]
        public string Criteria {
            get { return GetPropertyValue<string>("Criteria"); }
            set { SetPropertyValue("Criteria", value); }
        }
        [Association]
        [VisibleInListView(false), VisibleInDetailView(false)]
        public WXafPermissionPolicyTypePermissionObject TypePermissionObject {
            get { return GetPropertyValue<WXafPermissionPolicyTypePermissionObject>("TypePermissionObject"); }
            set { SetPropertyValue("TypePermissionObject", value); }
        }
        public WXafPermissionPolicyMemberPermissionsObject(Session session)
          : base(session) {
        }
    }
}
