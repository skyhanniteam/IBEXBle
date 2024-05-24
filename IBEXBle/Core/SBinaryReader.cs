using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IBEXBle.Core
{
    public class SBinaryReader : BinaryReader
    {
        public SBinaryReader(Stream input) : base(input)
        {
        }

        public SBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {            
        }

        public string ReadString(int count)
        {
            var c = new char[count];
            var b = this.ReadBytes(count);
            for (int i = 0; i < count; i++)
                c[i] = (char)b[i];            
            return new string(c);
        }
    }
}
