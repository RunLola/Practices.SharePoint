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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StartForfeit.aspx.cs" 
    Inherits="Practices.SharePoint.ApplicationPages.StartForfeitPage" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    进行罚款
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0" id="diidProjectPageOverview">
        <wssuc:InputFormSection runat="server" 
            Title="隐患信息"
            Description="">
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server" LabelText="责任对象">
				    <Template_Control>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl runat="server" LabelText="检查部门">
				    <Template_Control>
                        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl runat="server" LabelText="检查人员">
				    <Template_Control>
                        <asp:Literal ID="Literal3" runat="server"></asp:Literal>
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl runat="server" LabelText="检查时间">
				    <Template_Control>
                        <asp:Literal ID="Literal4" runat="server"></asp:Literal>
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl runat="server" LabelText="检查地点">
				    <Template_Control>
                        <asp:Literal ID="Literal5" runat="server"></asp:Literal>
				    </Template_Control>
			    </wssuc:InputFormControl>                
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection runat="server" Title="隐患内容" >
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
                        <asp:Literal ID="Literal6" runat="server"></asp:Literal>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection runat="server" Title="标准条款" >
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
                        <asp:Literal ID="Literal7" runat="server"></asp:Literal>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection runat="server" Title="罚款金额"
            Description="分析造成隐患、不安全行为的根源" >
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server" LabelText="部门">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="Dep" Runat="server" 
                               class="ms-input" Columns="65" MaxLength="255" />  
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl runat="server" LabelText="个人">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="Pso" Runat="server" 
                               class="ms-input" Columns="65" MaxLength="255" />  
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>        
        <wssuc:ButtonSection runat="server">
            <Template_Buttons>
                <asp:Button runat="server" id="ButtonSave" class="ms-ButtonHeightWidth" 
                    UseSubmitBehavior="false" OnClick="ButtonSave_Click"                     
                    Text="<%$Resources:wss,multipages_okbutton_text%>" 
                    accesskey="<%$Resources:wss,okbutton_accesskey%>" />
	        </Template_Buttons>
        </wssuc:ButtonSection>
    </table>
</asp:Content>