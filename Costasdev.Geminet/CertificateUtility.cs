using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Costasdev.Geminet.Config;

namespace Costasdev.Geminet;

public class CertificateUtility
{
    private string _certFile;
    private string _certPassword;

    public CertificateUtility(string certFile, string certPassword)
    {
        _certFile = certFile;
        _certPassword = certPassword;
    }

    public X509Certificate2 GetCertificateForHost(string host)
    {
        if (CertificateExists(host))
        {
            var path = Path.Join(_certFile, host + ".pfx");

            return new X509Certificate2(path, _certPassword);
        }

        return GenerateCertificate(host);
    }

    private bool CertificateExists(string host)
    {
        var expectedCertificatePath = Path.Join(_certFile, host + ".pfx");
        return File.Exists(expectedCertificatePath);
    }

    private X509Certificate2 GenerateCertificate(string host)
    {
        var rsa = RSA.Create(4096);

        var req = new CertificateRequest(
            $"CN={host}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1
        );

        var cert = req.CreateSelfSigned(
            DateTimeOffset.Now, DateTimeOffset.Now.AddMonths(12)
        );

        var certBytes = cert.Export(X509ContentType.Pfx, _certPassword);
        var certPath = Path.Combine(_certFile, $"{host}.pfx");

        File.WriteAllBytes(certPath, certBytes);

        return cert;
    }
}