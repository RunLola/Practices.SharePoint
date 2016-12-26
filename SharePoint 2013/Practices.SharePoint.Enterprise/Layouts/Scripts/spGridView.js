/// <reference path="_references.js" />
EnsureScriptFunc("jquery.js", "$", function () {
    (function ($) {
        $.fn.extend({
            spGridView: function () {
                var $grid = $(this);
                $grid.find(".ms-vb-itmcbx").click(function () {
                    $(this).closest("tr").toggleClass("s4-itm-selected");
                    RefreshCommandUI();
                });
                $grid.find(".ms-cellstyle").click(function () {
                    if ($(this).closest("tr").hasClass("s4-itm-selected")) {
                        $grid.find(".ms-itmhover").removeClass("s4-itm-selected");
                    } else {
                        $grid.find(".ms-itmhover").removeClass("s4-itm-selected");
                        $(this).closest("tr").addClass("s4-itm-selected");
                    }
                    RefreshCommandUI();
                });
                $grid.find(".ms-vh-selectAllIcon").click(function () {
                    if ($grid.find(".ms-itmhover").length > $grid.find(".s4-itm-selected").length) {
                        $grid.find(".ms-itmhover").addClass("s4-itm-selected");
                    } else {
                        $grid.find(".ms-itmhover").removeClass("s4-itm-selected");
                    }
                    RefreshCommandUI();
                });
            },
            getSelectedItems: function () {
                var $grid = $(this);
                var selectedItems = [];
                $grid.find(".s4-itm-selected").each(function () {
                    var json = $(this).find(".ms-vb-itmcbx input").val();
                    var selectedItem = JSON.parse(json);
                    selectedItems.push(selectedItem);
                });
                return selectedItems;
            }
        });
    })(jQuery);
})
