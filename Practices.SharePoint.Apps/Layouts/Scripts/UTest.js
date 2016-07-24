/// <reference path="_references.js" />

var productId = "3B7D7274-3652-4FA9-A9C1-32839880E052";

function aa() {
    
}

$.ajax({
    type: "POST",
    url: "/Sites/Portal/_vti_bin/Apps.svc/",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    data: JSON.stringify({
        title: "版本测试 - 01",
        launchUrl: "http://practices.contoso.com/sites/Apps/"
    }),
    success: function (data) {
        alert(data["CreateResult"]);
    }
});

$.ajax({
    type: "POST",
    url: "/Sites/Portal/_vti_bin/Apps.svc/" + productId,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    data: JSON.stringify({        
        title: "勘探与生产ERP WEB系统",
        launchUrl: "http://ep.erp.petrochina:8160/zuep/SSORedirect?sap-client=806&redirectUrl=%2fsap%2fbc%2fgui%2fsap%2fits%2fwebgui%3fsap-client%3d806%26sap-language%3dZH%26%7eTransaction%3dSMEN"
    }),
    success: function (msg) {
        alert(msg["UpgradeResult"]);
    }
});

$.ajax({
    type: "PUT",
    url: "/Sites/Portal/_vti_bin/Apps.svc/" + productId,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    data: JSON.stringify({
        fields: [{ Key: "AppShortDescription", Value: "This is the app's description." },
                 { Key: "AppDescription", Value: "This is the app's short description." }]
    }),
    success: function (msg) {
        alert(msg["UpdateResult"]);
    }
});

$.ajax({
    type: "DELETE",
    url: "/Sites/Portal/_vti_bin/Apps.svc/" + productId,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
        alert(msg["DeleteResult"]);
    }
});

$.ajax({
    type: "POST",
    url: "/Sites/Portal/_vti_bin/Apps.svc/Push/" + productId,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
        alert(msg["PushResult"]);
    }
});

$.ajax({
    type: "POST",
    url: "/Sites/Portal/_vti_bin/Apps.svc/Pull/" + productId,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
        alert(msg["PullResult"]);
    }
});

var productId = "00000000-0000-0000-0000-000000000000";
var productId = "F150DF3C-450F-4036-9CF9-EEB6FB7E9E9E";

$.ajax({
    type: "POST",
    url: "/Sites/Portal/_vti_bin/Apps.svc/Generate/" + productId,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    data: JSON.stringify({
        title: "勘探与生产ERP WEB系统",
        launchUrl: "http://ep.erp.petrochina:8160/zuep/SSORedirect?sap-client=806&redirectUrl=%2fsap%2fbc%2fgui%2fsap%2fits%2fwebgui%3fsap-client%3d806%26sap-language%3dZH%26%7eTransaction%3dSMEN",
        fields: [{ Key: "AppShortDescription", Value: "This is the app's description." },
                 { Key: "AppDescription", Value: "This is the app's short description." }]
    }),
    success: function (msg) {
        alert(msg["GenerateResult"]);
    }
});

$.ajax({
    type: "POST",
    url: "/Sites/Portal/_vti_bin/Apps.svc/Destroy/" + productId,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
        alert(msg["DestroyResult"]);
    }
});




