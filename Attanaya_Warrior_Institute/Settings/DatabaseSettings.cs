using System.Configuration;

namespace Attanaya_Warrior_Institute.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public DatabaseSettings()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["AzureDbConnection"].ConnectionString;
        }

        public string ConnectionString { get; private set; }
    }
}