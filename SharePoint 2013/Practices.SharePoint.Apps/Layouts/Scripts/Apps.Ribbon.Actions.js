/// <reference path="_references.js" />

//  Developing Page Components for the Server Ribbon
//  https://msdn.microsoft.com/en-us/library/office/ff407303.aspx

////////////////////////////////////////////////////////////////////////////////
//  Namespace Apps.Actions
Type.registerNamespace('Apps.Ribbon');

////////////////////////////////////////////////////////////////////////////////
//  Apps.Actions.ActionsPageComponent
Apps.Ribbon.ActionsPageComponent = function () {
    Apps.Ribbon.ActionsPageComponent.initializeBase(this);
    this.registerWithPageManager();
}

Apps.Ribbon.ActionsPageComponent.prototype = {
    focusedCommands: null,
    globalCommands: null,

    // Initializes the page component
    init: function () {
        this.focusedCommands = [];
        this.globalCommands = [
            Apps.Ribbon.ActionsCommandNames.Create,
            Apps.Ribbon.ActionsCommandNames.Upgrade,
            Apps.Ribbon.ActionsCommandNames.Delete
        ];
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
            case Apps.Ribbon.ActionsCommandNames.Create:
                return Apps.Ribbon.ActionsCommands.CreateEnabled();
            case Apps.Ribbon.ActionsCommandNames.Upgrade:
                return Apps.Ribbon.ActionsCommands.UpgradeEnabled();
            case Apps.Ribbon.ActionsCommandNames.Delete:
                return Apps.Ribbon.ActionsCommands.DeleteEnabled();
            default:
                return false;
        }
    },

    // Execute the commands that come from our ribbon button
    handleCommand: function (commandId, properties, sequence) {
        switch (commandId) {
            case Apps.Ribbon.ActionsCommandNames.Create:
                Apps.Ribbon.ActionsCommands.Create();
                break;
            case Apps.Ribbon.ActionsCommandNames.Upgrade:
                Apps.Ribbon.ActionsCommands.Upgrade();
                break;
            case Apps.Ribbon.ActionsCommandNames.Delete:
                Apps.Ribbon.ActionsCommands.Delete();
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
    },

    registerWithPageManager: function () {
        SP.Ribbon.PageManager.get_instance().addPageComponent(this);
    },

    unregisterWithPageManager: function () {
        SP.Ribbon.PageManager.get_instance().removePageComponent(this);
    }
}

////////////////////////////////////////////////////////////////////////////////
// Apps.Actions.AppActionsCommands
Apps.Ribbon.ActionsCommands = function () {
}

Apps.Ribbon.ActionsCommands.CreateEnabled = function () {
    return true;
}
Apps.Ribbon.ActionsCommands.Create = function () {
    var url = "/_vti_bin/Apps.svc/Apps/Generate/";
    //if (_spPageContextInfo.webServerRelativeUrl.length > 1) {
    //    url = _spPageContextInfo.webServerRelativeUrl + url;
    //}
    url = "/Sites/Team" + url;
    var data = {
        title: "应用测试01",
        launchUrl: "http://practices.contoso.com/sites/Team/_layouts/15/addanapp.aspx",
        fields: [{ Key: "AppShortDescription", Value: "This is the app's description." },
                 { Key: "AppDescription", Value: "This is the app's short description." }]
    };

    var productId = "00000000-0000-0000-0000-000000000000";
    $.ajax({
        type: "POST",
        url: url + productId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
    }).done(function (result) {
        refreshListView();
    }).fail(function () {
    })
}

