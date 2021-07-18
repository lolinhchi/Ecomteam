/*

Template:  Theme Name
Author: author name
Version: 1
Design and Developed by: BM Rafiq + Masud Rana
NOTE: If you have any note put here.

*/
/*================================================
[  Table of contents  ]
================================================
	01. jQuery MeanMenu
	02. wow js active
	03. scrollUp jquery active
	04. slick carousel





======================================
[ End table content ]
======================================*/

$('#ensign-nivoslider').nivoSlider({
    effect: 'random',
    slices: 15,
    boxCols: 8,
    boxRows: 4,
    animSpeed: 500,
    pauseTime: 3000,
    startSlide: 0,
    directionNav: true,
    controlNav: true,
    controlNavThumbs: false,
    pauseOnHover: true,
    manualAdvance: false,
    prevText: 'Prev',
    nextText: 'Next',
    randomStart: false,
    beforeChange: function () { },
    afterChange: function () { },
    slideshowEnd: function () { },
    lastSlide: function () { },
    afterLoad: function () { }
});


/*---------rating star-----------------*/
/*---------rating star-----------------*/
var rating = 0;

$(document).ready(function () {
    ResetstarColor();
    $('.fa-star').click(function () {
        rating = parseInt($(this).data('value'));
    });
    $('.fa-star').mouseover(function () {
        ResetstarColor();
        var crrValue = parseInt($(this).data('value'));
        for (i = 0; i < crrValue; i++) {
            $('.fa-star:eq(' + i + ')').css('color', '#FF6F00');
        }
    });
    $('.fa-star').mouseleave(function () {
        ResetstarColor();
        if (rating != 0) {
            for (i = 0; i < rating; i++) {
                $('.fa-star:eq(' + i + ')').css('color', '#FF6F00');
            }
        }
    });


    // cart
    function AddCart(idpro) {

        var sl = $("#sl").val();
        if (sl == null) {
            sl = 1;
        }
        alert(idpro + sl);
        $.ajax({
            url: '/cart/AddItem?idpro=' + idpro + '&soluong=' + sl,
            type: "POST",
            data: { idProduct: idProduct, stars: stars, content: content },


            success: function (data) {
                $("#content").val("");
                $("#cat-treeview").html(data);

            },
        });

    };


});
function ResetstarColor() {
    $('.fa-star').css('color', '#90A4AE');
}


