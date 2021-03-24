using System;
using System.Security.Cryptography;
using System.Text;

namespace Piovra.Web {
    public static class WsseeSoapHeaderCreator {        
        public static string CreateSoapHeader(string userName, string password) {
            var nonceBase64 = GetNonceBase64();
            
            var createdAt = FormatTZ(DateTime.UtcNow);

            var passwordDigest = GeneratePasswordDigest(nonceBase64, createdAt, password);

            var result = new StringBuilder(@"<soapenv:Envelope xmlns:sear=""http://www.remotesite.com/serviceName"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">");
            result.Append(@"<soapenv:Header>");
            result.Append(@"<wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">");
            result.Append(@$"<wsse:UsernameToken wsu:Id=""UsernameToken-{Guid.NewGuid().ToString("N").ToUpper()}"">");
            result.Append(@$"<wsse:Username>{userName}</wsse:Username>");
            result.Append(@$"<wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">{passwordDigest}</wsse:Password>");
            result.Append(@$"<wsse:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">{nonceBase64}</wsse:Nonce>");
            result.Append(@$"<wsu:Created>{createdAt}</wsu:Created>");
            result.Append(@"</wsse:UsernameToken>");
            result.Append(@"</wsse:Security>");
            result.Append(@"</soapenv:Header>");

            return result.ToString();
        }        

        public static string GeneratePasswordDigest(string password) {
            var nonceBase64 = GetNonceBase64();
            var createdAtString = FormatTZ(DateTime.UtcNow);
            return GeneratePasswordDigest(nonceBase64, createdAtString, password);
        }

        public static string GeneratePasswordDigest(string nonceBase64, DateTime createdAt, string password) {
            var utcCreatedAt = createdAt.ToUniversalTime();
            return GeneratePasswordDigest(nonceBase64, FormatTZ(utcCreatedAt), password);
        }

        public static string GeneratePasswordDigest(string nonceBase64, string createdAt, string password) {
            var nonceBytes = Convert.FromBase64String(nonceBase64);

            var createdAtBytes = Encoding.UTF8.GetBytes(createdAt);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            var result = new byte[nonceBytes.Length + createdAtBytes.Length + passwordBytes.Length];

            Array.Copy(nonceBytes, result, nonceBytes.Length);
            Array.Copy(createdAtBytes, 0, result, nonceBytes.Length, createdAtBytes.Length);
            Array.Copy(passwordBytes, 0, result, nonceBytes.Length + createdAtBytes.Length, passwordBytes.Length);

            return Convert.ToBase64String(Hash(result));

            static byte[] Hash(byte[] buffer) => SHA1.Create().ComputeHash(buffer);
        }

        public static string GetNonceBase64() {
            var bytes = GetNonceBytes();
            return Convert.ToBase64String(bytes);
        }

        static byte[] GetNonceBytes() {
            var result = new byte[16];
            var random = new Random(DateTime.Now.Millisecond);
            random.NextBytes(result);
            return result;
        }

        static string FormatTZ(DateTime t) => t.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
    }
}
