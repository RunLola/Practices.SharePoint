/// <reference path="_references.js" />
SP.SOD.executeOrDelayUntilScriptLoaded(function () {
    SP.SOD.executeOrDelayUntilScriptLoaded(function () {
        SP.SOD.executeOrDelayUntilScriptLoaded(function () {
            SP.SOD.registerSod("Apps.Ribbon.Actions.js", "/_layouts/15/Scripts/Apps.Ribbon.Actions.js");
            SP.SOD.execute('Apps.Ribbon.Actions.js', 'Practices.Apps.ActionsPageComponent.load');
        }, "sp.ribbon.js");
    }, "cui.js");
}, "sp.js");
