// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $(".wrapper").css("margin-top", ($(window).height()) / 5);

    var dt = new Date()
    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    $('#day').html(days[dt.getDay()]);
    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
    $('#date').html(months[dt.getMonth()] + " " + dt.getDate() + ", " + dt.getFullYear());
    $('#time').html((dt.getHours() > 12 ? (dt.getHours() - 12) : dt.getHours()).toString() + ":" + ((dt.getMinutes() < 10 ? '0' : '').toString() + dt.getMinutes().toString()) + (dt.getHours() < 12 ? ' AM' : ' PM').toString());

    var temp = 0;
    $('#fahrenheit').click(function () {
        $(this).css("color", "white");
        $('#celsius').css("color", "#b0bec5");
        //$('#temperature').html(Math.round(temp * 1.8 + 32));
    });

    $('#celsius').click(function () {
        $(this).css("color", "white");
        $('#fahrenheit').css("color", "#b0bec5");
        //$('#temperature').html(Math.round(temp));
    });

});