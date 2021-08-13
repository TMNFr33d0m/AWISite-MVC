function GetPackageObject() {

    var dataVals = {
        'id': $("#SelectedPackageType").val()
    };

    $.ajax({
        type: "GET",
        url: "/Home/GetPackageFromId",
        data: dataVals,
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (result) {
        var object = JSON.parse(result);

        document.getElementById("PackageDescription").innerHTML = object.PackageDescription;
        document.getElementById("totalPrice").value = object.PackagePrice + ".00";
        $("#TotalPriceBox").text("$" + object.PackagePrice.toFixed(2) + "");

    }).fail(function (result) {
        console.log("Failed to get description.");
        console.log(result);

    });
}    

function ValidateGiftCertificateForm() {

    var PurchaserFirstName = $("#PurchaserFirstName").val();
    var PurchaserMiddleName = $("#PurchaserMiddleName").val();
    var PurchaserLastName = $("#PurchaserLastName").val();
    var PurchaserEmailAddress = $("#PurchaserEmailAddress").val();
    var PurchaserPhoneNumber = $("#PurchaserPhoneNumber").val();
    var RecipientName = $("#RecipientName").val();
    var RecipientPhone = $("#RecipientPhone").val();
    var RecipientEmail = $("#RecipientEmail").val();

    var passesValidation = true;
    var validationMessage = "";
    
    //validate the name
    if (PurchaserFirstName.length < 2) {
        if (!$("#PurchaserFirstName").hasClass("field-error-flash")) {
            toggleErrorFlash("#PurchaserFirstName");
        }

        validationMessage = validationMessage + "The purchaser first name field does not appear to contain a valid name. Please enter a valid first name. ";
        passesValidation = false;

    } else {
        if ($("#PurchaserFirstName").hasClass("field-error-flash")) {
            toggleErrorFlash("#PurchaserFirstName");
        }
    }

    if (PurchaserMiddleName.length < 1) {
        if (!$("#PurchaserMiddleName").hasClass("field-error-flash")) {
            toggleErrorFlash("#PurchaserMiddleName");
        }

        validationMessage = validationMessage + "The purchaser middle name field does not appear to contain a valid name. Please enter a valid middle name. ";
        passesValidation = false;

    } else {
        if ($("#PurchaserMiddleName").hasClass("field-error-flash")) {
            toggleErrorFlash("#PurchaserPurchaserMiddleNameFirstName");
        }
    }


    if (PurchaserLastName.length < 2) {
        if (!$("#PurchaserLastName").hasClass("field-error-flash")) {
            toggleErrorFlash("#PurchaserLastName");
        }

        validationMessage = validationMessage + "The purchaser last name field does not appear to contain a valid name. Please enter a valid last name. ";
        passesValidation = false;

    } else {
        if ($("#PurchaserLastName").hasClass("field-error-flash")) {
            toggleErrorFlash("#PurchaserLastName");
        }
    }

    //validate the name
    if (RecipientName.length < 2) {
        if (!$("#RecipientName").hasClass("field-error-flash")) {
            toggleErrorFlash("#RecipientName");
        }

        validationMessage = validationMessage + "The recipient name field does not appear to contain a valid name. Please enter a valid recipient name. ";
        passesValidation = false;

    } else {
        if ($("#RecipientName").hasClass("field-error-flash")) {
            toggleErrorFlash("#RecipientName");
        }
    }
    
    //validate the phone numbers
    var phoneno = /^(\+[0-1]{1})?\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    if (!PurchaserPhoneNumber.match(phoneno)) {

        if (!$("#PurchaserPhoneNumber").hasClass("field-error-flash")) {
            toggleErrorFlash("#PurchaserPhoneNumber");
        }

        validationMessage = validationMessage + "The purchaser phone number field does not appear to contain a valid phone number. Please enter a valid phone number (format: XXX-XXX-XXXX). ";
        passesValidation = false;
    }
    else {
        if ($("#PurchaserPhoneNumber").hasClass("field-error-flash")) {
            toggleErrorFlash("#PurchaserPhoneNumber");
        }
    }

    if (!RecipientPhone.match(phoneno)) {

        if (!$("#RecipientPhone").hasClass("field-error-flash")) {
            toggleErrorFlash("#RecipientPhone");
        }

        validationMessage = validationMessage + "The recipient phone number field does not appear to contain a valid phone number. Please enter a valid phone number (format: XXX-XXX-XXXX). ";
        passesValidation = false;
    }
    else {
        if ($("#RecipientPhone").hasClass("field-error-flash")) {
            toggleErrorFlash("#RecipientPhone");
        }
    }

    // validate Email
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (!PurchaserEmailAddress.match(re)) {
        if (!$("#PurchaserEmailAddress").hasClass("field-error-flash")) {
            toggleErrorFlash("#PurchaserEmailAddress");
        }

        validationMessage = validationMessage + "The purchaser email field does not appear to contain a valid email address. Please enter a valid email address.";
        passesValidation = false;
    }
    else {
        if ($("#PurchaserEmailAddress").hasClass("field-error-flash")) {
            toggleErrorFlash("#PurchaserEmailAddress");
        }
    }

    if (!RecipientEmail.match(re)) {
        if (!$("#RecipientEmail").hasClass("field-error-flash")) {
            toggleErrorFlash("#RecipientEmail");
        }

        validationMessage = validationMessage + "The recipient email field does not appear to contain a valid email address. Please enter a valid email address.";
        passesValidation = false;
    }
    else {
        if ($("#RecipientEmail").hasClass("field-error-flash")) {
            toggleErrorFlash("#RecipientEmail");
        }
    }

    $("#validation-message").text(validationMessage);
    return passesValidation;
}


function showPaymentOnValidation() {
    if (ValidateGiftCertificateForm()) {
        toggleVisibility('#paypal-button-container');
    } else {
        if ($('#paypal-button-container').hasClass("d-block")) {
            toggleVisibility('#paypal-button-container');
        }
    }
} 

function pushGiftCertDataToServer(url) {

    window.location.href = url +
        "?PackageType=" +
        $("#SelectedPackageType").val() +
        "&PurchaserFirstName=" +
        $("#PurchaserFirstName").val() +
        "&PurchaserMiddleName=" +
        $("#PurchaserMiddleName").val() +
        "&PurchaserLastName=" +
        $("#PurchaserLastName").val() +
        "&PurchaserEmailAddress=" +
        $("#PurchaserEmailAddress").val() +
        "&PurchaserPhoneNumber=" +
        $("#PurchaserPhoneNumber").val() +
        "&RecipientName=" +
        $("#RecipientName").val() +
        "&RecipientName=" +
        $("#RecipientName").val() +
        "&RecipientPhone= " +
        $("#RecipientPhone").val() +
        "&GiftCertificateNumber= " +
        $("#GiftCertificateNumber").val() +
        "&RecipientEmail=" +
        $("#RecipientEmail").val() +
        "&PackageDescription=" +
        $("#SelectedPackageType").val();
}