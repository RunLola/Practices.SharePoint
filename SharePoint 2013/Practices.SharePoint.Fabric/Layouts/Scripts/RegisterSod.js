/// <reference path="_references.js" />
/**
 * SP.SOD Methods: https://msdn.microsoft.com/en-us/library/office/ff408081(v=office.14).aspx
 * SP.SOD.execute(key, functionName, args) eq EnsureScriptParams
 * SP.SOD.executeFunc(key, functionName, fn) eq EnsureScriptFunc
 * SP.SOD.executeOrDelayUntilEventNotified(func, eventName) eq ExecuteOrDelayUntilEventNotified
 * SP.SOD.executeOrDelayUntilScriptLoaded(func, depScriptFileName) eq ExecuteOrDelayUntilScriptLoaded
 * SP.SOD.notifyEventAndExecuteWaitingJobs(eventName)  eq NotifyEventAndExecuteWaitingJobs
 * SP.SOD.notifyScriptLoadedAndExecuteWaitingJobs(scriptFileName) eq NotifyScriptLoadedAndExecuteWaitingJobs
 * SP.SOD.registerSod(key, url) eq RegisterSod
 * SP.SOD.registerSodDep(key, dep) eq RegisterSodDep
 * http://stackoverflow.com/questions/36458198/successful-use-of-sp-sod-executeordelayuntileventnotifiedfunc-eventname-in-cs 
 */

// Underscore
RegisterSod("underscore.js", "/_layouts/15/Scripts/underscore/1.8.3/underscore.min.js");

// jQuery
if (!document.addEventListener) {
    RegisterSod("jquery.js", "/_layouts/15/Scripts/jquery/1.12.4/jquery.min.js");
} else {
    RegisterSod("jquery.js", "/_layouts/15/Scripts/jquery/3.1.0/jquery.min.js");
}

// jQuery UI
RegisterSod("jquery.ui.js", "/_layouts/15/Scripts/jquery.ui/1.12.1/jquery.ui.min.js");
RegisterSodDep("jquery.ui.js", "jquery.js");

// Bootstrap
RegisterSod("bootstrap.js", "/_layouts/15/Scripts/bootstrap/3.3.7/bootstrap.min.js");
RegisterSodDep("bootstrap.js", "jquery.js");
// HTML5 Shiv and Respond.js IE8 support of HTML5 elements and media queries
if (!document.addEventListener) {
    RegisterSod("html5shiv.js", "/_layouts/15/Scripts/html5shiv/3.7.2/html5shiv.min.js");
    RegisterSod("respond.js", "/_layouts/15/Scripts/respond/1.4.2/respond.min.js");
    RegisterSodDep("bootstrap.js", "html5shiv.js");
    RegisterSodDep("bootstrap.js", "respond.js");
}

// React
RegisterSod("react.js", "/_layouts/15/Scripts/react/0.14.8/react.min.js");
// es5-shim IE8 support of React components
if (!document.addEventListener) {
    RegisterSod("es5-shim.js", "/_layouts/15/Scripts/es5-shim/4.5.9/es5-shim.min.js");
    RegisterSod("es5-sham.js", "/_layouts/15/Scripts/es5-shim/4.5.9/es5-sham.min.js");
    RegisterSodDep("react.js", "es5-shim.js");
    RegisterSodDep("react.js", "es5-sham.js");
}
RegisterSod("react-dom.js", "/_layouts/15/Scripts/react/0.14.8/react-dom.min.js");
RegisterSodDep("react-dom.js", "react.js");