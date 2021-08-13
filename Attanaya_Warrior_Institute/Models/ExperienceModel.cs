using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Attanaya_Warrior_Institute.Models
{
    public class Experience
    {

        public int ExperienceID { get; set; }
        public string ExperienceTitle { get; set; }
        public string ExperienceImageLink { get; set; }
        public int ExperiencePrice { get; set; }
        public string EquippedRoomIdArray { get; set; }
    
    
        public static Experience GetExperienceByID (int experienceId)
        {
            Experience experience = new Experience();


            string queryString = "SELECT * FROM dbo.Experiences WHERE ExperienceID = '" + experienceId + "'";

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
                            experience.ExperienceID = (int)reader["ExperienceID"];
                            experience.ExperienceTitle = (string)reader["ExperienceTitle"];
                            experience.ExperienceImageLink = (string)reader["ExperienceImageLink"];
                            experience.ExperiencePrice = (int)reader["ExperiencePrice"];
                            experience.EquippedRoomIdArray = (string)reader["EquippedRoomIdArray"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetExperienceByID));
                    throw;
                }
            }

            return experience;
        }

        public static List<Experience> GetAllExperiences()
        {
            List<Experience> experiences = new List<Experience>();


            string queryString = "SELECT * FROM dbo.Experiences";

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
                            Experience experience = new Experience(); 

                            experience.ExperienceID = (int)reader["ExperienceID"];
                            experience.ExperienceTitle = (string)reader["ExperienceTitle"];
                            experience.ExperienceImageLink = (string)reader["ExperienceImageLink"];
                            experience.ExperiencePrice = (int)reader["ExperiencePrice"];
                            experience.EquippedRoomIdArray = (string)reader["EquippedRoomIdArray"];

                            experiences.Add(experience);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetExperienceByID));
                    throw;
                }
            }

            return experiences;
        }


    }
}