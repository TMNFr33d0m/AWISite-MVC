using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using static System.DateTime;

namespace Attanaya_Warrior_Institute.Models
{
    public class Reservation
    {

        private const string @break = "<br /> <br />";

        public Guid ID { get; set; }

        #region
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        [DisplayName("Reservation Name:")]
        public string ReservationName { get; set; }

        [Required]
        [Phone]
        [DisplayName("Reservation Phone:")]
        public string ReservationPhone { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Reservation Email:")]
        public string ReservationEmail { get; set; }

        [Required]
        [DisplayName("Number In Party:")]
        public int ReservationGuestCount { get; set; }

        [Required]
        [DisplayName("Reservation Date:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string ReservationDate { get; set; }

        [Required]
        [DisplayName("Reservation Time:")]
        public TimeSpan SelectedReservationTime { get; set; }

        [Required]
        [DisplayName("Duration (Number of Hours):")]
        public int ReservationDurationHrs { get; set; }

        [Required]
        [DisplayName("Simulation Room:")]
        public int SelectedReservationRoom { get; set; }

        [Required]
        [DisplayName("Desired Experience:")]
        public int DesiredExperience { get; set; }

        [DisplayName("Discount Code:")]
        public string DiscountCode { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public string TotalMoney { get; set; }

        public Guid AccountID { get; set; }
        #endregion

        /// <summary>
        /// Get the company email address from the config file. 
        /// </summary>
        protected string CompanyCCEmailAddress = ConfigurationManager.AppSettings["adminEmail"];

        /// <summary>
        /// Gets a reservation object by it's reservation ID. Returns a single reservation object.
        /// </summary>
        /// <param name="ReservationID"></param>
        /// <returns></returns>
        public static Reservation GetReservationByReservationId(Guid ReservationID)
        {
            Reservation res = new Reservation();

            string queryString = "SELECT * FROM dbo.Reservations WHERE ID = '" + ReservationID + "';";

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.ID = (Guid)reader["ID"];
                            res.ReservationName = (string)reader["ReservationName"];
                            res.ReservationPhone = (string)reader["ReservationPhone"];
                            res.ReservationEmail = (string)reader["ReservationEmail"];
                            res.ReservationGuestCount = (int)reader["ReservationGuestCount"];
                            res.ReservationDate = Convert.ToString((DateTime)reader["ReservationDate"], CultureInfo.InvariantCulture);
                            res.SelectedReservationTime = (TimeSpan)reader["ReservationStartTime"];
                            res.ReservationDurationHrs = (int)reader["ReservationDurationHrs"];
                            res.SelectedReservationRoom = (int)reader["ReservationRoom"];
                            res.DesiredExperience = (int)reader["DesiredExperience"];
                            res.DiscountCode = (string)reader["DiscountCode"];
                            res.TotalMoney = Convert.ToString((decimal)reader["TotalPrice"], CultureInfo.InvariantCulture);
                            res.AccountID = (Guid)reader["AccountID"];

                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetReservationByReservationId));
                    throw;
                }
            }

            return res;
        }

