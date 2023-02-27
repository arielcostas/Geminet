using Newtonsoft.Json;

namespace Costasdev.Geminet.Config
{
    public class ConfigRoot
    {
        [JsonProperty("certRoot")] public string CertRoot { get; set; }

        [JsonProperty("certPassword")] public string CertPassword { get; set; }

        [JsonProperty("sites")] public ConfigSite[] Sites { get; set; }
    }


    public class ConfigSite
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("hostname")] public string Hostname { get; set; }

        [JsonProperty("listen")] public Int32? Listen { get; set; }

        [JsonProperty("serve")] public string Serve { get; set; }

        [JsonProperty("index")] public Boolean? GenerateIndex { get; set; }
    }
}