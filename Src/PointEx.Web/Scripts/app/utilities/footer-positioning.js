(function () {
    // Window load event used just in case window height is dependant upon images
    $(window).bind("load", function () {
        
        var footerHeight = 0,
            navbarHeight = 0,
            $footer = $("#footer"),
            $navbar = $('.navbar'),
            $container = $("#container .left-container");

        if ($container.length === 0 || $footer.length === 0 || $navbar === 0) {
            return;
        }

        positionFooter();

        function positionFooter() {

            footerHeight = $footer.height();
            navbarHeight = $navbar.height();

            if (($container.height() + footerHeight) < $(window).height()) {

                //increase container height
                var newContainerHeight = $(window).height() - footerHeight - navbarHeight;

                $container.height(newContainerHeight);
            }
        }
    });
})();