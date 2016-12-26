<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI"
    Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/15/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/15/InputFormControl.ascx" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuoteFieldEditor.ascx.cs"
    Inherits="Practices.SharePoint.WebControls.QuoteFieldEditor" %>

<SharePoint:ScriptLink Language="javascript" Name="PickerTreeDialog.js" OnDemand="true" runat="server" />
<script>
    function openListPickerDialog() {
        SP.SOD.executeOrDelayUntilScriptLoaded(function () {
            SP.SOD.executeFunc("PickerTreeDialog.js", "LaunchPickerTreeDialog", function () {
                LaunchPickerTreeDialog(
                    "PickerListTitle",
                    "PickerListTitle",
                    "listsOnly",
                    "", _spPageContextInfo.siteAbsoluteUrl, "", "", "", "/_layouts/images/smt_icon.gif", 0, callback, '', '');
            });
        }, "core.js");
    }
    function callback(dest) {
        document.getElementById('<%=WebListIds.ClientID%>').value = dest[0];
        document.getElementById('<%=ListUrl.ClientID%>').value = dest[3];
    }
</script>
<wssuc:InputFormSection Id="FirstDivision" runat="server"
    Title="<%$Resources:wss,fldedit_getinfofrom%>">
    <template_inputformcontrols>
		<Template_Control>
            <wssuc:InputFormControl LabelText="<%$Resources:wss,fldedit_getinfofrom%>" runat="server">
			    <Template_Control>
                    <asp:HiddenField ID="WebListIds" runat="server" />
                    <asp:TextBox id="ListUrl" runat="server" CssClass="ms-input"></asp:TextBox>
                    <input type="button" value="Browse..." onclick="openListPickerDialog()" />
                </Template_Control>
            </wssuc:InputFormControl>
        </Template_Control>
    </template_inputformcontrols>
</wssuc:InputFormSection>