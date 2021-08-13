using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;

namespace Attanaya_Warrior_Institute.Models
{
    public class Students
    {
        [Key]
        public Guid StudentID { get; set; }
        public Guid AccountID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public List<Students> GetAllStudents()
        {
            List<Students> Students = new List<Students>();
            string queryString = "SELECT * FROM dbo.ClassBookings;";

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
                            Students student = new Students
                            {
                                StudentID = (Guid)reader["StudentID"],
                                AccountID = (Guid)reader["AccountID"],
                                FirstName = (string)reader["FirstName"],
                                MiddleName = (string)reader["MiddleName"],
                                LastName = (string)reader["LastName"],
                                EmailAddress = (string)reader["EmailAddress"],
                                PhoneNumber = (string)reader["PhoneNumber"],

                            };

                            Students.Add(student);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetAllStudents));
                    throw;
                }
            }

            return Students;
        }

        public static Students GetStudentByStudentID(Guid StudentID)
        {
           
            string queryString = "SELECT * FROM [dbo].[Students] where [StudentID] = '" + StudentID+"';";
            Students student = new Students(); 

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

                            student.StudentID = (Guid)reader["StudentID"];
                            student.AccountID = (Guid)reader["AccountID"];
                            student.FirstName = (string)reader["FirstName"];
                            student.MiddleName = (string)reader["MiddleName"];
                            student.LastName = (string)reader["LastName"];
                            student.EmailAddress = (string)reader["EmailAddress"];
                            student.PhoneNumber = (string)reader["PhoneNumber"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetAllStudents));
                    throw;
                }
            }

            return student;
        }

        public static Students GetStudentByAccountID(Guid AccountID)
        {

            string queryString = "SELECT * FROM [dbo].[Students] where [AccountID] = '" + AccountID + "';";
            Students student = new Students();

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

                            student.StudentID = (Guid)reader["StudentID"];
                            student.AccountID = (Guid)reader["AccountID"];
                            student.FirstName = (string)reader["FirstName"];
                            student.MiddleName = (string)reader["MiddleName"];
                            student.LastName = (string)reader["LastName"];
                            student.EmailAddress = (string)reader["EmailAddress"];
                            student.PhoneNumber = (string)reader["PhoneNumber"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetAllStudents));
                    throw;
                }
            }

            return student;
        }

        public static Guid GetStudentIdFromAccountId(Guid AccountID)
        {
            Guid StudentID = Guid.Empty;
            string queryString = "SELECT [StudentID] FROM [dbo].[Students] where [AccountID] = '" + AccountID+"'";

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
                            StudentID = (Guid)reader["StudentID"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetAllStudents));
                    throw;
                }
            }

            return StudentID;
        }


        public static HttpStatusCode SaveStudent(Students student)
        {
            if (student == null)
            {
                return HttpStatusCode.BadRequest;
            }

            using (var connection = new SqlConnection(Utility.ConnectionString))

            {
                string queryString1 = "INSERT INTO dbo.Students(" +
                                     "[StudentID], " +
                                     "[AccountID], " +
                                     "[FirstName], " +
                                     "[MiddleName], " +
                                     "[LastName], " +
                                     "[EmailAddress], " +
                                     "[PhoneNumber]" +
                                     ") VALUES ('" +
                                     student.StudentID + "','" +
                                     student.AccountID + "','" +
                                     student.FirstName + "','" +
                                     student.MiddleName + "','" +
                                     student.LastName + "','" +
                                     student.EmailAddress + "','" +
                                     student.PhoneNumber + "'"
                                     + ")";

                var command = new SqlCommand(queryString1, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                        "{0} ({1}) enrolled as an AWI student. ",
                        student.FirstName + " " + student.MiddleName + " " + student.LastName,
                        student.StudentID), nameof(SaveStudent));
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(SaveStudent));
                    return HttpStatusCode.InternalServerError;
                }

                return HttpStatusCode.OK;

            }
        }


    }
}