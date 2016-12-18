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
//  Namespace Practices.Apps
Type.registerNamespace("Practices.Apps");

////////////////////////////////////////////////////////////////////////////////
//  Apps.Actions.ActionsPageComponent
Practices.Apps.ActionsPageComponent = function () {
    Practices.Apps.ActionsPageComponent.initializeBase(this);
    this.registerWithPageManager();
}

Practices.Apps.ActionsPageComponent.prototype = {
    focusedCommands: null,
    globalCommands: null,

    // Initializes the page component
    init: function () {
        this.focusedCommands = [];
        this.globalCommands = [
            Practices.Apps.ActionsCommandNames.Create,
            Practices.Apps.ActionsCommandNames.Upgrade,
            Practices.Apps.ActionsCommandNames.Delete
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
            case Practices.Apps.ActionsCommandNames.Create:
                return Practices.Apps.ActionsCommands.CreateEnabled();
            case Practices.Apps.ActionsCommandNames.Upgrade:
                return Practices.Apps.ActionsCommands.UpgradeEnabled();
            case Practices.Apps.ActionsCommandNames.Delete:
                return Practices.Apps.ActionsCommands.DeleteEnabled();
            default:
                return false;
        }
    },

    // Execute the commands that come from our ribbon button
    handleCommand: function (commandId, properties, sequence) {
        switch (commandId) {
            case Practices.Apps.ActionsCommandNames.Create:
                Practices.Apps.ActionsCommands.Create();
                break;
            case Practices.Apps.ActionsCommandNames.Upgrade:
                Practices.Apps.ActionsCommands.Upgrade();
                break;
            case Practices.Apps.ActionsCommandNames.Delete:
                Practices.Apps.ActionsCommands.Delete();
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
//  Practices.Apps.AppActionsCommandNames
Practices.Apps.ActionsCommandNames = function () {
}
Practices.Apps.ActionsCommandNames.Create = "Apps.Actions.Create";
Practices.Apps.ActionsCommandNames.Upgrade = "Apps.Actions.Upgrade";
Practices.Apps.ActionsCommandNames.Delete = "Apps.Actions.Delete";

////////////////////////////////////////////////////////////////////////////////
// Practices.Apps.AppActionsCommands
Practices.Apps.ActionsCommands = function () {
}

Practices.Apps.ActionsCommands.CreateEnabled = function () {
    return true;
}
Practices.Apps.ActionsCommands.Create = function () {
    //var url = "/_vti_bin/Apps.svc/Apps/Generate/";
    ////if (_spPageContextInfo.webServerRelativeUrl.length > 1) {
    ////    url = _spPageContextInfo.webServerRelativeUrl + url;
    ////}
    //url = "/Sites/Team" + url;
    //var data = {
    //    title: "应用测试01",
    //    launchUrl: "http://practices.contoso.com/sites/Team/_layouts/15/addanapp.aspx",
    //    fields: [{ Key: "AppShortDescription", Value: "This is the app's description." },
    //             { Key: "AppDescription", Value: "This is the app's short description." }]
    //};

    //var productId = "00000000-0000-0000-0000-000000000000";
    //$.ajax({
    //    type: "POST",
    //    url: url + productId,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    data: JSON.stringify(data),
    //}).done(function (result) {
    //    refreshListView();
    //}).fail(function () {
    //})
}

Practices.Apps.ActionsCommands.UpgradeEnabled = function () {
    if (undefined == window.itemState) {
        window.itemState = [];
    }
    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var count = CountDictionary(selectedItems);
    if (count == 1) {
        if (IsNullOrUndefined(window.itemState[selectedItems[0].id])) {
            var clientContext = SP.ClientContext.get_current();
            var listId = SP.ListOperation.Selection.getSelectedList();
            var list = clientContext.get_web().get_lists().getById(listId);
            var listItem = list.getItemById(selectedItems[0].id);
            clientContext.load(listItem);
            clientContext.executeQueryAsync(OnQuerySucceeded, OnQueryFailed);
            return false;
        }
        else {
            return window.itemState[selectedItems[0].id];
        }
    } else {
        return false;
    }

    function OnQuerySucceeded(sender, args) {
        var isValid = listItem.get_item('IsValidAppPackage');
        window.itemState[listItem.get_id()] = isValid;
        RefreshCommandUI();
    }

    function OnQueryFailed() {
    }
}
Practices.Apps.ActionsCommands.Upgrade = function () {
    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var clientContext = SP.ClientContext.get_current();
    var listId = SP.ListOperation.Selection.getSelectedList();
    var corporateCatalog = clientContext.get_web().get_lists().getById(listId);
    var listItem = corporateCatalog.getItemById(selectedItems[0].id);
    clientContext.load(listItem);
    clientContext.executeQueryAsync(OnSelectedItemQuerySucceeded, OnSelectedItemQueryFailed);

    function OnSelectedItemQuerySucceeded(sender, args) {
        var productId = listItem.get_item("AppProductID").toString();
        var sourceUrl = SP.PageContextInfo.get_webServerRelativeUrl() + "/AppCatalog";
        var url = SP.Utilities.Utility.getLayoutsPageUrl("Apps/Register.aspx?ProductId=" + productId + "&Source=" + sourceUrl);
        STSNavigate(url);
    }

    function OnSelectedItemQueryFailed() {
    }
}

Practices.Apps.ActionsCommands.DeleteEnabled = function () {
    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var count = CountDictionary(selectedItems);
    if (count > 0) {
        return true;
    } else {
        return false;
    }
}
Practices.Apps.ActionsCommands.Delete = function () {
    //var context = SP.ClientContext.get_current();
    //var listId = SP.ListOperation.Selection.getSelectedList();
    //var corporateCatalog = context.get_web().get_lists().getById(listId);

    //var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    //var count = CountDictionary(selectedItems);
    //var i = 0;
    //destoryApp(i, selectedItems);

    //function destoryApp(i, selectedItems) {
    //    var listItem = corporateCatalog.getItemById(selectedItems[i].id);
    //    context.load(listItem);
    //    context.executeQueryAsync(OnSelectedItemQuerySucceeded, OnSelectedItemQueryFailed);

    //    function OnSelectedItemQuerySucceeded(sender, args) {
    //        var productId = listItem.get_item("AppProductID").toString();
    //        //alert(productId);
    //        var url = "/_vti_bin/Apps.svc/" + productId;
    //        if (_spPageContextInfo.webServerRelativeUrl.length > 1) {
    //            url = _spPageContextInfo.webServerRelativeUrl + url;
    //        }

    //        $.ajax({
    //            type: "DELETE",
    //            url: url,
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //        }).done(function (result) {
    //            i += 1;
    //            if (i < count) {
    //                SP.UI.Notify.addNotification("已删除" + i + "个应用，共" + count + "个。", false);
    //                destoryApp(i, selectedItems);
    //            } else {
    //                SP.UI.Notify.addNotification("操作完毕。", false);
    //                refreshListView();
    //            }
    //        }).fail(function () {
    //        })
    //    }
    //    function OnSelectedItemQueryFailed() {
    //    }
    //}
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
Practices.Apps.ActionsCommands.registerClass("Practices.Apps.ActionsCommands");
Practices.Apps.ActionsCommandNames.registerClass("Practices.Apps.ActionsCommandNames");
Practices.Apps.ActionsPageComponent.registerClass("Practices.Apps.ActionsPageComponent", CUI.Page.PageComponent);


////////////////////////////////////////////////////////////////////////////////
//  Load Apps.Ribbon.Actions.PageComponent instance.
Practices.Apps.ActionsPageComponent.instance = null;
Practices.Apps.ActionsPageComponent.load = function () {
    Practices.Apps.ActionsPageComponent.instance = new Practices.Apps.ActionsPageComponent();
}

//ExecuteAndRegisterBeginEndFunctions("Apps.Ribbon.Actions.js",
//    null, null,
//    function () {
//        if (typeof (_spBodyOnLoadCalled) == 'undefined' || _spBodyOnLoadCalled) {
//            window.setTimeout(Practices.Apps.ActionsPageComponent.load, 0);
//        }
//        else {
//            _spBodyOnLoadFunctionNames.push("Practices.Apps.ActionsPageComponent.load");
//        }
//    });

////////////////////////////////////////////////////////////////////////////////
//  SP.SOD ( SharePoint scripts on demand ).
if (typeof (Sys) != "undefined" && Sys && Sys.Application) {
    Sys.Application.notifyScriptLoaded();
}
if (typeof (NotifyScriptLoadedAndExecuteWaitingJobs) != "undefined") {
    NotifyScriptLoadedAndExecuteWaitingJobs("Apps.Ribbon.Actions.js");
}
