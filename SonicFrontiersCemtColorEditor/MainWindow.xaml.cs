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
        string[] blacklistedFiles =
        {
            "ec_so_spinatk01_dist01.cemt",
            "ef_so_spinatk01.cemt",
            "ef_so_spinatk01_end01.cemt"
        };

        string folderPath = string.Empty;
        List<string> cemtFiles = new List<string>();

        public static int colorOffset = 0x308;
        public static int secondColorOffset = 0x328;
         
        public static int rOffset = 0;
        public static int gOffset = 8;
        public static int bOffset = 16;
        public static int aOffset = 24;

        List<CemtData> cemtData = new List<CemtData>();

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
            //int newR = int.Parse(colorText.Text.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            //int newG = int.Parse(colorText.Text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            //int newB = int.Parse(colorText.Text.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            ////int newA = int.Parse(colorText.Text.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            //byte[] newRBytes = BitConverter.GetBytes(newR / 255f);
            //byte[] newGBytes = BitConverter.GetBytes(newG / 255f);
            //byte[] newBBytes = BitConverter.GetBytes(newB / 255f);
            ////byte[] newABytes = BitConverter.GetBytes(newA / 255f);

            //byte[] bytes = File.ReadAllBytes(file);

            //for (int i = 0; i < 4; i++)
            //{
            //    bytes[i + offset + rOffset] = newRBytes[i];
            //    bytes[i + offset + gOffset] = newGBytes[i];
            //    bytes[i + offset + bOffset] = newBBytes[i];
            //    //bytes[i + offset + aOffset] = newABytes[i];
            //}

            //File.WriteAllBytes(file, bytes);
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
                    if (!blacklistedFiles.Contains(fileName))
                    {
                        cemtFiles.Add(file);
                        outputText.Text += fileName + " | 1: " + GenerateColorCodeFromOffset(file, colorOffset) + " | 2: " + GenerateColorCodeFromOffset(file, secondColorOffset) + "\n";

                        string name = Path.GetFileName(file).Substring(fileName.IndexOf("spinatk01_"));
                        name = name.Substring(9, name.Length - 9 - ".cemt".Length);
                        CreateCemtEntry(jumpBallEntries, name, file, GenerateColorFromOffset(file, colorOffset), GenerateColorFromOffset(file, secondColorOffset));
                    }
                }
            }
        }

        private string GenerateColorCodeFromOffset(string file, int offset)
        {
            byte[] bytes = File.ReadAllBytes(file);

            string r = ((int)(BitConverter.ToSingle(bytes, offset + rOffset) * 255f)).ToString("X2");
            string g = ((int)(BitConverter.ToSingle(bytes, offset + gOffset) * 255f)).ToString("X2");
            string b = ((int)(BitConverter.ToSingle(bytes, offset + bOffset) * 255f)).ToString("X2");
            //string a = ((int)(BitConverter.ToSingle(bytes, offset + aOffset) * 255f)).ToString("X2");

            return r + g + b;
        }

        private Color GenerateColorFromOffset(string file, int offset)
        {
            byte[] bytes = File.ReadAllBytes(file);

            Color color = new Color();

            color.ScR = BitConverter.ToSingle(bytes, offset + rOffset);
            color.ScG = BitConverter.ToSingle(bytes, offset + gOffset);
            color.ScB = BitConverter.ToSingle(bytes, offset + bOffset);
            color.ScA = 1;

            return color;
        }



        private void CreateCemtEntry(Panel panel, string name, string filepath, Color firstColor, Color secondColor)
        {
            StackPanel stack = new StackPanel();
            stack.Width = 140;

            Label label = new Label();
            Rectangle color1 = new Rectangle();
            Rectangle color2 = new Rectangle();

            label.Content = name;
            label.Width = 140;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;

            color1.Width = 110;
            color1.Height = 20;
            color1.Fill = new SolidColorBrush(firstColor);
            color1.Stroke = new SolidColorBrush(Colors.DarkGray);
            color1.MouseUp += CemtEntryColor_Click;

            color2.Width = 110;
            color2.Height = 20;
            color2.Fill = new SolidColorBrush(secondColor);
            color2.Stroke = new SolidColorBrush(Colors.DarkGray);
            color2.MouseUp += CemtEntryColor_Click;

            stack.Children.Add(label);
            stack.Children.Add(color1);
            stack.Children.Add(color2);

            panel.Children.Add(stack);

            CemtData data = new CemtData(name, filepath, color1, color2);
            cemtData.Add(data);
        }

        private void CemtEntryColor_Click(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;

            Color initialColor = ((SolidColorBrush)rect.Fill).Color;
            ColorPickerDialog dialog = new ColorPickerDialog(initialColor);
            bool? result = dialog.ShowDialog();
        
            if (result.HasValue && result.Value)
            {
                rect.Fill = new SolidColorBrush(dialog.Color);
            }
        }

        private void SaveJumpBall_Click(object sender, RoutedEventArgs e)
        {
            foreach (var data in cemtData)
            {
                data.SaveDataToFile();
            }
        }
    }
}