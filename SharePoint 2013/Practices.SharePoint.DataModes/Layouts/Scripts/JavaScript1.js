/// <reference path="_references.js" />

function createIssueTracking(siteUrl, listId, relatedId) {

}

SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
    var siteUrl = "";
    var listId = "";
    var relatedId = 1;
    //var clientContext = new SP.ClientContext.getcurrent();
    var clientContext = new SP.ClientContext(siteUrl);
    this.list = clientContext.get_web().get_lists().getById(listId);
    var itemCreateInfo = new SP.ListItemCreationInformation();
    this.listItem = list.addItem(itemCreateInfo);
    listItem.set_item("RelatedIssues", new SP.FieldLookupValue().set_lookupId(relatedId));
    listItem.update();
    clientContext.load(listItem);
    clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));

    function onQuerySucceeded(sender, args) {
        alert('IssueTracking created!\n\nId: ' + listItem.get_id() + '\nTitle: ' + listItem.get_item('Title'));
        var listId = list.get_id();
        //var listId = listItem.get_parentList().get_id();
        var id = listItem.get_id();
        ExecuteOrDelayUntilScriptLoaded(function () {
            var url = _spPageContextInfo.siteServerRelativeUrl + _spPageContextInfo.layoutsUrl +
                "/listform.aspx?ListId=" + listId + "&PageType=6&ID=" + id + "&Source=" + encodeURIComponent(serverRequestPath);
            STSNavigate(url)
        }, "core.js");
    }

    function onQueryFailed(sender, args) {
        alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
        console.log("Error: " + args.get_message() + "\n" + args.get_stackTrace());
    }
});

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
        switch (status)
        {

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
    for (var i = 0; i < pickerEntities.length; i++)
    {
        var pickerEntity = pickerEntities[i];
        if (pickerEntity.EntityType == "User")
        {
            var loginName = pickerEntity.Key;
            //GetUserId(loginName);
        } else if (pickerEntity.EntityData["PrincipalType"] == "SharePointGroup")
        {
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
