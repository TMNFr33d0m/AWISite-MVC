namespace Attanaya_Warrior_Institute.Settings
{
    public interface IWebConfig
    {
        IDatabaseSettings DatabaseSettings { get; }

        IApplicationSettings ApplicationSettings { get; }
    }
}