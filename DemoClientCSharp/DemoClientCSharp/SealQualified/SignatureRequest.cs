using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoClientCSharp.SealQualified
{
    class SignatureRequest
    {
        public string AuthSerial;
        public string Hash;
        public string HashSignature;
        public string HashSignatureMechanism; 
    }
}
