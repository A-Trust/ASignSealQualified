using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClientCSharp.SealQualified
{
    public sealed class Signature_Request
    { 
        public string AuthSerial { get; set; }
        public string Hash { get; set; }
        public string HashSignature { get; set; }
        public string HashSignatureMechanism { get; set; }
    }
}
