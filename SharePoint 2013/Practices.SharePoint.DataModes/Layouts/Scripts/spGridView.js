/// <reference path="_references.js" />

LoadSodByKey("jquery.js", function () {
    (function ($) {
        $.fn.extend({
            spGridView: function () {
                var $grid = $(this);
                $grid.find(".ms-itmhover").click(function () {
                    $(this).toggleClass("s4-itm-selected");
                });
                $grid.find(".ms-vh-selectAllIcon").click(function () {
                    if ($grid.find(".ms-itmhover").length > $grid.find(".s4-itm-selected").length) {
                        $grid.find(".ms-itmhover").addClass("s4-itm-selected");
                    } else {
                        $grid.find(".ms-itmhover").removeClass("s4-itm-selected");
                    }
                });
            },
            getSelectedItems: function () {
                var selectedItems = [];
                $(this).find(".s4-itm-selected").each(function () {
                    var json = $(this).find(".ms-vb-itmcbx input").val();
                    var selectedItem = JSON.parse(json);
                    selectedItems.push(selectedItem);
                });
                return selectedItems;
            }
        });
    })(jQuery);
})