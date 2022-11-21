using SF_CemtEffectEditor.Cemt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_CemtEffectEditor
{
    public class CemtData
    {
        string path;
        byte[] bytes;

        public CemtColorData[] colors =
        {
            new CemtColorData(offset: 0x308,  valueDistance: 0x08),
            new CemtColorData(offset: 0x328,  valueDistance: 0x08),
            new CemtColorData(offset: 0x30C,  valueDistance: 0x08),
            new CemtColorData(offset: 0x32C,  valueDistance: 0x08),
            new CemtColorData(offset: 0x688,  valueDistance: 0x08),
            new CemtColorData(offset: 0x690,  valueDistance: 0x08),
            new CemtColorData(offset: 0x3688, valueDistance: 0x2C),
            new CemtColorData(offset: 0x369C, valueDistance: 0x2C)
        };

        public CemtData(string file)
        {
            path = file;
            bytes = File.ReadAllBytes(file);
            Load(bytes);
        }

        public void Load(byte[] cemtFile)
        {
            foreach (var color in colors)
            {
                color.Load(cemtFile);
            }
        }

        public void SaveToFile()
        {
            foreach (var color in colors)
            {
                color.Save(ref bytes);
            }

            File.WriteAllBytes(path, bytes);
        }

        public override string ToString()
        {
            string output = string.Empty;

            foreach (var color in colors)
            {
                output += color.ToString() + "\n";
            }

            return output;
        }
    }


    
}
