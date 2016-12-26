/// <reference path="_references.js" />

//  Developing Page Components for the Server Ribbon
//  https://msdn.microsoft.com/en-us/library/office/ff407303.aspx

////////////////////////////////////////////////////////////////////////////////
//  Namespace Practices.IssueTracking
Type.registerNamespace("Practices.IssueTracking");

////////////////////////////////////////////////////////////////////////////////
//  Apps.ActionsActionsPageComponent
Practices.IssueTracking.ActionsPageComponent = function () {
    Practices.IssueTracking.ActionsPageComponent.initializeBase(this);
    this.registerWithPageManager();
}

Practices.IssueTracking.ActionsPageComponent.prototype = {
    focusedCommands: null,
    globalCommands: null,

    // Initializes the page component
    init: function () {
        this.focusedCommands = [];
        this.globalCommands = [
            Practices.IssueTracking.ActionsCommandNames.StartTracking,
            Practices.IssueTracking.ActionsCommandNames.StartBlaming,
            Practices.IssueTracking.ActionsCommandNames.StartForfeit
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
            case Practices.IssueTracking.ActionsCommandNames.StartTracking:
                return Practices.IssueTracking.ActionsCommands.StartTrackingEnabled();
            case Practices.IssueTracking.ActionsCommandNames.StartBlaming:
                return Practices.IssueTracking.ActionsCommands.StartBlamingEnabled();
            case Practices.IssueTracking.ActionsCommandNames.StartForfeit:
                return Practices.IssueTracking.ActionsCommands.StartForfeitEnabled();
            default:
                return false;
        }
    },

    // Execute the commands that come from our ribbon button
    handleCommand: function (commandId, properties, sequence) {
        switch (commandId) {
            case Practices.IssueTracking.ActionsCommandNames.StartTracking:
                Practices.IssueTracking.ActionsCommands.StartTracking();
                break;
            case Practices.IssueTracking.ActionsCommandNames.StartBlaming:
                Practices.IssueTracking.ActionsCommands.StartBlaming();
                break;
            case Practices.IssueTracking.ActionsCommandNames.StartForfeit:
                Practices.IssueTracking.ActionsCommands.StartForfeit();
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
//  Practices.IssueTracking.AppActionsCommandNames
Practices.IssueTracking.ActionsCommandNames = function () {
}
Practices.IssueTracking.ActionsCommandNames.StartTracking = "IssueTracking.Actions.StartTracking";
Practices.IssueTracking.ActionsCommandNames.StartBlaming = "IssueTracking.Actions.StartBlaming";
Practices.IssueTracking.ActionsCommandNames.StartForfeit = "IssueTracking.Actions.StartForfeit";

////////////////////////////////////////////////////////////////////////////////
// Practices.IssueTracking.ActionsCommands
Practices.IssueTracking.ActionsCommands = function () {
}
Practices.IssueTracking.ActionsCommands.StartTrackingEnabled = function () {
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
Practices.IssueTracking.ActionsCommands.StartTracking = function () {
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
Practices.IssueTracking.ActionsCommands.StartBlamingEnabled = function () {
    var selectedItems = $("#can").find("table[id$='GridView']").getSelectedItems();
    var count = selectedItems.length;
    if (count == 1) {
        return true;
    } else {
        return false;
    }
}
Practices.IssueTracking.ActionsCommands.StartBlaming = function () {
    var selectedItems = $("#can").find("table[id$='GridView']").getSelectedItems();
    var selectedItem = selectedItems[0];
    var webId = selectedItem.WebId;
    var listId = selectedItem.ListId;
    var itemId = selectedItem.ItemId;
    EnsureScriptFunc("SP.js", "SP.Utilities.Utility", function () {
        var url = SP.Utilities.Utility.getLayoutsPageUrl("IssueTracking/StartBlaming.aspx?WebId=" + webId + "&ListId=" + listId + "&ItemId=" + itemId);
        SP.SOD.execute("sp.ui.dialog.js", "SP.UI.ModalDialog.showModalDialog", {
            title: "责任追究",
            width: 750,
            height: 450,
            allowMaximize: true,
            autoSize: true,
            url: url,
            dialogReturnValueCallback: onDialogClose
        });
    });
    function onDialogClose(dialogResult, returnValue) {
        if (dialogResult == SP.UI.DialogResult.OK) {
            console.log(returnValue);
            alert('责任追究已完成!');
            refreshListView();
        }
        if (dialogResult == SP.UI.DialogResult.Cancel) {
            console.log(returnValue);
            alert('责任追究已取消');
            refreshListView();
        }
    }
    ////var clientContext = new SP.ClientContext.getcurrent();
    //var clientContext = new SP.ClientContext(siteUrl);
    //this.list = clientContext.get_web().get_lists().getById(listId);
    //var itemCreateInfo = new SP.ListItemCreationInformation();
    //this.listItem = list.addItem(itemCreateInfo);
    //listItem.set_item("RelatedIssues", new SP.FieldLookupValue().set_lookupId(relatedId));
    //listItem.update();
    //clientContext.load(listItem);
    //clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));

    //function onQuerySucceeded(sender, args) {
    //    alert('IssueTracking created!\n\nId: ' + listItem.get_id() + '\nTitle: ' + listItem.get_item('Title'));
    //    var listId = list.get_id();
    //    //var listId = listItem.get_parentList().get_id();
    //    var id = listItem.get_id();
    //    ExecuteOrDelayUntilScriptLoaded(function () {
    //        var url = _spPageContextInfo.siteServerRelativeUrl + _spPageContextInfo.layoutsUrl +
    //            "/listform.aspx?ListId=" + listId + "&PageType=6&ID=" + id + "&Source=" + encodeURIComponent(serverRequestPath);
    //        STSNavigate(url)
    //    }, "core.js");
    //}

    //function onQueryFailed(sender, args) {
    //    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
    //    console.log("Error: " + args.get_message() + "\n" + args.get_stackTrace());
    //}
}
Practices.IssueTracking.ActionsCommands.StartForfeitEnabled = function () {
    var selectedItems = $("#can").find("table[id$='GridView']").getSelectedItems();
    var count = selectedItems.length;
    if (count == 1) {
        return true;
    } else {
        return false;
    }
}
Practices.IssueTracking.ActionsCommands.StartForfeit = function () {
    var selectedItems = $("#can").find("table[id$='GridView']").getSelectedItems();
    var selectedItem = selectedItems[0];
    var webId = selectedItem.WebId;
    var listId = selectedItem.ListId;
    var itemId = selectedItem.ItemId;
    EnsureScriptFunc("SP.js", "SP.Utilities.Utility", function () {
        var url = SP.Utilities.Utility.getLayoutsPageUrl("IssueTracking/StartForfeit.aspx?WebId=" + webId + "&ListId=" + listId + "&ItemId=" + itemId);
        //STSNavigate(url);
        SP.SOD.execute("sp.ui.dialog.js", "SP.UI.ModalDialog.showModalDialog", {
            title: "进行罚款",
            width: 750,
            height: 450,
            allowMaximize: true,
            autoSize: true,
            url: url,
            dialogReturnValueCallback: onDialogClose
        });
    });
    function onDialogClose(dialogResult, returnValue) {
        if (dialogResult == SP.UI.DialogResult.OK) {
            console.log(returnValue);
            alert('进行罚款已下达!');
            refreshListView();
        }
        if (dialogResult == SP.UI.DialogResult.Cancel) {
            console.log(returnValue);
            alert('进行罚款已取消');
            refreshListView();
        }
    }
}
function refreshListView() {
    var evtAjax = {
        currentCtx: ctx,
        csrAjaxRefresh: true
    };
    AJAXRefreshView(evtAjax, SP.UI.DialogResult.OK);
}

////////////////////////////////////////////////////////////////////////////////
//  RegisterClass
Practices.IssueTracking.ActionsCommands.registerClass("Practices.IssueTracking.ActionsCommands");
Practices.IssueTracking.ActionsCommandNames.registerClass("Practices.IssueTracking.ActionsCommandNames");
Practices.IssueTracking.ActionsPageComponent.registerClass("Practices.IssueTracking.ActionsPageComponent", CUI.Page.PageComponent);


////////////////////////////////////////////////////////////////////////////////
//  Load Apps.Ribbon.ActionsPageComponent instance.
Practices.IssueTracking.ActionsPageComponent.instance = null;
Practices.IssueTracking.ActionsPageComponent.load = function () {
    Practices.IssueTracking.ActionsPageComponent.instance = new Practices.IssueTracking.ActionsPageComponent();
}

////////////////////////////////////////////////////////////////////////////////
//  SP.SOD ( SharePoint scripts on demand ).
if (typeof (Sys) != "undefined" && Sys && Sys.Application) {
    Sys.Application.notifyScriptLoaded();
}
if (typeof (NotifyScriptLoadedAndExecuteWaitingJobs) != "undefined") {
    NotifyScriptLoadedAndExecuteWaitingJobs("Practices.IssueTracking.Ribbon.Actionsjs");
}