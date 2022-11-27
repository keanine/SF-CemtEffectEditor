using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_CemtEffectEditor.Cemt
{
    public class CemtColorData : BinaryData
    {
        public enum ColorFormat { RGBA, RGB, Invalid }
        public ColorFormat format = ColorFormat.RGBA;

        public float r;
        public float g;
        public float b;
        public float a;

        public int valueDistance;

        private int rOffset;
        private int gOffset;
        private int bOffset;
        private int aOffset;

        public CemtColorData(int offset, int valueDistance) : base(offset)
        {
            this.valueDistance = valueDistance;

            rOffset = offset + (0 * valueDistance);
            gOffset = offset + (1 * valueDistance);
            bOffset = offset + (2 * valueDistance);
            aOffset = offset + (3 * valueDistance);
        }

        public void Load(byte[] cemtFile)
        {
            if (offset < 0x1000)
            {
                r = BitConverter.ToSingle(cemtFile, rOffset);
                g = BitConverter.ToSingle(cemtFile, gOffset);
                b = BitConverter.ToSingle(cemtFile, bOffset);
                a = BitConverter.ToSingle(cemtFile, aOffset);
            }
            else if (cemtFile.Length > bOffset)
            {
                r = BitConverter.ToSingle(cemtFile, rOffset);
                g = BitConverter.ToSingle(cemtFile, gOffset);
                b = BitConverter.ToSingle(cemtFile, bOffset);
                format = ColorFormat.RGB;
            }
            else
            {
                r = 1000;
                g = 1000;
                b = 1000;
                format = ColorFormat.Invalid;
            }

            if (format == ColorFormat.RGB)
            {
                a = 0;
            }
        }

        public void Save(ref byte[] cemtFile)
        {
            if (format == ColorFormat.Invalid)
                return;

            byte[] newRBytes = BitConverter.GetBytes(r);
            byte[] newGBytes = BitConverter.GetBytes(g);
            byte[] newBBytes = BitConverter.GetBytes(b);
            byte[] newABytes = BitConverter.GetBytes(a);

            for (int i = 0; i < 4; i++)
            {
                cemtFile[rOffset + i] = newRBytes[i];
                cemtFile[gOffset + i] = newGBytes[i];
                cemtFile[bOffset + i] = newBBytes[i];

                if (format == ColorFormat.RGBA)
                {
                    cemtFile[aOffset + i] = newABytes[i];
                }
            }
        }

        public override string ToString()
        {
            if (IsWithinBounds())
            {
                if (format == ColorFormat.RGBA)
                {
                    return floatToHex(r).ToString("X2") + floatToHex(g).ToString("X2") + floatToHex(b).ToString("X2") + floatToHex(a).ToString("X2");
                }
                else if(format == ColorFormat.RGB)
                {
                    return floatToHex(r).ToString("X2") + floatToHex(g).ToString("X2") + floatToHex(b).ToString("X2");
                }
            }

            return "N/A";
        }

        public bool IsWithinBounds()
        {
            return r <= 1 && g <= 1 && b <= 1 && a <= 1
                && r >= 0 && g >= 0 && b >= 0 && a >= 0;
        }

        private int floatToHex(float f)
        {
            float temp = f * 255f;
            return (int)temp;
        }

        public System.Windows.Media.Color GetRGBASystemColor()
        {
            if (format == ColorFormat.RGB)
                return GetRGBSystemColor();

            System.Windows.Media.Color color = new System.Windows.Media.Color();
            if (IsWithinBounds())
            {
                color.ScR = r;
                color.ScG = g;
                color.ScB = b;
                color.ScA = a;
            }
            else
            {
                color.ScA = 0;
            }

            return color;
        }

        public System.Windows.Media.Color GetRGBSystemColor()
        {
            System.Windows.Media.Color color = new System.Windows.Media.Color();
            if (IsWithinBounds())
            {
                color.ScR = r;
                color.ScG = g;
                color.ScB = b;
                color.ScA = 1;
            }
            else
            {
                color.ScA = 0;
            }

            return color;
        }

        public void Set(float r, float g, float b, float a)
        {
            if (format == ColorFormat.RGBA)
            {
                this.a = a;
            }

            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}
