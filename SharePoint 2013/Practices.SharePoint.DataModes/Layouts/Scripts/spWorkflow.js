/// <reference path="_references.js" />

function StartWorkflow(subscriptionId, itemId, startParams) {
    showInProgressDialog();
    var clientContext = SP.ClientContext.get_current();
    var workflowServiceManager = SP.WorkflowServices.WorkflowServicesManager.newObject(clientContext, clientContext.get_web());
    var workflowSubscriptionService = workflowServiceManager.getWorkflowSubscriptionService();
    var subscription = workflowSubscriptionService.getSubscription(subscriptionId);
    clientContext.load(subscription, 'PropertyDefinitions');
    clientContext.executeQueryAsync(Function.createDelegate(this, onQuerySucceeded), Function.createDelegate(this, onQueryFailed));

    function onQuerySucceeded(sender, args) {
        console.log("Subscription load success. Attempting to start workflow.");
        var payload = new Object();
        var formData = subscription.get_propertyDefinitions()["FormData"];
        if (formData != null && formData != 'undefined' && formData != "") {
            var assocParams = formData.split(";#");
            for (var i = 0; i < assocParams.length; i++) {
                payload[assocParams[i]] = subscription.get_propertyDefinitions()[assocParams[i]];
            }
        }

        payload["Init"] = "任务类型2";
        payload["FieldName"] = "Approvers";

        var workflowInstanceService = workflowServiceManager.getWorkflowInstanceService();
        workflowInstanceService.startWorkflowOnListItem(subscription, itemId, payload);

        clientContext.executeQueryAsync(
           function (sender, args) {
               closeInProgressDialog();
               console.log("Successfully starting workflow.");
           },
           function (sender, args) {
               closeInProgressDialog();
               console.log("Failed to start workflow.");
               console.log("Error: " + args.get_message() + "\n" + args.get_stackTrace());
           }
        );
    }

    function onQueryFailed(sender, args) {
        alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
        console.log("Error: " + args.get_message() + "\n" + args.get_stackTrace());
    }
}

////////////////////////////////////////////////////////////////////////////////
//  SP.SOD ( SharePoint scripts on demand ).
if (typeof (Sys) != "undefined" && Sys && Sys.Application) {
    Sys.Application.notifyScriptLoaded();
}
if (typeof (NotifyScriptLoadedAndExecuteWaitingJobs) != "undefined") {
    NotifyScriptLoadedAndExecuteWaitingJobs("spWorkflow.js");
}