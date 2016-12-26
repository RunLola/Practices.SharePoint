<%@ Control Language="C#" AutoEventWireup="false" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="ApplicationPages" Namespace="Microsoft.SharePoint.ApplicationPages.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SPHttpUtility" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Practices" Namespace="Practices.SharePoint.WebControls"
    Assembly="$SharePoint.Project.AssemblyFullName$" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/15/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/15/ToolBarButton.ascx" %>

<SharePoint:RenderingTemplate id="MemberField" runat="server">
	<Template>
        <input type="hidden" runat="server" id="HiddenUserFieldValue"/>
		<SharePoint:PeopleEditor id="UserField" runat="server" ValidatorEnabled="true" />
        <table class="ms-input" style="border-collapse: collapse;" cellspacing="0" cellpadding="0">
            <tr valign="bottom">
                <td valign="top" style="width:88%">
                    <SharePoint:ClientPeoplePicker ID="peoplePicker" runat="server" ValidationEnabled="true" />
                </td>
                <td align="center" nowrap="true" valign="top" style="padding-left: 5px; float: right;">

                </td>
            </tr>
        </table>   
	</Template>
</SharePoint:RenderingTemplate>