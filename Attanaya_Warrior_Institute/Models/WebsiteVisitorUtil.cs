using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Attanaya_Warrior_Institute.Models
{
    public class WebsiteVisitorUtil
    {

        #region Private Variables

        private readonly string _connectionString;
        #endregion
        
        public WebsiteVisitorUtil() { }

        public WebsiteVisitorUtil(string connectionString)
        {
            _connectionString = connectionString;

        }

        #region Database Properties

        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public DateTime DateLastVisited { get; set; }
        public int TotalTimesVisited { get; set; }

        #endregion Database Properties

        #region Model Properties

        #endregion Model Properties 

        #region Calculated Properties

        #endregion Calculated Properties

        #region Navigation Propterties

        #endregion Navigation Properties

        #region Methods

        public WebsiteVisitorUtil GetVisitorRecord(
            string ipAddress
        )
        {
            return GetVisitorRecordSet(
                    ipAddress: ipAddress
                )
                .SingleOrDefault();
        }

        public List<WebsiteVisitorUtil> GetVisitorRecordSet(
            string ipAddress
        )
        {
            var modelSet = new List<WebsiteVisitorUtil>();

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.CommandText = "GetVisitorRecord";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;

                    command.Parameters.Add("@ipAddress", SqlDbType.VarChar).Value = ipAddress;

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WebsiteVisitorUtil model = new WebsiteVisitorUtil(_connectionString)
                            {
                                IpAddress = (string)reader["IPAddress"],
                                HostName = (string)reader["HostName"],
                                DateLastVisited = (DateTime)reader["DateLastVisited"],
                                TotalTimesVisited = (int)reader["TotalTimesVisited"],
                            };

                            modelSet.Add(model);
                        }
                    }
                }
            }

            return modelSet;
        }

        public void WriteVisitorRecord(
            string ip, 
            string hostname
            )
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.CommandText = "MergeVisitorRecord";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;

                    command.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = ip;
                    command.Parameters.Add("@Hostname", SqlDbType.VarChar).Value = hostname;

                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion Methods

    }
}