Apps.Ribbon.ActionsCommands.UpgradeEnabled = function () {
    if (undefined == window.itemState) {
        window.itemState = [];
    }
    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var count = CountDictionary(selectedItems);
    if (count == 1) {
        if (IsNullOrUndefined(window.itemState[selectedItems[0].id])) {
            var context = SP.ClientContext.get_current();
            var listId = SP.ListOperation.Selection.getSelectedList();
            var corporateCatalog = context.get_web().get_lists().getById(listId);
            var listItem = corporateCatalog.getItemById(selectedItems[0].id);
            context.load(listItem);
            context.executeQueryAsync(OnSelectedItemQuerySucceeded, OnSelectedItemQueryFailed);
            return false;
        }
        else {
            return window.itemState[selectedItems[0].id];
        }
    } else {
        return false;
    }

    function OnSelectedItemQuerySucceeded(sender, args) {
        var isValid = listItem.get_item('IsValidAppPackage');
        window.itemState[listItem.get_id()] = isValid;
        RefreshCommandUI();
    }

    function OnSelectedItemQueryFailed() {
    }
}
Apps.Ribbon.ActionsCommands.Upgrade = function () {
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

Apps.Ribbon.ActionsCommands.DeleteEnabled = function () {
    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var count = CountDictionary(selectedItems);
    if (count > 0) {
        return true;
    } else {
        return false;
    }
}
Apps.Ribbon.ActionsCommands.Delete = function () {
    var context = SP.ClientContext.get_current();
    var listId = SP.ListOperation.Selection.getSelectedList();
    var corporateCatalog = context.get_web().get_lists().getById(listId);

    var selectedItems = SP.ListOperation.Selection.getSelectedItems();
    var count = CountDictionary(selectedItems);    
    var i = 0;
    destoryApp(i, selectedItems);

    function destoryApp(i, selectedItems) {
        var listItem = corporateCatalog.getItemById(selectedItems[i].id);
        context.load(listItem);
        context.executeQueryAsync(OnSelectedItemQuerySucceeded, OnSelectedItemQueryFailed);

        function OnSelectedItemQuerySucceeded(sender, args) {
            var productId = listItem.get_item("AppProductID").toString();
            //alert(productId);
            var url = "/_vti_bin/Apps.svc/" + productId;
            if (_spPageContextInfo.webServerRelativeUrl.length > 1) {
                url = _spPageContextInfo.webServerRelativeUrl + url;
            }

            $.ajax({
                type: "DELETE",
                url: url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
            }).done(function (result) {
                i += 1;
                if (i < count) {
                    SP.UI.Notify.addNotification("已删除" + i + "个应用，共" + count + "个。", false);
                    destoryApp(i, selectedItems);                    
                } else {
                    SP.UI.Notify.addNotification("操作完毕。", false);
                    refreshListView();
                }
            }).fail(function () {
            })
        }
        function OnSelectedItemQueryFailed() {
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
//  Apps.Actions.AppActionsCommandNames
Apps.Ribbon.ActionsCommandNames = function () {
}

Apps.Ribbon.ActionsCommandNames.Create = 'Apps.Actions.Create';
Apps.Ribbon.ActionsCommandNames.Upgrade = 'Apps.Actions.Upgrade';
Apps.Ribbon.ActionsCommandNames.Delete = 'Apps.Actions.Delete';

////////////////////////////////////////////////////////////////////////////////
//  RegisterClass
Apps.Ribbon.ActionsPageComponent.registerClass('Apps.Ribbon.ActionsPageComponent', CUI.Page.PageComponent);
Apps.Ribbon.ActionsCommands.registerClass('Apps.Ribbon.ActionsCommands');
Apps.Ribbon.ActionsCommandNames.registerClass('Apps.Ribbon.ActionsCommandNames');

////////////////////////////////////////////////////////////////////////////////
//  Load Apps.Ribbon.Actions.PageComponent instance.
Apps.Ribbon.ActionsPageComponent.instance = null;
Apps.Ribbon.ActionsPageComponent.load = function () {
    Apps.Ribbon.ActionsPageComponent.instance = new Apps.Ribbon.ActionsPageComponent();
}

//Apps.Ribbon.ActionsPageComponent.instance = new Apps.Ribbon.ActionsPageComponent();
//Apps.Ribbon.ActionsPageComponent.initialize = function () {
//    SP.SOD.executeOrDelayUntilScriptLoaded(
//            Function.createDelegate(null, Apps.Ribbon.ActionsPageComponent.initializePageComponent),
//            "SP.Ribbon.js");
//}
//Apps.Ribbon.ActionsPageComponent.initializePageComponent = function () {
//    var ribbonPageManager = SP.Ribbon.PageManager.get_instance();
//    if (null !== ribbonPageManager) {
//        ribbonPageManager.addPageComponent(Apps.Ribbon.ActionsPageComponent.instance);
//    }
//}

////////////////////////////////////////////////////////////////////////////////
//  SP.SOD ( SharePoint scripts on demand ).
if (typeof (Sys) != "undefined" && Sys && Sys.Application) {
    Sys.Application.notifyScriptLoaded();
}
if (typeof (NotifyScriptLoadedAndExecuteWaitingJobs) != "undefined") {
    NotifyScriptLoadedAndExecuteWaitingJobs("Apps.Ribbon.Actions.js");
}