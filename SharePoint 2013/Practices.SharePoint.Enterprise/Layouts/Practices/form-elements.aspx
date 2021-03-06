﻿<%@ Import Namespace="Microsoft.SharePoint" %>
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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="form-elements.aspx.cs" 
    Inherits="Practices.SharePoint.ApplicationPages.StartTrackingPage" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    下达隐患
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:CssRegistration Name="/_layouts/15/Scripts/plugins/daterangepicker.min.css" runat="server" />
<SharePoint:ScriptLink Language="javascript" Name="Scripts/plugins/moment.min.js" runat="server" Localizable="false" />
<SharePoint:ScriptLink Language="javascript" Name="Scripts/plugins/daterangepicker.min.js" runat="server" Localizable="false" />
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
                <asp:Button id="BtnSave" runat="server" CssClass="ms-ButtonHeightWidth"
                     Text="Save" UseSubmitBehavior="false" OnClientClick="StartTracking();return false;"/>
	        </template_buttons>
        </wssuc:ButtonSection>
    </table>    
</asp:Content>