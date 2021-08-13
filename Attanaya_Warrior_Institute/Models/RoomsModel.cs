using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Attanaya_Warrior_Institute.Models
{
    public class Rooms
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int PricePerHour { get; set; }
        public string Description { get; set; }
        public int IsPublic { get; set; }

        public static IEnumerable<Rooms> GetAvailableRooms()
        {
            List<Rooms> listOfRooms = new List<Rooms>();
            string queryString = "SELECT * FROM dbo.Rooms;";

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
                            Rooms room = new Rooms
                            {   ID = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                Capacity = (int)reader["Capacity"],
                                PricePerHour = (int)reader["PricePerHour"],
                                Description = (string)reader["Description"],
                                IsPublic = (int)reader["IsPublic"]
                            };

                            listOfRooms.Add(room);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetAvailableRooms));
                    throw;
                }
            }

            return listOfRooms;
        }

        public static List<Rooms> GetAvailableRoomsInList()
        {
            List<Rooms> listOfRooms = new List<Rooms>();
            string queryString = "SELECT * FROM dbo.Rooms;";

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
                            Rooms room = new Rooms
                            {
                                ID = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                Capacity = (int)reader["Capacity"],
                                PricePerHour = (int)reader["PricePerHour"],
                                Description = (string)reader["Description"],
                                IsPublic = (int)reader["IsPublic"]
                            };

                            listOfRooms.Add(room);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetAvailableRooms));
                    throw;
                }
            }

            return listOfRooms;
        }

        public static Rooms GetRoomFromRoomName(string roomName)
        {
            Rooms Payload = new Rooms(); 

            string queryString = "SELECT * FROM dbo.Rooms where Name = '"+ roomName + "'";

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
                            Rooms room = new Rooms
                            {
                                ID = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                Capacity = (int)reader["Capacity"],
                                PricePerHour = (int)reader["PricePerHour"],
                                Description = (string)reader["Description"],
                                IsPublic = (int)reader["IsPublic"]
                            };

                            Payload = room;
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(GetAvailableRooms));
                    throw;
                }
            }
            return Payload;
        }


    }
}