$(function () {

    /*-- Strict mode enabled --*/
    "use strict";

    //varialbles
    var _document = $(document),
        _window = $(window),
        _html = $('html'),
        _body = $('body'),
        _bannerCarousel = $('.banner-carousel');


    // Animate loader off screen
    $('.loader_wrapper').delay(500).fadeOut('slow');

    //window load funtion
    _window.on("load resize", function () {

        if ($('.measure-performance-banner').length > 0 && _window.width() > 1600) {
            _body.addClass('p-0');
        }

        //banner-carousel initialization
        _bannerCarousel.carousel({
            interval: 5000
        })

        //banner-carousel animation on-load and on-slide
        _bannerCarousel.find('.carousel-item').addClass('active-anim');

        _bannerCarousel.on('slide.bs.carousel', function () {
            $(this).find('.active').removeClass('active-anim');
            $(this).find('.carousel-item:not(.active)').addClass('active-anim');
        })

        // isotope initialization
        var $grid = $('.grid').imagesLoaded(function () {
            $grid.isotope({
                // set itemSelector so .grid-sizer is not used in layout
                itemSelector: '.grid-item',
                percentPosition: true,
                masonry: {
                    // use element for option
                    columnWidth: '.grid-sizer'
                }
            });

            // filter items on button click
            $('.filter-button-group').on('click', 'button', function () {
                var filterValue = $(this).attr('data-filter');
                $grid.isotope({
                    filter: filterValue
                });
            });

            $('.button-group').each(function (i, buttonGroup) {
                var $buttonGroup = $(buttonGroup);
                $buttonGroup.on('click', 'button', function () {
                    $buttonGroup.find('.is-checked').removeClass('is-checked');
                    $(this).addClass('is-checked');
                });
            });
        });

        //script for box equal height
        var maxHeight = 0;
        $(".equalHeight").each(function () {
            if ($(this).height() > maxHeight) {
                maxHeight = $(this).height();
            }
        });
        $(".equalHeight").height(maxHeight);
    });
    //End of window.load function

    //fullscreen menu
    var _fullNav = $(".fullscreen-menu");
    _document.on('click', '.navbar-toggler', function (e) {
        _fullNav.addClass("show-menu");
        _html.css("overflow", "hidden");
    });
    _document.on('click', '.close-menu', function (e) {
        _fullNav.removeClass("show-menu");
        _html.css("overflow", "auto");
    });

    //script for page scroll to top and bottom
    _document.on('click', '.page-scroll , .go-to-top', function () {
        if (location.pathname.replace(/^\//, '') === this.pathname.replace(/^\//, '') && location.hostname === this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html, body').animate({
                    scrollTop: target.offset().top - 80
                }, 1000, 'easeInOutExpo');

                return false;
            }
        }
    });
    //end script for page scroll to top and bottom

    //animated nav button
    $(".navbar").on('click', '.navbar-toggler-right', function () {
        $(this).toggleClass("change");
    });

    // Slick Slider js
    $('.services .carousel-inner').slick({
        dots: true,
        infinite: true,
        speed: 200,
        slidesToShow: 5,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        responsive: [{
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1,
                    infinite: true,
                    dots: true
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 575,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
            // You can unslick at a given breakpoint now by adding:
            // settings: "unslick"
            // instead of a settings object
        ]
    });

    $('.awards .carousel-inner').slick({
        dots: true,
        infinite: true,
        speed: 200,
        slidesToShow: 4,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        responsive: [{
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1,
                    infinite: true,
                    dots: true
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
            // You can unslick at a given breakpoint now by adding:
            // settings: "unslick"
            // instead of a settings object
        ]
    });

    $('.customer-review .carousel-inner').slick({
        dots: true,
        infinite: true,
        speed: 200,
        slidesToShow: 1,
        centerMode: true,
        centerPadding: '555px',
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 5000,
        responsive: [{
                breakpoint: 1600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: true,
                    centerPadding: '400px',
                }
            },
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: true,
                    centerPadding: '200px',
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    centerPadding: '100px',
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    centerPadding: '0',
                }
            },
            {
                breakpoint: 575,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    centerPadding: '0',
                }
            }
            // You can unslick at a given breakpoint now by adding:
            // settings: "unslick"
            // instead of a settings object
        ]
    });

    $('.quotes-from-team .carousel-inner').slick({
        dots: true,
        infinite: true,
        speed: 1000,
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 3000
    });

    //Closing menu after clicking on mobile
    var $menuCollapse = $('.collapse ul li a');
    $menuCollapse.on('click', function (e) {
        $('.collapse').removeClass('show');
        $('.navbar .container .navbar-toggler-right').removeClass("change");
    });

    //Typed js
    $(".typed").typed({
        strings: ["AC Repair Service in New York", "Video Tutorials in Java", "Garage Door Repair Service in Las Vegas"],
        stringsElement: null,
        typeSpeed: 15,
        startDelay: 15,
        backSpeed: 15,
        backDelay: 2000,
        loop: true,
        loopCount: true,
        showCursor: false,
        cursorChar: " ",
        attr: null,
        contentType: 'html',
        callback: function () {},
        preStringTyped: function (value) {
            var value = value + 1;
            $('.search-slider').find('img.img-' + value).removeClass('hide-img').addClass('show-img');
        },
        onStringTyped: function (value) {
            var value = value + 1;
            setTimeout(function () {
                $('.search-slider').find('img').removeClass('show-img').addClass('hide-img');
            }, 2000);

        },
        resetCallback: function () {}
    });

    //script for popup video modal 
    $('.video-popup').magnificPopup({
        items: {
            src: 'http://www.youtube.com/watch?v=7eo8XpT4CmM'
        },
        type: 'iframe',
        iframe: {
            markup: '<div class="mfp-iframe-scaler">' +
                '<div class="mfp-close">fssdfsdf</div>' +
                '<iframe class="mfp-iframe" frameborder="0" allowfullscreen></iframe>' +
                '</div>',
            patterns: {
                youtube: {
                    index: 'youtube.com/',
                    id: 'v=',
                    src: '//www.youtube.com/embed/%id%?autoplay=1'
                }
            },
            srcAction: 'iframe_src'
        },
        closeMarkup: '<button title="%title%" type="button" class="mfp-close">go back</button>',
        mainClass: 'mfp-fade'
    });

    //Script for magnific pop up 
    $('.magnify').magnificPopup({
        type: 'image',
        removalDelay: 300,
        mainClass: 'mfp-fade'
    });

    //User ratings
    function addScore(score, $domElement) {
        $("<span class='stars-container'>")
            .addClass("stars-" + score.toString())
            .text("★★★★★")
            .appendTo($domElement);
    }
    addScore(90, $(".fixture"));

    //Counter up js
    $('.counter').counterUp({
        delay: 10,
        time: 1000
    });

    //floating body on footer
    if (_window.width() > 767) {
        _window.on("load resize", function () {
            var footerHeight = $('.page-footer').height();
            $('body').css('margin-bottom', footerHeight);

            var _comp = $('.components .nav').height();
            $('.components .nav').css('margin-top', -_comp / 2);

            var mapNav = $('.location-map .tab-container.type-1 .nav-tabs').height();
            $(".location-map .tab-container.type-1 .nav-tabs").css('top', -mapNav / 2);
        }).trigger("resize");
    }

    //Tab nav
    var _navLink = $('.type-2');
    _navLink.on('click', '.nav-item a[data-toggle="tab"]', function (e) {
        var _curr = $(this).closest('li');

        _curr.prevAll().addClass("visited");
        _curr.removeClass("visited");
        _curr.nextAll().removeClass("visited");
    });

    //Bootstrap Carousel
    $('.carousel').carousel({
        interval: 2000
    })

    //Custom file input 
    var _a = $(".fileinputgroup");
    _a.on('change', '.form-control-file', function (e) {
        e.preventDefault();
        $(this).next('.fileLabel').text($(this)[0].files[0].name).css('background', 'url(images/icons/check-theme.png) left 3px no-repeat');
    });

    //Toggleable dropdown menu
    $('.fullscreen-menu .dropdown-toggle').on('click', function (e) {
        e.preventDefault();
        $(this).next('.dropdown-menu').slideToggle('1000');
    });

    //Trigger counter in button click
    var _navTab = $('.featured-projects');
    _navTab.on('click', '.nav-link', function () {
        $('.show .counter').counterUp();
    });

    // Call Gridder
    if ($('ul').hasClass('gridder')) {
        $(".gridder").gridderExpander({
            scrollOffset: 300,
            scrollTo: "panel", // "panel" or "listitem"
            animationSpeed: 400,
            animationEasing: "easeInOutExpo",
            onStart: function () {},
            onExpanded: function (object) {},
            onChanged: function (object) {},
            onClosed: function () {}
        });
    }

    //Global Form validation
    $('.contact-form').on('submit', function (e) {
        e.preventDefault();
        var _self = $(this);
        var __selector = _self.closest('input,textarea');
        _self.closest('div').find('input,textarea').removeAttr('style');
        _self.find('.err-msg').remove();
        _self.find('.form-success').removeClass('form-success');
        $('.submit-loading-img').css('display', 'block');
        _self.closest('div').find('button[type="submit"]').attr('disabled', 'disabled');
        var data = $(this).serialize();
        $.ajax({
            url: 'scripts/email.php',
            type: "post",
            dataType: 'json',
            data: data,
            success: function (data) {
                $('.submit-loading-img').css('display', 'none');
                _self.closest('div').find('button[type="submit"]').removeAttr('disabled');
                if (data.code == false) {
                    _self.closest('div').find('[name="' + data.field + '"]').addClass('form-success');
                    _self.closest('div').find('[name="' + data.field + '"]').after('<div class="err-msg">*' + data.err + '</div>');
                } else {
                    _self.find('.form-group').last().after('<div class="msg">' + data.success + '</div>');
                    _self.closest('div').find('input,textarea').val('');
                    _self.closest('div').find("input[type='checkbox']").prop('checked', false);

                }
            }
        });
    });

    //Apply for a job Form validation
    $('.apply-job-contact-form').on('submit', function (e) {
        e.preventDefault();
        var _self = $(this);
        var __selector = _self.closest('input,textarea');
        _self.closest('div').find('input,textarea').removeAttr('style');
        _self.find('.err-msg').remove();
        _self.find('.msg').remove();
        _self.find('.form-success').removeClass('form-success');
        $('.submit-loading-img').css('display', 'block');
        var formData = new FormData($(this)[0]);
        $.ajax({
            url: 'scripts/apply-job.php',
            type: "post",
            dataType: 'json',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                $('.submit-loading-img').css('display', 'none');
                _self.closest('div').find('button[type="submit"]').removeAttr('disabled');
                if (data.code == false) {
                    _self.closest('div').find('[name="' + data.field + '"]').addClass('form-success');
                    _self.closest('div').find('[name="' + data.field + '"]').after('<div class="err-msg">*' + data.err + '</div>');
                    $('#' + data.field).next().next().next().after('<div class="err-msg">*' + data.err + '</div>');
                } else {
                    _self.find('.form-group').last().after('<div class="msg">' + data.success + '</div>');
                    _self.closest('div').find('input,select,textarea').val('');
                    _self.closest('div').find("input[type='checkbox']").prop('checked', false);
                    $('.fileLabel').each(function () {
                        $(this).removeAttr('style');
                        $(this).html('Choose File');
                    });
                }
            }
        });
    });

    $("input").on('focus', function () {
        $('.msg').fadeOut();
    });

    //Go to top button
    _body.append('<a class="go-to-top" href="#top"><i class="fa fa-angle-up" aria-hidden="true"></i></a>');

    _window.on('scroll', function () {
        if (_window.scrollTop() >= 200) {
            $('.go-to-top').addClass('show');
        } else {
            $('.go-to-top').removeClass('show');
        }
    });



}()); /*ready*/