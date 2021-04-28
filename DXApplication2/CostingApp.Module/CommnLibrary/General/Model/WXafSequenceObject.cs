using System;
using DevExpress.Xpo;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System.Linq;
using DevExpress.ExpressApp;


namespace CostingApp.Module.CommonLibrary.General.Model {
    [NonPersistent]
    public abstract class WXafSequenceObject : WXafBase, ISupportSequentialNumber {
        public WXafSequenceObject(Session session)
            : base(session) {
        }
        protected override void OnSaving() {
            try {
                base.OnSaving();
                if (!(Session is NestedUnitOfWork)
                    && (Session.DataLayer != null)
                        && (Session.ObjectLayer is SimpleObjectLayer)
                            //OR
                            //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
                            && Session.IsNewObject(this)) {
                    GenerateSequence();
                }
            }
            catch {
                CancelSequence();
                throw;
            }
        }
        private long _SequentialNumber;
        [Browsable(false)]
        [Indexed(Unique = false)]
        public long SequentialNumber {
            get { return _SequentialNumber; }
            set { SetPropertyValue(nameof(SequentialNumber), ref _SequentialNumber, value); }
        }

        private static SequenceGenerator sequenceGenerator;
        private static object syncRoot = new object();

        private void OnSequenceGenerated(long newId) {
            SequentialNumber = newId;
        }
        // Override this method to create multiple sequences based on the current object's property values
        protected virtual string GetSequenceName() {
            return SequenceGenerator.GetBaseSequenceName(ClassInfo);
        }
        public void GenerateSequence() {
            lock (syncRoot) {
                Dictionary<string, bool> typeToExistsMap = new Dictionary<string, bool>();
                foreach (object item in Session.GetObjectsToSave()) {
                    typeToExistsMap[Session.GetClassInfo(item).FullName] = true;
                }
                if (sequenceGenerator == null) {
                    sequenceGenerator = new SequenceGenerator(typeToExistsMap);
                }
                SubscribeToEvents();
                OnSequenceGenerated(sequenceGenerator.GetNextSequence(GetSequenceName()));
            }
        }
        private void AcceptSequence() {
            lock (syncRoot) {
                if (sequenceGenerator != null) {
                    try {
                        sequenceGenerator.Accept();
                    }
                    finally {
                        CancelSequence();
                    }
                }
            }
        }
        private void CancelSequence() {
            lock (syncRoot) {
                UnSubscribeFromEvents();
                if (sequenceGenerator != null) {
                    sequenceGenerator.Close();
                    sequenceGenerator = null;
                }
            }
        }
        private void Session_AfterCommitTransaction(object sender, SessionManipulationEventArgs e) {
            AcceptSequence();
        }
        private void Session_AfterRollBack(object sender, SessionManipulationEventArgs e) {
            CancelSequence();
        }
        private void Session_FailedCommitTransaction(object sender, SessionOperationFailEventArgs e) {
            CancelSequence();
        }
        private void SubscribeToEvents() {
            if (!(Session is NestedUnitOfWork)) {
                Session.AfterCommitTransaction += Session_AfterCommitTransaction;
                Session.AfterRollbackTransaction += Session_AfterRollBack;
                Session.FailedCommitTransaction += Session_FailedCommitTransaction;
            }
        }
        private void UnSubscribeFromEvents() {
            if (!(Session is NestedUnitOfWork)) {
                Session.AfterCommitTransaction -= Session_AfterCommitTransaction;
                Session.AfterRollbackTransaction -= Session_AfterRollBack;
                Session.FailedCommitTransaction -= Session_FailedCommitTransaction;
            }
        }
    }
}