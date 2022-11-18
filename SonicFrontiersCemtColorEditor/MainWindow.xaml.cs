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

        int colorOffset = 0x308;
        int secondColorOffset = 0x328;

        int rOffset = 0;
        int gOffset = 8;
        int bOffset = 16;
        int aOffset = 24;

        public MainWindow()
        {
            InitializeComponent();
            outputText.Text = colorOffset.ToString();
        }

        private void EditColor_Click(object sender, RoutedEventArgs e)
        {
            foreach (string file in cemtFiles)
            {
                SetColor(file, colorOffset);
            }
        }

        private void EditSecondSet_Click(object sender, RoutedEventArgs e)
        {
            foreach (string file in cemtFiles)
            {
                SetColor(file, secondColorOffset);
            }
        }

        private void SetColor(string file, int offset)
        {
            int newR = int.Parse(colorText.Text.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int newG = int.Parse(colorText.Text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int newB = int.Parse(colorText.Text.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            int newA = int.Parse(colorText.Text.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            byte[] newRBytes = BitConverter.GetBytes(newR / 255f);
            byte[] newGBytes = BitConverter.GetBytes(newG / 255f);
            byte[] newBBytes = BitConverter.GetBytes(newB / 255f);
            byte[] newABytes = BitConverter.GetBytes(newA / 255f);

            byte[] bytes = File.ReadAllBytes(file);

            for (int i = 0; i < 4; i++)
            {
                bytes[i + offset + rOffset] = newRBytes[i];
                bytes[i + offset + gOffset] = newGBytes[i];
                bytes[i + offset + bOffset] = newBBytes[i];
                bytes[i + offset + aOffset] = newABytes[i];
            }

            File.WriteAllBytes(file, bytes);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            folderPath = fileText.Text;

            string[] allFiles = Directory.GetFiles(folderPath);
            outputText.Text = string.Empty;

            foreach (string file in allFiles)
            {
                string fileName = Path.GetFileName(file);
                if (fileName.EndsWith(".cemt") && fileName.Contains("spinatk01"))
                {
                    cemtFiles.Add(file);
                    outputText.Text += fileName + "\n";
                }
            }
        }
    }
}
