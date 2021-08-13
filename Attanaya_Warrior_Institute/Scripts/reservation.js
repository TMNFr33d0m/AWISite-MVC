var discountPercentage;

function updatePrice() {

    var discountCode = $("#DiscountCode").val();
    checkDiscountCode(discountCode);
    doTotalCostMath();
}

function checkDiscountCode(discountCode) {

    if (discountCode.length > 1) {

        $.ajax({
            type: "GET",
            url: "/Home/GetDiscount",
            data: `discountCode=${discountCode}`,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false
        }).done(function (payload) {
            discountPercentage = payload.DiscountPercentage;
            $("#discountIndication").text(payload.Description + ' - ' + discountPercentage + "% Discount Applied!");
        }).fail(function (payload) {
            console.log(payload);
        });

    } else {
        discountPercentage = 0;
    }
}


function getExperiencePrice(experienceId) {

    var res;

    $.ajax({
        type: "GET",
        url: "/Home/GetExperienceFromId",
        data: `id=${experienceId}`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false
    }).done(function (result) {
        var response = JSON.parse(result);
        res = response.ExperiencePrice;
    }).fail(function (data) {
        console.log(data);
    });

    return res;
}


function doTotalCostMath() {

    var duration = parseInt($("#ReservationDurationHrs").val());
    var partyCount = parseInt($("#ReservationGuestCount").val());
    var desiredExperience = $('input[name="DesiredExperience"]:checked').val();
 

    // Set default values if values are null
    if (isNaN(duration) || typeof duration === "undefined") {
        duration = 1;
    }

    if (isNaN(partyCount) || typeof partyCount === "undefined") {
        partyCount = 1;
        $("#ReservationGuestCount").val(partyCount);
    }

    if (isNaN(desiredExperience) || typeof desiredExperience === "undefined") {
        desiredExperience = 1;
        $("#DesiredExperience").val(desiredExperience);
    }

    let experiencePrice = getExperiencePrice(desiredExperience);
    let totalPrice = (experiencePrice + (partyCount * 10)) * duration;
    
    var totalPriceWithDiscount; 

    if (discountPercentage > 0) { 
        totalPriceWithDiscount = totalPrice * (100 - discountPercentage) / 100;
        
    } else {
     //   $("#discountIndication").text("No Discount Applied");
        totalPriceWithDiscount = totalPrice;
    }
    
    $("#TotalMoney").val(totalPriceWithDiscount.toFixed(2));
    $("#totalValueDisplay").text("Total: $" + totalPriceWithDiscount.toFixed(2) + "");
}

function doTotalCostMathForMembers(MaxAllowedPerSession) {

    var duration = parseInt($("#ReservationDurationHrs").val());
    var partyCount = parseInt($("#ReservationGuestCount").val());
    var desiredExperience = $('input[name="DesiredExperience"]:checked').val();


    // Set default values if values are null
    if (isNaN(duration) || typeof duration === "undefined") {
        duration = 1;
    }

    if (isNaN(partyCount) || typeof partyCount === "undefined") {
        partyCount = 1;
        $("#ReservationGuestCount").val(partyCount);
    }

    if (isNaN(desiredExperience) || typeof desiredExperience === "undefined") {
        desiredExperience = 1;
        $("#DesiredExperience").val(desiredExperience);
    }
    let experiencePrice = getExperiencePrice(desiredExperience);
    let totalPrice;

    if (duration > 1) {
        totalPrice = (experiencePrice + (partyCount * 10)) * (duration - 1);
    } else {
        totalPrice = ((partyCount - MaxAllowedPerSession) * 10) * duration;
    }

    $("#TotalMoney").val(totalPrice.toFixed(2));
    $("#totalValueDisplay").text("Total: $" + totalPrice.toFixed(2) + "");

    if (totalPrice > 0) {

        if ($('#MemberSessionPaidMessage').hasClass("d-block")) {
            toggleVisibility('#MemberSessionPaidMessage');
        }

        if ($('#MemberAdditionalPaymentArea').hasClass("d-none")) {
            toggleVisibility('#MemberAdditionalPaymentArea');
        }
    } else {
        if ($('#MemberSessionPaidMessage').hasClass("d-none")) {
            toggleVisibility('#MemberSessionPaidMessage');
        }

        if ($('#MemberAdditionalPaymentArea').hasClass("d-block")) {
            toggleVisibility('#MemberAdditionalPaymentArea');
        }
    }
}


function pushDataToServer(url) {
    var desiredExperienceValue = $('input[name="DesiredExperience"]:checked').val();

    window.location.href = url +
        "?ReservationName=" +
        $("#ReservationName").val() +
        "&ReservationPhone=" +
        $("#ReservationPhone").val() +
        "&ReservationEmail=" +
        $("#ReservationEmail").val() +
        "&ReservationGuestCount=" + 
        $("#ReservationGuestCount").val() +
        "&ReservationDate=" +
        $("#datepicker").val()+
        "&SelectedReservationTime=" +
        $("#SelectedReservationTime").val() +
        "&ReservationDurationHrs=" +
        $("#ReservationDurationHrs").val() +
        "&SelectedReservationRoom=" +
        $("#SelectedReservationRoom").val() +
        "&desiredExperience=" + 
        desiredExperienceValue +
        "&discountCode=" +
        $("#DiscountCode").val() +
        "&totalMoney=" +
        $("#TotalMoney").val();

}

