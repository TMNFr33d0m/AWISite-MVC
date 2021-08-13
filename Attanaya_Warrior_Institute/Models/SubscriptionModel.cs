using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Attanaya_Warrior_Institute.Models
{
    public class SubscriptionModel
    {
        public Guid RecordID { get; set; }
        public Guid AccountID { get; set; }
        public string SubscriptionID { get; set; }
        public string SubscriptionType { get; set; }
        public DateTime SubscriptionStartDate { get; set; }

        public static SubscriptionModel GetSubscriptionByAccountID (Guid accountId)
        {
            SubscriptionModel sub = new SubscriptionModel();

            string queryString = "SELECT * FROM dbo.Subscriptions WHERE AccountID = '" + accountId + "'";

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
                            sub.RecordID = (Guid)reader["RecordId"];
                            sub.AccountID = (Guid)reader["AccountID"];
                            sub.SubscriptionID = (string)reader["SubscriptionID"];
                            sub.SubscriptionType = (string)reader["SubscriptionType"];
                            sub.SubscriptionStartDate = (DateTime)reader["SubscriptionStartDate"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetSubscriptionByAccountID));
                    throw;
                }
            }

            return sub;  
        }

        public static int GetPlanCountByPlanID(string planId)
        {
            int result = 0; 
            string queryString = "SELECT Count(*) AS ExistingSubCount FROM dbo.Subscriptions WHERE SubscriptionType = '" + planId + "'";

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
                            result = (int)reader["ExistingSubCount"];

                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetSubscriptionByAccountID));
                    throw;
                }
            }

            return result;
        }

        public HttpStatusCodeResult CreateNewSubscription(SubscriptionModel sub)
        {
            if (sub == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                using (var connection = new SqlConnection(Utility.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[CreateSubscription]", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = sub.AccountID;
                        cmd.Parameters.Add("@SubscriptionID", SqlDbType.NVarChar).Value = sub.SubscriptionID;
                        cmd.Parameters.Add("@SubscriptionType", SqlDbType.NVarChar).Value = sub.SubscriptionType;
                        cmd.Parameters.Add("@SubscriptionStartDate", SqlDbType.DateTime).Value = sub.SubscriptionStartDate;


                        connection.Open();
                        cmd.ExecuteNonQuery();

                        LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                            "Account ID: {0} has created a new subscription! SubID: {1} -  ",
                            sub.AccountID, sub.SubscriptionID), nameof(CreateNewSubscription));
                    }
                }
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(CreateNewSubscription));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }

    }
}