        /// <summary>
        /// Gets all reservations for an accountID, returns a list of Reservations objects.
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public static List<Reservation> GetReservationsForAccountId(Guid AccountId)
        {
            var reservationList = new List<Reservation>();

            string queryString = "SELECT * FROM dbo.Reservations WHERE AccountID = '" + AccountId + "';";

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reservation res = new Reservation();

                            res.ID = (Guid)reader["ID"];
                            res.ReservationName = (string)reader["ReservationName"];
                            res.ReservationPhone = (string)reader["ReservationPhone"];
                            res.ReservationEmail = (string)reader["ReservationEmail"];
                            res.ReservationGuestCount = (int)reader["ReservationGuestCount"];
                            res.ReservationDate = Convert.ToString((DateTime)reader["ReservationDate"], CultureInfo.InvariantCulture);
                            res.SelectedReservationTime = (TimeSpan)reader["ReservationStartTime"];
                            res.ReservationDurationHrs = (int)reader["ReservationDurationHrs"];
                            res.SelectedReservationRoom = (int)reader["ReservationRoom"];
                            res.DesiredExperience = (int)reader["DesiredExperience"];
                            res.DiscountCode = (string)reader["DiscountCode"];
                            res.TotalMoney = Convert.ToString((decimal)reader["TotalPrice"], CultureInfo.InvariantCulture);
                            res.AccountID = (Guid)reader["AccountID"];

                            reservationList.Add(res);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetReservationsForAccountId));
                    throw;
                }
            }

            return reservationList;
        }

        /// <summary>
        /// Helps the rooms dropdown know which rooms to populate with.  Returns a dictionary object with an integer RoomID as the key and the string of it's name as the value. 
        /// </summary>
        /// <param name="showPrivateRooms">Dictated wether private rooms that arent meant for public consumption are included in the list.</param>
        /// <param name="DesiredExperience">The desired experience may only be available in certain rooms, so we pass it in. If desired experience = 0, returns them all, ordered by ID</param>
        /// <returns></returns>
        public Dictionary<int, string> SelectedReservationRoomHelper(bool showPrivateRooms, int DesiredExperience)
        {
            Experience DesExp = new Experience();

            // If no experience was selected (0) then we get all of the experiences, order them by the ID, and then grab the first one in the list. 
            if (DesiredExperience == 0)
            {
                var experienceList = Experience.GetAllExperiences();
                DesiredExperience = experienceList.OrderBy(r => r.ExperienceID).First().ExperienceID;
            }

            DesExp = Experience.GetExperienceByID(DesiredExperience);
            List<Rooms> LocalEquippedRoomList = new List<Rooms>();

            if (showPrivateRooms)
            {
                LocalEquippedRoomList = Rooms.GetAvailableRooms().ToList();
            }
            else
            {
                List<int> EquippedRoomList = new List<int>();

                if (DesExp.EquippedRoomIdArray.Length == 1)
                {
                    EquippedRoomList.Add(int.Parse(DesExp.EquippedRoomIdArray, CultureInfo.InvariantCulture));
                }

                if (DesExp.EquippedRoomIdArray.Length > 1)
                {
                    var stringArray = DesExp.EquippedRoomIdArray.Split(',');

                    for (int i = 0; i < stringArray.Count(); i++)
                    {
                        EquippedRoomList.Add(int.Parse(stringArray[i], CultureInfo.InvariantCulture));
                    }
                }

                if (EquippedRoomList.Count >= 1)
                {
                    List<Rooms> localCopyOfRoomList = Rooms.GetAvailableRoomsInList();

                    foreach (var RoomIDFromExperienceTable in EquippedRoomList)
                    {
                        Rooms equippedRoom = localCopyOfRoomList.Where(r => r.ID.Equals(RoomIDFromExperienceTable)).SingleOrDefault();
                        LocalEquippedRoomList.Add(equippedRoom);
                    }
                }
                else
                {
                    LocalEquippedRoomList = Rooms.GetAvailableRooms().Where(r => r.IsPublic == 1).ToList();
                }

            }

            IEnumerable<Rooms> records = LocalEquippedRoomList;
            return records.ToDictionary(record => record.ID, record => record.Name);
        }

        /// <summary>
        /// Helps the times dropsdown know which times to populate with. Returns a Dictionary of two strings, separated by a static hyphen. 
        /// </summary>
        /// <param name="selectedDay"></param>
        /// <param name="simulationRoomID"></param>
        /// <returns></returns>
        public Dictionary<string, string> SelectedReservationTimeHelper(string selectedDay, int simulationRoomID)
        {
            if (selectedDay == MinValue.ToString(CultureInfo.CurrentCulture) || string.IsNullOrWhiteSpace(selectedDay))
            {
                selectedDay = Utility.GetCurrentTucsonTime().Date.ToString(CultureInfo.CurrentCulture);
            }

            DateTime selectedDayDateTime = Parse(selectedDay, CultureInfo.CurrentCulture);
            IEnumerable<AvailableTimeSlots> records = GetAvailableTimes(selectedDayDateTime, simulationRoomID);
            Dictionary<string, string> timeDic = new Dictionary<string, string>();

            foreach (var record in records)
            {
                timeDic.Add(record.StartTimeSlot.ToString(CultureInfo.CurrentCulture), record.StartTimeSlot.ToString(CultureInfo.CurrentCulture) + " - " + record.EndTimeSlot);
            }

            return timeDic;

        }

        /// <summary>
        /// Gets available times and returns them in an Ienumerable. 
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <param name="simulationRoomID"></param>
        /// <returns></returns>
        public static IEnumerable<AvailableTimeSlots> GetAvailableTimes(DateTime selectedDate, int simulationRoomID)
        {
            List<HoursOfOperation> hoursOfOperation = HoursOfOperation.GetHoursOfOperation();
            List<BookingStub> bookedTimes = BookingStub.GetStubbedReservationsByRoomId(selectedDate, simulationRoomID);
            List<AvailableTimeSlots> availableTimes = new List<AvailableTimeSlots>();
            var currentTucsonDateTime = Utility.GetCurrentTucsonTime();

            if (Utility.IsClosedOnDay(selectedDate))
            {
                AvailableTimeSlots closedSlot = new AvailableTimeSlots
                {
                    StartTimeSlot = "CLOSED",
                    EndTimeSlot = "TODAY"
                };

                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User has been shown the CLOSED TODAY (IsClosedOnDay = true) message. Selected Date: {0}, ", selectedDate), nameof(GetAvailableTimes));
                availableTimes.Add(closedSlot);

                return availableTimes;
            }

            // Get list of classes for the selected day, convert to BookingStub and append to bookedTimes. 
            try
            {
                using (var connection = new SqlConnection(Utility.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetClassBookingsByRoomID", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@queryDateString", SqlDbType.VarChar).Value = selectedDate.Equals(MinValue)
                            ? currentTucsonDateTime.Date.ToShortDateString()
                            : selectedDate.ToShortDateString();

                        cmd.Parameters.Add("@roomID", SqlDbType.VarChar).Value = simulationRoomID;
                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookingStub Reservation = new BookingStub
                                {
                                    ReservationTime = (TimeSpan)reader["StartTime"],
                                    ReservationEndTime = (TimeSpan)reader["EndTime"],
                                    SelectedReservationRoom = (int)reader["MainRoom"]
                                };

                                Reservation.ReservationDurationHrs =
                                    (int)(Reservation.ReservationEndTime - Reservation.ReservationTime).TotalHours;

                                bookedTimes.Add(Reservation);

                                if (reader["SecondaryRoom"] != DBNull.Value)
                                {
                                    BookingStub Reservation2 = new BookingStub
                                    {
                                        ReservationTime = (TimeSpan)reader["StartTime"],
                                        ReservationEndTime = (TimeSpan)reader["EndTime"],
                                        SelectedReservationRoom = (int)reader["SecondaryRoom"]
                                    };

                                    Reservation2.ReservationDurationHrs =
                                        (int)(Reservation2.ReservationEndTime - Reservation2.ReservationTime)
                                        .TotalHours;

                                    bookedTimes.Add(Reservation2);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(GetAvailableTimes));
                throw;
            }

            // Grab the hours of operation for the selected day
            var todaysHours = hoursOfOperation.Where(r => r.DayOfWeek.Equals(selectedDate.Equals(MinValue) ? currentTucsonDateTime.DayOfWeek.ToString() : selectedDate.DayOfWeek.ToString()))
                .Select(r => new { r.DayOfWeek, r.OpenTime, r.CloseTime }).First();

            // Get a count of how many hours we're open that day, accounting for the day change at midnight
            double openHours;
            if (todaysHours.OpenTime > todaysHours.CloseTime && todaysHours.CloseTime.Hours < 12)
            {
                var openDateTime = selectedDate.Date + todaysHours.OpenTime;
                var closeDateTime = selectedDate.AddDays(1).Date + todaysHours.CloseTime;
                var openHoursNumber = closeDateTime - openDateTime;
                openHours = openHoursNumber.TotalHours;
            }
            else
            {
                openHours = (todaysHours.CloseTime - todaysHours.OpenTime).TotalHours;
            }

            // As long as we're open during the selected timespan, do the below...
            if (openHours > 1)
            {
                for (int i = 0; i <= openHours - 1; i++)
                {
                    var timeSlotBeingCheckedStart = todaysHours.OpenTime.Add(TimeSpan.FromHours(i));
                    var timeSlotBeingCheckedEnd = timeSlotBeingCheckedStart.Add(TimeSpan.FromHours(1));

                    if (!bookedTimes.Any(
                        t => t.ReservationTime.Equals(timeSlotBeingCheckedStart) // if a reservation time from the list is exactly equal to the time slot being checked.
                        || timeSlotBeingCheckedStart >= t.ReservationTime  // OR if the time slot being checked's beginning time is greater than or equal to the reservation time
                        && timeSlotBeingCheckedStart <= (t.ReservationEndTime < TimeSpan.FromHours(6) ? t.ReservationEndTime.Add(TimeSpan.FromDays(1)) : t.ReservationEndTime))) // AND the time slot being checked's beginning time is less than or equal to ( if a rervation end time is less than 06:00, add a day to the reservation's end date to account for 24 hour open, otherwise: ) the reservation end time. 
                    {

                        AvailableTimeSlots
                            unbookedTimeSlot = new AvailableTimeSlots();
                        unbookedTimeSlot.StartTimeSlot = timeSlotBeingCheckedStart > TimeSpan.FromDays(1)
                            ? (timeSlotBeingCheckedStart - TimeSpan.FromDays(1)).ToString()
                            : timeSlotBeingCheckedStart.ToString();
                        unbookedTimeSlot.EndTimeSlot =
                            timeSlotBeingCheckedEnd > TimeSpan.FromDays(1)
                                ? (timeSlotBeingCheckedEnd - TimeSpan.FromDays(1)).ToString()
                                : timeSlotBeingCheckedEnd.ToString();

                        if (unbookedTimeSlot.StartTimeSlot == unbookedTimeSlot.EndTimeSlot)
                        {
                            unbookedTimeSlot.EndTimeSlot = "0:00:00";
                        }

                        if (selectedDate.Date == currentTucsonDateTime.Date)
                        {
                            if (timeSlotBeingCheckedStart.Hours > currentTucsonDateTime.Hour
                                || timeSlotBeingCheckedStart.Hours
                                < currentTucsonDateTime.Hour
                                && currentTucsonDateTime.Hour <= 23
                                && timeSlotBeingCheckedStart.Hours < 6
                                )
                            {
                                availableTimes.Add(unbookedTimeSlot);
                            }
                        }
                        else
                        {
                            availableTimes.Add(unbookedTimeSlot);
                        }
                    }
                }
            }
            else // Otherwise show that we're closed during the selected time.
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User has been shown the CLOSED TODAY message. Selected Date: {0}, ", selectedDate), nameof(GetAvailableTimes));
                availableTimes.Add(new AvailableTimeSlots { StartTimeSlot = "CLOSED", EndTimeSlot = "TODAY" });
            }

            // If there were no available times, show sold out, and log it. 
            if (availableTimes.Count == 0)
            {
                AvailableTimeSlots closedSlot = new AvailableTimeSlots
                {
                    StartTimeSlot = "CLOSED",
                    EndTimeSlot = "SOLD OUT"
                };

                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User has been shown the CLOSED SOLD OUT message. Selected Date: {0}, ", selectedDate), nameof(GetAvailableTimes));
                availableTimes.Add(closedSlot);
            }

            return availableTimes;
        }

        /// <summary>
        /// Save sa booking record to the database. Accepts a Reservation object. 
        /// </summary>
        /// <param name="booking"></param>
        public void SaveBooking(Reservation booking)
        {

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                if (booking?.ReservationDate != null)
                {

                    TimeSpan selectedReservationTimeDayCompensator;

                    if (booking.SelectedReservationTime.Add(TimeSpan.FromHours(booking.ReservationDurationHrs)) >
                        TimeSpan.FromDays(1))
                    {
                        selectedReservationTimeDayCompensator = booking.SelectedReservationTime.Add(TimeSpan.FromHours(booking.ReservationDurationHrs)) - TimeSpan.FromDays(1);

                    }
                    else
                    {
                        selectedReservationTimeDayCompensator =
                            booking.SelectedReservationTime.Add(TimeSpan.FromHours(booking.ReservationDurationHrs));
                    }

                    string queryString1 = "INSERT INTO dbo.Reservations (" +
                                          "[ReservationName], " +
                                          "[ReservationPhone], " +
                                          "[ReservationEmail], " +
                                          "[ReservationGuestCount], " +
                                          "[ReservationDate], " +
                                          "[ReservationStartTime], " +
                                          "[ReservationEndTime], " +
                                          "[ReservationDurationHrs], " +
                                          "[ReservationRoom], " +
                                          "[DesiredExperience], " +
                                          "[DiscountCode], " +
                                          "[TotalPrice], " +
                                          "[AccountID] " +
                                          ") VALUES('" +
                                          booking.ReservationName + "','" +
                                          booking.ReservationPhone + "','" +
                                          booking.ReservationEmail + "','" +
                                          booking.ReservationGuestCount + "','" +
                                          Parse(booking.ReservationDate, CultureInfo.CurrentCulture) + "','" +
                                          booking.SelectedReservationTime + "','" +
                                          selectedReservationTimeDayCompensator + "','" +
                                          booking.ReservationDurationHrs + "','" +
                                          booking.SelectedReservationRoom + "','" +
                                          booking.DesiredExperience + "','" +
                                          booking.DiscountCode + "','" +
                                          booking.TotalMoney + "','" +
                                          booking.AccountID + "'"
                                          + ")";

                    var command = new SqlCommand(queryString1, connection);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Utility.UpdateDiscountCodeCount(booking.DiscountCode);
                        LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                            "User {0} ({1}) has booked a reservation for {2}, {3} hours long, in room {4}.",
                            booking.ReservationName,
                            booking.ReservationEmail,
                            booking.ReservationDate,
                            booking.ReservationDurationHrs,
                            booking.SelectedReservationRoom), nameof(SaveBooking));

                    }
                    catch (Exception e)
                    {
                        LoggingModel.LogCriticalException(e, nameof(SaveBooking));
                        throw;
                    }

                }
            }
        }

        /// <summary>
        /// Sends a confirmation email to the purchaser. Accepts a Reservation object. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<HttpStatusCode> SendConfirmationEmail(Reservation model)
        {

            const string subject = "Your Reservation Is Confirmed";

            if (model != null)
            {
                IEnumerable<Rooms> rooms = Rooms.GetAvailableRooms();
                Experience Exp = Experience.GetExperienceByID(model.DesiredExperience);

                string plainTextContent = @"Are you ready? 
                                    " + model.ReservationName +
                                          @", we know you'll love the experience in store for you at AWI! Here's your booking information! 
                                          Date Of Reservation: " + DateTime.Parse(model.ReservationDate, CultureInfo.InvariantCulture).ToShortDateString() +
                                          "Reservation Begins At: " + model.SelectedReservationTime +
                                          "This is a " + model.ReservationDurationHrs + " hour reservation for " + model.ReservationGuestCount + " participants." +
                                          "Simulation Room:" + rooms.Where(r => r.ID.Equals(model.SelectedReservationRoom)).Single().Name +
                                          "Scheduled Experience: " + Exp.ExperienceTitle
                                          + @"We're located at 1802 W Grant Rd, #107, between I-10 and Silverbell, right next to Sherwin-Williams.
                                        If you are bringing your own gun, please make sure you are not bringing any ammo!
                                        We do not allow live ammunition in our facility, for safety reasons. Please bring your weapon in the locked-open position with NO AMMUNITION in your weapon or on your person.
                                        Your time starts at your scheduled time slot! Please do not be late. We cannot compensate you for time lost, and we cannot extend your session in order to be fair to others who may have booked the slot after you. Any time missed is time lost.
                                        NO REFUNDS, NO RESCHEDULES INSIDE 24 HOURS! With 24 hours notice on a future reservation, we can attempt to accommodate rescheduling to any date in the future of the reservation date. Call (520) 253-5270 to talk to someone about rescheduling.
                                        <br />
                                        Please do not replay to this email. We will miss your message!
                                        ";


                string htmlContent = "<strong>Are You Ready?</strong>"
                                     + @break
                                     + model.ReservationName +
                                     @", we know you'll love the experience in store for you at AWI! Here's your booking information!"
                                     + @break
                                     + "<strong>Date Of Reservation: </strong>" + DateTime.Parse(model.ReservationDate, CultureInfo.InvariantCulture).ToShortDateString()
                                     + @break
                                     + "<strong>Reservation Begins At: </strong>" + model.SelectedReservationTime
                                     + @break
                                     + "This is a " + model.ReservationDurationHrs + " hour reservationfor " + model.ReservationGuestCount + " participants."
                                     + @break
                                     + "<strong>Simulation Room:</strong> " + rooms.Where(r => r.ID.Equals(model.SelectedReservationRoom)).Single().Name
                                     + @break
                                     + "<strong>Scheduled Experience:</strong> " + Exp.ExperienceTitle
                                     + @break
                                     + "<strong>Paid:</strong> " + model.TotalMoney
                                     + @break
                                     + @"We're located at 1802 W Grant Rd, #107, between I-10 and Silverbell, right next to Sherwin-Williams. <a href=""https://goo.gl/maps/C8Cgi7iDp6gfDnyA9"">Google Maps </a> 
                                <br /> <br /> 
                                <strong> If you are bringing your own gun, please make sure you are not bringing any ammo! </strong> 
                                <br /> We do not allow live ammunition in our facility, for safety reasons. Please bring your weapon in the locked-open position with NO AMMUNITION in your weapon or on your person. <br /> 
                                <br /> 
                                <strong> Your time starts at your scheduled time slot! </strong> Please do not be late. We cannot compensate you for time lost, and we cannot extend your session in order to be fair to others who may have booked the slot after you. Any time missed is time lost. <br />
                                <br />                               
                                <strong> NO REFUNDS, NO RESCHEDULES INSIDE 24 HOURS! </strong> - With 24 hours notice on a future reservation, we can attempt to accommodate rescheduling to any date in the future of the reservation date. Call (520) 253-5270 to talk to someone about rescheduling.
                                <br />
                                Please do not reply to this email. It is unmonitored. We will miss your message!   
                              ";


                var response = Utility.SendReservationMessage(model.ReservationEmail, subject, plainTextContent, htmlContent, null, null);

                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User {0} has been sent a confirmation email to {1}. ", model.ReservationName, model.ReservationEmail), nameof(SendConfirmationEmail));

                return response.StatusCode;
            }

            return HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Sends an exciting email to the main company email address telling them there is a new reservation! Accepts a Reservation object. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<HttpStatusCode> SendNewBookingEmail(Reservation model)
        {
            const string subject = "New Reservation";
            if (model != null)
            {

                IEnumerable<Rooms> rooms = Rooms.GetAvailableRooms();
                Experience Exp = Experience.GetExperienceByID(model.DesiredExperience);


                string plainTextContent = @"New Reservation for " + model.ReservationName + " ( " + model.ReservationGuestCount + " participants )" +
                                          "Phone Number: " + model.ReservationPhone +
                                          "Date Of Reservation: " + DateTime.Parse(model.ReservationDate, CultureInfo.InvariantCulture).ToShortDateString() +
                                          "Reservation Begins At: " + model.SelectedReservationTime +
                                          "and will span for " + model.ReservationDurationHrs + " hours in room " + rooms.Where(r => r.ID.Equals(model.SelectedReservationRoom)).Single().Name + "." +
                                          "Selected Experience: " + Exp.ExperienceTitle
                                          + @" REMINDER: We do not allow live ammunition in our facility, for safety reasons. Please make sure the customer's weapon is in the locked-open position with NO AMMUNITION in the weapon or on  the customer's person.
                                    The customer's time starts at their scheduled time slot! If they are late, we cannot compensate them for time lost, and we cannot extend their session in order to be fair to others who may have booked the slot after them. Any time missed is time lost.
                                    NO REFUNDS, NO RESCHEDULES INSIDE 24 HOURS! With 24 hours notice on a future reservation, we can attempt to accommodate rescheduling to any date in the future of the reservation date.
                                    ";


                string htmlContent = "<strong>New Reservation For:</strong>"
                                     + @break
                                     + model.ReservationName + " ( " + model.ReservationGuestCount + " participants )"
                                     + @break
                                     + "<strong>Date Of Reservation: </strong>" + model.ReservationDate
                                     + @break
                                     + "<strong>Phone Number: </strong>" + model.ReservationPhone
                                     + @break
                                     + "<strong>Reservation Begins At: </strong>" + model.SelectedReservationTime
                                     + @break
                                     + "and will span for " + model.ReservationDurationHrs + " hours"
                                     + @break
                                     + "We will have the following equipment ready for them:"
                                     + @break
                                     + "<strong>Simulation Room: </strong>" + rooms.Where(r => r.ID.Equals(model.SelectedReservationRoom)).Single().Name
                                     + @break
                                     + "<strong>Selected Experience:</strong>" + Exp.ExperienceTitle
                                     + @break
                                     + @" 
                                <br /> We do not allow live ammunition in our facility, for safety reasons. Please make sure the customer's weapon is in the locked-open position with NO AMMUNITION in the weapon or on  the customer's person. <br /> 
                                <br /> 
                                <strong> The customer's time starts at their scheduled time slot! If they are late, we cannot compensate them for time lost, and we cannot extend their session in order to be fair to others who may have booked the slot after them. Any time missed is time lost.<br />
                                <br />                                
                                <strong> NO REFUNDS, NO RESCHEDULES INSIDE 24 HOURS! </strong> - With 24 hours notice on a future reservation, we can attempt to accommodate rescheduling to any date in the future of the reservation date.
                              ";

                var response = Utility.SendReservationMessage(CompanyCCEmailAddress, subject, plainTextContent, htmlContent, null, null);

                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "AWI has been sent a booking notification regarding {0} ({1}) ", model.ReservationName, model.ReservationEmail), nameof(SendNewBookingEmail));

                return response.StatusCode;
            }

            return HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<HttpStatusCode> SendNewBookingEmailWithFailureAlert(Reservation model)
        {
            IEnumerable<Rooms> rooms = Rooms.GetAvailableRooms();
            Experience Exp = Experience.GetExperienceByID(model.DesiredExperience);

            const string subject = "WARNING: New Reservation in contingency mode! ";
            if (model != null)
            {
                string plainTextContent = @"New Reservation for " + model.ReservationName +
                                          "BOOKING DB INSERT FAILED!" +
                                          "Phone: " + model.ReservationPhone +
                                          "Date Of Reservation: " + DateTime.Parse(model.ReservationDate, CultureInfo.InvariantCulture).ToShortDateString() + " ( " + model.ReservationGuestCount + " participants )" +
                                          "Reservation Begins At: " + model.SelectedReservationTime +
                                          "and will span for " + model.ReservationDurationHrs + " hours" +
                                          "Selected Experience: " + Exp.ExperienceTitle +
                                          "Simulation Room: " + rooms.Where(r => r.ID.Equals(model.SelectedReservationRoom)).Single().Name
                                          + @" We do not allow live ammunition in our facility, for safety reasons. Please make sure the customer's weapon is in the locked-open position with NO AMMUNITION in the weapon or on  the customer's person.
                                    The customer's time starts at their scheduled time slot! If they are late, we cannot compensate them for time lost, and we cannot extend their session in order to be fair to others who may have booked the slot after them. Any time missed is time lost.
                                    NO REFUNDS, NO RESCHEDULES INSIDE 24 HOURS! With 24 hours notice on a future reservation, we can attempt to accommodate rescheduling to any date in the future of the reservation date.           
                                    ";


                string htmlContent = "<strong>New Reservation For:</strong>"
                                     + @break
                                     + model.ReservationName
                                     + @break
                                     + "<span style=\"color: red\">BOOKING DB INSERT FAILED </span>"
                                     + "call" + model.ReservationPhone
                                     + @break
                                     + "<strong>Date Of Reservation: </strong>" + DateTime.Parse(model.ReservationDate, CultureInfo.InvariantCulture).ToShortDateString()
                                     + @break
                                     + "<strong>Reservation Begins At: </strong>" + model.SelectedReservationTime + " ( " + model.ReservationGuestCount + " participants )"
                                     + @break
                                     + "and will span for " + model.ReservationDurationHrs + " hours"
                                     + @break
                                     + "We will have the following equipment ready for them:"
                                     + @break
                                     + "<strong>Simulation Room: </strong>" + rooms.Where(r => r.ID.Equals(model.SelectedReservationRoom)).Single().Name
                                     + @break
                                     + "<strong>Selected Experience:</strong> " + Exp.ExperienceTitle
                                     + @break
                                     + @" 
                                <br /> We do not allow live ammunition in our facility, for safety reasons. Please make sure the customer's weapon is in the locked-open position with NO AMMUNITION in the weapon or on  the customer's person. <br /> 
                                <br /> 
                                <strong> The customer's time starts at their scheduled time slot! If they are late, we cannot compensate them for time lost, and we cannot extend their session in order to be fair to others who may have booked the slot after them. Any time missed is time lost.<br />
                                <br />                                
                                <strong> NO REFUNDS, NO RESCHEDULES INSIDE 24 HOURS! </strong> - With 24 hours notice on a future reservation, we can attempt to accommodate rescheduling to any date in the future of the reservation date.
                              ";

                var response = Utility.SendReservationMessage(CompanyCCEmailAddress, subject, plainTextContent, htmlContent, null, null);
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "CONTINGENCY MODE: User {0} has been sent a confirmation email to {1}. Could not insert data for booking into database.", model.ReservationName, model.ReservationEmail), nameof(SendNewBookingEmailWithFailureAlert));

                return response.StatusCode;
            }

            return HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Gets an object containing a htmlAttributes collection for any Razor HTML helper component,
        /// supporting a static set (anonymous object) and/or a dynamic set (Dictionary)
        /// </summary>
        /// <param name="fixedHtmlAttributes">A fixed set of htmlAttributes (anonymous object)</param>
        /// <param name="dynamicHtmlAttributes">A dynamic set of htmlAttributes (Dictionary)</param>
        /// <returns>A collection of htmlAttributes including a merge of the given set(s)</returns>
        public static IDictionary<string, object> GetHtmlAttributes(
            object fixedHtmlAttributes = null,
            IDictionary<string, object> dynamicHtmlAttributes = null
            )
        {
            var rvd = (fixedHtmlAttributes == null)
                ? new RouteValueDictionary()
                : HtmlHelper.AnonymousObjectToHtmlAttributes(fixedHtmlAttributes);
            if (dynamicHtmlAttributes != null)
            {
                foreach (KeyValuePair<string, object> kvp in dynamicHtmlAttributes)
                    rvd[kvp.Key] = kvp.Value;
            }
            return rvd;
        }
    }

    public class BookingStub
    {
        #region
        public TimeSpan ReservationTime { get; set; }
        public TimeSpan ReservationEndTime { get; set; }
        public int ReservationDurationHrs { get; set; }
        public int SelectedReservationRoom { get; set; }
        #endregion


        /// <summary>
        /// Gets a "stubbed" set of reservations for quick schedule management. 
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <param name="simulationRoomID"></param>
        /// <returns></returns>
        public static List<BookingStub> GetStubbedReservationsByRoomId(DateTime selectedDate, int simulationRoomID)
        {

            List<BookingStub> bookedTimes = new List<BookingStub>();

            // Get list of bookings for the selected day.
            string queryString2 = "SELECT * FROM dbo.Reservations WHERE ReservationDate = '" +
                                  (selectedDate.Equals(MinValue) ? Utility.GetCurrentTucsonTime().Date.ToShortDateString() : selectedDate.ToShortDateString()) + "'" +
                                  "AND ReservationRoom = '" + simulationRoomID + "'";

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString2, connection);
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookingStub Reservation = new BookingStub
                            {
                                ReservationTime = (TimeSpan)reader["ReservationStartTime"],
                                ReservationEndTime = (TimeSpan)reader["ReservationEndTime"],
                                ReservationDurationHrs = (int)reader["ReservationDurationHrs"],
                                SelectedReservationRoom = (int)reader["ReservationRoom"]
                            };

                            bookedTimes.Add(Reservation);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetStubbedReservationsByRoomId));
                    throw;
                }
            }

            return bookedTimes;
        }

        /// <summary>
        /// Gets a "stubbed" set of reservations for quick schedule management. 
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <param name="simulationRoomID"></param>
        /// <returns></returns>
        public static List<BookingStub> GetStubbedReservationsAndClassesByRoomID(DateTime selectedDate, int simulationRoomID)
        {
            var currentTucsonDateTime = Utility.GetCurrentTucsonTime();

            List<BookingStub> bookedTimes = new List<BookingStub>();

            // Get list of bookings for the selected day.
            string queryString2 = "SELECT * FROM dbo.Reservations WHERE ReservationDate = '" +
                                  (selectedDate.Equals(MinValue) ? Utility.GetCurrentTucsonTime().Date.ToShortDateString() : selectedDate.ToShortDateString()) + "'" +
                                  "AND ReservationRoom = '" + simulationRoomID + "'";

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString2, connection);
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookingStub Reservation = new BookingStub
                            {
                                ReservationTime = (TimeSpan)reader["ReservationStartTime"],
                                ReservationEndTime = (TimeSpan)reader["ReservationEndTime"],
                                ReservationDurationHrs = (int)reader["ReservationDurationHrs"],
                                SelectedReservationRoom = (int)reader["ReservationRoom"]
                            };

                            bookedTimes.Add(Reservation);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetStubbedReservationsAndClassesByRoomID));
                    throw;
                }

                // Get list of classes for the selected day, convert to BookingStub and append to bookedTimes. 
                try
                {

                    using (SqlCommand cmd = new SqlCommand("GetClassBookingsByRoomID", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@queryDateString", SqlDbType.VarChar).Value = selectedDate.Equals(MinValue)
                            ? currentTucsonDateTime.Date.ToShortDateString()
                            : selectedDate.ToShortDateString();

                        cmd.Parameters.Add("@roomID", SqlDbType.VarChar).Value = simulationRoomID;
                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookingStub Reservation = new BookingStub
                                {
                                    ReservationTime = (TimeSpan)reader["StartTime"],
                                    ReservationEndTime = (TimeSpan)reader["EndTime"],
                                    SelectedReservationRoom = (int)reader["MainRoom"]
                                };

                                Reservation.ReservationDurationHrs =
                                                                (int)(Reservation.ReservationEndTime - Reservation.ReservationTime).TotalHours;

                                bookedTimes.Add(Reservation);

                                if (reader["SecondaryRoom"] != DBNull.Value)
                                {
                                    BookingStub Reservation2 = new BookingStub
                                    {
                                        ReservationTime = (TimeSpan)reader["StartTime"],
                                        ReservationEndTime = (TimeSpan)reader["EndTime"],
                                        SelectedReservationRoom = (int)reader["SecondaryRoom"]
                                    };

                                    Reservation2.ReservationDurationHrs =
                                                                        (int)(Reservation2.ReservationEndTime - Reservation2.ReservationTime)
                                                                        .TotalHours;

                                    bookedTimes.Add(Reservation2);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetStubbedReservationsAndClasses));
                    throw;
                }
            }

            return bookedTimes;
        }

        public static List<BookingStub> GetStubbedReservationsAndClasses()
        {
            var currentTucsonDateTime = Utility.GetCurrentTucsonTime();

            List<BookingStub> bookedTimes = new List<BookingStub>();

            // Get list of bookings for the selected day.
            string queryString2 = "SELECT * FROM dbo.Reservations";

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString2, connection);
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookingStub Reservation = new BookingStub
                            {
                                ReservationTime = (TimeSpan)reader["ReservationStartTime"],
                                ReservationEndTime = (TimeSpan)reader["ReservationEndTime"],
                                ReservationDurationHrs = (int)reader["ReservationDurationHrs"],
                                SelectedReservationRoom = (int)reader["ReservationRoom"]
                            };

                            bookedTimes.Add(Reservation);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetStubbedReservationsAndClassesByRoomID));
                    throw;
                }

                // Get list of classes, convert to BookingStub and append to bookedTimes. 
                try
                {

                    using (SqlCommand cmd = new SqlCommand("GetClassBookingsByRoomID", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookingStub Reservation = new BookingStub
                                {
                                    ReservationTime = (TimeSpan)reader["StartTime"],
                                    ReservationEndTime = (TimeSpan)reader["EndTime"],
                                    SelectedReservationRoom = (int)reader["MainRoom"]
                                };

                                Reservation.ReservationDurationHrs =
                                                                (int)(Reservation.ReservationEndTime - Reservation.ReservationTime).TotalHours;

                                bookedTimes.Add(Reservation);

                                if (reader["SecondaryRoom"] != DBNull.Value)
                                {
                                    BookingStub Reservation2 = new BookingStub
                                    {
                                        ReservationTime = (TimeSpan)reader["StartTime"],
                                        ReservationEndTime = (TimeSpan)reader["EndTime"],
                                        SelectedReservationRoom = (int)reader["SecondaryRoom"]
                                    };

                                    Reservation2.ReservationDurationHrs =
                                                                        (int)(Reservation2.ReservationEndTime - Reservation2.ReservationTime)
                                                                        .TotalHours;

                                    bookedTimes.Add(Reservation2);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetStubbedReservationsAndClasses));
                    throw;
                }
            }

            return bookedTimes;
        }
    }

    public class EnhancedBookingStub
    {
        #region
        public Guid RecordID { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationTime { get; set; }
        public TimeSpan ReservationEndTime { get; set; }
        public int ReservationDurationHrs { get; set; }
        public int SelectedReservationRoom { get; set; }
        public string Type { get; set; }
        #endregion


        /// <summary>
        /// Gets a "stubbed" set of reservations for quick schedule management. 
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <param name="simulationRoomID"></param>
        /// <returns></returns>
        public static List<EnhancedBookingStub> GetEStubbedReservationsByRoomId(DateTime selectedDate, int simulationRoomID)
        {

            List<EnhancedBookingStub> bookedTimes = new List<EnhancedBookingStub>();

            // Get list of bookings for the selected day.
            string queryString2 = "SELECT * FROM dbo.Reservations WHERE ReservationDate = '" +
                                  (selectedDate.Equals(MinValue) ? Utility.GetCurrentTucsonTime().Date.ToShortDateString() : selectedDate.ToShortDateString()) + "'" +
                                  "AND ReservationRoom = '" + simulationRoomID + "'";

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString2, connection);
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EnhancedBookingStub Reservation = new EnhancedBookingStub
                            {
                                RecordID = (Guid)reader["ID"],
                                ReservationDate = (DateTime)reader["ReservationDate"],
                                ReservationTime = (TimeSpan)reader["ReservationStartTime"],
                                ReservationEndTime = (TimeSpan)reader["ReservationEndTime"],
                                ReservationDurationHrs = (int)reader["ReservationDurationHrs"],
                                SelectedReservationRoom = (int)reader["ReservationRoom"],
                                Type="Experience"
                            };

                            bookedTimes.Add(Reservation);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetEStubbedReservationsByRoomId));
                    throw;
                }
            }

            return bookedTimes;
        }

        /// <summary>
        /// Gets a "stubbed" set of reservations for quick schedule management. 
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <param name="simulationRoomID"></param>
        /// <returns></returns>
        public static List<EnhancedBookingStub> GetEStubbedReservationsAndClassesByRoomID(DateTime selectedDate, int simulationRoomID)
        {
            var currentTucsonDateTime = Utility.GetCurrentTucsonTime();

            List<EnhancedBookingStub> bookedTimes = new List<EnhancedBookingStub>();

            // Get list of bookings for the selected day.
            string queryString2 = "SELECT * FROM dbo.Reservations WHERE ReservationDate = '" +
                                  (selectedDate.Equals(MinValue) ? Utility.GetCurrentTucsonTime().Date.ToShortDateString() : selectedDate.ToShortDateString()) + "'" +
                                  "AND ReservationRoom = '" + simulationRoomID + "'";

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString2, connection);
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EnhancedBookingStub Reservation = new EnhancedBookingStub
                            {
                                RecordID = (Guid)reader["ID"],
                                ReservationDate = (DateTime)reader["ReservationDate"],
                                ReservationTime = (TimeSpan)reader["ReservationStartTime"],
                                ReservationEndTime = (TimeSpan)reader["ReservationEndTime"],
                                ReservationDurationHrs = (int)reader["ReservationDurationHrs"],
                                SelectedReservationRoom = (int)reader["ReservationRoom"],
                                Type = "Experience"
                            };

                            bookedTimes.Add(Reservation);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetEStubbedReservationsAndClassesByRoomID));
                    throw;
                }

                // Get list of classes for the selected day, convert to BookingStub and append to bookedTimes. 
                try
                {

                    using (SqlCommand cmd = new SqlCommand("GetClassBookingsByRoomID", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@queryDateString", SqlDbType.VarChar).Value = selectedDate.Equals(MinValue)
                            ? currentTucsonDateTime.Date.ToShortDateString()
                            : selectedDate.ToShortDateString();

                        cmd.Parameters.Add("@roomID", SqlDbType.VarChar).Value = simulationRoomID;
                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EnhancedBookingStub Reservation = new EnhancedBookingStub
                                {
                                    RecordID = (Guid)reader["EduClassID"],
                                    ReservationDate = (DateTime)reader["StartDate"],
                                    ReservationTime = (TimeSpan)reader["StartTime"],
                                    ReservationEndTime = (TimeSpan)reader["EndTime"],
                                    SelectedReservationRoom = (int)reader["MainRoom"],
                                    Type = "Class"
                                };

                                Reservation.ReservationDurationHrs =
                                                                (int)(Reservation.ReservationEndTime - Reservation.ReservationTime).TotalHours;

                                bookedTimes.Add(Reservation);

                                if (reader["SecondaryRoom"] != DBNull.Value)
                                {
                                    EnhancedBookingStub Reservation2 = new EnhancedBookingStub
                                    {
                                        RecordID = (Guid)reader["EduClassID"],
                                        ReservationDate = (DateTime)reader["StartDate"],
                                        ReservationTime = (TimeSpan)reader["StartTime"],
                                        ReservationEndTime = (TimeSpan)reader["EndTime"],
                                        SelectedReservationRoom = (int)reader["SecondaryRoom"],
                                        Type = "Class"
                                    };

                                    Reservation2.ReservationDurationHrs =
                                                                        (int)(Reservation2.ReservationEndTime - Reservation2.ReservationTime)
                                                                        .TotalHours;

                                    bookedTimes.Add(Reservation2);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetEStubbedReservationsAndClassesByRoomID));
                    throw;
                }
            }

            return bookedTimes;
        }

        public static List<EnhancedBookingStub> GetEStubbedReservationsAndClasses()
        {
            var currentTucsonDateTime = Utility.GetCurrentTucsonTime();

            List<EnhancedBookingStub> bookedTimes = new List<EnhancedBookingStub>();

            // Get list of bookings for the selected day.
            string queryString2 = "SELECT * FROM dbo.Reservations";

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString2, connection);
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EnhancedBookingStub Reservation = new EnhancedBookingStub
                            {
                                RecordID = (Guid)reader["ID"],
                                ReservationDate = (DateTime)reader["ReservationDate"],
                                ReservationTime = (TimeSpan)reader["ReservationStartTime"],
                                ReservationEndTime = (TimeSpan)reader["ReservationEndTime"],
                                ReservationDurationHrs = (int)reader["ReservationDurationHrs"],
                                SelectedReservationRoom = (int)reader["ReservationRoom"],
                                Type = "Experience"
                            };

                            bookedTimes.Add(Reservation);
                        }
                    }
                    connection.Close();
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetEStubbedReservationsAndClasses));
                    throw;
                }

                // Get list of classes, convert to BookingStub and append to bookedTimes. 
                try
                {

                    using (SqlCommand cmd = new SqlCommand("GetClassBookings", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EnhancedBookingStub Reservation = new EnhancedBookingStub
                                {
                                    RecordID = (Guid)reader["EduClassID"],
                                    ReservationDate = (DateTime)reader["StartDate"],
                                    ReservationTime = (TimeSpan)reader["StartTime"],
                                    ReservationEndTime = (TimeSpan)reader["EndTime"],
                                    SelectedReservationRoom = (int)reader["MainRoom"],
                                    Type = "Class"
                                };

                                Reservation.ReservationDurationHrs = (int)(Reservation.ReservationEndTime - Reservation.ReservationTime).TotalHours;

                                bookedTimes.Add(Reservation);
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetEStubbedReservationsAndClasses));
                    throw;
                }
            }

            return bookedTimes;
        }
    }
}
