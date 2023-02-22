namespace Costasdev.Geminet.Config
{
    public class ConfigRoot
    {
        public ConfigSite[] sites { get; set; }
    }


    public class ConfigSite
    {
        public string name { get; set; }
        public string hostname { get; set; }
        public int listen { get; set; }
        public string serve { get; set; }
    }

}
