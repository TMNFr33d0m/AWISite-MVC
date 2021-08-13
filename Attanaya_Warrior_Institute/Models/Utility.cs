using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using static System.DateTime;

namespace Attanaya_Warrior_Institute.Models
{
    public static class Utility
    {
        public static string ConnectionString { get; } =
            ConfigurationManager.ConnectionStrings["AzureDbConnection"].ConnectionString;

        public static DateTime GetCurrentTucsonTime()
        {
            var tucsonTimeZone = TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time");
            var tucsonTime = TimeZoneInfo.ConvertTime(SpecifyKind(UtcNow, DateTimeKind.Utc), TimeZoneInfo.Utc, tucsonTimeZone);

            return tucsonTime;
        }

        public static Guid ForceUserIDRetrievalFromName(string name)
        {
            Guid AccountId = Guid.Empty;

            string queryString = "SELECT Id FROM dbo.AspNetUsers WHERE UserName = '" + name + "';";

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var x = (string)reader["Id"];
                            AccountId = Guid.Parse(x);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(ForceUserIDRetrievalFromName));
                    throw;
                }
            }

            return AccountId;
        }

        public static Discounts GetDiscounts(string discountCode)
        {

            if (discountCode == null)
            {
                return new Discounts();
            }

            List<Discounts> discountCollection = new List<Discounts>();

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                // Get hours of Operation from DB, assemble in list.
                const string queryString1 = "SELECT * FROM dbo.Discounts;";

                var command = new SqlCommand(queryString1, connection);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Discounts discount = new Discounts
                            {
                                DiscountCode = (string)reader["DiscountCode"],
                                DiscountPercentage = (int)reader["DiscountPercentage"],
                                Description = (string)reader["Description"],
                                UseCount = (int)reader["UseCount"],
                                MaxUses = (int)reader["MaxUses"],
                                ExpirationDate = (DateTime?)reader["ExpirationDate"]
                            };

                            discountCollection.Add(discount);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetDiscounts));
                    throw;
                }
            }

            var validCodeCollection = discountCollection.Where(discount => discount.DiscountCode.ToUpper(CultureInfo.CurrentCulture).Equals(discountCode.ToUpper(CultureInfo.CurrentCulture), StringComparison.CurrentCulture))
                .Select(discount => discount);

            var validCode = validCodeCollection.FirstOrDefault();

            if (validCode == null)
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User attempted discount code {0}, but the code was not valid. ", discountCode), nameof(GetDiscounts));

                Discounts response = new Discounts
                {
                    DiscountPercentage = 0,
                    Description = "Invalid Discount Code. No such code."
                };

                return response;
            }

            // If max uses is set to 0, it's unlimited, and we don't evaluate any further on max uses.
            if (validCode.MaxUses != 0)
            {
                // Otherwise, we need to see if the use count has exceeded the max use count. If so, this code is no longer good. 
                if (validCode.UseCount >= validCode.MaxUses)
                {

                    LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User attempted to get {0}% discount with code: {1}, but the code was expired. Code has exceeded max uses.", validCode.DiscountPercentage.ToString(), discountCode), nameof(GetDiscounts));
                    Discounts response = new Discounts
                    {
                        DiscountPercentage = 0,
                        Description = "Invalid Discount Code. Code has been exhausted and is no longer valid."
                    };

                    return response;
                }
            }

            // If we made it this far, the code is either unlimited use count or within it's use count. Next we evaluate it's expiration date. 
            if (validCode.ExpirationDate < DateTime.Now)
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User attempted to get {0}% discount with code: {1}, but the code was expired. Code has exceeded expiration date.", validCode.DiscountPercentage.ToString(), discountCode), nameof(GetDiscounts));
                Discounts response = new Discounts
                {
                    DiscountPercentage = 0,
                    Description = "Invalid Discount Code. Code has expired and is no longer valid."
                };

                return response;
            }

            // If the code does not exceed max uses and is not expired, it's a valid code, so we apply it. 
            if (validCode.DiscountPercentage == 0)
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User attempted to get {0}% discount with code: {1}, but the code was worthless. Code has 0 value.", validCode.DiscountPercentage.ToString(), discountCode), nameof(GetDiscounts));
                Discounts response = new Discounts
                {
                    DiscountPercentage = 0,
                    Description = "Worthless Discount Code. Code appears to be valid, but has no value."
                };

                return response;
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User has been credited with a discount of {0} % with code: {1}, ", validCode.DiscountPercentage.ToString(), discountCode), nameof(GetDiscounts));
            return validCode;
        }

        public static void UpdateDiscountCodeCount(string discountCode)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var query = "Update[dbo].[Discounts] SET UseCount = UseCount + 1 WHERE DiscountCode = '" + discountCode + "'";
                    var command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(UpdateDiscountCodeCount));
            }
        }

        public static bool IsClosedOnDay(DateTime selectedDate)
        {

            string queryString3 = "SELECT " +
                                  "ClosedDate, " +
                                  "ReasonForClose " +
                                  "FROM dbo.ClosedDates WHERE ClosedDate ='" +
                                  selectedDate.ToShortDateString() + "'";

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString3, connection);
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            return true;
                        }

                        return false;
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(IsClosedOnDay));
                    return false;
                }
            }
        }


        public static RestResponse SendReservationMessage(string RecipientEmail, string Subject, string TextMessage, string HtmlMessage, string instructorCC, string facilityCC)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
            new HttpBasicAuthenticator("api",
                                       "a3e838f619fad2e579f9686399e4f3db-4d640632-ff3d181d");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "mail.attanaya.com", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Attanaya Warrior Institute <reservationbot@attanaya.com>");
            request.AddParameter("to", RecipientEmail);
            
            if (!string.IsNullOrEmpty(instructorCC))
            {
                request.AddParameter("cc", instructorCC);
            }

            if (!string.IsNullOrEmpty(facilityCC))
            {
                request.AddParameter("bcc", facilityCC);
            }

            request.AddParameter("subject", Subject);
            request.AddParameter("text", TextMessage);
            request.AddParameter("html", HtmlMessage);
            request.Method = Method.POST;

            var x = (RestResponse)client.Execute(request);

            return x;
        }

        public static RestResponse SendSystemMessage(string RecipientEmail, string Subject, string TextMessage, string HtmlMessage)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
            new HttpBasicAuthenticator("api",
                                       "a3e838f619fad2e579f9686399e4f3db-4d640632-ff3d181d");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "mail.attanaya.com", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Attanaya Warrior Institute <system@attanaya.com>");
            request.AddParameter("to", RecipientEmail);
            request.AddParameter("subject", Subject);
            request.AddParameter("text", TextMessage);
            request.AddParameter("html", HtmlMessage);
            request.Method = Method.POST;

            var x = (RestResponse)client.Execute(request);

            return x;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }


    }
}

