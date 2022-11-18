using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SonicFrontiersCemtColorEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string folderPath = string.Empty;
        List<string> cemtFiles = new List<string>();

        int rOffset = 776; //HEX 308
        int gOffset = 784; //HEX 310
        int bOffset = 792; //HEX 318
        int aOffset = 800; //HEX 320

        public MainWindow()
        {
            InitializeComponent();
        }

        private void EditColor_Click(object sender, RoutedEventArgs e)
        {
            SetColors();
            //string filepath = fileText.Text;
            //byte[] bytes = File.ReadAllBytes(filepath);

            //int newR = int.Parse(colorText.Text.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            //int newG = int.Parse(colorText.Text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            //int newB = int.Parse(colorText.Text.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //int newA = int.Parse(colorText.Text.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            ////outputText.Text = (newA).ToString();

            ////float currentA = BitConverter.ToSingle(bytes, aOffset);
            ////outputText.Text = currentA.ToString();

            //byte[] newRBytes = BitConverter.GetBytes(newR / 255f);
            //byte[] newGBytes = BitConverter.GetBytes(newG / 255f);
            //byte[] newBBytes = BitConverter.GetBytes(newB / 255f);
            //byte[] newABytes = BitConverter.GetBytes(newA / 255f);

            //for (int i = 0; i < 4; i++)
            //{
            //    bytes[i + rOffset] = newRBytes[i];
            //    bytes[i + gOffset] = newGBytes[i];
            //    bytes[i + bOffset] = newBBytes[i];
            //    bytes[i + aOffset] = newABytes[i];
            //}

            ////float currentR = BitConverter.ToSingle(bytes, rOffset);
            ////outputText.Text = currentR.ToString();
            //File.WriteAllBytes(filepath, bytes);
        }

        private void SetColors()
        {
            int newR = int.Parse(colorText.Text.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int newG = int.Parse(colorText.Text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int newB = int.Parse(colorText.Text.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            int newA = int.Parse(colorText.Text.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            byte[] newRBytes = BitConverter.GetBytes(newR / 255f);
            byte[] newGBytes = BitConverter.GetBytes(newG / 255f);
            byte[] newBBytes = BitConverter.GetBytes(newB / 255f);
            byte[] newABytes = BitConverter.GetBytes(newA / 255f);

            foreach (string file in cemtFiles)
            {
                byte[] bytes = File.ReadAllBytes(file);

                for (int i = 0; i < 4; i++)
                {
                    bytes[i + rOffset] = newRBytes[i];
                    bytes[i + gOffset] = newGBytes[i];
                    bytes[i + bOffset] = newBBytes[i];
                    bytes[i + aOffset] = newABytes[i];
                }

                File.WriteAllBytes(file, bytes);
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            folderPath = fileText.Text;

            string[] allFiles = Directory.GetFiles(folderPath);

            foreach (string file in allFiles)
            {
                string fileName = Path.GetFileName(file);
                if (fileName.EndsWith(".cemt") && fileName.Contains("spinatk01"))
                {
                    cemtFiles.Add(file);
                }
            }
        }
    }
}
