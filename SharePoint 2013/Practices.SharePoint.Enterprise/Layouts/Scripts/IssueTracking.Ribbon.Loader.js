/// <reference path="_references.js" />

//Type.registerNamespace('CUI');
//Type.registerNamespace('CUI.Page');
//if (typeof (CUI.Page.PageComponent) == "undefined") {
//    CUI.Page.ICommandHandler = function () { };
//    CUI.Page.ICommandHandler.registerInterface('CUI.Page.ICommandHandler');
//    CUI.Page.PageComponent = function () { };
//    CUI.Page.PageComponent.prototype = {
//        init: function () {
//        },
//        getGlobalCommands: function () {
//            return null;
//        }, getFocusedCommands: function () {
//            return null;
//        }, handleCommand: function (commandId, properties, sequence) {
//            return false;
//        }, canHandleCommand: function (commandId) {
//            return false;
//        }, isFocusable: function () {
//            return false;
//        }, receiveFocus: function () {
//            return false;
//        }, yieldFocus: function () {
//            return true;
//        }, getId: function () {
//            return 'PageComponent';
//        }
//    };
//    CUI.Page.PageComponent.registerClass('CUI.Page.PageComponent', null, CUI.Page.ICommandHandler);
//}
SP.SOD.executeOrDelayUntilScriptLoaded(function () {
    SP.SOD.executeOrDelayUntilScriptLoaded(function () {
        SP.SOD.executeOrDelayUntilScriptLoaded(function () {
            SP.SOD.registerSod("IssueTracking.Ribbon.Actions.js", "/_layouts/15/Scripts/IssueTracking.Ribbon.Actions.js");
            SP.SOD.execute("IssueTracking.Ribbon.Actions.js", "Practices.IssueTracking.ActionsPageComponent.load");
        }, "sp.ribbon.js");
    }, "cui.js");
}, "sp.js");