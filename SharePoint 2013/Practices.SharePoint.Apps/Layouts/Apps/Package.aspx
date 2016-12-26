<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI"
    Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Taxonomy" Namespace="Microsoft.SharePoint.Taxonomy"
    Assembly="Microsoft.SharePoint.Taxonomy, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/15/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/15/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="~/_controltemplates/15/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Package.aspx.cs"
    Inherits="Practices.SharePoint.ApplicationPages.PackagePage" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    应用管理
</asp:Content>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:ScriptLink Language="javascript" Name="AssetPickers.js" runat="server" OnDemand="true" Defer="true" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Language="javascript" Name="Scripts/underscore.js" runat="server" />
    <SharePoint:ScriptLink Language="javascript" Name="Scripts/jquery.js" runat="server" LoadAfterUI="true" />

    <SharePoint:ScriptLink Language="javascript" Name="Scripts/Plugins/jquery.iconpicker.js" runat="server" LoadAfterUI="true" />
    <SharePoint:CssRegistration name="Plugins/jquery.iconpicker.css" runat="server"/> 

    <SharePoint:ScriptLink Language="javascript" Name="Scripts/Plugins/jquery.chosen.js" runat="server" LoadAfterUI="true" />
    <SharePoint:CssRegistration name="Plugins/jquery.chosen.css" runat="server" /> 

    <style type="text/css">                     
        .pull-left {
          float: left !important;
        }
        .pull-right {
          float: right !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            function formate(str) {
                if (str.lastIndexOf(";") == str.length - 1) {
                    str = str.substring(0, str.length - 1);
                }
                return str;
            }
            var url = "/_vti_bin/Apps.svc/WelcomePage";
            if (_spPageContextInfo.webServerRelativeUrl.length > 1) {
                url = _spPageContextInfo.webServerRelativeUrl + url;
            }
            $.ajax({
                type: "GET",
                url: url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var configs = data["GetWelcomePageConfigsResult"];
                    _.each(configs, function (config, index) {
                        if (_.isEmpty(config.RoleId)) {
                            $("#defaltPageUrl").val(config.PageUrl);
                        } else {
                            $("#roles").val($("#roles").val() + config.RoleId + ";");
                            $("#pageUrls").val($("#pageUrls").val() + config.PageUrl + ";");
                        }
                    })
                }
            });
            $("#btn").click(function () {
                var configs = [];
                var roles = formate($("#roles").val()).split(";");
                var pageUrls = formate($("#pageUrls").val()).split(";");
                var defaultPageUrl = $("#defaltPageUrl").val();

                _.each(roles, function (roleId, index) {
                    var config = { RoleId: roleId, Category: "Page", PageUrl: pageUrls[index] };
                    configs.push(config);
                })

                var defaultConfig = { RoleId: "", Category: "Page", PageUrl: defaultPageUrl };
                configs.push(defaultConfig);
                $.ajax({
                    type: "POST",
                    url: url,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ configs: configs }),
                    success: function (data) {
                        result = data["SetWelcomePageConfigsResult"];
                        console.log(result);
                    }
                });
            })
        });
    </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <input type="text" id="roles" value="rolea;rolev;rolec;" />
    <input type="text" id="pageUrls" value="/Pages/a.aspx;/Pages/b.aspx;/Pages/c.aspx;" />
    <input type="text" id="defaltPageUrl" value="/Pages/default.aspx" />
    <input type="button" id="btn"/>
    <asp:ValidationSummary ID="ValidationSummary" runat="server" 
     DisplayMode="BulletList" ShowSummary="true" HeaderText="请验证以下信息:" />
    <table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0" id="diidProjectPageOverview">
        <wssuc:InputFormSection id="titleSection"
            Title="名称及地址"
            Description="填写应用的基础信息，包括应用的类别，名称，地址等。"
            runat="server">
            <template_inputformcontrols>
                   <wssuc:InputFormControl LabelText="类别" runat="server">
                       <Template_Control>
                           <asp:DropDownList ID="AppType" runat="server" Width="490">
                               <asp:ListItem Value="3">外部系统应用</asp:ListItem>
                               <asp:ListItem Value="5">与用户平台身份验证集成（JWT）</asp:ListItem>
                               <%--
                               <asp:ListItem Value="1">ERP应用（SAP WebGUI）</asp:ListItem>
                               <asp:ListItem Value="2">ERP应用（SAP Web GUI 用户界面优化的应用)</asp:ListItem>
                               --%>
                               <asp:ListItem Value="4">外部系统服务</asp:ListItem>
                               <asp:ListItem Value="7">A4系统</asp:ListItem>
                           </asp:DropDownList>
				       </Template_Control>
			       </wssuc:InputFormControl>
			       <wssuc:InputFormControl LabelText="名称" runat="server">
				       <Template_Control>
                           <wssawc:InputFormTextBox ID="AppTitle" Runat="server" 
                               class="ms-input" Columns="65" MaxLength="255" />                           
                           <wssawc:InputFormRequiredFieldValidator ControlToValidate="AppTitle" Runat="server"
                               Display="Dynamic" SetFocusOnError="true"/>
				       </Template_Control>
			       </wssuc:InputFormControl>
                   <wssuc:InputFormControl LabelText="地址" runat="server">
				       <Template_Control>
                           <wssawc:InputFormTextBox ID="AppLaunchUrl" Runat="server" 
                               class="ms-input" TextMode="MultiLine" Columns="66" Rows="4"/>     
                           <wssawc:InputFormRequiredFieldValidator ControlToValidate="AppLaunchUrl" Runat="server"
                               Display="Dynamic" SetFocusOnError="true"/>
                           <wssawc:InputFormRegularExpressionValidator ControlToValidate="AppLaunchUrl" Runat="server" 
                               Display="Dynamic" SetFocusOnError="true" 
                               ValidationExpression="(http|ftp|https):\/\/([\w.]+\/?)\S*" 
                               ErrorMessage="请输入正确的链接地址，形如：http://;https://" />
				       </Template_Control>
			       </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection id="logoSection"
            Title="分类及图标" runat="server">
            <template_description>
                <SharePoint:EncodedLiteral runat="server" EncodeMethod="HtmlEncode"
                   Text="配置应用的分类，图标及打开方式等。" />     
                <div>
                    <img id="loading" alt="" src="/_layouts/15/images/loadingcirclests16.gif?rev=23" />
				    <img id="icon" class="ms-siteicon-img" style="margin:0; padding:0; max-width:12em; max-height:12em; visibility:hidden;"/>
				</div>           
	        </template_description>
            <template_inputformcontrols>
                <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_ItemCategory%>" runat="server">
				     <Template_Control>
                         <asp:Table ID="TaxonomyWebTaggingControls" Runat="server" Width="495"></asp:Table>
				     </Template_Control>
			    </wssuc:InputFormControl>
			    <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_ThumbnailURLFieldTitle%>" runat="server">
				     <Template_Control>
					    <div>
						    <a id="upload" href="#" class="ms-commandLink">
                                <SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,prjsetng_icon_upload%>" EncodeMethod='HtmlEncode'/>
						    </a>
                            <span class="ms-commandLink"> | </span>
                            <a id="select" href="#" class="ms-commandLink">
                                <SharePoint:EncodedLiteral runat="server" text="<%$Resources:cmscore,prjsetng_fromsp%>" EncodeMethod='HtmlEncode'/>
						    </a>
					    </div>
					    <div style="padding-bottom:.5em;padding-top:.5em;">
						    <wssawc:InputFormTextBox ID="AppThumbnailURL" Runat="server" 
                                class="ms-input" Columns="65" MaxLength="512" Direction="LeftToRight"/>
					    </div>
                        <div style="padding-bottom:.5em;padding-top:.5em;">
						     <wssawc:InputFormTextBox ID="AppStyleClass" Runat="server" class="ms-input" Columns="75" MaxLength="512"/>
                             <wssawc:InputFormTextBox ID="AppStyleColor" Runat="server" class="ms-input" Columns="75" MaxLength="512"/>
					    </div>
				     </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl LabelText="其他" runat="server">
				     <Template_Control>                         					    
                         <div class="ms-slDlg-IndentedFieldLabel">
					         <asp:CheckBox id="AppOpenInNewWindow" Runat="server" Checked="true" Text="在新窗口打开该应用"/>
                             <asp:CheckBox id="AppRecommend" Runat="server" Checked="false" Text="推荐到首页"/>
				         </div>
				     </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection id="authorizationSection"
            Title="应用的授权" runat="server">
            <template_description>
		       <SharePoint:EncodedLiteral runat="server" 
                   Text="新建应用可在此配置初始角色，修改应用授权请到基础平台中授权。" 
                   EncodeMethod='HtmlEncode'/>
	        </template_description>
            <template_inputformcontrols>
                <wssuc:InputFormControl LabelText="角色" runat="server">
                    <Template_Control>
                        <asp:ListBox ID="AppRoles" runat="server" SelectionMode="Multiple" Width="450">
                        </asp:ListBox>
                        <wssawc:InputFormRequiredFieldValidator ControlToValidate="AppRoles" Runat="server"
                               Display="Dynamic" SetFocusOnError="true"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
            </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection id="showSection" runat="server">
            <template_description>
                <a class="ms-commandLink" href="#">显示更多</a>
            </template_description>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection id="additionalSection"
            Title="扩展的信息"
            Description="为注册应用填写基础信息，包括应用的类别，名称，地址等。"
            runat="server">
            <template_inputformcontrols>
                <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_ShortDescriptionFieldTitle;%>" runat="server">
                    <Template_Control>
                        <wssawc:InputFormTextBox ID="AppShortDescription" Runat="server" 
                            class="ms-input" TextMode="MultiLine" Columns="66" Rows="2"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
			    <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_DescriptionFieldTitle;%>" runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="AppDescription" Runat="server" 
                            class="ms-input" TextMode="MultiLine" Columns="66" Rows="4"/>
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_SupportURLFieldTitle;%>" runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="AppSupportURL" Runat="server" 
                            class="ms-input" Columns="65" MaxLength="512" />   
                        <wssawc:InputFormRegularExpressionValidator ControlToValidate="AppSupportURL" Runat="server" 
                            Display="Dynamic" SetFocusOnError="true" 
                            ValidationExpression="(http|ftp|https):\/\/([\w.]+\/?)\S*" 
                            ErrorMessage="请输入正确的链接地址，形如：http://;https://" />                        
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_VideoURLFieldTitle;%>" runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="AppVideoURL" Runat="server" 
                            class="ms-input" Columns="65" MaxLength="512" />
                        <wssawc:InputFormRegularExpressionValidator ControlToValidate="AppVideoURL" Runat="server" 
                            Display="Dynamic" SetFocusOnError="true" 
                            ValidationExpression="(http|ftp|https):\/\/([\w.]+\/?)\S*" 
                            ErrorMessage="请输入正确的链接地址，形如：http://;https://" />
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_ImageURLFieldTitle1;%>" runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="AppImageURL1" Runat="server" 
                            class="ms-input" Columns="65" MaxLength="512" />
                        <wssawc:InputFormRegularExpressionValidator ControlToValidate="AppImageURL1" Runat="server" 
                            Display="Dynamic" SetFocusOnError="true" 
                            ValidationExpression="(http|ftp|https):\/\/([\w.]+\/?)\S*" 
                            ErrorMessage="请输入正确的链接地址，形如：http://;https://" />
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_ImageURLFieldTitle2;%>" runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="AppImageURL2" Runat="server" 
                            class="ms-input" Columns="65" MaxLength="512" />
                        <wssawc:InputFormRegularExpressionValidator ControlToValidate="AppImageURL2" Runat="server" 
                            Display="Dynamic" SetFocusOnError="true" 
                            ValidationExpression="(http|ftp|https):\/\/([\w.]+\/?)\S*" 
                            ErrorMessage="请输入正确的链接地址，形如：http://;https://" />
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_ImageURLFieldTitle3;%>" runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="AppImageURL3" Runat="server" 
                            class="ms-input" Columns="65" MaxLength="512" />
                        <wssawc:InputFormRegularExpressionValidator ControlToValidate="AppImageURL3" Runat="server" 
                            Display="Dynamic" SetFocusOnError="true" 
                            ValidationExpression="(http|ftp|https):\/\/([\w.]+\/?)\S*" 
                            ErrorMessage="请输入正确的链接地址，形如：http://;https://" />
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_ImageURLFieldTitle4;%>" runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="AppImageURL4" Runat="server" 
                            class="ms-input" Columns="65" MaxLength="512" />
                        <wssawc:InputFormRegularExpressionValidator ControlToValidate="AppImageURL4" Runat="server" 
                            Display="Dynamic" SetFocusOnError="true" 
                            ValidationExpression="(http|ftp|https):\/\/([\w.]+\/?)\S*" 
                            ErrorMessage="请输入正确的链接地址，形如：http://;https://" />
				    </Template_Control>
			    </wssuc:InputFormControl>
                <wssuc:InputFormControl LabelText="<%$Resources:core,Marketplace_ImageURLFieldTitle5;%>" runat="server">
				    <Template_Control>
                        <wssawc:InputFormTextBox ID="AppImageURL5" Runat="server" 
                            class="ms-input" Columns="65" MaxLength="512" />
                        <wssawc:InputFormRegularExpressionValidator ControlToValidate="AppImageURL5" Runat="server" 
                            Display="Dynamic" SetFocusOnError="true" 
                            ValidationExpression="(http|ftp|https):\/\/([\w.]+\/?)\S*" 
                            ErrorMessage="请输入正确的链接地址，形如：http://;https://" />
				    </Template_Control>
			    </wssuc:InputFormControl>
	        </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:ButtonSection runat="server">
            <template_buttons>
                <asp:Button id="btnSubmit" runat="server" 
                    Text="<%$Resources:wss,multipages_createbutton_text%>" class="ms-ButtonHeightWidth" 
                    UseSubmitBehavior="false" OnClick="BtnSubmit_Click" accesskey="<%$Resources:wss,multipages_createbutton_accesskey%>"/>
                <asp:Button id="BtnCreate" runat="server" 
                    class="ms-ButtonHeightWidth" Text="创建" UseSubmitBehavior="false" OnClick="BtnCreate_Click"/>
                <asp:Button id="BtnSave" runat="server" 
                    class="ms-ButtonHeightWidth" Text="保存" UseSubmitBehavior="false" OnClick="BtnSave_Click" />
                <asp:Button id="BtnNext" runat="server" 
                    class="ms-ButtonHeightWidth" Text="继续" UseSubmitBehavior="false" OnClick="BtnNext_Click" />
	        </template_buttons>
        </wssuc:ButtonSection>
    </table>
    <SharePoint:ScriptBlock runat="server">        
        $(function () {
            $iconPicker = $("#<%=AppStyleClass.ClientID%>");
            $iconPicker.iconpicker({
                title: "请选择应用图标",
                placement: "bottomRight",
            });


            $roleSelect = $("#<%=AppRoles.ClientID%>");
            $roleSelect.chosen({
                allow_single_deselect: true,
                search_contains: true,
                placeholder_text_multiple: "请选择需授权角色",
                no_results_text: "未找到匹配角色"
            });

            $additionalSection = $("#<%=additionalSection.ClientID%>");
            $showSection = $("#<%=showSection.ClientID%>");

            $createButton = $("#<%=BtnCreate.ClientID%>");
            $saveBustton = $("#<%=BtnSave.ClientID%>");
            $nextButton = $("#<%=BtnNext.ClientID%>");

            $additionalSection.hide();
            $showSection.find("a").click(function () {
                $showSection.hide();
                $additionalSection.show();
            });
        });
        (function () {
            var getUrlParameter = function getUrlParameter(sParam) {
                var sPageURL = decodeURIComponent(window.location.search.substring(1)),
                    sURLVariables = sPageURL.split('&'),
                    sParameterName,
                    i;

                for (i = 0; i < sURLVariables.length; i++) {
                    sParameterName = sURLVariables[i].split('=');

                    if (sParameterName[0] === sParam) {
                        return sParameterName[1] === undefined ? true : sParameterName[1];
                    }
                }
            };
            if(imgsrc = document.getElementById(<%SPHttpUtility.WriteAddQuote(SPHttpUtility.NoEncode(AppThumbnailURL.ClientID), this.Page);%>)){
                var img = document.getElementById("icon"), loading = document.getElementById("loading"),
                    tid, sid;
                img.onload = function () {
					loading.style.display = "none";
					img.style.visibility = "visible";					 
					removeAllStatus(true);
					sid = null;
				};
				img.onerror = function () {
					loading.style.display = "none";
					imgsrc.value ?
						(sid || setStatusPriColor((sid = addStatus("<asp:Literal runat="server" Text="" />", "<asp:Literal runat="server" Text="" />", false)), "yellow")) :
						removeAllStatus(true);
				};
                (updateimg = function () {
					var src = imgsrc.value || "<asp:Literal ID="defaultLogo" runat="server" />";
					if (src.indexOf("?") === -1 && src.indexOf("\\") === -1) {
						src += "?" + new Date().getTime();
					}
					img.style.visibility = "hidden";
					loading.style.display = "inline";
					img.src = src;
				})();
                $addHandler(imgsrc, "keydown", function (e) {
					if (e.keyCode === Sys.UI.Key.enter) {
						e.preventDefault();
					} else {
						tid && clearTimeout(tid);
						tid = setTimeout(updateimg, 1000);
					}
				});
				$addHandler(imgsrc, "change", function () {
					tid && clearTimeout(tid);
					updateimg();
				});
                //serverRelativeUrl + (serverRelativeUrl.substr(-1) === "/" ? '' : '/') +
                $addHandler(document.getElementById("upload"), "click", function (e) {
                    EnsureScriptFunc("SP.js", "SP.Utilities.Utility", function () {
                        SP.SOD.execute("sp.ui.dialog.js", "SP.UI.ModalDialog.showModalDialog", {
                            url: SP.Utilities.Utility.getLayoutsPageUrl("upload.aspx?RootFolder=&List=<%= ThumbnailsList.ID %>&AllowMultipleUploads=0"),
                            dialogReturnValueCallback: function (result, retval) {
                                if (result === SP.UI.DialogResult.OK) {
                                    imgsrc.value = retval.newFileUrl;
                                    updateimg();
                                }
                            }
                        });
                    });
                    e.preventDefault();
                });
                $addHandler(document.getElementById("select"), "click", function (e) {
                    EnsureScriptFunc("SP.js", "SP.Utilities.Utility", function () {
                        EnsureScript("AssetPickers.js", typeof AssetPickerConfig, function () {
                            AssetPickerConfig.prototype.GetDialogUrl = function(a) {
                                return "/_layouts/15/" + a;
                            }
                            var config = new AssetPickerConfig("");
                            config.ClientID = "assetpicker";
                            config.CurrentWebBaseUrl = SP.PageContextInfo.get_webServerRelativeUrl();
                            config.AllowExternalUrls = true;
                            config.ManageHyperlink = false;
                            config.AssetUrlClientID = imgsrc.id;
                            config.ReturnCallback = updateimg;
                            new ImageAsset(imgsrc.value).LaunchModalAssetPicker(config);
                        });
                    });
                    e.preventDefault();
                });
            }
        })();
    </SharePoint:ScriptBlock>
</asp:Content>
