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
//  Namespace Practices.IssueTracking
Type.registerNamespace("Practices.IssueTracking");

////////////////////////////////////////////////////////////////////////////////
//  Apps.Actions.ActionsPageComponent
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
            Practices.IssueTracking.ActionsCommandNames.Create,
            Practices.IssueTracking.ActionsCommandNames.Upgrade,
            Practices.IssueTracking.ActionsCommandNames.Delete
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
            case Practices.IssueTracking.ActionsCommandNames.StartWorkflow:
                return Practices.IssueTracking.ActionsCommands.StartWorkflowEnabled();
            default:
                return false;
        }
    },

    // Execute the commands that come from our ribbon button
    handleCommand: function (commandId, properties, sequence) {
        switch (commandId) {
            case Practices.IssueTracking.ActionsCommandNames.StartWorkflow:
                Practices.IssueTracking.ActionsCommands.StartWorkflow();
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
Practices.IssueTracking.ActionsCommandNames.StartWorkflow = "IssueTracking.Actions.StartWorkflow";

////////////////////////////////////////////////////////////////////////////////
// Practices.IssueTracking.AppActionsCommands
Practices.IssueTracking.ActionsCommands = function () {
}

Practices.IssueTracking.ActionsCommands.StartWorkflowEnabled = function () {
    if (undefined == window.itemState) {
        window.itemState = [];
    }
    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var count = CountDictionary(selectedItems);
    if (count == 1) {
        if (IsNullOrUndefined(window.itemState[selectedItems[0].id])) {
            var context = SP.ClientContext.get_current();
            var listId = SP.ListOperation.Selection.getSelectedList();
            var list = context.get_web().get_lists().getById(listId);
            var item = list.getItemById(selectedItems[0].id);
            context.load(item);
            clientContext.executeQueryAsync(Function.createDelegate(this, onQuerySucceeded), Function.createDelegate(this, onQueryFailed));

            function onQuerySucceeded(sender, args) {
                var isValid = listItem.get_item('IsValidAppPackage');
                window.itemState[listItem.get_id()] = isValid;
                RefreshCommandUI();
            }

            function onQueryFailed(sender, args) {
                alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
                console.log("Error: " + args.get_message() + "\n" + args.get_stackTrace());
            }
            return false;
        }
        else {
            return window.itemState[selectedItems[0].id];
        }
    } else {
        return false;
    }


}
Practices.IssueTracking.ActionsCommands.StartWorkflow = function () {
    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var context = SP.ClientContext.get_current();
    var listId = SP.ListOperation.Selection.getSelectedList();
    var itemId = selectedItems[0].id;
    var url = SP.Utilities.Utility.getLayoutsPageUrl("Practices/StartTracking.aspx?ListId=" + listId + "&Id=" + itemId);
    //STSNavigate(url);
    var options = SP.UI.$create_DialogOptions();
    options.title = "下达隐患";
    options.width = 600;
    options.height = 450;
    options.url = url;
    SP.UI.ModalDialog.showModalDialog(options);
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
//  Load Apps.Ribbon.Actions.PageComponent instance.
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
    NotifyScriptLoadedAndExecuteWaitingJobs("IssueTracking.Ribbon.Actions.js");
}