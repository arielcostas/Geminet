using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Costasdev.Geminet.Config;
using Microsoft.Extensions.Logging;

namespace Costasdev.Geminet;

public class CertificateUtility
{
    private readonly ConfigRoot _configFile;
    private readonly ILogger _logger;
    private readonly X509Certificate2Collection _certificates;

    public CertificateUtility(ConfigRoot configFile)
    {
        _configFile = configFile;
        _logger = Loggers.CreateLogger("CERTUTIL");
        _certificates = new X509Certificate2Collection();
        if (File.Exists(_configFile.CertFile))
        {
            _certificates.Import(_configFile.CertFile, _configFile.CertPassword);
        }
    }

    /**
     * Returns the certificate for the specified host. If the certificate does not exist, it is generated, saved and returned.
     */
    public X509Certificate2 GetCertificateForHost(string host)
    {
        var cert = _certificates
            .Find(X509FindType.FindBySerialNumber, GenerateSerialNumber(host), true)
            .FirstOrDefault();

        return cert ?? GenerateCertificate(host);
    }

    /**
     * Generates a new certificate for the specified host.
     */
    private X509Certificate2 GenerateCertificate(string host)
    {
        _logger.LogInformation("Generating a new certificate for host {}", host);
        var rsa = RSA.Create(4096);

        var req = new CertificateRequest(
            $"CN={host}, {_configFile.CertOptions}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1
        );

        // Creates a self-signed certificate with the private key and a custom serial number.
        var cert = req.Create(
            req.SubjectName,
            X509SignatureGenerator.CreateForRSA(rsa, RSASignaturePadding.Pkcs1),
            DateTimeOffset.Now,
            DateTimeOffset.Now.AddYears(3),
            GenerateSerialNumber(host)
        );

        _logger.LogInformation("Saving certificate...");
        X509Certificate2Collection certCollection = new() { cert };

        if (File.Exists(_configFile.CertFile))
        {
            certCollection.Import(_configFile.CertFile, _configFile.CertPassword);
        }

        certCollection.Export(X509ContentType.Pfx, _configFile.CertPassword);

        _logger.LogInformation("Certificate ok!");
        return cert;
    }

    /**
     * Generates a deterministic serial number for the specified host. This is used to identify the certificate for the
     * host later. Using SHA256 we prevent collisions, at least in most cases and the serial number is 20 bytes long.
     */
    private byte[] GenerateSerialNumber(string host)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(host));
        var serial = new byte[20];
        Array.Copy(hash, serial, 20);
        return serial;
    }
}