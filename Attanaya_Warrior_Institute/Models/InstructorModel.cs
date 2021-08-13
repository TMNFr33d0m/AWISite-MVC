using System;
using System.Data.SqlClient;

namespace Attanaya_Warrior_Institute.Models
{
    public class Instructor
    {

        public Guid InstructorId { get; set; }
        public Guid AccountID {get; set;}
        public string InstructorCompanyName { get; set; }
        public DateTime AgreementStartDate { get; set; }
        public DateTime AgreementEndDate { get; set; }
        public int PartnershipPercentage { get; set; }
        public int FreeSimulationHoursRemaining { get; set; }
        public string DiscountCode { get; set; }

        /// <summary>
        /// Populates a new instructor object with data based on the Instructor ID
        /// </summary>
        /// <param name="instructorId"></param>
        /// <returns></returns>
        public static Instructor GetInstructorFromInstructorId(Guid instructorId)
        {
            Instructor result = new Instructor(); 

            string queryString = "SELECT * FROM dbo.Instructors WHERE InstructorID = '" + instructorId.ToString() + "';";

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
                            result.InstructorId = (Guid)reader["InstructorId"];
                            result.AccountID = (Guid)reader["AccountID"];
                            result.InstructorCompanyName = (string)reader["InstructorCompanyName"];
                            result.AgreementStartDate = (DateTime)reader["AgreementDateStart"];
                            result.AgreementEndDate = (DateTime)reader["AgreementEndDate"];
                            result.PartnershipPercentage = (int)reader["PartnershipPercentage"];
                            result.FreeSimulationHoursRemaining = (int)reader["FreeSimulationHoursRemaining"];
                            result.DiscountCode = (string)reader["DiscountCode"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetInstructorFromInstructorId));
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// Populates a new instructor object with data based on the Account ID
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static Instructor GetInstructorFromAccountId(Guid accountId)
        {
            Instructor result = new Instructor();

            string queryString = "SELECT * FROM dbo.Instructors WHERE AccountId = '" + accountId.ToString() + "';";

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
                            result.InstructorId = (Guid)reader["InstructorId"];
                            result.AccountID = (Guid)reader["AccountID"];
                            result.InstructorCompanyName = (string)reader["InstructorCompanyName"];
                            result.AgreementStartDate = (DateTime)reader["AgreementDateStart"];
                            result.AgreementEndDate = (DateTime)reader["AgreementEndDate"];
                            result.PartnershipPercentage = (int)reader["PartnershipPercentage"];
                            result.FreeSimulationHoursRemaining = (int)reader["FreeSimulationHoursRemaining"];
                            result.DiscountCode = (string)reader["DiscountCode"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetInstructorFromAccountId));
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instructorId"></param>
        /// <returns></returns>
        public static string GetInstructorEmailFromInstructorId(Guid instructorId)
        {

            // First, convert the instuctor ID to an account ID. We can only match the email on an account ID. 
            string accountId = string.Empty;
            string emailAddress = string.Empty;
            string queryString = "SELECT AccountId FROM dbo.Instructors WHERE InstructorID = '" + instructorId.ToString() + "';";

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
                            accountId = (string)reader["AccountId"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetInstructorEmailFromInstructorId));
                    throw;
                }
            }

            // Then, go get the email associated with the instructor account. 
            string queryString2 = "SELECT Email FROM dbo.AspNetUsers WHERE Id = '" + accountId + "';";

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
                            emailAddress = (string)reader["Email"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetInstructorEmailFromInstructorId));
                    throw;
                }
            }

            // Return the email address
            return emailAddress;
        }

    }
}