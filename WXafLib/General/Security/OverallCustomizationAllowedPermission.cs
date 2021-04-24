using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXafLib.General.Security {
    public class ExecuteActionPermission : IOperationPermission {
        private string fActionId;
        public ExecuteActionPermission(string actionId) {
            fActionId = actionId;
        }
        string IOperationPermission.Operation {
            get { return fActionId; }
        }
    }
    public class ExecuteActionPermissionRequest : IPermissionRequest {
        public ExecuteActionPermissionRequest(string actionId) {
            this.ActionId = actionId;
        }
        object IPermissionRequest.GetHashObject() {
            return GetType().FullName + ActionId;
        }
        public string ActionId { get; private set; }

        public static void Register(XafApplication application) {
            if (application != null) {
                application.SetupComplete += ApplicationOnSetupComplete;
            }
        }

        static void ApplicationOnSetupComplete(object sender, EventArgs eventArgs) {
            var securityStrategy = SecuritySystem.Instance as SecurityStrategy;
            if (securityStrategy != null) (securityStrategy).CustomizeRequestProcessors += OnCustomizeRequestProcessors;
        }
        static void OnCustomizeRequestProcessors(object sender, CustomizeRequestProcessorsEventArgs e) {
            List<IOperationPermission> result = new List<IOperationPermission>();
            SecurityStrategyComplex security = (SecurityStrategyComplex)sender;
            WXafUser user = (WXafUser)security.User;
            foreach (WXafRole role in user.Roles)
                foreach (ActionPermission action in role.ActionPermissions)
                    result.Add(new ExecuteActionPermission(action.ActionId));
            IPermissionDictionary dictionary = new PermissionDictionary(result);
            e.Processors.Add(typeof(ExecuteActionPermissionRequest), new ExecuteActionRequestProcessor(dictionary));
        }
    }
    public class ExecuteActionRequestProcessor : PermissionRequestProcessorBase<ExecuteActionPermissionRequest> {
        private IPermissionDictionary Permissions;
        public ExecuteActionRequestProcessor(IPermissionDictionary permissions) {
            this.Permissions = permissions;
        }

        public override bool IsGranted(ExecuteActionPermissionRequest permissionRequest) {
            foreach (IOperationPermission permission in Permissions.GetPermissions<IOperationPermission>())
                if (permission.Operation == permissionRequest.ActionId) return true;
            return false;
        }
    }
}
