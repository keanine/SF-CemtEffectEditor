using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_CemtEffectEditor.Cemt
{
    public class BinaryData
    {
        public int offset;

        public BinaryData(int offset)
        {
            this.offset = offset;
        }

        public string GetOffsetAsString()
        {
            return offset.ToString("X");
        }
    }
}
