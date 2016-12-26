<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>

<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI"
    Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/15/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/15/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="~/_controltemplates/15/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StartTracking.aspx.cs" 
    Inherits="Practices.SharePoint.ApplicationPages.StartTrackingPage" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    隐患下达
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:ScriptLink Language="javascript" Name="Scripts/spWorkflow.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <script>
        var QueryString = function () {
            // This function is anonymous, is executed immediately and 
            // the return value is assigned to QueryString!
            var query_string = {};
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                // If first entry with this name
                if (typeof query_string[pair[0]] === "undefined") {
                    query_string[pair[0]] = decodeURIComponent(pair[1]);
                    // If second entry with this name
                } else if (typeof query_string[pair[0]] === "string") {
                    var arr = [query_string[pair[0]], decodeURIComponent(pair[1])];
                    query_string[pair[0]] = arr;
                    // If third or later entry with this name
                } else {
                    query_string[pair[0]].push(decodeURIComponent(pair[1]));
                }
            }
            return query_string;
        }();

        function StartTracking() {
            var clientId = "<%= AssignedTo.ClientID %>_TopSpan";
            var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[clientId];
            var pickerEntities = peoplePicker.GetAllUserInfo();
            if (pickerEntities.length == 1) {
                var pickerEntity = pickerEntities[0];
                if (pickerEntity.EntityData["PrincipalType"] == "SharePointGroup") {
                    var groupId = pickerEntity.EntityData["SPGroupID"];
                    var groupName = pickerEntity.EntityData["AccountName"];
                    var listId = QueryString.ListId;
                    var itemId = QueryString.ItemId;
                    var payload = new Object();
                    payload["FieldName"] = groupName;
                    payload["FieldName1"] = document.getElementById('<%=TaskType.ClientID%>').value;                    
                    StartWorkflow(listId, itemId, "My Tasks", payload);
                }
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0" id="diidProjectPageOverview">
        <wssuc:InputFormSection id="titleSection"
            Title="隐患整改"
            Description="选择责任部门，并指定整改或下达。"
            runat="server">
            <template_inputformcontrols>
			    <wssuc:InputFormControl LabelText="责任部门" runat="server">
				    <Template_Control>
                        <SharePoint:ClientPeoplePicker ID="AssignedTo" runat="server" 
                            PrincipalAccountType="SPGroup" AllowMultipleEntities="false" Required="true" ValidationEnabled="true" />
                        <wssawc:InputFormRequiredFieldValidator ControlToValidate="AssignedTo" Runat="server"
                            Display="Dynamic" SetFocusOnError="true"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl LabelText="任务类别" runat="server">
                    <Template_Control>
                        <asp:DropDownList ID="TaskType" runat="server" Width="490">
                            <asp:ListItem Value="整改任务">整改任务</asp:ListItem>
                            <asp:ListItem Value="下达任务">下达任务</asp:ListItem>
                        </asp:DropDownList>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:ButtonSection runat="server">
            <template_buttons>
                <asp:Button runat="server" id="ButtonSave" class="ms-ButtonHeightWidth" 
                    UseSubmitBehavior="false" OnClientClick="StartTracking();return false;"               
                    Text="<%$Resources:wss,multipages_okbutton_text%>" 
                    accesskey="<%$Resources:wss,okbutton_accesskey%>" />
	        </template_buttons>
        </wssuc:ButtonSection>
    </table>    
</asp:Content>