(function ($) {
 "use strict";
 
    // tooltip
     $('.ratings a').tooltip();
    $('[data-toggle="tooltip"]').tooltip();
/*----------------------------
 jQuery MeanMenu
------------------------------ */
	jQuery('nav#dropdown').meanmenu();	 
/*----------------------------
 wow js active
------------------------------ */
	new WOW().init();
 
/*----------------------------
 owl active
------------------------------ */  
  $(".active-slider").owlCarousel({
      autoPlay: false, 
	  slideSpeed:2000,
	  pagination:false,
	  navigation:true,	  
      items : 4,
	  /* transitionStyle : "fade", */    /* [This code for animation ] */
	  navigationText:["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
      itemsDesktop : [1199,4],
	  itemsDesktopSmall : [980,3],
	  itemsTablet: [768,2],
	  itemsMobile : [479,1],
  });
    
    /*----------------------------
 owl active
------------------------------ */  
  $(".single-slider").owlCarousel({
      autoPlay: false, 
	  slideSpeed:2000,
	  pagination:false,
	  navigation:true,	  
      items : 1,
	  /* transitionStyle : "fade", */    /* [This code for animation ] */
	  navigationText:["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
      itemsDesktop : [1199,1],
	  itemsDesktopSmall : [980,1],
	  itemsTablet: [768,1],
	  itemsMobile : [479,1],
  });
    
    
    /*----------------------------
 owl active
------------------------------ */  
  $(".active-slider3").owlCarousel({
      autoPlay: false, 
	  slideSpeed:2000,
	  pagination:false,
	  navigation:true,	  
      items : 3,
	  /* transitionStyle : "fade", */    /* [This code for animation ] */
	  navigationText:["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
      itemsDesktop : [1199,3],
	  itemsDesktopSmall : [980,2],
	  itemsTablet: [768,2],
	  itemsMobile : [479,1],
  });
    
    
        /*----------------------------
 owl active
------------------------------ */  
  $(".slider-active-five").owlCarousel({
      autoPlay: false, 
	  slideSpeed:2000,
	  pagination:false,
	  navigation:false,	  
      items : 3,
	  /* transitionStyle : "fade", */    /* [This code for animation ] */
      itemsDesktop : [1199,3],
	  itemsDesktopSmall : [980,3],
	  itemsTablet: [768,2],
	  itemsMobile : [479,1],
  });
    
 /*----------------------------
 owl active
------------------------------ */  
  $(".item_all").owlCarousel({
      autoPlay: false, 
	  slideSpeed:2000,
	  pagination:false,
	  navigation:false,	  
      items : 5,
	  /* transitionStyle : "fade", */    /* [This code for animation ] */
      itemsDesktop : [1199,4],
	  itemsDesktopSmall : [980,3],
	  itemsTablet: [768,2],
	  itemsMobile : [479,1],
  });
    
    
    /*----------------------------
 owl active
------------------------------ */  
  $(".slider7").owlCarousel({
      autoPlay: false, 
	  slideSpeed:2000,
	  pagination:false,
	  navigation:true,	  
      items : 1,
	  /* transitionStyle : "fade", */    /* [This code for animation ] */
	  navigationText:["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
      itemsDesktop : [1199,1],
	  itemsDesktopSmall : [980,1],
	  itemsTablet: [768,1],
	  itemsMobile : [479,1],
  });
       /*----------------------------
 owl active
------------------------------ */  
  $(".active-slider8").owlCarousel({
      autoPlay: false, 
	  slideSpeed:2000,
	  pagination:false,
	  navigation:true,	  
      items : 3,
	  /* transitionStyle : "fade", */    /* [This code for animation ] */
	  navigationText:["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
      itemsDesktop : [1199,3],
	  itemsDesktopSmall : [980,2],
	  itemsTablet: [768,2],
	  itemsMobile : [479,1],
  });
    
    
/*----------------------------
 owl active
------------------------------ */  
  $(".active-slider9").owlCarousel({
      autoPlay: false, 
	  slideSpeed:2000,
	  pagination:false,
	  navigation:true,	  
      items : 4,
	  /* transitionStyle : "fade", */    /* [This code for animation ] */
	  navigationText:["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
      itemsDesktop : [1199,3],
	  itemsDesktopSmall : [980,2],
	  itemsTablet: [768,2],
	  itemsMobile : [479,1],
  });
    
    
    
    
    /*--
	Pro Slider for Pro Details
------------------------*/
$(".pro-img-tab-slider").owlCarousel({
	items : 4,
	itemsDesktop : [1199,4],
	itemsDesktopSmall : [768,6],
	itemsTablet: [767,5],
	itemsMobile : [479,3],
	slideSpeed : 1500,
	paginationSpeed : 1500,
	rewindSpeed : 1500,
	navigation : true,
	navigationText : ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
	pagination : false,
	addClassActive: true,
});
    
    
/*---------------------
 price slider
--------------------- */  
	
    $( "#slider-range" ).slider({
        range: true,
        min: 40,
        max: 600,
        values: [ 60, 570 ],
        slide: function( event, ui ) {
        $( "#amount" ).val( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
        }
    });
    $( "#amount" ).val( "$" + $( "#slider-range" ).slider( "values", 0 ) +
    " - $" + $( "#slider-range" ).slider( "values", 1 ) );
    
    
    
    
    
     /*--
	Simple Lens and Lightbox
------------------------*/
	$('.simpleLens-lens-image').simpleLens({
		loading_image: 'img/loading.gif'
	}); 
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    


/*--------------------------
 scrollUp
---------------------------- */	
    $(window).on('scroll',function () {
        if($(window).scrollTop()>200) {
            $("#toTop").fadeIn();
        } else {
            $("#toTop").fadeOut();
        }
    });
    $("#toTop").on('click', function() {
        $("html,body").animate({
            scrollTop:0
        }, 500)
    });
    
    
    
    
    
    
    
    
    
 
    
    
    
  
 
    
    
    
 
})(jQuery); 