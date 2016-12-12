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

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberFieldEditor.ascx.cs"
    Inherits="Practices.SharePoint.WebControls.MemberFieldEditor" %>

<wssuc:InputFormSection runat="server"
    Title="lookup"
    Id="FirstDivision">
    <template_description>
		<asp:Literal ID="multipleValueDescription" Text="lookup" runat="server"/>
	</template_description>
    <template_inputformcontrols>
		<Template_Control>
            <wssuc:InputFormControl LabelText="<%$Resources:wss,fldedit_getinfofrom%>" runat="server">
			    <Template_Control>
                </Template_Control>
            </wssuc:InputFormControl>
        </Template_Control>
    </template_inputformcontrols>
</wssuc:InputFormSection>