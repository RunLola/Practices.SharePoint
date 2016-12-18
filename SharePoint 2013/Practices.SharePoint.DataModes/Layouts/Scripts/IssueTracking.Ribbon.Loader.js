/// <reference path="_references.js" />
SP.SOD.executeOrDelayUntilScriptLoaded(function () {
    SP.SOD.executeOrDelayUntilScriptLoaded(function () {
        SP.SOD.executeOrDelayUntilScriptLoaded(function () {
            SP.SOD.registerSod("IssueTracking.Ribbon.Actions.js", "/_layouts/15/Scripts/IssueTracking.Ribbon.Actions.js");
            SP.SOD.execute("IssueTracking.Ribbon.Actions.js", "Practices.IssueTracking.ActionsPageComponent.load");
        }, "sp.ribbon.js");
    }, "cui.js");
}, "sp.js");
