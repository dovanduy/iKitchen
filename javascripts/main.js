// MAIN.JS
//--------------------------------------------------------------------------------------------------------------------------------
//This is main JS file that contains custom JS scipts and initialization used in this template*/
// -------------------------------------------------------------------------------------------------------------------------------
// Template Name: IDENTITY.
// Author: Designova.
// Version 1.0 - Initial Release
// Website: http://www.designova.net 
// Copyright: (C) 2014
// -------------------------------------------------------------------------------------------------------------------------------

/*global $:false */
/*global window: false */

(function(){
  "use strict";


$(function ($) {

     //Detecting viewpot dimension
     var vH = $(window).height();

     //Adjusting Intro Components Spacing based on detected screen resolution
     //$('#intro').css('height',vH);
     $('.full-height,  body.pace-running').css('height',vH);
     $('.intro, #mastwrap').css('min-height',vH);


    //Vertical Centering of natural content spcific elements (non-images)
     $(function ($) {
              /*if your element is an image then please use $(window).load() instead tha above function wrap, because we want the coding to take
              effect when the image is loaded. */
              
              //get the width of the parent
              var parent_height = $('.vertical-center').parent().height();
              var image_height = $('.vertical-center').height();
              
              var top_margin = (parent_height - image_height)/2;
              
              //center it
              $('.vertical-center').css( 'padding-top' , top_margin);
              //uncomment the following if ithe element to be centered is an image
              $('.vertical-center-img').css( 'margin-top' , top_margin);
       });



    //Parallax Init
    $(window).stellar({
        responsive: true,
        horizontalScrolling: false,
        parallaxBackgrounds: true,
        parallaxElements: true,
        hideDistantElements: true
    });


   
     //TWITTER INIT (Updated with compatibility on Twitter's new API):
     //PLEASE READ DOCUMENTATION FOR INFO ABOUT SETTING UP YOUR OWN TWITTER CREDENTIALS:
      $(function ($) {
          $('#ticker').tweet({
              modpath: './twitter/',
              count: 1,
              loading_text: 'loading twitter update...',
              username:'designovastudio'
              /* etc... */
          });
        }); 


   //carousel hover effects
    $('#home-carousel .home-slide').mouseenter(function(){
       $('#home-carousel .home-slide').css('opacity',0.3);
       $(this).css('opacity',1);
    });
    $('#home-carousel .home-slide').mouseleave(function(){
       $('#home-carousel .home-slide').css('opacity',1);
    });


    //Carousels

      //home with carousel:
      $("#home-carousel").owlCarousel({
        navigation : false,
        pagination: false,
        responsive: true,
        items: 3,
        touchDrag: true,
        mouseDrag: true,
        itemsDesktop: [5000,3],
        itemsDesktopSmall: [1440,3],
        itemsTablet:[1024,2],
        itemsTabletSmall: [640,1],
        itemsMobile: [360,1],
        autoHeight: true,
        autoPlay: true
      });

      //home with carousel:
      $("#home-carousel-04").owlCarousel({
        navigation : false,
        pagination: false,
        responsive: true,
        items: 3,
        touchDrag: true,
        mouseDrag: true,
        itemsDesktop: [5000,3],
        itemsDesktopSmall: [1440,3],
        itemsTablet:[1024,2],
        itemsTabletSmall: [640,1],
        itemsMobile: [360,1],
        autoHeight: true,
        autoPlay: false
      });

       //home with carousel:
      $("#home-carousel-06").owlCarousel({
        navigation : false,
        pagination: false,
        responsive: true,
        items: 1,
        touchDrag: true,
        mouseDrag: true,
        itemsDesktop: [5000,2],
        itemsDesktopSmall: [1440,2],
        itemsTablet:[1024,2],
        itemsTabletSmall: [640,1],
        itemsMobile: [360,1],
        autoHeight: true,
        autoPlay: true
      });


       //home with carousel:
      $("#home-carousel-08").owlCarousel({
        navigation : false,
        pagination: false,
        responsive: true,
        items: 2,
        touchDrag: true,
        mouseDrag: true,
        itemsDesktop: [5000,2],
        itemsDesktopSmall: [1440,2],
        itemsTablet:[1024,2],
        itemsTabletSmall: [640,1],
        itemsMobile: [360,1],
        autoHeight: true,
        autoPlay: true
      });


      $(".testimonial-carousel").owlCarousel({
        navigation : true,
        pagination: false,
        responsive: true,
        items: 1,
        autoHeight: true,
        navigationText: ["&lt;","&gt;"],
        touchDrag: true,
        mouseDrag: true,
        itemsDesktop: [3000,1],
        itemsDesktopSmall: [1440,1],
        itemsTablet:[800,1],
        autoPlay: true
      });


    //Venobox Responsive Lightbox
    $('.venobox, .image-lightbox-link').venobox({
    numeratio: true
    }); 

    //MediaFolio Engine
    $('.mediafolio-thumb-wrap').mixitup();


    //Mediafolio Hide & Show Filter
    $('.mediafolio-filter li').click(function(){
      $('.mediafolio-filter li').removeClass('triggered');
      $(this).addClass('triggered');
    });

     //Portfolio Hide & Show Filter
    $('.portfolio-filter a').click(function(){
      $('.portfolio-filter a').parent().removeClass('triggered');
      $(this).parent().addClass('triggered');
      var filterIndex = $(this).data('filter');
      $('.folio-item').css('opacity',0.1);
      $(filterIndex).css('opacity',1);
    });


});
// $(function ($)  : ends

})();
//  JSHint wrapper $(function ($)  : ends







  

