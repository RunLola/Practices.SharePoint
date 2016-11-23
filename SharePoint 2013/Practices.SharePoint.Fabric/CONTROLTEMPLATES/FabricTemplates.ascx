<%@ Control Language="C#" AutoEventWireup="false" %>
<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="ApplicationPages" Namespace="Microsoft.SharePoint.ApplicationPages.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SPHttpUtility" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Practices" Namespace="Practices.SharePoint.WebControls"
    Assembly="$SharePoint.Project.AssemblyFullName$" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/15/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/15/ToolBarButton.ascx" %>

<SharePoint:RenderingTemplate ID="FabricCompositeField" runat="server">
    <Template>
        <label class="ms-formlabel control-label col-sm-2">
            <h3 class="ms-standardheader">
                <SharePoint:FieldLabel runat="server" />
            </h3>
        </label>
        <div class="ms-formbody col-sm-10">
            <!-- 
                FieldName="<SharePoint:FieldProperty PropertyName="Title" runat="server"/>"
			    FieldInternalName="<SharePoint:FieldProperty PropertyName="InternalName" runat="server"/>"
			    FieldType="SPField<SharePoint:FieldProperty PropertyName="Type" runat="server"/>"
		    -->
            <SharePoint:FormField runat="server" CssClass="form-control" />
            <SharePoint:FieldDescription runat="server" />
            <SharePoint:AppendOnlyHistory runat="server" />
        </div>
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="DisplayFabricCompositeField" runat="server">
    <Template>
        <label class="ms-formlabel control-label col-sm-2">
            <h3 class="ms-standardheader">
                <SharePoint:FieldLabel runat="server" />
            </h3>
        </label>
        <div class="ms-formbody col-sm-10" id="SPField<SharePoint:FieldProperty PropertyName='Type' runat='server'/>">
            <!-- 
                FieldName="<SharePoint:FieldProperty PropertyName="Title" runat="server"/>"
			    FieldInternalName="<SharePoint:FieldProperty PropertyName="InternalName" runat="server"/>"
			    FieldType="SPField<SharePoint:FieldProperty PropertyName="Type" runat="server"/>"
		    -->
            <SharePoint:FormField runat="server" CssClass="form-control" />
            <SharePoint:AppendOnlyHistory runat="server" />
        </div>
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="FabricListFieldIterator" runat="server">
    <Template>
        <div id="GridCol" runat="server">
            <div class="form-group">
                <Practices:FabricCompositeField runat="server" />
            </div>
        </div>
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="FabricListForm" runat="server">
    <Template>
        <style>
            .ms-core-tableNoSpace {
                width: 100%;
            }
        </style>
        <table style="width: 100%">
            <tr>
                <td>
                    <span id='part1'>
                        <SharePoint:InformationBar runat="server" />
                        <div id="listFormToolBarTop">
                            <wssuc:ToolBar CssClass="ms-formtoolbar" id="toolBarTbltop" RightButtonSeparator="&amp;#160;" runat="server">
                                <template_rightbuttons>
									<SharePoint:NextPageButton runat="server"/>
									<SharePoint:SaveButton runat="server"/>
									<SharePoint:GoBackButton runat="server"/>
								</template_rightbuttons>
                            </wssuc:ToolBar>
                        </div>
                        <SharePoint:FormToolBar runat="server" />
                        <SharePoint:ItemValidationFailedMessage runat="server" />
                        <table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0" width="100%">
                            <SharePoint:ChangeContentType runat="server" />
                            <SharePoint:FolderFormFields runat="server" />
                            <Practices:FabricListFieldIterator runat="server" />
                            <SharePoint:ApprovalStatus runat="server" />
                            <SharePoint:FormComponent TemplateName="AttachmentRows" ComponentRequiresPostback="false" runat="server" />
                        </table>
                        <table cellpadding="0" cellspacing="0" width="100%" style="padding-top: 7px">
                            <tr>
                                <td width="100%">
                                    <SharePoint:ItemHiddenVersion runat="server" />
                                    <SharePoint:ParentInformationField runat="server" />
                                    <SharePoint:InitContentType runat="server" />
                                    <wssuc:ToolBar CssClass="ms-formtoolbar" id="toolBarTbl" RightButtonSeparator="&amp;#160;" runat="server">
                                        <template_buttons>
									<SharePoint:CreatedModifiedInfo runat="server"/>
								</template_buttons>
                                        <template_rightbuttons>
									<SharePoint:SaveButton runat="server"/>
									<SharePoint:GoBackButton runat="server"/>
								</template_rightbuttons>
                                    </wssuc:ToolBar>
                                </td>
                            </tr>
                        </table>
                    </span>
                </td>
                <td valign="top">
                    <SharePoint:DelegateControl runat="server" ControlId="RelatedItemsPlaceHolder" />
                </td>
            </tr>
        </table>
        <SharePoint:AttachmentUpload runat="server" />
    </Template>
</SharePoint:RenderingTemplate>
