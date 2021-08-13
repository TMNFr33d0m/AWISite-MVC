using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Attanaya_Warrior_Institute.Models
{
    public class EduClass
    {
        public Guid EduClassId { get; set; }

        [DisplayName("Instructor:")]
        public Guid InstructorId { get; set; }
        [DisplayName("Class Title:")]
        public string ClassTitle { get; set; }

        [DisplayName("Start Date / Time:")]
        public DateTime StartDateTime { get; set; }

        [DisplayName("End Date / Time:")]
        public DateTime EndDateTime { get; set; }

        [DisplayName("Description:")]
        public string Description { get; set; }

        [DisplayName("Classroom:")]
        public int MainRoom { get; set; }

        [DisplayName("Supporting Room:")]
        public int SecondaryRoom { get; set; }

        [DisplayName("Cost Per Student:")]
        public decimal CostPerStudent { get; set; }

        [DisplayName("Max Attendees:")]
        public int MaxAttendees { get; set; }

        [DisplayName("Customers book at external site?")]
        public bool ExternalBookingSource { get; set; }

        [DisplayName("Link to booking site:")]
        public string ExternalBookingLink { get; set; }

        public IEnumerable<EduClass> GetEduClasses()
        {
            List<EduClass> listOfClasses = new List<EduClass>();
            string queryString = "SELECT * FROM dbo.ClassBookings ORDER BY StartDateTime;";

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
                            EduClass eduClass = new EduClass
                            {
                                EduClassId = (Guid)reader["EduClassId"],
                                InstructorId = (Guid)reader["InstructorID"],
                                ClassTitle = (string)reader["ClassTitle"],
                                StartDateTime = (DateTime)reader["StartDateTime"],
                                EndDateTime = (DateTime)reader["EndDateTime"],
                                Description = (string)reader["Description"],
                                MainRoom = (int)reader["MainRoom"],
                                SecondaryRoom = (int)reader["SecondaryRoom"],
                                CostPerStudent = (decimal)reader["CostPerStudent"],
                                MaxAttendees = (int)reader["MaxAttendees"],
                                ExternalBookingSource = (bool)reader["ExternalBookingSource"],
                                ExternalBookingLink = (string)reader["ExternalBookingLink"]
                            };

                            listOfClasses.Add(eduClass);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetEduClasses));
                    throw;
                }
            }

            return listOfClasses;
        }

        public static EduClass GetEduClassById(Guid EduClassID)
        {
            string queryString = "SELECT * FROM dbo.ClassBookings WHERE EduClassId = '" + EduClassID + "';";
            EduClass eduClass = new EduClass();

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
                            eduClass.EduClassId = (Guid)reader["EduClassId"];
                            eduClass.InstructorId = (Guid)reader["InstructorID"];
                            eduClass.ClassTitle = (string)reader["ClassTitle"];
                            eduClass.StartDateTime = (DateTime)reader["StartDateTime"];
                            eduClass.EndDateTime = (DateTime)reader["EndDateTime"];
                            eduClass.Description = (string)reader["Description"];
                            eduClass.MainRoom = (int)reader["MainRoom"];
                            eduClass.SecondaryRoom = (int)reader["SecondaryRoom"];
                            eduClass.CostPerStudent = (decimal)reader["CostPerStudent"];
                            eduClass.MaxAttendees = (int)reader["MaxAttendees"];
                            eduClass.ExternalBookingSource = (bool)reader["ExternalBookingSource"];
                            eduClass.ExternalBookingLink = (string)reader["ExternalBookingLink"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetEduClasses));
                    throw;
                }
            }

            return eduClass;
        }

        public IEnumerable<EduClass> GetEduClassesForInstructor(Guid instructorId)
        {
            List<EduClass> listOfClasses = new List<EduClass>();
            string queryString = "SELECT * FROM dbo.ClassBookings WHERE InstructorID = '" + instructorId + "' ORDER BY StartDateTime;";

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
                            EduClass eduClass = new EduClass
                            {
                                EduClassId = (Guid)reader["EduClassId"],
                                InstructorId = instructorId,
                                ClassTitle = (string)reader["ClassTitle"],
                                StartDateTime = (DateTime)reader["StartDateTime"],
                                EndDateTime = (DateTime)reader["EndDateTime"],
                                Description = (string)reader["Description"],
                                MainRoom = (int)reader["MainRoom"],
                                SecondaryRoom = (int)reader["SecondaryRoom"],
                                CostPerStudent = (decimal)reader["CostPerStudent"],
                                MaxAttendees = (int)reader["MaxAttendees"],
                                ExternalBookingSource = (bool)reader["ExternalBookingSource"],
                                ExternalBookingLink = (string)reader["ExternalBookingLink"]
                            };

                            listOfClasses.Add(eduClass);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetEduClasses));
                    throw;
                }
            }

            return listOfClasses;
        }

        public IEnumerable<EduClass> GetEduClassesForStudent(Guid studentId) 
        {
            List<Guid> enrolledClasses = new List<Guid>(); 
            List<EduClass> listOfClasses = new List<EduClass>();


            string queryString = "SELECT EduClassID FROM dbo.StudentBookings WHERE StudentID = '" + studentId + "';";

            string queryString2 = "SELECT * FROM dbo.ClassBookings WHERE EduClassID = null OR ";

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
                            Guid classGuid = (Guid)reader["EduClassID"];
                            enrolledClasses.Add(classGuid);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetEduClasses));
                    throw;
                }

                var guidCount = 0;
                var totalGuidCount = enrolledClasses.Count();

                if (totalGuidCount > 0) { guidCount = 1; }

                foreach (var guid in enrolledClasses)
                {
                    queryString2 = queryString2 + " EduClassID = '" +guid.ToString()+"'";

                    if (guidCount < totalGuidCount)
                    {
                        queryString2 = queryString2 + " OR ";
                        guidCount++;
                    }
                    else
                    {
                        queryString2 = queryString2 + " ORDER BY StartDateTime DESC ";
                    }
                }

                if (totalGuidCount == 0)
                {
                    queryString2 = queryString2 + " EduClassID = null";
                }

                var command2 = new SqlCommand(queryString2, connection); 

                try
                {
                    using (var reader = command2.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EduClass eduClass = new EduClass
                            {
                                EduClassId = (Guid)reader["EduClassId"],
                                InstructorId = (Guid)reader["InstructorID"],
                                ClassTitle = (string)reader["ClassTitle"],
                                StartDateTime = (DateTime)reader["StartDateTime"],
                                EndDateTime = (DateTime)reader["EndDateTime"],
                                Description = (string)reader["Description"],
                                MainRoom = (int)reader["MainRoom"],
                                SecondaryRoom = (int)reader["SecondaryRoom"],
                                CostPerStudent = (decimal)reader["CostPerStudent"],
                                MaxAttendees = (int)reader["MaxAttendees"],
                                ExternalBookingSource = (bool)reader["ExternalBookingSource"],
                                ExternalBookingLink = (string)reader["ExternalBookingLink"]
                            };

                            listOfClasses.Add(eduClass);
                        }

                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetEduClasses));
                    throw;
                }

            }

            return listOfClasses;
        }

        public HttpStatusCodeResult CreateNewClass(EduClass newClass)
        {
            if (newClass == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                using (var connection = new SqlConnection(Utility.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("CreateNewClass", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@InstructorID", SqlDbType.UniqueIdentifier).Value = newClass.InstructorId;
                        cmd.Parameters.Add("@ClassTitle", SqlDbType.NVarChar).Value = newClass.ClassTitle;
                        cmd.Parameters.Add("@StartDateTime", SqlDbType.DateTime).Value = newClass.StartDateTime;
                        cmd.Parameters.Add("@EndDateTime", SqlDbType.DateTime).Value = newClass.EndDateTime;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = newClass.Description;
                        cmd.Parameters.Add("@MainRoom", SqlDbType.Int).Value = newClass.MainRoom;
                        cmd.Parameters.Add("@SecondaryRoom", SqlDbType.Int).Value = newClass.SecondaryRoom;
                        cmd.Parameters.Add("@CostPerStudent", SqlDbType.Decimal).Value = newClass.CostPerStudent;
                        cmd.Parameters.Add("@MaxAttendees", SqlDbType.Int).Value = newClass.MaxAttendees;
                        cmd.Parameters.Add("@ExternalBookingLink", SqlDbType.NVarChar).Value = newClass.ExternalBookingLink;
                        cmd.Parameters.Add("@ExternalBookingSource", SqlDbType.Bit).Value = newClass.ExternalBookingSource;
                        
                        connection.Open();
                        cmd.ExecuteNonQuery();

                        LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                            "Instructor {0} has booked a reservation for {1} from {2} to {3}. The class will be in room ID:{4},{5}. ",
                            newClass.InstructorId,
                            newClass.ClassTitle,
                            newClass.StartDateTime,
                            newClass.EndDateTime,
                            newClass.MainRoom,
                            newClass.SecondaryRoom), nameof(CreateNewClass));
                    }
                }
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(CreateNewClass));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }

        public HttpStatusCodeResult UpdateClass(EduClass educlass)
        {
            if (educlass == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                using (var connection = new SqlConnection(Utility.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateClass", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@InstructorID", SqlDbType.UniqueIdentifier).Value = educlass.InstructorId;
                        cmd.Parameters.Add("@EduClassID", SqlDbType.UniqueIdentifier).Value = educlass.EduClassId;
                        cmd.Parameters.Add("@ClassTitle", SqlDbType.NVarChar).Value = educlass.ClassTitle;
                        cmd.Parameters.Add("@StartDateTime", SqlDbType.DateTime).Value = educlass.StartDateTime;
                        cmd.Parameters.Add("@EndDateTime", SqlDbType.DateTime).Value = educlass.EndDateTime;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = educlass.Description;
                        cmd.Parameters.Add("@MainRoom", SqlDbType.Int).Value = educlass.MainRoom;
                        cmd.Parameters.Add("@SecondaryRoom", SqlDbType.Int).Value = educlass.SecondaryRoom;
                        cmd.Parameters.Add("@CostPerStudent", SqlDbType.Decimal).Value = educlass.CostPerStudent;
                        cmd.Parameters.Add("@MaxAttendees", SqlDbType.Int).Value = educlass.MaxAttendees;
                        cmd.Parameters.Add("@ExternalBookingLink", SqlDbType.NVarChar).Value = educlass.ExternalBookingLink;
                        cmd.Parameters.Add("@ExternalBookingSource", SqlDbType.Bit).Value = educlass.ExternalBookingSource;

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                            "Instructor {0} has updated class {1}, scheudled from {2} to {3}. The class will be in room(s) {4} {5}. ",
                            educlass.InstructorId,
                            educlass.ClassTitle,
                            educlass.StartDateTime,
                            educlass.EndDateTime,
                            educlass.MainRoom,
                            educlass.SecondaryRoom), nameof(UpdateClass));
                    }
                }
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(UpdateClass));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }

        public static HttpStatusCodeResult DeleteClass(string eduClassID)
        {
            if (string.IsNullOrEmpty(eduClassID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var educlass = EduClass.GetEduClassById(Guid.Parse(eduClassID));

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                string queryString1 = "DELETE FROM dbo.ClassBookings" +
                    " WHERE EduClassID = '" + eduClassID + "'";

                var command = new SqlCommand(queryString1, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                        "Instructor {0} has deleted a reservation for EduClass{1} from {2} to {3}. The class was in roomID: {4},{5}. ",
                        educlass.InstructorId,
                        educlass.EduClassId,
                        educlass.StartDateTime,
                        educlass.EndDateTime,
                        educlass.MainRoom,
                        educlass.SecondaryRoom), nameof(DeleteClass));
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(CreateNewClass));
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
                // End using
            }
        }

        public static ScheduleResponseModel CheckScheduleAvailability(
            DateTime CurrentClassStartTime, 
            DateTime CurrentClassEndTime, 
            int ClassRoomID, 
            int SupportingRoomID, 
            string eduClassId)
        {
            // Variable Instantiation
            ScheduleResponseModel Response = new ScheduleResponseModel { IsValid = true, ResponseText = "No Scheduling Conflicts!"};

            List<BookingStub> bookedTimes = new List<BookingStub>();
            DateTime currentTucsonDateTime = Utility.GetCurrentTucsonTime();
            EduClass eduClass = new EduClass();

            double openHours;


            if (!string.IsNullOrEmpty(eduClassId))
            {
                eduClass = GetEduClassById(Guid.Parse(eduClassId)); 
            }

            // First, check if the facility is closed. That's a deal breaker right there. 
             if (Utility.IsClosedOnDay(CurrentClassStartTime))
            {
                Response.IsValid = false;
                Response.ResponseText = "You cannot book a class on this day because the facility is scheduled to be closed.";
                return Response; 
            }

            // Otherwise, grab the hours of operation.
            List<HoursOfOperation> hoursOfOperation = HoursOfOperation.GetHoursOfOperation();

            // Next, grab all the reservations from the public that might comflict with the classroom or supporting room on this particular date.
            if (ClassRoomID != 7)
            {
                bookedTimes.AddRange(BookingStub.GetStubbedReservationsByRoomId(CurrentClassStartTime, ClassRoomID));
            }

            if (SupportingRoomID != 7)
            {
                bookedTimes.AddRange(BookingStub.GetStubbedReservationsByRoomId(CurrentClassStartTime, SupportingRoomID));
            }

            // Get list of classes for the selected day EXCEPT any class that matches the EduClassID param passed in, convert to BookingStub and append to bookedTimes. 
            try
            {
                using (var connection = new SqlConnection(Utility.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetClassBookingsForDashboard", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@queryDateString", SqlDbType.VarChar).Value = CurrentClassStartTime.Equals(DateTime.MinValue)
                            ? currentTucsonDateTime.Date.ToShortDateString()
                            : CurrentClassStartTime.ToShortDateString();
                        if (!String.IsNullOrEmpty(eduClassId))
                        {
                            cmd.Parameters.Add("@eduClassId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(eduClassId);
                        }
                        cmd.Parameters.Add("@roomID", SqlDbType.Int).Value = ClassRoomID;

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
                LoggingModel.LogCriticalException(e, nameof(CheckScheduleAvailability));
                throw;
            }

            // Grab the hours of operation for the selected day
            var todaysHours = hoursOfOperation.Where(r => r.DayOfWeek.Equals(CurrentClassStartTime.Equals(DateTime.MinValue) ? currentTucsonDateTime.DayOfWeek.ToString() : CurrentClassStartTime.DayOfWeek.ToString()))
                .Select(r => new { r.DayOfWeek, r.OpenTime, r.CloseTime }).Single();

            // Get a count of how many hours we're open that day, accounting for the day change at midnight
            if (todaysHours.OpenTime > todaysHours.CloseTime && todaysHours.CloseTime.Hours < 12)
            {
                var openDateTime = CurrentClassStartTime.Date + todaysHours.OpenTime;
                var closeDateTime = CurrentClassStartTime.AddDays(1).Date + todaysHours.CloseTime;
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
                foreach (var booking in bookedTimes)
                {
                    // If the class's starting time is greater than the reseved starting time, AND the class's starting time is less than the reserved classes ending time, throw a conflict, because the class starts when another class is going on. 
                    if ((CurrentClassStartTime.TimeOfDay > booking.ReservationTime) && (CurrentClassStartTime.TimeOfDay < (booking.ReservationEndTime < TimeSpan.FromHours(6) ? booking.ReservationEndTime.Add(TimeSpan.FromDays(1)) : booking.ReservationEndTime)))
                    {
                        Response.IsValid = false;
                        Response.ResponseText = "Scheduling conflict detected at " +
                            booking.ReservationTime + " in "
                            + booking.SelectedReservationRoom
                            + " (" + booking.ReservationTime
                            + " - " + booking.ReservationEndTime
                            + ")";
                        return Response;
                    }
                    // also...

                    // If the classes END time is greater than the booked classes start time,  the class extends into another booking and is conflicted

                    // If the class's starting time is greater than the reseved starting time, AND the class's starting time is less than the reserved classes ending time, throw a conflict, because the class starts when another class is going on. 
                    if ((CurrentClassEndTime.TimeOfDay > booking.ReservationTime) && (CurrentClassEndTime.TimeOfDay < (booking.ReservationEndTime < TimeSpan.FromHours(6) ? booking.ReservationEndTime.Add(TimeSpan.FromDays(1)) : booking.ReservationEndTime)))
                    {
                        Response.IsValid = false;
                        Response.ResponseText = "Scheduling conflict detected at " +
                           booking.ReservationTime + " in "
                            + booking.SelectedReservationRoom
                            + " (" + booking.ReservationTime
                            + " - " + booking.ReservationEndTime
                            + ")";
                        return Response;
                    }
                }
            }
            else // Otherwise show that we're closed during the selected time.
            {
                Response.IsValid = false;
                Response.ResponseText = "You cannot book a class on this day because the times of the class fall outside of the facility's normal operating hours.";
            }

            return Response;
        }

    }

    public class EduClassViewModel
    {
        public EduClassViewModel()
        {
            var eduClass = new EduClass();
            EduClasses = eduClass.GetEduClasses();
        }

        public EduClassViewModel(Instructor instructor)
        {
            var eduClass = new EduClass();
            EduClasses = eduClass.GetEduClassesForInstructor(instructor.InstructorId);            
        }

        public EduClassViewModel(Guid StudentID)
        {
            var eduClass = new EduClass();
            EduClasses = eduClass.GetEduClassesForStudent(StudentID);
        }

        public IEnumerable<EduClass> EduClasses { get; set; }

    }

    public class ScheduleResponseModel { 
        public string ResponseText { get; set; }
        public bool IsValid { get; set; }
    }

}