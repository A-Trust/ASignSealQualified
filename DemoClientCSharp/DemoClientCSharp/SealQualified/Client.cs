using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

namespace DemoClientCSharp.SealQualified
{
    public class Client
    {
        private AsymmetricKeyParameter AuthenticationCertificatePrivateKey;
        private X509Certificate AuthenticationCertificateCertificate;
        private string baseurl;

        public Client(string targetsystem)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            baseurl = targetsystem.TrimEnd('/');
            AuthenticationCertificatePrivateKey = null;
            AuthenticationCertificateCertificate = null;
        }


        public bool Init(string authentication_certificate_path, string authentication_certificate_password)
        {
            AuthenticationCertificatePrivateKey = null;
            AuthenticationCertificateCertificate = null;

            try
            {
                using (var cert = new FileStream(authentication_certificate_path, FileMode.Open, FileAccess.Read))
                {
                    var p12 = new Pkcs12StoreBuilder().Build();
                    p12.Load(cert, authentication_certificate_password.ToCharArray());

                    foreach (string alias in p12.Aliases)
                    {
                        if (p12.IsKeyEntry(alias))
                        {
                            AuthenticationCertificatePrivateKey = p12.GetKey(alias).Key;
                            break;
                        }
                    }

                    foreach (string alias in p12.Aliases)
                    {
                        X509CertificateEntry entry = p12.GetCertificate(alias);
                        if (VerifyPivateAndPublicKey(entry.Certificate.GetPublicKey(), AuthenticationCertificatePrivateKey))
                        {
                            AuthenticationCertificateCertificate = p12.GetCertificate(alias).Certificate;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static bool VerifyPivateAndPublicKey(AsymmetricKeyParameter pubkey, AsymmetricKeyParameter privkey)
        {
            if (null == pubkey)
                return false;

            if (null == privkey)
                return false;
            try
            {
                SecureRandom rand = new SecureRandom();
                byte[] message = Encoding.UTF8.GetBytes("correspondence prove for AsymmetricKeyParameter");
                ISigner signer = SignerUtilities.GetSigner("SHA-256withRSA");

                signer.Init(true, new ParametersWithRandom(privkey, rand));
                signer.BlockUpdate(message, 0, message.Length);
                byte[] sigBytes = signer.GenerateSignature();
                signer.Init(false, pubkey);
                signer.BlockUpdate(message, 0, message.Length);
                if (signer.VerifySignature(sigBytes))
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }


        public byte[] GetSealCertificate(string sid = null)
        {
            if (null == sid)
                sid = "dummy";

            if (null == AuthenticationCertificateCertificate)
                return null;

            string serial = AuthenticationCertificateCertificate.SerialNumber.ToString(10);
            string url = $"{baseurl}/Certificate/{HttpUtility.UrlEncode(serial)}/{HttpUtility.UrlEncode(sid)}";

            byte[] cert = null;
            if (Get(url, out cert))
            {
                return cert;
            }
            return null;
        }


        public byte[] Sign(byte[] hashToSign, string sid = null)
        {
            if (null == sid)
                sid = "dummy";

            if (null == AuthenticationCertificateCertificate)
                return null;

            if (null == AuthenticationCertificatePrivateKey)
                return null;

            const string HashSignatureMechanism = "SHA256withRSA";

            // prepare Request
            ISigner signer = SignerUtilities.GetSigner(HashSignatureMechanism);
            signer.Init(true, AuthenticationCertificatePrivateKey);
            signer.BlockUpdate(hashToSign, 0, hashToSign.Length);
            var HashSignature = signer.GenerateSignature();

            var o = new Signature_Request();
            o.AuthSerial = AuthenticationCertificateCertificate.SerialNumber.ToString(10);
            o.Hash = Convert.ToBase64String(hashToSign);
            o.HashSignature = Convert.ToBase64String(HashSignature);
            o.HashSignatureMechanism = HashSignatureMechanism;
            var reqStr = JsonSerializer.Serialize(o);

            string url = $"{baseurl}/Sign/{HttpUtility.UrlEncode(sid)}";

            if (!PostJson(url, reqStr, out string ResponseData))
            {
                return null;
            }

            var respObj = JsonSerializer.Deserialize<Signature_Response>(ResponseData);
            if (null != respObj)
            {
                return Convert.FromBase64String(respObj.Signature);
            }

            return null;
        }



        public SignatureData[] SignBatch(HashData[] list, string sid = null)
        {
            if (null == sid)
                sid = "dummy";

            if (null == AuthenticationCertificateCertificate)
                return null;

            if (null == AuthenticationCertificatePrivateKey)
                return null;

            var data = JsonSerializer.Serialize(list.ToArray());
            var data2Sign = Encoding.UTF8.GetBytes(data);


            const string HashSignatureMechanism = "SHA256withRSA";

            // prepare Request
            ISigner signer = SignerUtilities.GetSigner(HashSignatureMechanism);
            signer.Init(true, AuthenticationCertificatePrivateKey);
            signer.BlockUpdate(data2Sign, 0, data2Sign.Length);
            var HashSignature = signer.GenerateSignature();

            var o = new BatchSign_Request();
            o.AuthSerial = AuthenticationCertificateCertificate.SerialNumber.ToString(10);
            o.ListOfHashes = Convert.ToBase64String(data2Sign);
            o.HashSignature = Convert.ToBase64String(HashSignature);
            o.HashSignatureMechanism = HashSignatureMechanism;

            var url = $"{baseurl}/Batch/Sign/{HttpUtility.UrlEncode(sid)}";
            string ResponseData = "";
            if (!PostJson(url, JsonSerializer.Serialize(o), out ResponseData))
            {
                return null;
            }

            var respObj = JsonSerializer.Deserialize<BatchSign_Response>(ResponseData);
            var sig = respObj.ListOfSignature;

            if (null == sig || sig.Length <= 0)
            {
                return null;
            }

            return sig;
        }


        private static bool Get(string getUrl, out byte[] buffer)
        {
            buffer = null;
            try
            {
                Uri url = new Uri(getUrl);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "GET";
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                long len = webResponse.ContentLength;
                buffer = new byte[len];
                webResponse.GetResponseStream().Read(buffer, 0, buffer.Length);
            }
            catch (WebException wex)
            {
                if (WebExceptionStatus.ProtocolError == wex.Status)
                {
                    HttpWebResponse webResponse = (HttpWebResponse)wex.Response;
                    string ResponseData = ""; 
                    using (var reader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        ResponseData = reader.ReadToEnd();
                    }
                    if (!string.IsNullOrEmpty(ResponseData))
                    {
                        //TODO: error text
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        private static bool PostJson(string postUrl, string content, out string ResponseData)
        {
            ResponseData = null;

            byte[] rawContent = System.Text.Encoding.UTF8.GetBytes(content);

            try
            {
                Uri url = new Uri(postUrl);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = rawContent.Length;
                webRequest.GetRequestStream().Write(rawContent, 0, rawContent.Length);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                using (var reader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    ResponseData = reader.ReadToEnd();
                }
            }
            catch (WebException wex)
            {
                if (WebExceptionStatus.ProtocolError == wex.Status)
                {
                    HttpWebResponse webResponse = (HttpWebResponse)wex.Response;
                    using (var reader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        ResponseData = reader.ReadToEnd();
                    }

                    if (!string.IsNullOrEmpty(ResponseData))
                    {
                        //TODO: error text
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
