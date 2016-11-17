<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#"
    Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage, Microsoft.SharePoint.Publishing,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c"
    meta:progid="SharePoint.WebPartPage.Document" meta:webpartpageexpansion="full" %>

<asp:content contentplaceholderid="PlaceHolderAdditionalPageHead" runat="server"> 
    
</asp:content>
<asp:content contentplaceholderid="PlaceHolderLeftNavBar" runat="server">
	
    <ul class="noindex ms-core-listMenu-root nav">
		<li class="selected">
			<a href="index.html">
				<i class=" fa fa-tachometer"></i>
				<span class="menu-text">  </span>
			</a>
			<b class="arrow"></b>
		</li>

		<li class="">
			<a class="dropdown-toggle" href="#">
				<i class=" fa fa-desktop"></i>
				<span class="menu-text"> UI &amp; Elements </span>

				<b class="arrow fa fa-angle-down"></b>
			</a>
			<b class="arrow"></b>

			<ul>
				<li class="">
					<a class="dropdown-toggle" href="#">
						Layouts
						<b class="arrow fa fa-angle-down"></b>
					</a>

					<b class="arrow"></b>

					<ul>
						<li class="">
							<a href="top-menu.html">								
								Top Menu
							</a>

							<b class="arrow"></b>
						</li>

						<li class="">
							<a href="mobile-menu-1.html">
								
								Default Mobile Menu
							</a>

							<b class="arrow"></b>
						</li>

						<li class="">
							<a href="mobile-menu-2.html">
								
								Mobile Menu 2
							</a>

							<b class="arrow"></b>
						</li>

						<li class="">
							<a href="mobile-menu-3.html">
								
								Mobile Menu 3
							</a>

							<b class="arrow"></b>
						</li>
					</ul>
				</li>

				<li class="">
					<a href="typography.html">
						
						Typography
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="elements.html">
						
						Elements
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="buttons.html">
						
						Buttons &amp; Icons
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="treeview.html">
						
						Treeview
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="jquery-ui.html">
						
						jQuery UI
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="nestable-list.html">
						
						Nestable Lists
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a class="dropdown-toggle" href="#">
						

						Three Level Menu
						<b class="arrow fa fa-angle-down"></b>
					</a>

					<b class="arrow"></b>

					<ul>
						<li class="">
							<a href="#">
								<i class=" fa fa-leaf"></i>
								Item #1
							</a>

							<b class="arrow"></b>
						</li>

						<li class="">
							<a class="dropdown-toggle" href="#">
								<i class=" fa fa-pencil"></i>

								4th level
								<b class="arrow fa fa-angle-down"></b>
							</a>

							<b class="arrow"></b>

							<ul>
								<li class="">
									<a href="#">
										<i class=" fa fa-plus"></i>
										Add Product
									</a>

									<b class="arrow"></b>
								</li>

								<li class="">
									<a href="#">
										<i class=" fa fa-eye"></i>
										View Products
									</a>

									<b class="arrow"></b>
								</li>
							</ul>
						</li>
					</ul>
				</li>
			</ul>
		</li>

		<li class="">
			<a class="dropdown-toggle" href="#">
				<i class=" fa fa-list"></i>
				<span class="menu-text"> Tables </span>

				<b class="arrow fa fa-angle-down"></b>
			</a>

			<b class="arrow"></b>

			<ul>
				<li class="">
					<a href="tables.html">
						
						Simple &amp; Dynamic
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="jqgrid.html">
						
						jqGrid plugin
					</a>

					<b class="arrow"></b>
				</li>
			</ul>
		</li>

		<li class="">
			<a class="dropdown-toggle" href="#">
				<i class=" fa fa-pencil-square-o"></i>
				<span class="menu-text"> Forms </span>

				<b class="arrow fa fa-angle-down"></b>
			</a>

			<b class="arrow"></b>

			<ul>
				<li class="">
					<a href="form-elements.html">
						
						Form Elements
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="form-wizard.html">
						
						Wizard &amp; Validation
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="wysiwyg.html">
						
						Wysiwyg &amp; Markdown
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="dropzone.html">
						
						Dropzone File Upload
					</a>

					<b class="arrow"></b>
				</li>
			</ul>
		</li>

		<li class="">
			<a href="widgets.html">
				<i class=" fa fa-list-alt"></i>
				<span class="menu-text"> Widgets </span>
			</a>

			<b class="arrow"></b>
		</li>

		<li class="">
			<a href="calendar.html">
				<i class=" fa fa-calendar"></i>

				<span class="menu-text">
					Calendar

					<!-- #section:basics/sidebar.layout.badge -->
					<span title="" class="badge badge-transparent tooltip-error" data-original-title="2 Important Events">
						<i class="ace-icon fa fa-exclamation-triangle red bigger-130"></i>
					</span>

					<!-- /section:basics/sidebar.layout.badge -->
				</span>
			</a>

			<b class="arrow"></b>
		</li>

		<li class="">
			<a href="gallery.html">
				<i class=" fa fa-picture-o"></i>
				<span class="menu-text"> Gallery </span>
			</a>

			<b class="arrow"></b>
		</li>

		<li class="">
			<a class="dropdown-toggle" href="#">
				<i class=" fa fa-tag"></i>
				<span class="menu-text"> More Pages </span>

				<b class="arrow fa fa-angle-down"></b>
			</a>

			<b class="arrow"></b>

			<ul>
				<li class="">
					<a href="#">						
						User Profile
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="#">
						
						Inbox
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="#">
						
						Pricing Tables
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="#">
						
						Invoice
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="timeline.html">
						
						Timeline
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="login.html">
						
						Login &amp; Register
					</a>

					<b class="arrow"></b>
				</li>
			</ul>
		</li>

		<li class="">
			<a class="dropdown-toggle" href="#">
				<i class=" fa fa-file-o"></i>

				<span class="menu-text">
					Other Pages

					<!-- #section:basics/sidebar.layout.badge -->
					<span class="badge badge-primary">5</span>

					<!-- /section:basics/sidebar.layout.badge -->
				</span>

				<b class="arrow fa fa-angle-down"></b>
			</a>

			<b class="arrow"></b>

			<ul>
				<li class="">
					<a href="faq.html">
						
						FAQ
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="error-404.html">
						
						Error 404
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="error-500.html">
						
						Error 500
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="grid.html">
						
						Grid
					</a>

					<b class="arrow"></b>
				</li>

				<li class="">
					<a href="blank.html">
						
						Blank Page
					</a>

					<b class="arrow"></b>
				</li>
			</ul>
		</li>
	</ul>
	<div class="sidebar-toggle sidebar-collapse" id="sidebar-collapse">
		<i class="fa fa-angle-double-left"></i>
	</div>
</asp:content>
<asp:content contentplaceholderid="PlaceHolderMain" runat="server">

</asp:content>
