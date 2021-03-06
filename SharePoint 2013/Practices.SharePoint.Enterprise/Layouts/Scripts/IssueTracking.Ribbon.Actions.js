﻿/// <reference path="_references.js" />

//  Developing Page Components for the Server Ribbon
//  https://msdn.microsoft.com/en-us/library/office/ff407303.aspx

////////////////////////////////////////////////////////////////////////////////
//  Namespace IssueTracking.Ribbon
Type.registerNamespace("IssueTracking.Ribbon");

////////////////////////////////////////////////////////////////////////////////
//  Apps.ActionsActionsPageComponent
IssueTracking.Ribbon.ActionsPageComponent = function () {
    IssueTracking.Ribbon.ActionsPageComponent.initializeBase(this);
    this.registerWithPageManager();
}

IssueTracking.Ribbon.ActionsPageComponent.prototype = {
    focusedCommands: null,
    globalCommands: null,

    getId: function () {
        return 'IssueTrackingActionsPageComponent';
    },

    // Initializes the page component
    init: function () {
        this.focusedCommands = [];
        this.globalCommands = [
            IssueTracking.Ribbon.ActionsCommandNames.StartTracking,
            IssueTracking.Ribbon.ActionsCommandNames.StartBlaming,
            IssueTracking.Ribbon.ActionsCommandNames.StartForfeit,
            IssueTracking.Ribbon.ActionsCommandNames.StartSpotCheck
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
            case IssueTracking.Ribbon.ActionsCommandNames.StartTracking:
                return IssueTracking.Ribbon.ActionsCommands.StartTrackingEnabled();
            case IssueTracking.Ribbon.ActionsCommandNames.StartBlaming:
                return IssueTracking.Ribbon.ActionsCommands.StartBlamingEnabled();
            case IssueTracking.Ribbon.ActionsCommandNames.StartForfeit:
                return IssueTracking.Ribbon.ActionsCommands.StartForfeitEnabled();
            case IssueTracking.Ribbon.ActionsCommandNames.StartSpotCheck:
                return IssueTracking.Ribbon.ActionsCommands.StartSpotCheckEnabled();
            default:
                return false;
        }
    },

    // Execute the commands that come from our ribbon button
    handleCommand: function (commandId, properties, sequence) {
        switch (commandId) {
            case IssueTracking.Ribbon.ActionsCommandNames.StartTracking:
                IssueTracking.Ribbon.ActionsCommands.StartTracking();
                break;
            case IssueTracking.Ribbon.ActionsCommandNames.StartBlaming:
                IssueTracking.Ribbon.ActionsCommands.StartBlaming();
                break;
            case IssueTracking.Ribbon.ActionsCommandNames.StartForfeit:
                IssueTracking.Ribbon.ActionsCommands.StartForfeit();
            case IssueTracking.Ribbon.ActionsCommandNames.StartSpotCheck:
                IssueTracking.Ribbon.ActionsCommands.StartSpotCheck();
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
IssueTracking.Ribbon.ActionsCommandNames = function () {
}
IssueTracking.Ribbon.ActionsCommandNames.StartTracking = "IssueTracking.Actions.StartTracking";
IssueTracking.Ribbon.ActionsCommandNames.StartBlaming = "IssueTracking.Actions.StartBlaming";
IssueTracking.Ribbon.ActionsCommandNames.StartForfeit = "IssueTracking.Actions.StartForfeit";
IssueTracking.Ribbon.ActionsCommandNames.StartSpotCheck = "IssueTracking.Actions.StartSpotCheck";

////////////////////////////////////////////////////////////////////////////////
// IssueTracking.Ribbon.ActionsCommands
IssueTracking.Ribbon.ActionsCommands = function () {
}
IssueTracking.Ribbon.ActionsCommands.StartTrackingEnabled = function () {
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
IssueTracking.Ribbon.ActionsCommands.StartTracking = function () {
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
IssueTracking.Ribbon.ActionsCommands.StartBlamingEnabled = function () {
    var selectedItems = $("#can").find("table[id$='GridView']").getSelectedItems();
    var count = selectedItems.length;
    if (count == 1) {
        return true;
    } else {
        return false;
    }
}
IssueTracking.Ribbon.ActionsCommands.StartBlaming = function () {
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
IssueTracking.Ribbon.ActionsCommands.StartForfeitEnabled = function () {
    var selectedItems = $("#can").find("table[id$='GridView']").getSelectedItems();
    var count = selectedItems.length;
    if (count == 1) {
        return true;
    } else {
        return false;
    }
}
IssueTracking.Ribbon.ActionsCommands.StartForfeit = function () {
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
IssueTracking.Ribbon.ActionsCommands.StartSpotCheckEnabled = function () {
    var selectedItems = $("#table[id$='GridView']").getSelectedItems();
    var count = selectedItems.length;
    if (count == 1) {
        return true;
    } else {
        return false;
    }
}
IssueTracking.Ribbon.ActionsCommands.StartSpotCheck = function () {
    var selectedItems = $("#can").find("table[id$='GridView']").getSelectedItems();
    var selectedItem = selectedItems[0];
    var webId = selectedItem.WebId;
    var listId = selectedItem.ListId;
    var itemId = selectedItem.ItemId;
    EnsureScriptFunc("SP.js", "SP.Utilities.Utility", function () {
        var url = SP.Utilities.Utility.getLayoutsPageUrl("IssueTracking/StartSpotCheck.aspx?WebId=" + webId + "&ListId=" + listId + "&ItemId=" + itemId);
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
IssueTracking.Ribbon.ActionsCommands.registerClass("IssueTracking.Ribbon.ActionsCommands");
IssueTracking.Ribbon.ActionsCommandNames.registerClass("IssueTracking.Ribbon.ActionsCommandNames");
IssueTracking.Ribbon.ActionsPageComponent.registerClass("IssueTracking.Ribbon.ActionsPageComponent", CUI.Page.PageComponent);


////////////////////////////////////////////////////////////////////////////////
//  Load Apps.Ribbon.ActionsPageComponent instance.
IssueTracking.Ribbon.ActionsPageComponent.instance = null;
IssueTracking.Ribbon.ActionsPageComponent.load = function () {
    IssueTracking.Ribbon.ActionsPageComponent.instance = new IssueTracking.Ribbon.ActionsPageComponent();
}

////////////////////////////////////////////////////////////////////////////////
//  SP.SOD ( SharePoint scripts on demand ).
if (typeof (Sys) != "undefined" && Sys && Sys.Application) {
    Sys.Application.notifyScriptLoaded();
}
if (typeof (NotifyScriptLoadedAndExecuteWaitingJobs) != "undefined") {
    NotifyScriptLoadedAndExecuteWaitingJobs("IssueTracking.Ribbon.Ribbon.Actions.js");
}