using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SonicFrontiersCemtColorEditor
{
    public class CemtData
    {
        public string filename;
        public string filepath;
        public Rectangle color1;
        public Rectangle color2;

        public CemtData(string name, string filename, Rectangle color1, Rectangle color2)
        {
            this.filename = name;
            this.filepath = filename;
            this.color1 = color1;
            this.color2 = color2;
        }

        public void SaveDataToFile()
        {
            SetColor(filepath, MainWindow.colorOffset, GetRectColor(color1));
            SetColor(filepath, MainWindow.secondColorOffset, GetRectColor(color2));
        }

        private Color GetRectColor(Rectangle rect)
        {
            return ((SolidColorBrush)rect.Fill).Color;
        }

        private void SetColor(string file, int offset, Color color)
        {
            byte[] newRBytes = BitConverter.GetBytes(color.ScR);
            byte[] newGBytes = BitConverter.GetBytes(color.ScG);
            byte[] newBBytes = BitConverter.GetBytes(color.ScB);
            //byte[] newABytes = BitConverter.GetBytes(newA / 255f);

            byte[] bytes = File.ReadAllBytes(file);

            for (int i = 0; i < 4; i++)
            {
                bytes[i + offset + MainWindow.rOffset] = newRBytes[i];
                bytes[i + offset + MainWindow.gOffset] = newGBytes[i];
                bytes[i + offset + MainWindow.bOffset] = newBBytes[i];
                //bytes[i + offset + aOffset] = newABytes[i];
            }

            File.WriteAllBytes(file, bytes);
        }
    }
}
