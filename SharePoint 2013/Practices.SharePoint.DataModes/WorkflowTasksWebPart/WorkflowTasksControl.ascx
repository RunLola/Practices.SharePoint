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

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkflowTasksControl.ascx.cs"
        Inherits="Practices.SharePoint.WebParts.WorkflowTasksControl" %>
<style>
    .ms-listviewtable {
        border-spacing: 0;
    }
</style>
<SharePoint:ScriptLink Language="javascript" Name="Scripts/spGridView.js" runat="server" LoadAfterUI="true" />
<script>
    $(function () {
        $("#<%=GridView.ClientID%>").spGridView();
    });
</script>
<SharePoint:SPGridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="ms-listviewtable"
    ShowHeaderWhenEmpty="true" Width="100%" BorderWidth="0" BorderStyle="None" CellPadding="1" CellSpacing="-1" GridLines="None">
    <HeaderStyle CssClass="ms-viewheadertr ms-vhltr" />
    <RowStyle CssClass="ms-itmHoverEnabled ms-itmhover" />
    <AlternatingRowStyle CssClass="ms-itmHoverEnabled ms-itmhover ms-alternating" />
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <span id="cbxSelectAllItems" class="ms-selectall-span" tabindex="0" title="Select or deselect all items"">
                    <span tabindex="-1" class="ms-selectall-iconouter">
                        <img class="ms-selectall-icon" alt="" src="/_layouts/15/images/spcommon.png?rev=23">
                    </span>
                </span>
            </HeaderTemplate>
            <HeaderStyle CssClass="ms-headerCellStyleIcon ms-vh-icon ms-vh-selectAllIcon" />
            <ItemTemplate>
                <div class="s4-itm-cbx s4-itm-imgCbx" tabindex="-1" title=""
                    role="checkbox" aria-checked="false">
                    <span class="s4-itm-imgCbx-inner">
                        <span class="ms-selectitem-span">
                            <img class="ms-selectitem-icon" alt="" src="/_layouts/15/images/spcommon.png?rev=23">
                            <asp:HiddenField ID="Identity" runat="server" Value='<%# Eval("Identity") %>'></asp:HiddenField>
                        </span>
                    </span>
                </div>
            </ItemTemplate>
            <ItemStyle CssClass="ms-cellStyleNonEditable ms-vb-itmcbx ms-vb-imgFirstCell" />
        </asp:TemplateField>
        <asp:TemplateField>
            <%--<HeaderTemplate>                    
                <div class="ms-vh-div">
                    Title
                </div>
            </HeaderTemplate>--%>
            <HeaderStyle CssClass="ms-vh2" />
            <ItemTemplate>
                <div class="ms-vb ms-vb-menuPadding itx">
                    <asp:HyperLink ID="LinkTitle" runat="server"
                        Text='<%# Eval("Title") %>' NavigateUrl='<%# Eval("NavigateUrl") %>' CssClass="ms-listlink"></asp:HyperLink>
                </div>
            </ItemTemplate>
            <ItemStyle CssClass="ms-cellstyle ms-vb-title ms-positionRelative" />
        </asp:TemplateField>
        <%--<SharePoint:SPBoundField DataField="AppVersion" HeaderText="Version"
            HeaderStyle-CssClass="ms-vh2" ItemStyle-CssClass="ms-vb2 ms-cellstyle ms-vb-lastCell" />--%>
    </Columns>
</SharePoint:SPGridView>
<SharePoint:SPGridViewPager ID="GridViewPager" runat="server" GridViewId="GridView">
</SharePoint:SPGridViewPager>