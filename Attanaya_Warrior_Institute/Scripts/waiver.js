function ValildateWaiver() {

    var ParticipantName = $("#ParticipantName").val();
    var ParticipantPhone = $("#ParticipantPhone").val();
    var ParticipantEmail = $("#ParticipantEmail").val();
    var EContactName = $("#EContactName").val();
    var EContactPhone = $("#EContactPhone").val();
    var EContactRelationship = $("#EContactRelationship").val();
    var SignatureDate = $("#SignatureDate").val();
    var ParentGuradian = $("#ParentGuradian").val();
    var ParentGuardianRelationship = $("#ParentGuardianRelationship").val();
    var passesValidation = true;
    var validationMessage = ""; 
    var phoneno = /^(\+[0-1]{1})?\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    //validate the name
    if (ParticipantName.length < 2) {
        if (!$("#ParticipantName").hasClass("field-error-flash")) {
            toggleErrorFlash("#ParticipantName");
        }

        validationMessage = validationMessage + "The name field does not appear to contain a valid name. Please enter a valid first and last name. ";
        passesValidation = false;

    } else {
        if ($("#ParticipantName").hasClass("field-error-flash")) {
            toggleErrorFlash("#ParticipantName");
        }
    }

    //validate the phone number

    if (!ParticipantPhone.match(phoneno)) {

        if (!$("#ParticipantPhone").hasClass("field-error-flash")) {
            toggleErrorFlash("#ParticipantPhone");
        }

        validationMessage = validationMessage + "The phone number field does not appear to contain a valid phone number. Please enter a valid phone number (format: XXX-XXX-XXXX). ";
        passesValidation = false;
    }
    else {
        if ($("#ParticipantPhone").hasClass("field-error-flash")) {
            toggleErrorFlash("#ParticipantPhone");
        }
    }

    // validate Email

    if (!ParticipantEmail.match(re)) {
        if (!$("#ParticipantEmail").hasClass("field-error-flash")) {
            toggleErrorFlash("#ParticipantEmail");
        }

        validationMessage = validationMessage + "The email field does not appear to contain a valid email address. Please enter a valid email address. We'll need it to send you important information regarding your reservation! ";
        passesValidation = false;
    }
    else {
        if ($("#ParticipantEmail").hasClass("field-error-flash")) {
            toggleErrorFlash("#ParticipantEmail");
        }
    }

    //validate the name
    if (EContactName.length < 2) {
        if (!$("#EContactName").hasClass("field-error-flash")) {
            toggleErrorFlash("#EContactName");
        }

        validationMessage = validationMessage + "The emergency contact name field does not appear to contain a valid name. Please enter a valid first and last name for your emergency contact. If you do not have an emergency contact, simply enter EMS in the emergency contact field. ";
        passesValidation = false;

    } else {
        if ($("#EContactName").hasClass("field-error-flash")) {
            toggleErrorFlash("#EContactName");
        }
    }

    //validate the phone number
    if (!EContactPhone.match(phoneno) && EContactPhone != '911') {

        if (!$("#EContactPhone").hasClass("field-error-flash")) {
            toggleErrorFlash("#EContactPhone");
        }

        validationMessage = validationMessage + "The emergency contact phone number field does not appear to contain a valid phone number. Please enter a valid phone number (format: XXX-XXX-XXXX). If you do not have an emergency contact, simply enter '911' in the emergency contact phone number field.  ";
        passesValidation = false;
    }
    else {
        if ($("#EContactPhone").hasClass("field-error-flash")) {
            toggleErrorFlash("#EContactPhone");
        }
    }

    //validate the emergency contact relationship
    if (EContactRelationship.length < 2) {
        if (!$("#EContactRelationship").hasClass("field-error-flash")) {
            toggleErrorFlash("#EContactRelationship");
        }

        validationMessage = validationMessage + "The emergency contact relationship field does not appear to contain a valid relationship. Please enter a valid relationship type for your emergency contact. If you do not have an emergency contact, simply enter MEDICAL in the emergency contact relationship field. ";
        passesValidation = false;

    } else {
        if ($("#EContactName").hasClass("field-error-flash")) {
            toggleErrorFlash("#EContactName");
        }
    }

    $("#validation-message").text(validationMessage);
    return passesValidation;
}