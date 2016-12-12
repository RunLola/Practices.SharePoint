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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IssueTasks.aspx.cs"
    Inherits="Practices.SharePoint.ApplicationPages.IssueTasksPage" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    My Application Page
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <style>
        .ms-listviewtable {
            border-spacing: 0;
        }
    </style>
    <SharePoint:ScriptLink Language="javascript" Name="SP.Ribbon.js" OnDemand="true" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Language="javascript" Name="Scripts/jquery/1.12.4/jquery.min.js" runat="server" LoadAfterUI="true" />
    <script>
        LoadSodByKey("jquery.js", function () {
            
        });

        SP.SOD.executeOrDelayUntilScriptLoaded(function () {
            SP.SOD.executeOrDelayUntilScriptLoaded(function () {
                SP.SOD.executeOrDelayUntilScriptLoaded(function () {
                    SP.SOD.registerSod("IssueTasks.Ribbon.Actions", "/_layouts/15/Scripts/IssueTasks.Ribbon.Actions.js");
                    SP.SOD.execute('IssueTasks.Ribbon.Actions', 'Practices.IssueTasks.ActionsPageComponent.load');
                }, "sp.ribbon.js");
            }, "cui.js");
        }, "sp.js");

        $(function () {
            $(".ms-itmhover").click(function () {
                $(this).toggleClass("s4-itm-selected")
            });
            $(".ms-vh-selectAllIcon").click(function () {
                if ($(".ms-itmhover").length > $(".s4-itm-selected").length) {
                    $(".ms-itmhover").addClass("s4-itm-selected");
                } else {
                    $(".ms-itmhover").removeClass("s4-itm-selected");
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <SharePoint:SPGridView ID="DataGrid" runat="server" AutoGenerateColumns="false" CssClass="ms-listviewtable"
        Width="100%" BorderWidth="0" BorderStyle="None" CellPadding="1" CellSpacing="-1" GridLines="None">
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
                            </span>
                        </span>
                    </div>
                </ItemTemplate>
                <ItemStyle CssClass="ms-cellStyleNonEditable ms-vb-itmcbx ms-vb-imgFirstCell" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>                    
                    <div class="ms-vh-div">
                        Title
                    </div>
                </HeaderTemplate>
                <HeaderStyle CssClass="ms-vh2" />                
                <ItemTemplate>
                    <div class="ms-vb ms-vb-menuPadding itx" id="1">
                        <asp:HyperLink id="LinkTitle" runat="server" 
                            Text='<%# Bind("Title") %>' NavigateUrl='<%# Bind("NavigateUrl") %>' CssClass="ms-listlink" ></asp:HyperLink>
                    </div>
                </ItemTemplate>
                <ItemStyle CssClass="ms-cellstyle ms-vb-title ms-positionRelative" />
            </asp:TemplateField>
            <%--<SharePoint:SPBoundField DataField="AppVersion" HeaderText="Version"
                HeaderStyle-CssClass="ms-vh2" ItemStyle-CssClass="ms-vb2 ms-cellstyle ms-vb-lastCell" />--%>
        </Columns>
    </SharePoint:SPGridView>
</asp:Content>


