namespace Attanaya_Warrior_Institute.Settings
{
    public class WebConfig : IWebConfig
    {
        public WebConfig(IDatabaseSettings dbSettings, IApplicationSettings appSettings)
        {
            DatabaseSettings = dbSettings;
            ApplicationSettings = appSettings;
        }

        public IDatabaseSettings DatabaseSettings { get; private set; }

        public IApplicationSettings ApplicationSettings { get; private set; }
    }
}