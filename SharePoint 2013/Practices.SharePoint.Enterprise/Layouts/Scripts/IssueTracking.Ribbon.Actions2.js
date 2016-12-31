/// <reference path="_references.js" />

//  Developing Page Components for the Server Ribbon
//  https://msdn.microsoft.com/en-us/library/office/ff407303.aspx

////////////////////////////////////////////////////////////////////////////////
//  Namespace IssueTracking.Ribbon
Type.registerNamespace("IssueTracking.Ribbon2");

////////////////////////////////////////////////////////////////////////////////
//  Apps.ActionsActionsPageComponent
IssueTracking.Ribbon.ActionsPageComponent2 = function () {
    IssueTracking.Ribbon.ActionsPageComponent2.initializeBase(this);
    this.registerWithPageManager();
}

IssueTracking.Ribbon.ActionsPageComponent2.prototype = {
    focusedCommands: null,
    globalCommands: null,

    getId: function () {
        return 'IssueTrackingActionsPageComponent';
    },

    // Initializes the page component
    init: function () {
        this.focusedCommands = [];
        this.globalCommands = [
            IssueTracking.Ribbon.ActionsCommandNames2.StartTracking
        ];
    },

    registerWithPageManager: function () {
        SP.Ribbon.PageManager.get_instance().addPageComponent(this);
    },

    unregisterWithPageManager: function () {
        SP.Ribbon.PageManager.get_instance().removePageComponent(this);
    },

    // Returns a string array with the names of the focused commands. 
    getFocusedCommands: function () {
        return this.focusedCommands;
    },

    // Returns a string array with the names of the global commands
    getGlobalCommands: function () {
        return this.globalCommands;
    },

    // Indicates whether the page component can handle the command that was passed to it.
    canHandleCommand: function (commandId) {
        switch (commandId) {
            case IssueTracking.Ribbon.ActionsCommandNames2.StartTracking:
                return IssueTracking.Ribbon.ActionsCommands2.StartTrackingEnabled();
            default:
                return false;
        }
    },

    // Execute the commands that come from our ribbon button
    handleCommand: function (commandId, properties, sequence) {
        switch (commandId) {
            case IssueTracking.Ribbon.ActionsCommandNames2.StartTracking:
                IssueTracking.Ribbon.ActionsCommands2.StartTracking();
                break;
            default:
                break;
        }
    },

    // Indicates whether the page component can receive the focus. 
    // If this method returns false, the page manager will not register the page component's focused commands.
    isFocusable: function () {
        return true;
    },

    // Is used when the page component receives focus.
    receiveFocus: function () {
        //alert('The page component has received focus.');
        return true;
    },

    // Is called when the page component loses focus.
    yieldFocus: function () {
        //alert('The page component has lost focus.');
        return true;
    }
}

////////////////////////////////////////////////////////////////////////////////
//  IssueTracking.Ribbon.AppActionsCommandNames
IssueTracking.Ribbon.ActionsCommandNames2 = function () {
}
IssueTracking.Ribbon.ActionsCommandNames2.StartTracking = "IssueTracking.Actions.StartTracking";

////////////////////////////////////////////////////////////////////////////////
// IssueTracking.Ribbon.ActionsCommands2
IssueTracking.Ribbon.ActionsCommands2 = function () {
}
IssueTracking.Ribbon.ActionsCommands2.StartTrackingEnabled = function () {
    if (undefined == window.itemState) {
        window.itemState = [];
    }
    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var count = CountDictionary(selectedItems);
    if (count == 1) {
        var listId = SP.ListOperation.Selection.getSelectedList();
        var itemId = selectedItems[0].id;

        if (IsNullOrUndefined(window.itemState[itemId])) {
            EnsureScriptFunc("SP.WorkflowServices.js", "SP.WorkflowServices.WorkflowServicesManager", function () {
                var clientContext = SP.ClientContext.get_current();
                var serviceManager = SP.WorkflowServices.WorkflowServicesManager.newObject(clientContext, clientContext.get_web());
                var instanceService = serviceManager.getWorkflowInstanceService();
                var instances = instanceService.enumerateInstancesForListItem(listId, itemId);
                clientContext.load(instances);
                clientContext.executeQueryAsync(Function.createDelegate(this, onQuerySucceeded), Function.createDelegate(this, onQueryFailed));
                return false;

                function onQuerySucceeded(sender, args) {
                    console.log("Instances load success. Attempting to find workflow.");
                    var flag = true;
                    for (var i = 0; i < instances.get_count() ; i++) {
                        var instance = instances.get_item(i);
                        if (instance.get_status() == 1) {
                            flag = false;
                            break;
                        }
                    }
                    window.itemState[itemId] = flag;
                    RefreshCommandUI();
                }

                function onQueryFailed(sender, args) {
                    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
                    console.log("Error: " + args.get_message() + "\n" + args.get_stackTrace());
                }
            });
        } else {
            return window.itemState[selectedItems[0].id];
        }
    } else {
        return false;
    }
}
IssueTracking.Ribbon.ActionsCommands2.StartTracking = function () {
    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var context = SP.ClientContext.get_current();
    var listId = SP.ListOperation.Selection.getSelectedList();
    var itemId = selectedItems[0].id;
    EnsureScriptFunc("SP.js", "SP.Utilities.Utility", function () {
        var url = SP.Utilities.Utility.getLayoutsPageUrl("IssueTracking/StartTracking.aspx?ListId=" + listId + "&ItemId=" + itemId);
        //STSNavigate(url);
        SP.SOD.execute("sp.ui.dialog.js", "SP.UI.ModalDialog.showModalDialog", {
            title: "下达隐患整改任务",
            //options.width = 750;
            //options.height = 450;
            allowMaximize: true,
            autoSize: true,
            url: url,
            dialogReturnValueCallback: onDialogClose
        });
    });
    function onDialogClose(dialogResult, returnValue) {
        if (dialogResult == SP.UI.DialogResult.OK) {
            console.log(returnValue);
            alert('隐患整改任务已下达!');
            refreshListView();
        }
        if (dialogResult == SP.UI.DialogResult.Cancel) {
            console.log(returnValue);
            alert('隐患整改任务已取消');
            refreshListView();
        }
    }
}
////////////////////////////////////////////////////////////////////////////////
//  RegisterClass
IssueTracking.Ribbon.ActionsCommands2.registerClass("IssueTracking.Ribbon.ActionsCommands2");
IssueTracking.Ribbon.ActionsCommandNames2.registerClass("IssueTracking.Ribbon.ActionsCommandNames2");
IssueTracking.Ribbon.ActionsPageComponent2.registerClass("IssueTracking.Ribbon.ActionsPageComponent2", CUI.Page.PageComponent);


////////////////////////////////////////////////////////////////////////////////
//  Load Apps.Ribbon.ActionsPageComponent instance.
IssueTracking.Ribbon.ActionsPageComponent2.instance = null;
IssueTracking.Ribbon.ActionsPageComponent2.load = function () {
    IssueTracking.Ribbon.ActionsPageComponent2.instance = new IssueTracking.Ribbon.ActionsPageComponent2();
}

////////////////////////////////////////////////////////////////////////////////
//  SP.SOD ( SharePoint scripts on demand ).
if (typeof (Sys) != "undefined" && Sys && Sys.Application) {
    Sys.Application.notifyScriptLoaded();
}
if (typeof (NotifyScriptLoadedAndExecuteWaitingJobs) != "undefined") {
    NotifyScriptLoadedAndExecuteWaitingJobs("IssueTracking.Ribbon.Ribbon.Actions2.js");
}