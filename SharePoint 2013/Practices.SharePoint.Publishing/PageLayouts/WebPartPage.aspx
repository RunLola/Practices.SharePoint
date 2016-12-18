<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" 
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" 
    Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" 
    Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page language="C#" 
    Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage, Microsoft.SharePoint.Publishing,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" 
    meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document" %>

<asp:content contentplaceholderid="PlaceHolderAdditionalPageHead" runat="server"> 
    <SharePointWebControls:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/pagelayouts15.css %>" runat="server"/>
	<PublishingWebControls:EditModePanel runat="server">
		<!-- Styles for edit mode only-->
		<SharePointWebControls:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/editmode15.css %>"
			After="<% $SPUrl:~sitecollection/Style Library/~language/Themable/Core Styles/pagelayouts15.css %>" runat="server"/>
	</PublishingWebControls:EditModePanel>
</asp:content>
<asp:content contentplaceholderid="PlaceHolderLeftNavBar" runat="server">
	
<asp:content contentplaceholderid="PlaceHolderMain" runat="server">    
    <div class="ms-table ms-fullWidth">
        <div class="tableCol-100">
		    <div class="cell-margin">
			    <WebPartPages:WebPartZone runat="server" Title="<%$Resources:cms,WebPartZoneTitle_Header%>" ID="Header"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
		    </div>
		    <div class="ms-table ms-fullWidth">
			    <div class="cell-margin tableCol-50">
				    <WebPartPages:WebPartZone runat="server" Title="<%$Resources:cms,WebPartZoneTitle_TopLeft%>" ID="TopLeftRow"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
			    </div>
			    <div class="cell-margin tableCol-50">
				    <WebPartPages:WebPartZone runat="server" Title="<%$Resources:cms,WebPartZoneTitle_TopRight%>" ID="TopRightRow"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
			    </div>
		    </div>
		    <div class="ms-table ms-fullWidth">
			    <div class="cell-margin tableCol-33">
				    <WebPartPages:WebPartZone runat="server" Title="<%$Resources:cms,WebPartZoneTitle_CenterLeft%>" ID="CenterLeftColumn"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
			    </div>
			    <div class="cell-margin tableCol-33">
				    <WebPartPages:WebPartZone runat="server" Title="<%$Resources:cms,WebPartZoneTitle_Center%>" ID="CenterColumn"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
			    </div>
			    <div class="cell-margin tableCol-33">
				    <WebPartPages:WebPartZone runat="server" Title="<%$Resources:cms,WebPartZoneTitle_CenterRight%>" ID="CenterRightColumn"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
			    </div>
		    </div>
		    <div class="cell-margin">
			    <WebPartPages:WebPartZone runat="server" Title="<%$Resources:cms,WebPartZoneTitle_Footer%>" ID="Footer"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
		    </div>
	    </div>
        <SharePointWebControls:ScriptBlock runat="server">if(typeof(MSOLayout_MakeInvisibleIfEmpty) == "function") {MSOLayout_MakeInvisibleIfEmpty();}</SharePointWebControls:ScriptBlock>
    </div>
</asp:content>
