using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace Costasdev.Geminet.Config
{
    public class ConfigRoot
    {
        [JsonProperty("certFile")] public string CertFile { get; set; }

        [JsonProperty("certPassword")] public string CertPassword { get; set; }

        [JsonProperty("certSubject")] public CertificateSubject CertOptions { get; set; }

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

    public class CertificateSubject
    {
        [JsonProperty("c")]
        public string? Country { get; set; }
        
        [JsonProperty("o")]
        public string? Organization { get; set; }
        
        [JsonProperty("ou")]
        public string? OrganizationalUnit { get; set; }
        
        [JsonProperty("st")]
        public string? State { get; set; }
     
        [JsonProperty("locality")]
        public string? Locality { get; set; }

        public override string ToString()
        {
            List<String> parts = new();
            
            if (!string.IsNullOrEmpty(Organization))
            {
                parts.Add($"O={Organization}");
            }
            
            if (!string.IsNullOrEmpty(OrganizationalUnit))
            {
                parts.Add($"OU={OrganizationalUnit}");
            }
            
            if (!string.IsNullOrEmpty(Country))
            {
                parts.Add($"C={Country}");
            }
            
            if (!string.IsNullOrEmpty(State))
            {
                parts.Add($"ST={State}");
            }
            
            if (!string.IsNullOrEmpty(Locality))
            {
                parts.Add($"L={Locality}");
            }
            
            return string.Join(", ", parts);
        }
    }
}