using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClientCSharp.SealQualified
{
    public sealed class BatchSign_Response
    {
        public SignatureData[] ListOfSignature { get; set; }
    }
}
