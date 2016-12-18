/// <reference path="_references.js" />


//payload["Init"] = "任务类型2";
//payload["FieldName"] = "Approvers";
function GetWorkflow(listId, itemId, workflowName) {
    var clientContext = SP.ClientContext.get_current();
    var serviceManager = SP.WorkflowServices.WorkflowServicesManager.newObject(clientContext, clientContext.get_web());
    var instanceService = serviceManager.getWorkflowInstanceService();
    var instances = instanceService.enumerateInstancesForListItem(listId, listItemId);
    clientContext.load(instances);
    clientContext.executeQueryAsync(Function.createDelegate(this, onQuerySucceeded), Function.createDelegate(this, onQueryFailed));

    function onQuerySucceeded(sender, args) {
        console.log("Instances load success. Attempting to find workflow.");
        for (var i = 0; i < instances.get_count() ; i++) {
            var instance = instances.get_item(i);
        }
    }

    function onQueryFailed(sender, args) {
        console.log("Instances load failed. Attempting to find workflow.");
        console.log("Error: " + args.get_message() + "\n" + args.get_stackTrace());
    }
}


function StartWorkflow(listId, itemId, workflowName, payload) {
    var clientContext = SP.ClientContext.get_current();
    var serviceManager = SP.WorkflowServices.WorkflowServicesManager.newObject(clientContext, clientContext.get_web());
    var subscriptionService = serviceManager.getWorkflowSubscriptionService();
    var subscriptions = subscriptionService.enumerateSubscriptionsByList(listId);
    clientContext.load(subscriptions);
    clientContext.executeQueryAsync(Function.createDelegate(this, onQuerySucceeded), Function.createDelegate(this, onQueryFailed));

    function onQuerySucceeded(sender, args) {
        console.log("Subscriptions load success. Attempting to start workflow.");
        for (var i = 0; i < subscriptions.get_count() ; i++) {
            var subscription = subscriptions.get_item(i);
            if (subscription.get_name() == workflowName) {
                console.log("Subscriptions find success. Attempting to start workflow.");
                //var payload = new Object();
                var formData = subscription.get_propertyDefinitions()["FormData"];
                if (formData != null && formData != 'undefined' && formData != "") {
                    var assocParams = formData.split(";#");
                    for (var i = 0; i < assocParams.length; i++) {
                        payload[assocParams[i]] = subscription.get_propertyDefinitions()[assocParams[i]];
                    }
                }

                var instanceService = serviceManager.getWorkflowInstanceService();
                instanceService.startWorkflowOnListItem(subscription, itemId, payload);

                clientContext.executeQueryAsync(
                   function (sender, args) {
                       console.log("Successfully starting workflow.");
                       SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.OK, "Successfully starting workflow.");
                   },
                   function (sender, args) {
                       console.log("Failed to start workflow.");
                       console.log("Error: " + args.get_message() + "\n" + args.get_stackTrace());
                   }
                );
            }
        }
    }

    function onQueryFailed(sender, args) {
        console.log("Failed to load Subscriptions.");
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