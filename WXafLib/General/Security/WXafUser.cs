using System;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base.Security;
using System.Linq;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Model;

namespace WXafLib.General.Security {
    [ImageName("BO_User"), DefaultProperty("UserName"), Persistent("SequrityUser")]
    [System.ComponentModel.DisplayName("User")]
    [RuleCriteria("SequrityUser_XPO_Prevent_delete_logged_in_user", DefaultContexts.Delete, "[Oid] != CurrentUserId()", "Cannot delete the current logged-in user. Please log in using another user account and retry.")]
    public class WXafUser : XPObject, IPermissionPolicyUser, ISecurityUser, ISecurityUserWithRoles, IAuthenticationActiveDirectoryUser, IAuthenticationStandardUser {
        public WXafUser(Session session)
            : base(session) {
        }
        protected virtual IEnumerable<ISecurityRole> GetSecurityRoles() {
            IList<ISecurityRole> result = new List<ISecurityRole>();
            foreach (WXafRole role in Roles) {
                result.Add(role);
            }
            return new ReadOnlyCollection<ISecurityRole>(result);
        }
        [Association]
        public XPCollection<WXafRole> Roles {
            get {
                return GetCollection<WXafRole>("Roles");
            }
        }
        IList<ISecurityRole> ISecurityUserWithRoles.Roles {
            get {
                IList<ISecurityRole> result = new List<ISecurityRole>();
                foreach (WXafRole role in GetSecurityRoles()) {
                    result.Add(role);
                }
                return new ReadOnlyCollection<ISecurityRole>(result);
            }
        }
        IEnumerable<IPermissionPolicyRole> IPermissionPolicyUser.Roles {
            get {
                return Roles.OfType<IPermissionPolicyRole>();
            }
        }
        private string userName = string.Empty;
        private bool isActive = true;
        private string storedPassword;
        private bool changePasswordOnFirstLogon = false;
        private bool showInternalID;
        string firstName;
        string lastName;
        string fullName;
        string email;

        [Browsable(false)]
        [Size(SizeAttribute.Unlimited)]
        [Persistent]
        [SecurityBrowsable]
        protected string StoredPassword {
            get { return storedPassword; }
            set { SetPropertyValue("StoredPassword", ref storedPassword, value); }
        }
        public bool ChangePasswordOnFirstLogon {
            get { return changePasswordOnFirstLogon; }
            set { SetPropertyValue("ChangePasswordOnFirstLogon", ref changePasswordOnFirstLogon, value); }
        }
        [RuleRequiredField("SequrityUser_UserName_RuleRequiredField", DefaultContexts.Save)]
        [RuleUniqueValue("SequrityUser_UserName_RuleUniqueValue", DefaultContexts.Save, "The login with the entered user name was already registered within the system")]
        public string UserName {
            get { return userName; }
            set { SetPropertyValue("UserName", ref userName, string.IsNullOrEmpty(value) ? value : value.TrimStart().TrimEnd()); }
        }
        public bool IsActive {
            get { return isActive; }
            set { SetPropertyValue("IsActive", ref isActive, value); }
        }
        public bool ComparePassword(string password) {
            return PasswordCryptographer.VerifyHashedPasswordDelegate(storedPassword, password);
        }
        public void SetPassword(string password) {
            StoredPassword = PasswordCryptographer.HashPasswordDelegate(password);
        }
        public bool ShowInternalID {
            get { return showInternalID; }
            set { SetPropertyValue<bool>(nameof(ShowInternalID), ref showInternalID, value); }
        }
        [Size(50)]
        [VisibleInListView(false)]
        [RuleRequiredField("SequrityUser_FirstName_RuleRequiredField", DefaultContexts.Save)]
        public string FirstName {
            get { return firstName; }
            set {
                SetPropertyValue<string>(nameof(FirstName), ref firstName, string.IsNullOrEmpty(value) ? value : value.TrimStart().TrimEnd());
                FullName = string.Format("{0} {1}", WXafHelper.IsNull(FirstName, ""), WXafHelper.IsNull(LastName, ""));
            }
        }
        [Size(50)]
        [VisibleInListView(false)]
        [RuleRequiredField("SequrityUser_LastName_RuleRequiredField", DefaultContexts.Save)]
        public string LastName {
            get { return lastName; }
            set {
                SetPropertyValue<string>(nameof(LastName), ref lastName, string.IsNullOrEmpty(value) ? value : value.TrimStart().TrimEnd());
                FullName = string.Format("{0} {1}", WXafHelper.IsNull(FirstName, ""), WXafHelper.IsNull(LastName, ""));
            }
        }
        [Size(110)]
        [ModelDefault("AllowEdit", "False")]
        public string FullName {
            get { return fullName; }
            set { SetPropertyValue<string>(nameof(FullName), ref fullName, value); }
        }
        [RuleRequiredField("SequrityUser_Email_RuleRequiredField", DefaultContexts.Save, TargetCriteria = "UserName <> 'Administrator'")]
        public string Email {
            get { return email; }
            set { SetPropertyValue<string>(nameof(Email), ref email, value); }
        }

    }
}
