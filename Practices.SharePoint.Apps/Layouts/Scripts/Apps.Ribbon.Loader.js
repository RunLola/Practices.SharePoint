/// <reference path="_references.js" />
SP.SOD.executeOrDelayUntilScriptLoaded(function () {
    SP.SOD.executeOrDelayUntilScriptLoaded(function () {
        SP.SOD.executeOrDelayUntilScriptLoaded(function () {
            //var ctx = SP.ClientContext.get_current();
            //var site = ctx.get_site();
            //ctx.load(site);
            //ctx.executeQueryAsync(Function.createDelegate(this, function (sender, args) {
            //    var pageComponentScriptUrl = SP.Utilities.UrlBuilder.urlCombine(site.get_url(), "Style Library/Mavention/Labs/Mavention.SharePoint.Labs.MyCustomAction.PageComponent.js");
            //}));
            SP.SOD.registerSod("Apps.Ribbon.Actions.js", "/_layouts/15/Scripts/Apps.Ribbon.Actions.js");
            SP.SOD.execute("Apps.Ribbon.Actions.js", 'Apps.Ribbon.ActionsPageComponent.load');
            //SP.SOD.execute('Apps.Ribbon.Actions.js', 'Apps.Ribbon.ActionsPageComponent.initialize');            
        }, "sp.ribbon.js");
    }, "cui.js");
}, "sp.js");
