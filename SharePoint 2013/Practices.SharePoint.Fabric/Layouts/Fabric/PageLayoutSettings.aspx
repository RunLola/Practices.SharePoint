<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>

<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI"
    Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/15/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/15/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="~/_controltemplates/15/ButtonSection.ascx" %>


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageLayoutSettings.aspx.cs" DynamicMasterPageFile="~masterurl/default.master"
    Inherits="Practices.SharePoint.ApplicationPages.FormTemplateSettingsPage" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">

</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:CssRegistration Name="forms.css" runat="server" />
    <SharePoint:CssRegistration Name="bootstarp/3.3.7/bootstrap.min.css" runat="server" />
    <SharePoint:CssRegistration Name="bootstrap.sharepoint.min.css" runat="server" />
    <SharePoint:CssRegistration Name="jquery.ui/1.12.1/jquery.ui.min.css" runat="server" />
    <SharePoint:CssRegistration Name="FormTemplateSettings.min.css" runat="server" />

    <SharePoint:ScriptLink Language="javascript" Name="Scripts/RegisterSod.min.js" runat="server" />
    <SharePoint:ScriptLink Language="javascript" Name="Scripts/FormTemplateSettings.js" LoadAfterUI="true" runat="server" />
</asp:Content>

<asp:Content ID="LeftNavBar" ContentPlaceHolderID="PlaceHolderLeftNavBar" runat="server">
    <div id="gridRows">
        <ul>
            <li class="gridRow">Row
            </li>
        </ul>
    </div>
    <div id="gridCols">
        <ul>
            <asp:Repeater ID="GridCols" runat="server">
                <ItemTemplate>
                    <li class="gridCol" id="<%# ((SPField)Container.DataItem).InternalName %>">
                        <%# ((SPField)Container.DataItem).Title %>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <!--
    <ul class="nav nav-pills">
        <li class="active"><a href="#" data-value="0">All</a></li>
        <li><a href="#" data-value="3">New</a></li>
        <li><a href="#" data-value="2">Edit</a></li>
        <li><a href="#" data-value="1">Display</a></li>
    </ul>
    -->
    <div class="container-fluid"></div>
    <table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0" id="diidProjectPageOverview">        
        <asp:HiddenField ID="JsonString" runat="server" ClientIDMode="Static" />
        <wssuc:buttonsection runat="server">
            <template_buttons>
                <asp:Button id="BtnSave" runat="server" 
                    Text="Save" CssClass="ms-ButtonHeightWidth" UseSubmitBehavior="false" 
                    OnClientClick="CollectData();" OnClick="BtnSave_Click"/>
	        </template_buttons>
        </wssuc:buttonsection>
    </table>
</asp:Content>
