/// <reference path="_references.js">

var SuiteLink = React.createClass({
    getInitialState: function () {
        return {
            badge: "加载中..."
        }
    },
    componentDidMount: function () {
        this.loadBadge;
        setInterval(this.loadBadge, this.props.pollInterval);
    },
    render: function () {
        return (<li className="ms-core-suiteLink">
                    <a className="ms-core-suiteLink-a" href="#" onClick={this.handleShowDialog }>
                        <i className={this.props.icon }></i>
                        <span>{this.props.title}</span>
                        <span className="badge">{this.state.badge}</span>
                    </a>
                </li>);
    },
    loadBadge: function () {
        var url = this.props.serviceUrl + this.props.loginName + "&callback=?";
        $.getJSON(url, function (d) {
            this.setState({ badge: d.count });
        });
    },
    handleShowDialog: function () {
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

ExecuteOrDelayUntilScriptLoaded(function () {
    var loginName = _.last(_spPageContextInfo.systemUserKey.split("|"));
    if (loginName.length == 8 && loginName.indexOf("T") == 0) {
        loginName = loginName.toUpperCase();
    }
    var links = [];
    links.push((<SuiteLink title="待办"
                       icon="fa fa-tasks btn-warning"
                       loginName={loginName}
                       serviceUrl=""
                       pollInterval="13"
                       pageUrl="" />));
    links.push((<SuiteLink title="待办"
                       icon="fa fa-tasks btn-warning"
                       loginName={loginName}
                       serviceUrl=""
                       pollInterval="13"
                       pageUrl="" />));
    React.render(links, $(".ms-core-suiteLinkList").get(0));
}, "core.js");