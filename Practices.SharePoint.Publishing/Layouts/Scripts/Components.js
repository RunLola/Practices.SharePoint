/// <reference path="_references.js">

"use strict";

var SuiteLink = React.createClass({
    displayName: "SuiteLink",

    getInitialState: function getInitialState() {
        return {
            badge: "加载中..."
        };
    },
    componentDidMount: function componentDidMount() {
        this.loadBadge;
        setInterval(this.loadBadge, this.props.pollInterval);
    },
    render: function render() {
        return React.createElement(
            "li",
            { className: "ms-core-suiteLink" },
            React.createElement(
                "a",
                { className: "ms-core-suiteLink-a", href: "#", onClick: this.handleShowDialog },
                React.createElement("i", { className: this.props.icon }),
                React.createElement(
                    "span",
                    null,
                    this.props.title
                ),
                React.createElement(
                    "span",
                    { className: "badge" },
                    this.state.badge
                )
            )
        );
    },
    loadBadge: function loadBadge() {
        var url = this.props.serviceUrl + this.props.loginName + "&callback=?";
        $.getJSON(url, function (d) {
            this.setState({ badge: d.count });
        });
    },
    handleShowDialog: function handleShowDialog() {
        //EnsureScriptFunc("SP.js", "SP.Utilities.Utility", function () {
        //    SP.SOD.execute("SP.UI.Dialog.js", "SP.UI.ModalDialog.showModalDialog", {
        //        url: this.props.pageUrl,
        //        autoSize: true,
        //        width: $(document).width() * 0.8,
        //        height: 450
        //    })
        //});
        $("#suiteDialog").modal({ show: true });
        $('#suiteDialog').on('show.bs.modal', function () {
            $('iframe', this).attr("src", this.props.pageUrl);
        });
    }
});

//ExecuteOrDelayUntilScriptLoaded(function () {
//    var loginName = _.last(_spPageContextInfo.systemUserKey.split("|"));
//    if (loginName.length == 8 && loginName.indexOf("T") == 0) {
//        loginName = loginName.toUpperCase();
//    }
//    var suiteLinks = [];
//    React.render(<SuiteLink title="待办"
//                            icon="fa fa-tasks btn-warning"
//                            loginName = {loginName}
//                            serviceUrl =""
//                            pollInterval="13"
//                            pageUrl="" />,
//                $(".ms-core-suiteLinkList").get(0));
//    React.render(<SuiteLink title="提醒"
//                            icon="fa fa-bell btn-danger"
//                            loginName = {loginName}
//                            serviceUrl =""
//                            pollInterval="13"
//                            pageUrl="" />,
//                $(".ms-core-suiteLinkList").get(0));
//}, "core.js");

