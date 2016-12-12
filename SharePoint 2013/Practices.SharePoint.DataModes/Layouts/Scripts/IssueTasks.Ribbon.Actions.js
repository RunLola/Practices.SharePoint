/// <reference path="_references.js" />

//  Developing Page Components for the Server Ribbon
//  https://msdn.microsoft.com/en-us/library/office/ff407303.aspx

function ULS_SP() {
    if (ULS_SP.caller) {
        ULS_SP.caller.ULSTeamName = "Windows SharePoint Services 4";
        ULS_SP.caller.ULSFileName = "SP.Ribbon.Custom.UI.js";
    }
}

////////////////////////////////////////////////////////////////////////////////
//  Namespace Practices.IssueTasks
Type.registerNamespace("Practices.IssueTasks");

////////////////////////////////////////////////////////////////////////////////
//  Apps.Actions.ActionsPageComponent
Practices.IssueTasks.ActionsPageComponent = function () {
    Practices.IssueTasks.ActionsPageComponent.initializeBase(this);
    this.registerWithPageManager();
}

Practices.IssueTasks.ActionsPageComponent.prototype = {
    focusedCommands: null,
    globalCommands: null,

    // Initializes the page component
    init: function () {
        this.focusedCommands = [];
        this.globalCommands = [
            Practices.IssueTasks.ActionsCommandNames.Create,
            Practices.IssueTasks.ActionsCommandNames.Upgrade,
            Practices.IssueTasks.ActionsCommandNames.Delete
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
            case Practices.IssueTasks.ActionsCommandNames.Create:
                return Practices.IssueTasks.ActionsCommands.CreateEnabled();
            case Practices.IssueTasks.ActionsCommandNames.Upgrade:
                return Practices.IssueTasks.ActionsCommands.UpgradeEnabled();
            case Practices.IssueTasks.ActionsCommandNames.Delete:
                return Practices.IssueTasks.ActionsCommands.DeleteEnabled();
            default:
                return false;
        }
    },

    // Execute the commands that come from our ribbon button
    handleCommand: function (commandId, properties, sequence) {
        switch (commandId) {
            case Practices.IssueTasks.ActionsCommandNames.Create:
                Practices.IssueTasks.ActionsCommands.Create();
                break;
            case Practices.IssueTasks.ActionsCommandNames.Upgrade:
                Practices.IssueTasks.ActionsCommands.Upgrade();
                break;
            case Practices.IssueTasks.ActionsCommandNames.Delete:
                Practices.IssueTasks.ActionsCommands.Delete();
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
//  Practices.IssueTasks.AppActionsCommandNames
Practices.IssueTasks.ActionsCommandNames = function () {
}
Practices.IssueTasks.ActionsCommandNames.Create = "IssueTasks.Actions.Create";
Practices.IssueTasks.ActionsCommandNames.Upgrade = "IssueTasks.Actions.Upgrade";
Practices.IssueTasks.ActionsCommandNames.Delete = "IssueTasks.Actions.Delete";

////////////////////////////////////////////////////////////////////////////////
// Practices.IssueTasks.AppActionsCommands
Practices.IssueTasks.ActionsCommands = function () {
}

Practices.IssueTasks.ActionsCommands.CreateEnabled = function () {
    return true;
}
Practices.IssueTasks.ActionsCommands.Create = function () {
}

Practices.IssueTasks.ActionsCommands.UpgradeEnabled = function () {
    return true;
    //if (undefined == window.itemState) {
    //    window.itemState = [];
    //}
    //var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    //var count = CountDictionary(selectedItems);
    //if (count == 1) {
    //    if (IsNullOrUndefined(window.itemState[selectedItems[0].id])) {
    //        var context = SP.ClientContext.get_current();
    //        var listId = SP.ListOperation.Selection.getSelectedList();
    //        var corporateCatalog = context.get_web().get_lists().getById(listId);
    //        var listItem = corporateCatalog.getItemById(selectedItems[0].id);
    //        context.load(listItem);
    //        context.executeQueryAsync(OnSelectedItemQuerySucceeded, OnSelectedItemQueryFailed);
    //        return false;
    //    }
    //    else {
    //        return window.itemState[selectedItems[0].id];
    //    }
    //} else {
    //    return false;
    //}

    //function OnSelectedItemQuerySucceeded(sender, args) {
    //    var isValid = listItem.get_item('IsValidAppPackage');
    //    window.itemState[listItem.get_id()] = isValid;
    //    RefreshCommandUI();
    //}

    //function OnSelectedItemQueryFailed() {
    //}
}
Practices.IssueTasks.ActionsCommands.Upgrade = function () {
    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var context = SP.ClientContext.get_current();
    var listId = SP.ListOperation.Selection.getSelectedList();
    var corporateCatalog = context.get_web().get_lists().getById(listId);
    var listItem = corporateCatalog.getItemById(selectedItems[0].id);
    context.load(listItem);
    context.executeQueryAsync(OnSelectedItemQuerySucceeded, OnSelectedItemQueryFailed);

    function OnSelectedItemQuerySucceeded(sender, args) {
        var productId = listItem.get_item("AppProductID").toString();
        var sourceUrl = SP.PageContextInfo.get_webServerRelativeUrl() + "/AppCatalog";
        var url = SP.Utilities.Utility.getLayoutsPageUrl("Apps/Register.aspx?ProductId=" + productId + "&Source=" + sourceUrl);
        STSNavigate(url);
    }

    function OnSelectedItemQueryFailed() {
    }
}

Practices.IssueTasks.ActionsCommands.DeleteEnabled = function () {
    return true;
}
Practices.IssueTasks.ActionsCommands.Delete = function () {
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
Practices.IssueTasks.ActionsCommands.registerClass("Practices.IssueTasks.ActionsCommands");
Practices.IssueTasks.ActionsCommandNames.registerClass("Practices.IssueTasks.ActionsCommandNames");
Practices.IssueTasks.ActionsPageComponent.registerClass("Practices.IssueTasks.ActionsPageComponent", CUI.Page.PageComponent);


////////////////////////////////////////////////////////////////////////////////
//  Load Apps.Ribbon.Actions.PageComponent instance.
Practices.IssueTasks.ActionsPageComponent.instance = null;
Practices.IssueTasks.ActionsPageComponent.load = function () {
    Practices.IssueTasks.ActionsPageComponent.instance = new Practices.IssueTasks.ActionsPageComponent();
}

////////////////////////////////////////////////////////////////////////////////
//  SP.SOD ( SharePoint scripts on demand ).
if (typeof (Sys) != "undefined" && Sys && Sys.Application) {
    Sys.Application.notifyScriptLoaded();
}
if (typeof (NotifyScriptLoadedAndExecuteWaitingJobs) != "undefined") {
    NotifyScriptLoadedAndExecuteWaitingJobs("IssueTasks.Ribbon.Actions.js");
}
