/// <reference path="_references.js" />
"use strict";

LoadSodByKey("jQuery.js", function () {
    $(function () {
        $(".ms-core-listMenu-verticalBox .ms-core-listMenu-root > li > ul").hide();
        $(".ms-core-listMenu-verticalBox .ms-core-listMenu-root > li > a").click(function () {
            $(this).siblings("ul").slideToggle();
            return false;
        });
        $(".ms-core-listMenu-verticalBox .ms-core-listMenu-root > li > span").click(function () {
            $(this).siblings("ul").slideToggle();
            return false;
        });
    });
});

