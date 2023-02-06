namespace StreamSubscriber
{
    public interface IAppSettings
    {
        string HostName { get; }
        string UserName { get; }
        string Password { get; }
    }
    public class AppSettings : IAppSettings
    {
        required public string HostName { get; set; }
        required public string UserName { get; set; }
        required public string Password { get; set; }
    }
}
