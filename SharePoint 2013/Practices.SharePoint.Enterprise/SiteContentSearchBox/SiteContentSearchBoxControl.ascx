<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI"
    Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteContentSearchBoxControl.ascx.cs" 
    Inherits="Practices.SharePoint.WebParts.SiteContentSearchBoxControl" %>

<asp:CheckBox ID="CheckBox0" runat="server" Text="需提交隐患"  />
<asp:CheckBox ID="CheckBox1" runat="server" Text="可追责隐患"  />
<asp:CheckBox ID="CheckBox2" runat="server" Text="可罚款隐患"  />
<asp:CheckBox ID="CheckBox3" runat="server" Text="可抽查隐患"  />