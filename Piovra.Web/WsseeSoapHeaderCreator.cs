using System;
using System.Security.Cryptography;
using System.Text;

namespace Piovra.Web {
    public static class WsseeSoapHeaderCreator {
        public static string CreateSoapHeader(string userName, string password) => CreateSoapHeader(userName, password, new Options());        

        public static string CreateSoapHeader(string userName, string password, Options options) {
            var nonceBase64 = GetNonceBase64();
            var createdAt = FormatTZ(DateTime.UtcNow);
            var passwordDigest = GeneratePasswordDigest(nonceBase64, createdAt, password);
            var userToken = Guid.NewGuid().ToString("N").ToUpper();

            var result = new StringBuilder(@$"<{options.SoapNamespaceName}:Header>");            
            result.Append(@$"<wsse:Security {options.SoapNamespaceName}:mustUnderstand=""1"" xmlns:wsse=""{options.WsseNamespace}"" xmlns:wsu=""{options.WsuNamespace}"">");
            result.Append(@$"<wsse:UsernameToken wsu:Id=""UsernameToken-{userToken}"">");
            result.Append(@$"<wsse:Username>{userName}</wsse:Username>");
            result.Append(@$"<wsse:Password Type=""{options.WssePasswordType}"">{passwordDigest}</wsse:Password>");
            result.Append(@$"<wsse:Nonce EncodingType=""{options.WsseNonceEncodingType}"">{nonceBase64}</wsse:Nonce>");
            result.Append(@$"<wsu:Created>{createdAt}</wsu:Created>");
            result.Append(@"</wsse:UsernameToken>");
            result.Append(@"</wsse:Security>");
            result.Append(@$"</{options.SoapNamespaceName}:Header>");

            return result.ToString();
        }  
        
        public class Options {
            public string SoapNamespaceName { get; set; } = DEFAULT_SOAP_NAMESPACE_NAME;
            public string WsseNamespace { get; set; } = DEFAULT_WSSE_NAMESPACE;
            public string WsuNamespace { get; set; } = DEFAULT_WSU_NAMESPACE;
            public string WssePasswordType { get; set; } = DEFAULT_WSSE_PASSWORD_TYPE;
            public string WsseNonceEncodingType { get; set; } = DEFAULT_WSSE_NONCE_ENCODING_TYPE;            

            public const string DEFAULT_SOAP_NAMESPACE_NAME = "soap";
            public const string DEFAULT_WSSE_NAMESPACE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            public const string DEFAULT_WSU_NAMESPACE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
            public const string DEFAULT_WSSE_PASSWORD_TYPE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest";
            public const string DEFAULT_WSSE_NONCE_ENCODING_TYPE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
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
