﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXafLib.General.Security;

namespace WXafLib.General.Model {

    public interface IWXafObject {
        bool IsActive { get; set; }
        WXafUser CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        WXafUser UpdatedBy { get; set; }
        DateTime UpdatedOn { get; set; }
    }

    [NonPersistent]
    public abstract class WXafBase : XPObject, IWXafObject, IObjectSpaceLink {
        IObjectSpace fObjectSpace;
        [Browsable(false)]
        public IObjectSpace ObjectSpace { get { return fObjectSpace; }  set { fObjectSpace = value; } }
        bool fIsActive;
        [ImmutableObject(true)]
        [XafDisplayName("Actvie")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public bool IsActive {
            get { return fIsActive; }
            set { SetPropertyValue<bool>(nameof(IsActive), ref fIsActive, value); }
        }
        WXafUser fCreatedBy;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public WXafUser CreatedBy {
            get { return fCreatedBy; }
            set { SetPropertyValue<WXafUser>(nameof(CreatedBy), ref fCreatedBy, value); }
        }
        DateTime fCreatedOn;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public DateTime CreatedOn {
            get { return fCreatedOn; }
            set { SetPropertyValue<DateTime>(nameof(CreatedOn), ref fCreatedOn, value); }
        }
        WXafUser fUpdatedBy;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public WXafUser UpdatedBy {
            get { return fUpdatedBy; }
            set { SetPropertyValue<WXafUser>(nameof(UpdatedBy), ref fUpdatedBy, value); }
        }
        DateTime fUpdatedOn;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public DateTime UpdatedOn {
            get { return fUpdatedOn; }
            set { SetPropertyValue<DateTime>(nameof(UpdatedOn), ref fUpdatedOn, value); }
        }
        public WXafBase(Session session) : base(session) { }
        public override void AfterConstruction() {
            base.AfterConstruction();
            IsActive = true;
        }
        protected override void OnSaving() {
            base.OnSaving();
            if (Session.IsNewObject(this)) {
                CreatedOn = DateTime.Now;
                CreatedBy = Session.GetObjectByKey<WXafUser>(((WXafUser)SecuritySystem.CurrentUser).Oid);
            }
            else {
                UpdatedOn = DateTime.Now;
                UpdatedBy = Session.GetObjectByKey<WXafUser>(((WXafUser)SecuritySystem.CurrentUser).Oid);
            }
        }
        protected override void OnDeleting() {
            base.OnDeleting();
            UpdatedOn = DateTime.Now;
            UpdatedBy = Session.GetObjectByKey<WXafUser>(((WXafUser)SecuritySystem.CurrentUser).Oid);
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && oldValue != newValue) {
                if (ClassInfo.FindMember(propertyName).MemberType.Name.ToUpper() == "STRING")
                    newValue = WXafHelper.TrimStringValue(newValue.ToString());
            }
        }
    }
}
