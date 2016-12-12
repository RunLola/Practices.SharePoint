<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI"
    Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<ul class="ms-core-suiteLinkList">
    <li class="ms-core-suiteLink">
        <a class="ms-core-suiteLink-a" id="MyTasks" href="#">
            <span>我的任务<span class="badge">4</span></span>
        </a>
    </li>
    <li class="ms-core-suiteLink">
        <a class="ms-core-suiteLink-a" id="My" href="#">
            <span>我的记录<span class="badge">12</span></span>
        </a>
    </li>
    <!--<li class="ms-core-suiteLink">
        <a class="ms-core-suiteLink-a" id="ctl00_ctl59_ShellSites" href="#">
            <span>网站
                <span class="ms-suitenav-caratBox" id="Suite_ActiveLinkIndicator_Clip">
                    <img class="ms-suitenav-caratIcon" id="Suite_ActiveLinkIndicator" src="/_layouts/15/images/spcommon.png?rev=23">
                </span>
            </span>
        </a>
    </li>-->
</ul>
