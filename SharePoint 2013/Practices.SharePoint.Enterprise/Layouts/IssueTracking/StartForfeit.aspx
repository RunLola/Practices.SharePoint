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
    Inherits="Practices.SharePoint.Layouts.IssueTracking.StartForfeitPage" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    进行罚款
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0" id="diidProjectPageOverview">
        <wssuc:InputFormSection id="InputFormSection1"
            Title="罚款金额"
            Description=""
            runat="server">
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
        <wssuc:InputFormSection id="InputFormSection2"
            Title="原因分析"
            Description="分析造成隐患、不安全行为的根源"
            runat="server">
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="InputFormTextBox1" Runat="server" 
                               class="ms-input" TextMode="MultiLine" Columns="66" Rows="5"/>
                        <wssawc:InputFormRequiredFieldValidator ControlToValidate="InputFormTextBox1" Runat="server"
                            Display="Dynamic" SetFocusOnError="true"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection id="InputFormSection3"
            Title="履责情况"
            Description="相关责任人员履行职责情况"
            runat="server">
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="InputFormTextBox2" Runat="server" 
                               class="ms-input" TextMode="MultiLine" Columns="66" Rows="5"/>
                        <wssawc:InputFormRequiredFieldValidator ControlToValidate="InputFormTextBox2" Runat="server"
                            Display="Dynamic" SetFocusOnError="true"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection id="InputFormSection4"
            Title="追责时间"
            Description="相关责任人员履行职责情况"
            runat="server">
            <template_inputformcontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
                        <asp:TextBox runat="server" id="txtDate" ClientIDMode="Static"></asp:TextBox>
                        <iframe id='txtDateDatePickerFrame' title='Select a date from the calendar.' style='display:none; position:absolute; width:200px; z-index:101;' src='/_layouts/15/images/blank.gif?rev=23' class="owl-date-picker "></iframe>
                        <a href='javascript:void()' style='vertical-align:top' onclick="clickDatePicker('txtNDate', '/_layouts/15/iframe.aspx?&cal=1&lcid=1033&langid=1033&tz=-08:00:00.0002046&ww=0111110&fdow=0&fwoy=0&hj=0&swn=false&minjday=109207&maxjday=2666269&date=', '', event); return false;">
                        <img id='txtDateDatePickerImage' border='0' alt='Select a date from the calendar.' src='/_layouts/15/images/calendar_25.gif?rev=23'>
                        </a>
                        <wssawc:InputFormRequiredFieldValidator ControlToValidate="txtDate" Runat="server"
                            Display="Dynamic" SetFocusOnError="true"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection id="InputFormSection5"
            Title="追责状态"
            Description="相关责任人员履行职责情况"
            runat="server">
            <Template_InputFormontrols>
			    <wssuc:InputFormControl runat="server">
				    <Template_Control>
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </Template_InputFormontrols>
        </wssuc:InputFormSection>
        <wssuc:ButtonSection runat="server">
            <Template_Buttons>
                <asp:Button id="BtnSave" runat="server" 
                    CssClass="ms-ButtonHeightWidth" Text="Save" 
                    UseSubmitBehavior="false" OnClientClick="StartTracking();return false;" OnClick="BtnSave_Click" />
	        </Template_Buttons>
        </wssuc:ButtonSection>
    </table>
</asp:Content>