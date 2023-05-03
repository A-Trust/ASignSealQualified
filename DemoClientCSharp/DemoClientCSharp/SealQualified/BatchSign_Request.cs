using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClientCSharp.SealQualified
{
    public sealed class BatchSign_Request
    {
        public string? AuthCert { get; set; }
        public string AuthSerial { get; set; }
        public string ListOfHashes { get; set; }
        public string HashSignature { get; set; }
        public string HashSignatureMechanism { get; set; }
    }
}
