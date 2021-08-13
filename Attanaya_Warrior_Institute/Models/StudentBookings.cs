using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Configuration;

namespace Attanaya_Warrior_Institute.Models
{
    public class StudentBookings
    {
        [Key]
        public Guid ReservationID { get; set; }
        public Guid StudentID { get; set; }
        public Guid EduClassID { get; set; }
        public DateTime TimeStamp { get; set; }

        public static List<StudentBookings> GetAllStudentBookings()
        {
            List<StudentBookings> StudentBookings = new List<StudentBookings>();
            string queryString = "SELECT * FROM dbo.StudentBookings";

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
                            StudentBookings booking = new StudentBookings
                            {
                                StudentID = (Guid)reader["StudentID"],
                                ReservationID = (Guid)reader["ReservationID"],
                                EduClassID = (Guid)reader["EduClassID"],
                                TimeStamp = (DateTime)reader["TimeStamp"]
                            };

                            StudentBookings.Add(booking);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetAllStudentBookings));
                    throw;
                }
            } 
            return StudentBookings;
        }


        public static List<StudentBookings> GetAllStudentBookingsForEduClassID(Guid EduClassID)
        {
            List<StudentBookings> StudentBookings = new List<StudentBookings>();
            string queryString = "SELECT * FROM dbo.StudentBookings WHERE EduClassID = '"+ EduClassID.ToString() + "'";

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
                            StudentBookings booking = new StudentBookings
                            {
                                StudentID = (Guid)reader["StudentID"],
                                ReservationID = (Guid)reader["ReservationID"],
                                EduClassID = (Guid)reader["EduClassID"],
                                TimeStamp = (DateTime)reader["TimeStamp"]
                            };

                            StudentBookings.Add(booking);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetAllStudentBookings));
                    throw;
                }
            }
            return StudentBookings;
        }

        public static List<StudentBookings> GetAllStudentBookingsForStudentID(Guid StudentID)
        {
            List<StudentBookings> StudentBookings = new List<StudentBookings>();
            string queryString = "SELECT * FROM dbo.StudentBookings WHERE StudentID = '" + StudentID.ToString() + "'";

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
                            StudentBookings booking = new StudentBookings
                            {
                                StudentID = (Guid)reader["StudentID"],
                                ReservationID = (Guid)reader["ReservationID"],
                                EduClassID = (Guid)reader["EduClassID"],
                                TimeStamp = (DateTime)reader["TimeStamp"]
                            };

                            StudentBookings.Add(booking);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetAllStudentBookings));
                    throw;
                }
            }
            return StudentBookings;
        }

        public static HttpStatusCode SaveBooking(StudentBookings booking)
        {
            if (booking == null)
            {
                return HttpStatusCode.BadRequest;
            }

            using (var connection = new SqlConnection(Utility.ConnectionString))

            {
                string queryString1 = "INSERT INTO dbo.StudentBookings(" +
                                     "[ReservationID], " +
                                     "[StudentID], " +
                                     "[EduClassID], " + 
                                     "[TimeStamp]" +
                                     ") VALUES('" +
                                     booking.ReservationID + "','" +
                                     booking.StudentID + "','" +
                                     booking.EduClassID + "','" +
                                     booking.TimeStamp + "'"
                                     + ")"; 

                var command = new SqlCommand(queryString1, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                        " Reservation ID: ({0}), Student ID: {1} enrolled in EduClassID {2} - SQL Save Successful.",
                        booking.ReservationID,
                        booking.StudentID,
                        booking.EduClassID
                       ), nameof(SaveBooking));
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(SaveBooking));
                    return HttpStatusCode.InternalServerError;
                }

                return HttpStatusCode.OK;

            }
        }

        public static async System.Threading.Tasks.Task<HttpStatusCode> SendClassBookingConfirmationEmail(EduClass eClass, Students student)
        {

            const string subject = "Your Reservation Is Confirmed";

            if (eClass != null && student != null)
            {
                var instructor = Instructor.GetInstructorFromInstructorId(eClass.InstructorId);
                var instructorEmail = Instructor.GetInstructorEmailFromInstructorId(eClass.InstructorId);

                string plainTextContent = @"Are you ready? 
                                    " + student.FirstName +
                                          @", we know you'll love the unique learning experience in store for you at AWI! Here's your booking information! " +
                                          "You have enrolled in: " + eClass.ClassTitle +
                                          "Date Of class: " + eClass.StartDateTime.ToShortDateString() +
                                          "Class Begins At: " + eClass.StartDateTime.TimeOfDay +
                                          " and runs until " + eClass.EndDateTime.TimeOfDay + ". You will want to arrive around 15 minutes early to get your gear and papaerwork squared away." 
                                          + @"We're located at 1802 W Grant Rd, #107, between I-10 and Silverbell, right next to Sherwin-Williams.
                                            If you are bringing your own gun, please make sure you are not bringing any ammo!
                                            We do not allow live ammunition in our facility, for safety reasons. Please bring your weapon in the locked-open position with NO AMMUNITION in your weapon or on your person.
                                            Please do not reply to this email. We will miss your message!                                
                                            ";


                string htmlContent = " <strong>Are You Ready?</strong>"
                                     + "<br /> <br />"
                                     + student.FirstName +
                                     @",we know you'll love the unique learning experience in store for you at AWI! Here's your booking information!"
                                     + "<br /> <br />"
                                     + "<strong>You have enrolled in:  </strong>" + eClass.ClassTitle
                                     + "<br /> <br />"
                                     + "<strong>Date Of Class: </strong>" + eClass.StartDateTime.Date
                                     + "<br /> <br />"
                                     + "<strong>Class Begins At: </strong>" + eClass.StartDateTime.TimeOfDay + " and runs until " + eClass.EndDateTime.TimeOfDay + ". You will want to arrive around 15 minutes early to get your gear and papaerwork squared away."
                                     + "<br /> <br />"
                                     + @"We're located at 1802 W Grant Rd, #107, between I-10 and Silverbell, right next to Sherwin-Williams. <a href=""https://goo.gl/maps/C8Cgi7iDp6gfDnyA9"">Google Maps </a> 
                                        <br /> <br /> 
                                        <strong> If you are bringing your own gun, please make sure you are not bringing any ammo! </strong> 
                                        <br /> We do not allow live ammunition in our facility, for safety reasons. Please bring your weapon in the locked-open position with NO AMMUNITION in your weapon or on your person. <br /> 
                                        <br />
                                        Please do not reply to this email. It is unmonitored. We will miss your message!   
                                      ";




                var response = Utility.SendReservationMessage(student.EmailAddress, subject, plainTextContent, htmlContent, instructorEmail, "attanaya@outlook.com");

                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User {0} has been sent a confirmation email to {1}. ", student.FirstName + " " + student.MiddleName + " " + student.LastName, student.EmailAddress), nameof(SendClassBookingConfirmationEmail));

                return response.StatusCode;
            }

            return HttpStatusCode.BadRequest;
        }

    }
}