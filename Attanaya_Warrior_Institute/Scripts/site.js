"use strict";

function toggleVisibility(elementId) {
    if ($(elementId).hasClass("d-none")) {
        $(elementId).removeClass("d-none");
        $(elementId).addClass("d-block");
    } else {
        $(elementId).removeClass("d-block");
        $(elementId).addClass("d-none");
    }
}

function toggleErrorFlash(elementId) {
    if ($(elementId).hasClass("field-error-flash")) {
        $(elementId).removeClass("field-error-flash");

    } else {
        $(elementId).addClass("field-error-flash");
    }
}

// This will toggle a button's CSS to make it disabled or enabled. It also preventsDefault by default, so it prevents the button's onClick (or whatever) from firing. 
function toggleDisabledClass(elementId) {

    if ($(elementId).hasClass("disabled")) {
        $(elementId).removeClass("disabled");
    } else {
        $(elementId).addClass("disabled");
    }
}

function validateDateFormat(inputText) {

    // Set up some regex to determine if the date string looks correct. MM/DD/YYYY HH:MM:SSS (Time and spaces optional, dash and slash supported, 24 hour or AM/PM label supported)
    const dateFormat = /^\d{1,2}( )?[//-]( )?\d{1,2}( )?[//-]( )?\d{4}( )?(\d{0,2}:\d{0,2}(:\d{0,3})?)?( )?([AaPp][Mm])?$/;

    // Check if the text entered looks like a date we want.
    if (inputText.match(dateFormat)) {
        // Check to see if we can parse the provided value into a date object
        if (isNaN(Date.parse(inputText))) {
            return false; //If it doesn't parse, return false
        } else {
            return true; // Otherwise it's a valid date object that looks how we want it to look, return true. 
        }
    } else { // If not, return false 
        return false;
    }
}

function GenerateGuid() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}