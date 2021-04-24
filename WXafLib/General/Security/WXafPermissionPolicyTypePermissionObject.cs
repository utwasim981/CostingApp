using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Utils;
using System.Collections.Generic;
using System;
using DevExpress.ExpressApp.DC;
using System.ComponentModel;
using DevExpress.Persistent.Validation;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base.Security;

namespace WXafLib.General.Security {
    [System.ComponentModel.DisplayName("Type Permissions")]
    [ImageName("BO_Security_Permission_Type")]
    [Persistent("SequrityTypePermissionsObject")]
    [DefaultListViewOptions(true, NewItemRowPosition.Top)]
    public class WXafPermissionPolicyTypePermissionObject : XPObject, ICheckedListBoxItemsProvider, IPermissionPolicyTypePermissionObject {
        public WXafPermissionPolicyTypePermissionObject(Session session)
      : base(session) {
        }
        [Association]
        [VisibleInListView(false), VisibleInDetailView(false)]
        public WXafPermissionPolicyRoleBase Role {
            get { return GetPropertyValue<WXafPermissionPolicyRoleBase>("Role"); }
            set { SetPropertyValue("Role", value); }
        }
        IPermissionPolicyRole IPermissionPolicyTypePermissionObject.Role {
            get { return Role; }
        }
        [VisibleInListView(true)]
        [ValueConverter(typeof(TypeToStringConverter))]
        [Size(SizeAttribute.Unlimited)]
        [RuleRequiredField]
        [TypeConverter(typeof(SecurityTargetTypeConverter))]
        [ImmediatePostData]
        public Type TargetType {
            get {
                return GetPropertyValue<Type>("TargetType");
            }
            set {
                SetPropertyValue("TargetType", value);
                OnItemsChanged();
            }
        }
        [System.ComponentModel.DisplayName("Read")]
        public SecurityPermissionState? ReadState {
            get { return GetPropertyValue<SecurityPermissionState?>("ReadState"); }
            set { SetPropertyValue("ReadState", value); }
        }
        [System.ComponentModel.DisplayName("Write")]
        public SecurityPermissionState? WriteState {
            get { return GetPropertyValue<SecurityPermissionState?>("WriteState"); }
            set { SetPropertyValue("WriteState", value); }
        }
        [System.ComponentModel.DisplayName("Create")]
        public SecurityPermissionState? CreateState {
            get { return GetPropertyValue<SecurityPermissionState?>("CreateState"); }
            set { SetPropertyValue("CreateState", value); }
        }
        [System.ComponentModel.DisplayName("Delete")]
        public SecurityPermissionState? DeleteState {
            get { return GetPropertyValue<SecurityPermissionState?>("DeleteState"); }
            set { SetPropertyValue("DeleteState", value); }
        }
        [System.ComponentModel.DisplayName("Navigate")]
        public SecurityPermissionState? NavigateState {
            get { return GetPropertyValue<SecurityPermissionState?>("NavigateState"); }
            set { SetPropertyValue("NavigateState", value); }
        }
        protected virtual void OnItemsChanged() {
            if (ItemsChanged != null) {
                ItemsChanged(this, EventArgs.Empty);
            }
        }
        [DevExpress.Xpo.Aggregated]
        [Association]
        public XPCollection<WXafPermissionPolicyMemberPermissionsObject> MemberPermissions {
            get { return GetCollection<WXafPermissionPolicyMemberPermissionsObject>("MemberPermissions"); }
        }
        IEnumerable<IPermissionPolicyMemberPermissionsObject> IPermissionPolicyTypePermissionObject.MemberPermissions {
            get { return MemberPermissions.OfType<IPermissionPolicyMemberPermissionsObject>(); }
        }
        [DevExpress.Xpo.Aggregated]
        [Association]
        public XPCollection<WXafPermissionPolicyObjectPermissionsObject> ObjectPermissions {
            get { return GetCollection<WXafPermissionPolicyObjectPermissionsObject>("ObjectPermissions"); }
        }
        IEnumerable<IPermissionPolicyObjectPermissionsObject> IPermissionPolicyTypePermissionObject.ObjectPermissions {
            get { return ObjectPermissions.OfType<IPermissionPolicyObjectPermissionsObject>(); }
        }
        public virtual IPermissionPolicyObjectPermissionsObject CreateObjectPermission() {
            WXafPermissionPolicyObjectPermissionsObject permissionPolicyObjectPermissionsObject = new WXafPermissionPolicyObjectPermissionsObject(Session);
            permissionPolicyObjectPermissionsObject.TypePermissionObject = this;
            ObjectPermissions.Add(permissionPolicyObjectPermissionsObject);
            return permissionPolicyObjectPermissionsObject;
        }
        Dictionary<object, string> ICheckedListBoxItemsProvider.GetCheckedListBoxItems(string targetMemberName) {
            Dictionary<Object, String> result = new Dictionary<Object, String>();
            if (targetMemberName == "Members" && TargetType != null) {
                ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(TargetType);
                foreach (IMemberInfo memberInfo in typeInfo.Members) {
                    if (memberInfo.IsVisible || (memberInfo.FindAttribute<SecurityBrowsableAttribute>() != null)) {
                        string caption = CaptionHelper.GetMemberCaption(memberInfo);
                        if (result.ContainsKey(memberInfo.Name)) {
                            throw new LightDictionary<string, string>.DuplicatedKeyException(memberInfo.Name, result[memberInfo.Name], caption);
                        }
                        result.Add(memberInfo.Name, caption);
                    }
                }
            }
            return result;
        }
        public virtual IPermissionPolicyMemberPermissionsObject CreateMemberPermission() {
            WXafPermissionPolicyMemberPermissionsObject permissionPolicyObjectPermissionsObject = new WXafPermissionPolicyMemberPermissionsObject(Session);
            permissionPolicyObjectPermissionsObject.TypePermissionObject = this;
            MemberPermissions.Add(permissionPolicyObjectPermissionsObject);
            return permissionPolicyObjectPermissionsObject;
        }
        public event EventHandler ItemsChanged;
    }
}