(function ($) {
    "use strict";



    /*------------------------------------------------
          Top menu stick
         -------------------------------------------------- */
    // var sticky_menu = $("#sticky-header");
    // var pos = sticky_menu.position();
    // if (sticky_menu.length) {
    //     var windowpos = sticky_menu.top;
    //     $(window).on('scroll', function() {
    //         var windowpos = $(window).scrollTop();
    //         if (windowpos > pos.top) {
    //             sticky_menu.addClass("sticky");
    //         } else {
    //             sticky_menu.removeClass("sticky");
    //         }
    //     });
    // }

    //---------------------------------------------
    //Nivo slider
    //---------------------------------------------

    /*-------------------------------------------
    02. wow js active
    --------------------------------------------- */
    new WOW().init();

    /*-------------------------------------------
    03. scrollUp jquery active
    --------------------------------------------- */
    $.scrollUp({
        scrollText: '<i class="zmdi zmdi-chevron-up"></i>',
        easingType: 'linear',
        scrollSpeed: 900,
        animation: 'fade'
    });


    /*-------------------------------------------
    04. slick carousel
    --------------------------------------------- */
    $('.your-class').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000
    });


    /*************************
          tooltip
    *************************/
    $('[data-toggle="tooltip"]').tooltip();

    /*----------------------------
     treeview active
    ------------------------------ */
    $("#cat-treeview ul").treeview({
        animated: "fast",
        collapsed: true,
        unique: true,
        persist: "cookie"
    });

    /*---------------------
    5. countdown
    --------------------- */
    $('[data-countdown]').each(function () {
        var $this = $(this), finalDate = $(this).data('countdown');
        $this.countdown(finalDate, function (event) {
            $this.html(event.strftime('<span class="cdown days"><span class="time-count">%-D</span> <p>Days</p></span> <span class="cdown hour"><span class="time-count">%-H</span> <p>Hour</p></span> <span class="cdown minutes"><span class="time-count">%M</span> <p>Min</p></span> <span class="cdown second"> <span><span class="time-count">%S</span> <p>Sec</p></span>'));
        });
    });

    /*----------------------------
    Testimonial List owl active
    ------------------------------ */
    $(".testimonial-list").owlCarousel({
        autoPlay: false,
        slideSpeed: 2000,
        pagination: true,
        animateOut: 'slideOutDown',
        animateIn: 'flipInX',
        navigation: false,
        items: 1,
        itemsDesktop: [1199, 1],
        itemsDesktopSmall: [991, 1],
        itemsTablet: [767, 1],
        itemsMobile: [479, 1]
    });

    /*----------------------------
    owl active Deal products
    ------------------------------ */
    $(".brand-list").owlCarousel({
        autoPlay: false,
        slideSpeed: 2000,
        pagination: false,
        navigation: false,
        items: 5,
        itemsDesktop: [1199, 4],
        itemsDesktopSmall: [991, 3],
        itemsTablet: [767, 2],
        itemsMobile: [479, 1]
    });




    /*----------------------------
     price-slider active
    ------------------------------ */
    $("#slider-range").slider({
        range: true,
        min: 0,
        max: 999,
        values: [60, 750],
        slide: function (event, ui) {
            $("#amount").val("£" + ui.values[0] + " - £" + ui.values[1]);
        }
    });
    $("#amount").val("£" + $("#slider-range").slider("values", 0) +
        " - £" + $("#slider-range").slider("values", 1));

    /*---------------------
    venobox
    --------------------- */
    $('.venobox').venobox();


    /*----------------------------
    jQuery MeanMenu
    ------------------------------ */
    jQuery('nav#dropdown').meanmenu();

    /*----------------------------
      Input Plus Minus Button
    ------------------------------ */
    $(".cart-plus-minus-box").append('<div class="dec qtybutton">-</div><div class="inc qtybutton">+</div>');
    $(".qtybutton").on("click", function () {
        var $button = $(this);
        var oldValue = $button.parent().find("input").val();
        if ($button.text() == "+") {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        $button.parent().find("input").val(newVal);
    });


})(jQuery);

$(window).scroll(function () {
   
    if ($(this).scrollTop() > 1) {
        $(document).find('#cart_i').show();
        $('#sticky-header').addClass("sticky");
        
      
    }
    else {
        
        $(document).find('#cart_i').hide();
        $('#sticky-header').removeClass("sticky");

    }
});
$(function () {
    //var poshide = $('.sale-area section-padding').offset().top;
    var posshow = ($('.slider-area').offset().top + $('.slider-area').height()) / 2;
    var poshide = $('.sale-area').offset().top;

    $(window).scroll(function (event) {
        var posbody = $(window).scrollTop();
       //// console.log("Scroll from Top: " + posbody.toString());
        //console.log("Scroll from show: " + posshow);
        //console.log("Scroll from hide: " + poshide);
        if (posbody >= posshow && posbody < poshide) {
            $('.ads').show();
        }
        else {
            $('.ads').hide(300);
        }


    });
});



function hide_float_right() {

    var content = document.getElementById('float_content_right');

    var hide = document.getElementById('hide_float_right');

    if (content.style.display == "none") { content.style.display = "block"; hide.innerHTML = '<a href="javascript:hide_float_right()">Tắt quảng cáo [X]</a>'; }

    else {
        content.style.display = "none"; hide.innerHTML = '<a href="javascript:hide_float_right()">Xem quảng cáo...</a>';

    }

}
function hide_float_left() {

    var content = document.getElementById('float_content_left');

    var hide = document.getElementById('hide_float_left');

    if (content.style.display == "none") { content.style.display = "block"; hide.innerHTML = '<a href="javascript:hide_float_left()">Tắt quảng cáo [X]</a>'; }

    else {
        content.style.display = "none"; hide.innerHTML = '<a href="javascript:hide_float_left()">Xem quảng cáo...</a>';

    }

}