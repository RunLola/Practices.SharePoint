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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StartBlaming.aspx.cs"
    Inherits="Practices.SharePoint.ApplicationPages.StartBlamingPage" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    责任追究
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:ScriptLink Language="javascript" Name="datepicker.js" runat="server" LoadAfterUI="true" Localizable="false" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0" id="diidProjectPageOverview">
        <wssuc:InputFormSection runat="server"
            Title="追责人员"
            Description="" >
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
                        <SharePoint:ClientPeoplePicker ID="AssignedTo" runat="server" 
                            PrincipalAccountType="SPGroup" AllowMultipleEntities="false" Required="true" ValidationEnabled="true" />
                        <wssawc:InputFormRequiredFieldValidator ControlToValidate="AssignedTo" Runat="server"
                            Display="Dynamic" SetFocusOnError="true"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection runat="server"
            Title="原因分析"
            Description="分析造成隐患、不安全行为的根源" >
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="Reasons" Runat="server" 
                               class="ms-input" TextMode="MultiLine" Columns="66" Rows="5"/>
                        <wssawc:InputFormRequiredFieldValidator ControlToValidate="InputFormTextBox1" Runat="server"
                            Display="Dynamic" SetFocusOnError="true"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection runat="server"
            Title="履责情况"
            Description="相关责任人员履行职责情况" >
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="Situations" Runat="server" 
                               class="ms-input" TextMode="MultiLine" Columns="66" Rows="5"/>
                        <wssawc:InputFormRequiredFieldValidator ControlToValidate="InputFormTextBox2" Runat="server"
                            Display="Dynamic" SetFocusOnError="true"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection runat="server"
            Title="追责时间"
            Description="" >
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
                        <SharePoint:DateTimeControl ID="BlameDate" runat="server" DateOnly="true" /> 
                        <wssawc:InputFormRequiredFieldValidator ControlToValidate="txtDate" Runat="server"
                            Display="Dynamic" SetFocusOnError="true"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection runat="server"
            Title="追责地点"
            Description="" >
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="BlameLocation" Runat="server" 
                               class="ms-input" Columns="65" MaxLength="255" />  
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection runat="server"
            Title="追责状态"
            Description="相关责任人员履行职责情况" >
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
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
