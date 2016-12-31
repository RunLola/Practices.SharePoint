/// <reference path="_references.js" />
EnsureScriptFunc("jquery.js", "$", function () {
    $(function () {
        $(".input-daterange").daterangepicker({
            "locale": {
                "direction": "ltr",
                "format": "YYYY/MM/DD",
                "separator": " - ",
                "applyLabel": "确定",
                "cancelLabel": "取消",
                "fromLabel": "从",
                "toLabel": "到",
                "daysOfWeek": [
                    "周日",
                    "周一",
                    "周二",
                    "周三",
                    "周四",
                    "周五",
                    "周六"
                ],
                "monthNames": [
                    "一月",
                    "二月",
                    "三月",
                    "四月",
                    "五月",
                    "六月",
                    "七月",
                    "八月",
                    "九月",
                    "十月",
                    "十一月",
                    "十二月"
                ],
                "firstDay": 1
            },
            "alwaysShowCalendars": true,
        })
    });
});


function createIssueTracking(siteUrl, listId, relatedId) {
    
}

function isIssueCheckingEnable() {

}

SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
    var siteUrl = "";
    var listId = "";
    var itemId = 1;
    var clientContext = new SP.ClientContext(siteUrl);
    var list = clientContext.get_web().get_lists().getById(listId);
    var item = list.getItemById(itemId);
    clientContext.load(item);
    clientContext.executeQueryAsync(Function.createDelegate(this, onQuerySucceeded), Function.createDelegate(this, onQueryFailed));

    function onQuerySucceeded(sender, args) {
        var status = item.get_item("Status");
        switch (status) {

        }
    }

    function onQueryFailed(sender, args) {
        alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
        console.log("Error: " + args.get_message() + "\n" + args.get_stackTrace());
    }
})

var dlg = null;
function showInProgressDialog() {
    if (dlg == null) {
        dlg = SP.UI.ModalDialog.showWaitScreenWithNoClose("Please wait...", "Waiting for workflow...", null, null);
    }
}

function closeInProgressDialog() {
    if (dlg != null) {
        dlg.close();
    }
}


function GetUserFieldValues(clientId) {
    var fieldValues = [];
    var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[clientId];
    var pickerEntities = peoplePicker.GetAllUserInfo();
    for (var i = 0; i < pickerEntities.length; i++) {
        var pickerEntity = pickerEntities[i];
        if (pickerEntity.EntityType == "User") {
            var loginName = pickerEntity.Key;
            //GetUserId(loginName);
        } else if (pickerEntity.EntityData["PrincipalType"] == "SharePointGroup") {
            var groupId = pickerEntity.EntityData["SPGroupID"];
            var groupName = pickerEntity.EntityData["AccountName"];
            var fieldValue = new SP.FieldUserValue();
            fieldValue.lookupId = groupId;
            fieldValues.push(fieldValue);
        }
    }
    return fieldValues;
}

function GetUserId(loginName) {
    var clientContext = new SP.ClientContext.get_current();
    this.user = clientContext.get_web().ensureUser(loginName);
    clientContext.load(user);
    clientContext.executeQueryAsync(
        Function.createDelegate(this, onQuerySucceeded),
        Function.createDelegate(this, onQueryFailed)
    );

    function onQuerySucceeded(sender, args) {
        var fieldValue = new SP.FieldUserValue();
        fieldValue.lookupId = user.get_id();
    }

    function onQueryFailed(sender, args) {
        alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
        console.log("Error: " + args.get_message() + "\n" + args.get_stackTrace());
    }
}
