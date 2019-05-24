using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DemoClientCSharp
{
    public partial class MainForm : Form
    {
        private static Random random = new Random();
        private byte[] seal_cert; 

        public MainForm()
        {
            InitializeComponent();
            seal_cert = null; 
            show_seal_cert.Enabled = false; 
        }

        private void button_load_auth_cert_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Certificate files|*.p12;*.pfx|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    path_load_auth_cert.Text = openFileDialog.FileName;
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            targetsystem.Items.Clear();
            targetsystem.Items.Add(new MyListItem("test system", "https://hs-abnahme.a-trust.at/SealQualified/v1"));
            targetsystem.Items.Add(new MyListItem("live system", "https://www.a-trust.at/SealQualified/v1"));
            targetsystem.SelectedIndex = 0; 
        }

        public static string RandomString()
        {            
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 32).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private SealQualified.Client InitClient()
        {
            var ts = targetsystem.SelectedItem as MyListItem;
            var c = new SealQualified.Client(ts.Value);
            if (!c.Init(path_load_auth_cert.Text, auth_cert_pwd.Text))
            {
                WriteText("error init SealQualified client");
                WriteText("wrong password for authentication certificate?");
                return null;
            }
            return c; 
        }

        private void buttonGetCertificate_Click(object sender, EventArgs e)
        {
            seal_cert = null;
            show_seal_cert.Enabled = false;

            var c = InitClient();
            if (null == c)
                return;

            byte[] cert = c.GetSealCertificate();
            if (null == cert)
            {
                WriteText("get seal certificate failed");
            }
            else
            {
                WriteText("Certificate: " + Convert.ToBase64String(cert));
                seal_cert = cert; 
                show_seal_cert.Enabled = true;
            }
        }

        private void buttonSignTest_Click(object sender, EventArgs e)
        {
            // perpare simple hash value to sign
            byte[] DataToSign = Encoding.UTF8.GetBytes("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.");
            var hashAlgo = new Sha256Digest();
            hashAlgo.BlockUpdate(DataToSign, 0, DataToSign.Length);
            byte[] hashToSign = new byte[hashAlgo.GetDigestSize()];
            hashAlgo.DoFinal(hashToSign, 0);


            var c = InitClient();
            if (null == c)
                return;

            var signature = c.Sign(hashToSign);

            if (null == signature)
            {
                WriteText("seal sign failed");
            }
            else
            {
                WriteText("signature: " + Convert.ToBase64String(signature));
            }

            if (null != seal_cert)
            {
                VerifySignature(DataToSign, signature);
            }
        }

        private void VerifySignature(byte[] DataToSign, byte[] signature)
        {
            // verify signature
            WriteText("start verify signature");

            // get public key from seal certificate
            var p = new Org.BouncyCastle.X509.X509CertificateParser();
            var x509 = p.ReadCertificate(seal_cert);

            // return value of signature is juse r and s buffers appended
            // for bouncycastle VerifySignature to work we need an ASN1 Sequence
            int len = signature.Length / 2;
            BigInteger sssrVal = new BigInteger(signature, 0, len);
            BigInteger rVal = new BigInteger(1, signature, 0, len);
            BigInteger sssVal = new BigInteger(signature, len, len);
            BigInteger sVal = new BigInteger(1, signature, len, len);
            DerSequence seq = new DerSequence(new DerInteger(rVal), new DerInteger(sVal));
            var signatureAsSequence = seq.GetDerEncoded();

            // actual verify
            ISigner signer = SignerUtilities.GetSigner("SHA256withECDSA");
            signer.Init(false, x509.GetPublicKey());
            signer.BlockUpdate(DataToSign, 0, DataToSign.Length);
            bool result = signer.VerifySignature(signatureAsSequence);
            WriteText("signature verification = " + result.ToString());
        }

        private void WriteText(string msg)
        {
            textBoxResult.AppendText(msg + "\r\n");
        }

        private void show_seal_cert_Click(object sender, EventArgs e)
        {
            if(null == seal_cert)
            {
                show_seal_cert.Enabled = false;
                return; 
            }

            var c = new System.Security.Cryptography.X509Certificates.X509Certificate2(seal_cert);
            System.Security.Cryptography.X509Certificates.X509Certificate2UI.DisplayCertificate(c);
        }
    }
}
