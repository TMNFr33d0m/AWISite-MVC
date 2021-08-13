function createReceipt(url) {

window.location.href = url +
    "?EduClassId=" +
    $("#EduClassId").val() +
    "&StudentId=" +
    $("#StudentId").val() +
    "&FirstName=" +
    $("#firstName").text() +
    "&MiddleName=" +
    $("#middleName").text() +
    "&LastName=" +
    $("#lastName").text() +
    "&EmailAddress=" +
    $("#emailAddress").text() +
    "&PhoneNumber=" +
    $("#phoneNumber").text() +
    "&ClassTitle=" +
    encodeURIComponent($("#classTitle").text()) +
    "&StartDateTime=" +
    $("#classTimeStart").val() +
    "&EndDateTime=" +
    $("#classTimeEnd").val() +
    "&MaxAttendees=" +
    $("#maxAttendees").text() +
    "&TotalPrice=" +
        $("#TotalMoney").val()

}



