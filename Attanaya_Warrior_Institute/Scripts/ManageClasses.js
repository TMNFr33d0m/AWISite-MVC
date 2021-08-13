function pushDataToServer(url) {

    window.location.href = url +
        "?InstructorID=" +
        $("#InstructorID").val() +
        "&ClassTitle=" +
        $("#ClassTitle").val() +
        "&StartDateTime=" +
        $("#datepicker").val() +
        "&EndDateTime=" +
        $("#datepicker2").val() +
        "&Description=" +
        $("#Description").val() +
        "&MainRoom=" +
        $("#MainRoom").val() +
        "&SecondaryRoom=" +
        $("#SecondaryRoom").val() +
        "&CostPerStudent=" +
        $("#CostPerStudent").val() +
        "&MaxAttendees = " +
        $("#MaxAttendees").val() +
        "&ExternalBookingSource=" +
        $("#ExternalBookingSource").checked +
        "&ExternalBookingLink=" +
        $("#ExternalBookingLink").val();

}

function diff_hours(dt2, dt1) {

    var endDateTime = new Date(Date.parse(dt2));
    var startDateTime = new Date(Date.parse(dt1));

    var diff = (endDateTime.getTime() - startDateTime.getTime()) / 1000;
    diff /= (60 * 60);
    return Math.abs(Math.round(diff));

}

function validateForm() {

    var instructorID = $("#InstructorID").val();
    var classTitle = $("#ClassTitle").val();
    var StartDateTime = $("#datepicker").val();
    var EndDateTime = $("#datepicker2").val();
    var Description = $("#Description").val();
    var MainRoom = $("#MainRoom").val();
    var SecondaryRoom = $("#SecondaryRoom").val();
    var CostPerStudent = $("#CostPerStudent").val();
    var MaxAttendees = $("#MaxAttendees").val();
    var ExternalBookingSource = $("#ExternalBookingSource").checked;
    var passesValidation = true;
    var validationMessage = "";

    //validate the name
    if (classTitle.length < 3 ) {
        if (!$("#classTitle").hasClass("field-error-flash")) {
            toggleErrorFlash("#classTitle");
        }

        validationMessage = validationMessage + "The class name field does not appear to contain a valid name. Please enter a valid title for your class. ";
        passesValidation = false;

    } else {
        if ($("#classTitle").hasClass("field-error-flash")) {
            toggleErrorFlash("#classTitle");
        }
    }

    //validate the description
    if (Description.length < 100) {
        if (!$("#Description").hasClass("field-error-flash")) {
            toggleErrorFlash("#Description");
        }

        validationMessage = validationMessage + "The description field does not appear to contain a valid description. Please enter a description for your class. Your description must be longer than 100 characters. ";
        passesValidation = false;

    } else {
        if ($("#Description").hasClass("field-error-flash")) {
            toggleErrorFlash("#Description");
        }
    }

    //validate the cost per student
    if (CostPerStudent < 25) {

        if (!$("#CostPerStudent").hasClass("field-error-flash")) {
            toggleErrorFlash("#CostPerStudent");
        }

        validationMessage = validationMessage + "You cannot create a class with a per-student cost of less than $25.00 per person.";
        passesValidation = false;

    } else {
        if ($("#CostPerStudent").hasClass("field-error-flash")) {
            toggleErrorFlash("#CostPerStudent");
        }
    }


    //validate the cost per student
    if (MaxAttendees < 6) {
        if (!$("#MaxAttendees").hasClass("field-error-flash")) {
            toggleErrorFlash("#MaxAttendees");
        }

        validationMessage = validationMessage + "You cannot create a public class with a maximum of less than 6 students.";
        passesValidation = false;

    } else {
        if ($("#MaxAttendees").hasClass("field-error-flash")) {
            toggleErrorFlash("#MaxAttendees");
        }
    }

    //validate the duration
    if (diff_hours(StartDateTime, EndDateTime) < 1) {

        if (!$("#datepicker").hasClass("field-error-flash")) {
            toggleErrorFlash("#datepicker");
        }

        validationMessage = validationMessage + "The class must be at least 1 hour in length. ";
        passesValidation = false;

    } else {
        if ($("#datepicker").hasClass("field-error-flash")) {
            toggleErrorFlash("#datepicker");
        }
    }

    if (new Date(EndDateTime).getDay() !== new Date(StartDateTime).getDay()) {

        if (!$("#datepicker").hasClass("field-error-flash")) {
            toggleErrorFlash("#datepicker");
        }

        validationMessage = validationMessage + "The class must start and end on the same day. ";
        passesValidation = false;

    } else {
        if ($("#datepicker").hasClass("field-error-flash")) {
            toggleErrorFlash("#datepicker");
        }
    }

    $("#validation-message").text(validationMessage);
    return passesValidation;
}