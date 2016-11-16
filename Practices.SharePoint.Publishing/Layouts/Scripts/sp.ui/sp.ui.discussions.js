/// <reference path="../_references.js" />

ExecuteOrDelayUntilEventNotified(function () {
    SP.UI.Discussions.HeaderBehavior.prototype.attachEvents = function () {
        var $v_0 = $get(this.$1J_0);
        if (!SP.UI.Discussions.Helpers.isNullOrUndefined($v_0)) {
            $addHandler($v_0, 'click', this.$$d_onClickNewPostLink);
        }

        var $v_1 = $get(this.$1P_0);
        if (!SP.UI.Discussions.Helpers.isNullOrUndefined($v_1)) {
            $addHandler($v_1, 'click', this.$$d_$2U_0);
        }

        var $sbtn = $get("DiscussionsSearchButton");
        if (!SP.UI.Discussions.Helpers.isNullOrUndefined($sbtn)) {
            //context = this.context
            $addHandler($sbtn, 'click', Function.createDelegate(this, function () {
                var key = $get("DiscussionsSearchInput").value;
                var queryItem = new SP.UI.Discussions.QueryOverrideItem(this.context, "查询",
                ' <Query><OrderBy><FieldRef Name="Created" Ascending="FALSE" /></OrderBy><Where><Contains><FieldRef Name="Title" /><Value Type="Text">' + key + '</Value></Contains></Where></Query>');
                queryItem.onClick();
            }));
        }

        this.sortFilterPickerControl.initialize();
    },
    SP.UI.Discussions.HeaderBehavior.prototype.render = function (builder) {
        if (!SP.UI.Discussions.Helpers.isAnonymousAccess(this.context)) {
            var $v_0 = this.getNewPostUrl();

            builder.addCommunitiesCssClass('heroLinkContainer');
            builder.renderBeginTag('div');
            builder.addAttribute('id', this.$1J_0);
            builder.addAttribute('href', $v_0);
            builder.addCssClass('ms-textXLarge');
            builder.addCssClass('ms-heroCommandLink');
            builder.addAttribute('title', Strings.STS.L_SPDiscHeroLinkAltText);
            builder.renderBeginTag('a');
            builder.addCssClass('ms-list-addnew-imgSpan20');
            builder.renderBeginTag('span');
            builder.addCssClass('ms-list-addnew-img20');
            builder.addAttribute('src', GetThemedImageUrl('spcommon.png'));
            builder.renderBeginTag('img');
            builder.renderEndTag();
            builder.renderEndTag();
            builder.renderBeginTag('span');
            builder.writeEncoded(Strings.STS.L_SPDiscHeroLinkFormat);
            builder.renderEndTag();
            builder.renderEndTag();
            // Start Render SearchBox
            builder.addAttribute('style', 'float: right;');
            builder.renderBeginTag('div');
            builder.addCssClass('ms-srch-sb ms-srch-sb-border');
            builder.renderBeginTag('div');
            builder.addCssClass('ms-textSmall ms-srch-sb-prompt ms-helperText');
            builder.addAttribute('type', 'text');
            builder.addAttribute('id', 'DiscussionsSearchInput');
            builder.renderBeginTag('input');
            builder.renderEndTag();
            builder.addCssClass('ms-srch-sb-searchLink');
            builder.addAttribute('title', '搜索');
            builder.addAttribute('id', 'DiscussionsSearchButton');
            builder.renderBeginTag('a');
            builder.addCssClass('ms-srch-sb-searchImg');
            builder.addAttribute('src', '/_layouts/15/images/searchresultui.png?rev=23');
            builder.renderBeginTag('img');
            builder.renderEndTag();
            builder.renderEndTag();
            builder.renderEndTag();
            builder.renderEndTag();
            // End Render SearchBox
            builder.addCssClass('ms-clear');
            builder.renderBeginTag('div');
            builder.renderEndTag();
            builder.renderEndTag();
        }
        builder.addCommunitiesCssClass('forumHeaderContainer');
        builder.renderBeginTag('div');
        this.sortFilterPickerControl.$1B_0 = true;
        this.sortFilterPickerControl.render(builder);
        builder.addAttribute('id', this.$1P_0);
        builder.addAttribute('href', 'javascript:;');
        builder.addAttribute('title', Strings.STS.L_SPDiscRefresh);
        builder.addCssClass('ms-floatRight');
        builder.addCommunitiesCssClass('refreshIcon-a');
        builder.renderBeginTag('a');
        builder.addAttribute('src', GetThemedImageUrl('spcommon.png'));
        builder.addCommunitiesCssClass('refreshIcon');
        builder.addAttribute('alt', Strings.STS.L_SPDiscRefresh);
        builder.addAttribute('title', Strings.STS.L_SPDiscRefresh);
        builder.renderBeginTag('img');
        builder.renderEndTag();
        builder.renderEndTag();
        builder.addCssClass('ms-clear');
        builder.renderBeginTag('div');
        builder.renderEndTag();
        builder.renderEndTag();
    },
    SP.UI.Discussions.HeaderBehavior.prototype.populateSortFilterItemList = function () {
        var $v_0 = [
            //new SP.UI.Discussions.QueryOverrideItem(this.context, Strings.STS.L_SPDiscSortMostRecent, ''),
            new SP.UI.Discussions.QueryOverrideItem(this.context, "最新发布", ""),
            new SP.UI.Discussions.QueryOverrideItem(this.context, "最多回复",
                '<Query><OrderBy><FieldRef Name="ItemChildCount" Ascending="FALSE" /></OrderBy></Query>'),
            new SP.UI.Discussions.QueryOverrideItem(this.context, Strings.STS.L_SPDiscSortUnanswered, '<Query><OrderBy UseIndexForOrderBy=\"TRUE\" Override=\"TRUE\"/><Where><Eq><FieldRef Name=\"IsAnswered\"/><Value Type=\"Integer\">0</Value></Eq></Where></Query>'),
            new SP.UI.Discussions.QueryOverrideItem(this.context, Strings.STS.L_SPDiscSortAnswered, '<Query><OrderBy UseIndexForOrderBy=\"TRUE\" Override=\"TRUE\"/><Where><Eq><FieldRef Name=\"IsAnswered\"/><Value Type=\"Integer\">1</Value></Eq></Where></Query>'),
            new SP.UI.Discussions.QueryOverrideItem(this.context, Strings.STS.L_SPDiscFilterFeatured, '<Query><OrderBy UseIndexForOrderBy=\"TRUE\" Override=\"TRUE\"/><Where><Eq><FieldRef Name=\"IsFeatured\"></FieldRef><Value Type=\"Integer\">1</Value></Eq></Where></Query>')
        ];

        for (var $$arr_1 = $v_0, $$len_2 = $$arr_1.length, $$idx_3 = 0; $$idx_3 < $$len_2; ++$$idx_3) {
            var $v_1 = $$arr_1[$$idx_3];

            this.sortFilterPickerControl.addItem($v_1);
        }
        if (!SP.UI.Discussions.Helpers.isAnonymousAccess(this.context)) {
            this.sortFilterPickerControl.insertItem(1, new SP.UI.Discussions.QueryOverrideItem(this.context, Strings.STS.L_SPDiscSortMyPosts, '<Query><OrderBy UseIndexForOrderBy=\"TRUE\" Override=\"TRUE\"/><Where><Eq><FieldRef Name=\"Author\"/><Value Type=\"Integer\"><UserID Type=\"Integer\"/></Value></Eq></Where></Query>'));
        }
    }
}, "sp.scriptloaded-sp.ui.discussions.js");