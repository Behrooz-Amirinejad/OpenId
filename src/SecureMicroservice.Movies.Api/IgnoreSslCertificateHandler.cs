using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class IgnoreSslCertificateHandler : HttpClientHandler
{
    public IgnoreSslCertificateHandler()
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
    }
}