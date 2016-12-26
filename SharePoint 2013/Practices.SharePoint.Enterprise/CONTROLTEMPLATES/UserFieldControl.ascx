<%@ Control Language="C#" AutoEventWireup="false" %>

<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="ApplicationPages" Namespace="Microsoft.SharePoint.ApplicationPages.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SPHttpUtility" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/15/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/15/ToolBarButton.ascx" %>

<SharePoint:RenderingTemplate id="QuoteText" runat="server">
	<Template>
        <SharePoint:ScriptLink Language="javascript" Name="Scripts/RegisterSod.min.js" runat="server" />
        <SharePoint:CssRegistration Name="jquery.ui/1.12.1/jquery.ui.min.css" runat="server" />
        <script>
            LoadSodByKey("jquery.ui.js", function () {
                var url = "<asp:Literal ID='RestUrl' runat='server' />"
                $("input[id*='TextField']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: url,
                            method: "GET",
                            headers: {
                                "accept": "application/json;odata=verbose",
                                "content-type": "application/json;odata=verbose",
                                "X-RequestDigest": $("#__REQUESTDIGEST").val()
                            },
                            data: {
                                term: request.term
                            },
                            success: function (data) {
                                var results = data.d.results;
                                response($.map(results, function (item) {
                                    return item.Title;
                                }));
                            },
                            error: function (data) {
                                console.error(data);
                            }
                        });
                    },
                    select: function (e, i) {
                        $(this).val(i);
                    },
                    minLength: 1
                });
            });
        </script>
		<asp:TextBox id="TextField" maxlength="255" runat="server"/><br />
	</Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate id="QuoteNote" runat="server">
	<Template>
		<asp:TextBox id="TextField" maxlength="255" runat="server"/><br />
	</Template>
</SharePoint:RenderingTemplate>