function validateForm() {

    var reservationName = $("#ReservationName").val();
    var reservationPhone = $("#ReservationPhone").val();
    var reservationEmail = $("#ReservationEmail").val();
    var reservationQty = $("#ReservationGuestCount").val();
    var bookingDate = $("#datepicker").val();
    var appointmentTime = $("#SelectedReservationTime").val();
    var ReservationDurationHrs = $("#ReservationDurationHrs").val();
    var selectedAppointmentRoom = $("#SelectedReservationRoom").val();
    var discountCode = $("#DiscountCode").val;
    var totalMoney = $("#TotalMoney").val();
    var passesValidation = true;
    var validationMessage = ""; 

    //validate the name
    if (reservationName.length < 2) {
        if (!$("#ReservationName").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationName");
        }

        validationMessage = validationMessage + "The name field does not appear to contain a valid name. Please enter a valid first and last name. ";
        passesValidation = false;

    } else {
        if ($("#ReservationName").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationName");
        }
    }

    //validate the phone number
    var phoneno = /^(\+[0-1]{1})?\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    if (!reservationPhone.match(phoneno)) {

        if (!$("#ReservationPhone").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationPhone");
        }

        validationMessage = validationMessage + "The phone number field does not appear to contain a valid phone number. Please enter a valid phone number (format: XXX-XXX-XXXX). ";
        passesValidation = false;
    }
    else {
        if ($("#ReservationPhone").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationPhone");
        }
    }

    // validate Email
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!reservationEmail.match(re)) {
        if (!$("#ReservationEmail").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationEmail");
        }

        validationMessage = validationMessage +"The email field does not appear to contain a valid email address. Please enter a valid email address. We'll need it to send you important information regarding your reservation! ";
        passesValidation = false;
    }
    else {
        if ($("#ReservationEmail").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationEmail");
        }
    }

    var maxQtyAttr = $("#ReservationGuestCount").attr("max");
    //validate the quantity of participants
    if (parseInt(reservationQty) > parseInt(maxQtyAttr)) {
        if (!$("#ReservationGuestCount").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationGuestCount");
        }

        validationMessage = validationMessage + "The number of participants cannot exceed four (4). ";
        document.getElementById("ReservationGuestCount").value = parseInt(maxQtyAttr);

        passesValidation = false;

    } else {
        if ($("#ReservationGuestCount").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationGuestCount");
        }
    }

    //validate the date
    if (!validateDateFormat(bookingDate)) {
        if (!$("#datepicker").hasClass("field-error-flash")) {
            toggleErrorFlash("#datepicker");
        }

        validationMessage = validationMessage + "The Booking Date field does not appear to contain a valid date. Please select a valid date for your reservation. ";
        passesValidation = false;

    } else {
        if ($("#datepicker").hasClass("field-error-flash")) {
            toggleErrorFlash("#datepicker");
        }
    }

    //validate the time for closed
    if (appointmentTime.includes("CLOSED")) {
        if (!$("#SelectedReservationRoom").hasClass("field-error-flash")) {
            toggleErrorFlash("#SelectedReservationRoom");
        }

        validationMessage = "The facility is closed at this time. Please select a valid date for your reservation. ";
        passesValidation = false;

        $("#validation-message").text(validationMessage);
        return passesValidation;

    } else {
        if ($("#SelectedReservationRoom").hasClass("field-error-flash")) {
            toggleErrorFlash("#datepicker");
        }
    }

    //validate the time
    if (appointmentTime.length < 2) {
        if (!$("#SelectedReservationRoom").hasClass("field-error-flash")) {
            toggleErrorFlash("#SelectedReservationRoom");
        }

        validationMessage = validationMessage + "The appointment Time field does not appear to contain a valid time. Please select a valid time for your reservation. ";
        passesValidation = false;

    } else {
        if ($("#SelectedReservationRoom").hasClass("field-error-flash")) {
            toggleErrorFlash("#SelectedReservationRoom");
        }
    }

    //validate the duration
    if (ReservationDurationHrs <= 0) {
        if (!$("#ReservationDurationHrs").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationDurationHrs");
        }

        validationMessage = validationMessage + "The reservation must be at least 1 hour in length. ";
        passesValidation = false;

    } else {
        if ($("#ReservationDurationHrs").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationDurationHrs");
        }
    }
    
    var currentHours = $("#ReservationDurationHrs").attr("max");

    //validate the duration value
    if (parseInt(ReservationDurationHrs) > parseInt(currentHours)) {
        if (!$("#ReservationDurationHrs").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationDurationHrs");
        }

        validationMessage = validationMessage + "The reservation's requested duration exceeds the amount of available hours remaining in the day. ";
        document.getElementById("ReservationDurationHrs").value = currentHours;
        updatePrice();
        passesValidation = false;

    } else {
        if ($("#ReservationDurationHrs").hasClass("field-error-flash")) {
            toggleErrorFlash("#ReservationDurationHrs");
        }
    }
    
    //validate the room
    if (selectedAppointmentRoom.length != 1) {
        if (!$("#SelectedReservationRoom").hasClass("field-error-flash")) {
            toggleErrorFlash("#SelectedReservationRoom");
        }

        validationMessage = validationMessage + "The reservation must be in a room. ";
        passesValidation = false;

    } else {
        if ($("#SelectedReservationRoom").hasClass("field-error-flash")) {
            toggleErrorFlash("#SelectedReservationRoom");
        }
    }

    $("#validation-message").text(validationMessage);
    return passesValidation;
}

function updateMaxHours() {

    var dataVals = {
        'selectedStartTime': $("#SelectedReservationTime").val(),
        'selectedDate': $("#datepicker").val(),
        'simulationRoom': $("#SelectedReservationRoom").val()
    };

    $.ajax({
        type: "GET",
        url: "/Home/GetMaxHoursFromSelectedTime",
        data: dataVals,
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (result) {

        document.getElementById("ReservationDurationHrs").value = "1";
        document.getElementById("ReservationDurationHrs").max = result;

    }).fail(function (result) {
        console.log("Failed to get max hours.");
        console.log(result);

    });
}
