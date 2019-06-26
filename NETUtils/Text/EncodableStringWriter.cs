using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace De.JanRoslan.NETUtils.Text {
    public class EncodableStringWriter : StringWriter
    {
        public override System.Text.Encoding Encoding { get; }

        public EncodableStringWriter(System.Text.Encoding encoding)
        {
            Encoding = encoding;
        }
    }
}
