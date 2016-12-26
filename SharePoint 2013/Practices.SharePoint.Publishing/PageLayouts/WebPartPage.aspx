<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI"
    Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls"
    Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation"
    Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#"
    Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage, Microsoft.SharePoint.Publishing,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c"
    meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document" %>

<asp:content contentplaceholderid="PlaceHolderAdditionalPageHead" runat="server"> 
    <SharePoint:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/pagelayouts15.css %>" runat="server"/>
	<PublishingWebControls:EditModePanel runat="server">
		<!-- Styles for edit mode only-->
		<SharePoint:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/editmode15.css %>"
			After="<% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/pagelayouts15.css %>" runat="server"/>
	</PublishingWebControls:EditModePanel>
    <SharePoint:ScriptBlock runat="server">
	    var navBarHelpOverrideKey = "WSSEndUser";
	</SharePoint:ScriptBlock>
    <SharePoint:StyleBlock runat="server">
        body #s4-leftpanel {
	        display:none;
        }
        .s4-ca {
	        margin-left:0px;
        }
    </SharePoint:StyleBlock>
</asp:content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
	<ul class="nav nav-tabs" role="tablist">
      <li role="presentation" class="active">
          <a href="#can" role="tab" data-toggle="tab">可追责隐患</a>
      </li>      
      <li role="presentation">
          <a href="#has" role="tab" data-toggle="tab">以追责</a>
      </li>
      <li role="presentation">
          <a href="#by" role="tab" data-toggle="tab">Profile</a>
      </li>
    </ul>
    <!-- Tab panes -->
    <div class="tab-content">
      <div role="tabpanel" class="tab-pane active" id="can">

      </div>
      <div role="tabpanel" class="tab-pane" id="by">

      </div>
      <div role="tabpanel" class="tab-pane" id="has">

      </div>
    </div>
    <SharePoint:ScriptBlock runat="server">
        if(typeof(MSOLayout_MakeInvisibleIfEmpty) == "function") {
            MSOLayout_MakeInvisibleIfEmpty();
        }
    </SharePoint:ScriptBlock>
</asp:Content>
