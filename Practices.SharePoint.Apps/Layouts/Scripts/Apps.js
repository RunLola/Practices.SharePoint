/// <reference path="_references.js" />

$(function () {
    $iconPicker = $("");
    $iconPicker.iconpicker({
        title: "请选择应用图标",
        placement: "bottomRight",
    });


    $roleSelect = $("#<%=AppRoles.ClientID%>");
    $roleSelect.chosen({
        allow_single_deselect: true,
        search_contains: true,
        placeholder_text_multiple: "请选择需授权角色",
        no_results_text: "未找到匹配角色"
    });

    $additionalSection = $("#<%=additionalSection.ClientID%>");
    $showSection = $("#<%=showSection.ClientID%>");

    $createButton = $("#<%=BtnCreate.ClientID%>");
    $saveBustton = $("#<%=BtnSave.ClientID%>");
    $nextButton = $("#<%=BtnNext.ClientID%>");

    $additionalSection.hide();
    $showSection.find("a").click(function () {
        $showSection.hide();
        $additionalSection.show();
    });
});

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    else {
        return results[1] || 0;
    }
}
var app = {
    title: "",
    launchUrl: ""
};

$.ajax({
    type: "POST",
    url: "/Sites/Portal/_vti_bin/Apps.svc/",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    data: JSON.stringify({
        title: "版本测试 - 01",
        launchUrl: "http://practices.contoso.com/sites/Apps/"
    })
}).success(function () {
    
});

(function ($) {
    $.QueryString = (function (a) {
        if (a == "") return {};
        var b = {};
        for (var i = 0; i < a.length; ++i) {
            var p = a[i].split('=');
            if (p.length != 2) continue;
            b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
        }
        return b;
    })(window.location.search.substr(1).split('&'))
})(jQuery);

SP.SOD.executeFunc("sp.js", "SP.ClientContext", getQuickLaunch);

var SideNavBar = React.createClass({
    displayName: "SideNavBar",

    getInitialState: function getInitialState() {
        return { data: [] };
    },
    render: function render() {
        var navNodes = [];
        var nodes = this.props.data.getEnumerator();
        while (nodes.moveNext()) {
            var node = nodes.get_current();
            var url = node.get_url();
            var title = node.get_title();
            var children = node.get_children();
            navNodes.push(React.createElement(NavNode, { title: title, url: url, children: children }));
        }
        return React.createElement(
            "div",
            null,
            React.createElement(
                "ul",
                null,
                navNodes
            )
        );
    }
});

var NavNode = React.createClass({
    displayName: "NavNode",

    componentDidMount: function componentDidMount() {
        var clientContext = new SP.ClientContext.get_current();
        clientContext.load(this.props.children);
        clientContext.executeQueryAsync(Function.createDelegate(this, this.onQueryChildrenSucceeded), Function.createDelegate(this, this.onQueryFailed));
        function onQueryChildrenSucceeded() {
            this.setState({ data: data });
        }
    },
    render: function render() {
        var navNodes = [];
        var nodes = this.props.data.getEnumerator();
        while (nodes.moveNext()) {
            var node = nodes.get_current();
            var url = node.get_url();
            var title = node.get_title();
            var children = node.get_children();
            navNodes.push(React.createElement(NavNode, { title: title, url: url, children: children }));
        }
        return React.createElement(
            "li",
            null,
            React.createElement(
                "a",
                { className: "dropdown-toggle", href: this.props.url },
                React.createElement("i", { className: "menu-icon" }),
                React.createElement(
                    "span",
                    { className: "menu-text" },
                    this.props.title
                ),
                React.createElement("b", { className: "arrow" })
            )
        );
    }
});

SP.SOD.executeFunc("sp.js", "SP.ClientContext", getQuickLaunch);

function getQuickLaunch() {
    var clientContext = new SP.ClientContext.get_current();
    this.navNodes = clientContext.get_web().get_navigation().get_quickLaunch();
    clientContext.load(this.navNodes);
    clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));
}

function onQuerySucceeded() {
    ReactDOM.render(React.createElement(SideNavBar, { data: this.navNodes }), document.getElementById("content"));
}

function onQueryFailed(sender, args) {
    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
}
