/// <reference path="_references.js" />

// Underscore
//RegisterSod("underscore.js", "~/_layouts/15/Scripts/underscore/1.8.3/underscore.min.js");
document.write("<script src='~/_layouts/15/Scripts/underscore/1.8.3/underscore.min.js'><\/script>");

// jQuery
if (!document.addEventListener) {
    //RegisterSod("jquery.js", "~/_layouts/15/Scripts/jquery/1.12.4/jquery.min.js");
    document.write("<script src='~/_layouts/15/Scripts/jquery/1.12.4/jquery.min.js'><\/script>");
} else {
    //RegisterSod("jquery.js", "~/_layouts/15/Scripts/jquery/3.1.0/jquery.min.js");
    document.write("<script src='~/_layouts/15/Scripts/jquery/3.1.0/jquery.min.js'><\/script>");
}

// HTML5 Shiv and Respond.js IE8 support of HTML5 elements and media queries
if (!document.addEventListener) {
    //RegisterSod("html5shiv.js", "~/_layouts/15/Scripts/html5shiv/3.7.2/html5shiv.min.js");
    //RegisterSod("respond.js", "~/_layouts/15/Scripts/respond/1.4.2/respond.min.js");
    document.write("<script src='~/_layouts/15/Scripts/html5shiv/3.7.2/html5shiv.min.js'><\/script>");
    document.write("<script src='~/_layouts/15/Scripts/respond/1.4.2/respond.min.js'><\/script>");
}

// Bootstrap
//RegisterSod("bootstrap.js", "~/_layouts/15/Scripts/bootstrap.min.js");
document.write("<script src='~/_layouts/15/Scripts/bootstrap.min.js'><\/script>");

// es5-shim
if (!document.addEventListener) {
    //RegisterSod("es5-shim.js", "~/_layouts/15/Scripts/es5-shim/4.5.9/es5-shim.min.js");
    //RegisterSod("es5-sham.js", "~/_layouts/15/Scripts/es5-shim/4.5.9/es5-sham.min.js");
    document.write("<script src='~/_layouts/15/Scripts/es5-shim/4.5.9/es5-shim.min.js'><\/script>");
    document.write("<script src='~/_layouts/15/Scripts/es5-shim/4.5.9/es5-sham.min.js'><\/script>");
}

// React
//RegisterSod("react.js", "~/_layouts/15/Scripts/react/0.14.8/react.min.js");
//RegisterSod("react-dom.js", "~/_layouts/15/Scripts/react/0.14.8/react-dom.min.js");
document.write("<script src='~/_layouts/15/Scripts/react/0.14.8/react.min.js'><\/script>");
document.write("<script src='~/_layouts/15/Scripts/react/0.14.8/react-dom.min.js'><\/script>");