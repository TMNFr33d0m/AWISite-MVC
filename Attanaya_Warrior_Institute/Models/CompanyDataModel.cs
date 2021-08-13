using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;

namespace Attanaya_Warrior_Institute.Models
{
    public class HoursOfOperation
    {
        public string DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }

        public static List<HoursOfOperation> GetHoursOfOperation()
        {
            List<HoursOfOperation> hoursOfOperation = new List<HoursOfOperation>();

            // Get hours of Operation from DB, assemble in list.
            string queryString1 = "SELECT * FROM dbo.HoursOfOperation;";
            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString1, connection);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HoursOfOperation day = new HoursOfOperation
                            {
                                DayOfWeek = (string)reader["DayOfWeek"],
                                OpenTime = (TimeSpan)reader["OpenTime"],
                                CloseTime = (TimeSpan)reader["CloseTime"]
                            };

                            hoursOfOperation.Add(day);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetHoursOfOperation));
                    throw;
                }
            }

            return hoursOfOperation;
        }
    }

    public class AvailableTimeSlots
    {
        public string StartTimeSlot { get; set; }
        public string EndTimeSlot { get; set; }
    }

    public class Discounts
    {
        public string DiscountCode { get; set; }
        public int DiscountPercentage { get; set; }
        public string Description { get; set; }
        public int UseCount { get; set; }
        public int MaxUses { get; set; }
        public DateTime? ExpirationDate { get; set; }

        internal static HttpStatusCodeResult UpdateDiscountCount(string discountCode)
        {
            try
            {
                using (var connection = new SqlConnection(Utility.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateDiscountUseCount", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DiscountCode", SqlDbType.NVarChar).Value = discountCode;

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        LoggingModel.LogMessage(string.Format("Discount Code {0} incremented", discountCode), nameof(UpdateDiscountCount));
                        return new HttpStatusCodeResult(HttpStatusCode.OK);

                    }
                }
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(UpdateDiscountCount));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }


        }
    }

    public class ClosedDates
    {
        public DateTime ClosedDate { get; set; }
        public string ReasonForClose { get; set; }

        public static HttpStatusCodeResult AddNewClosedDate(DateTime CloseDate, string ReasonForClose)
        {
            //ToDo: Isn't it obvious, fucker? 
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public static List<ClosedDates> GetClosedDates()
        {
            List<ClosedDates> Payload = new List<ClosedDates>();

            // Get hours of Operation from DB, assemble in list.
            string queryString1 = "SELECT * FROM dbo.ClosedDates;";
            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString1, connection);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClosedDates day = new ClosedDates
                            {
                                ClosedDate = (DateTime)reader["ClosedDate"],
                                ReasonForClose = (string)reader["ReasonForClose"]
                            };

                            Payload.Add(day);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetClosedDates));
                    throw;
                }
            }

            return Payload;
        }

    }
}