using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Attanaya_Warrior_Institute.Models
{
    public class LoggingModel
    {
        public LoggingModel(DateTime timeStamp)
        {
            TimeStamp = timeStamp;
        }

        private LoggingModel()
        {
            TimeStamp = DateTime.Now;
        }

        [DisplayName("Time Stamp:")]
        public DateTime TimeStamp { get; }

        [DisplayName("Type:")]
        public EventLogEntryType Type { get; set; }

        [DisplayName("User Info:")]
        public string UserInfo { get; set; }

        [DisplayName("Message:")]
        public string Message { get; set; }

        [DisplayName("Inner Ex:")]
        public string InnerException { get; set; }

        [DisplayName("Source Method:")]
        public string SourceMethod { get; set; }

        [DisplayName("Debug Level:")]
        public DebugLevel DebugLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static bool Log(LoggingModel model)
        {
            if (model == null)
            {
                return false;
            }

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {

                var queryString1 = "INSERT INTO dbo.Logging(" +
                                   "[type], " +
                                   "[userInfo], " +
                                   "[message], " +
                                   "[innerException], " +
                                   "[sourceMethod], " +
                                   "[debugLevel] " +
                                   ") VALUES('" +
                                   model.Type + "','" +
                                   model.UserInfo + "','" +
                                   model.Message + "','" +
                                   model.InnerException + "','" +
                                   model.SourceMethod + "','" +
                                   model.DebugLevel + "'" +
                                    ")";

                var command = new SqlCommand(queryString1, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }

        public static bool LogCriticalException(
            Exception e, 
            string method, 
            string userInfo = "SYSTEM")
        {
            return Log (new LoggingModel
            {
                Type = EventLogEntryType.Error,
                UserInfo = userInfo,
                Message = e.ToString(),
                InnerException = e.InnerException?.ToString(),
                SourceMethod = method,
                DebugLevel = DebugLevel.Critical
            });
        }

        public static bool LogMessage(
            string message, 
            string method, 
            string userInfo = "SYSTEM")
        {
            return Log(new LoggingModel
            {
                Type = EventLogEntryType.Information,
                UserInfo = userInfo,
                Message = message,
                InnerException = null,
                SourceMethod = method,
                DebugLevel = DebugLevel.Info
            });
        }

    }

    public enum DebugLevel
    {
        Noisy = 1, 
        Info = 2, 
        Debug = 3, 
        Critical = 4

    }
}