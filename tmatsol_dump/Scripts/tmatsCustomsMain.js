/// <reference path="jquery-3.3.1.js" />

//screen spliter------------------------------------

//document.addEventListener('DOMContentLoaded', function () {
//    let wrapper = document.getElementById('wrapper');
//    let topLayer = wrapper.querySelector('.top');
//    let handle = wrapper.querySelector('.handle');
//    let skew = 0;
//    let delta = 0;

//    if (wrapper.className.indexOf('skewed') != -1) {
//        skew = 1000;
//    }

//    wrapper.addEventListener('mousemove', function (e) {
//        delta = (e.clientX - window.innerWidth / 2) * 0.5;

//        handle.style.left = e.clientX + delta + 'px';

//        topLayer.style.width = e.clientX + skew + delta + 'px';
//    });
//});
 

// Number counter----------------------------------------
$(document).ready(function () {
    var pageURL = $(location).attr("href");
    var a = "Index", b = "About", c = "contactus", d = "Services", e = "Updates",f="Products";
    if (pageURL != null)
    {
        if (pageURL.indexOf(a) != -1) {
            $('#topheader .navbar-nav').find('li.active-nav').removeClass('active-nav');
            $('#Index').parent('li').addClass('active-nav');
        }
        else if (pageURL.indexOf(b) != -1) {
            $('#topheader .navbar-nav').find('li.active-nav').removeClass('active-nav');
            $('#About').parent('li').addClass('active-nav');
        }
        else if (pageURL.indexOf(c) != -1) {
            $('#topheader .navbar-nav').find('li.active-nav').removeClass('active-nav');
            $('#contactus').parent('li').addClass('active-nav');
        }
        else if (pageURL.indexOf(e) != -1) {
            $('#topheader .navbar-nav').find('li.active-nav').removeClass('active-nav');
            $('#Updates').parent('li').addClass('active-nav');
        }
        else if (pageURL.indexOf(d) != -1) {
            $('#topheader .navbar-nav').find('li.active-nav').removeClass('active-nav');
            $('#Services').parent('li').addClass('active-nav');
        }
        else if (pageURL.indexOf(f) != -1) {
            $('#topheader .navbar-nav').find('li.active-nav').removeClass('active-nav');
            $('#Products').parent('li').addClass('active-nav');
        }
        else {
            $('#topheader .navbar-nav').find('li.active-nav').removeClass('active-nav');
            $('#Index').parent('li').addClass('active-nav');
        }
    }
});
    $('.count').each(function navChange(e) {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });

    //$('#topheader .navbar-nav a').on('click', function () {
    //    $('#topheader .navbar-nav').find('li.active-nav').removeClass('active-nav');
    //    $(this).parent('li').addClass('active-nav');
    //});