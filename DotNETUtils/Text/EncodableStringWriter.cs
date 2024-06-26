﻿using System.IO;
using System.Text;

namespace Roslan.DotNetUtils.Text {



    /// <summary>
    /// See https://stackoverflow.com/a/1564727 for why this class exists
    /// </summary>
    public class EncodableStringWriter : StringWriter
    {
        public override System.Text.Encoding Encoding { get; }

        public EncodableStringWriter(System.Text.Encoding encoding)
        {
            Encoding = encoding;

        }
    }
}
