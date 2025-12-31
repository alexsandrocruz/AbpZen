var FaqPage = function () {

    var initAccordionFilter = function ($container) {

        var filterDocumentItems = function (filterText) {
            var $items = $container.find(".faq.accordion-item");

            if (!filterText) {
                $items.show();
                return;
            }

            var $filteredItems = $items.filter(function () {
                return $(this).text().toUpperCase().indexOf(filterText.toUpperCase()) > -1;
            });

            $items.hide();
            $filteredItems.show();
        };

        $(".faq-filter").on('input', function (e) {
            filterDocumentItems(e.target.value);
        });
    };

    var expandAccordionFromUrl = function () {
        if (location.hash && location.hash !== "undefined") {
            setTimeout(function () {
                var hash = location.hash;
                var $button = $("button[data-bs-target='" + hash + "']");
                if ($button.length === 0) {
                    return;
                }

                $button.trigger("click");
                var hashWithoutFirstChar = hash.substring(1);
                var $div = $("div.faq[data-anchor-id='" + hash + "']:first");

                var topHeight = 0;
                var $mainMenu = $("#product-nav");
                if ($mainMenu.length > 0) {
                    topHeight += $mainMenu.height();
                }
                var $bfBanner = $(".bf-top-banner");
                if ($bfBanner.length > 0) {
                    topHeight += $bfBanner.height();
                }

                if ($div.length > 0) {
                    $([document.documentElement, document.body]).animate({
                        scrollTop: $div.offset().top - topHeight - 100
                    });
                }
            }, 100);
        }
    };

    var addHashWhenClickToFaqItem = function () {
        $(".faq-container .accordion-button").click(function (el) {
            var target = $(this).data("bs-target");
            history.pushState(null, null, target);
            $(".faq.accordion-item").removeClass("shown");
            $(el.currentTarget).closest(".faq.accordion-item").addClass("shown");
        });
    };

    return {
        init: function () {
            initAccordionFilter($(".faq-container .accordion"));
            expandAccordionFromUrl();
            addHashWhenClickToFaqItem();
        }
    };
};

$(document).ready(function () {
    FaqPage().init();
});
