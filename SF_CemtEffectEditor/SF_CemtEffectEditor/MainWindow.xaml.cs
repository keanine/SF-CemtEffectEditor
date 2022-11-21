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

namespace SF_CemtEffectEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<CemtData> allData = new List<CemtData>();

        public string defaultFile = @"C:\Games\Steam\steamapps\common\SonicFrontiers\Mods\Testing Colours\raw\EffectCommon\ec_ft_pow_dash_ge01_plight01.cemt";
        public string defaultFolder = @"C:\Games\Steam\steamapps\common\SonicFrontiers\Mods\Testing Colours\raw\character\sonic";

        public MainWindow()
        {
            InitializeComponent();
            CemtFilepathTextbox.Text = defaultFile;
            CemtFolderpathTextbox.Text = defaultFolder;
        }

        private void LoadCemtButton_Click(object sender, RoutedEventArgs e)
        {
            CemtColorStackPanel.Children.Clear();
            string file = CemtFilepathTextbox.Text;
            LoadFile(file);
        }

        private void LoadCemtFolderButton_Click(object sender, RoutedEventArgs e)
        {
            CemtColorStackPanel.Children.Clear();
            string folder = CemtFolderpathTextbox.Text;
            string[] files = Directory.GetFiles(folder);

            foreach (string file in files)
            {
                LoadFile(file);
            }
        }

        private void LoadFile(string file)
        {
            if (file.EndsWith(".cemt"))
            {
                CemtData data = new CemtData(file);
                allData.Add(data);

                CreateCemtHeaderEntry(CemtColorStackPanel, Path.GetFileName(file), "");

                foreach (var color in data.colors)
                {
                    CreateColorEntry(CemtColorStackPanel, color, Path.GetFileName(file));
                }
            }
        }

        private void CreateCemtHeaderEntry(Panel panel, string name, string version)
        {
            TextBlock label = new TextBlock();
            label.Width = 330;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.TextAlignment = TextAlignment.Left;
            label.Inlines.Add(new Bold(new Run(name)));

            panel.Children.Add(label);
        }

        private void CreateColorEntry(Panel panel, Cemt.CemtColorData colorData, string filename)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            TextBlock label = new TextBlock();
            label.Text = "Offset: " + colorData.GetOffsetAsString();
            label.Margin = new Thickness(0, 0, 5, 0);
            label.Width = 110;
            label.TextAlignment = TextAlignment.Right;
            label.FontFamily = new FontFamily("Consolas");
            label.VerticalAlignment = VerticalAlignment.Center;

            Rectangle rectangle = new Rectangle();
            rectangle.Width = 110;
            rectangle.Height = 20;
            rectangle.Fill = new SolidColorBrush(colorData.GetRGBASystemColor());
            if (colorData.IsWithinBounds()) 
                rectangle.Stroke = new SolidColorBrush(Colors.DarkGray);
            else 
                rectangle.Stroke = new SolidColorBrush(Colors.Transparent);

            Rectangle rectangle2 = new Rectangle();
            rectangle2.Width = 110;
            rectangle2.Height = 20;
            rectangle2.Fill = new SolidColorBrush(colorData.GetRGBSystemColor());
            if (colorData.IsWithinBounds()) 
                rectangle2.Stroke = new SolidColorBrush(Colors.DarkGray);
            else 
                rectangle2.Stroke = new SolidColorBrush(Colors.Transparent);


            TextBlock colorHex = new TextBlock();
            if (colorData.format == Cemt.CemtColorData.ColorFormat.RGBA) 
                colorHex.Text = "RGBA: " + colorData.ToString();
            else 
                colorHex.Text = " RGB: " + colorData.ToString();

            colorHex.Margin = new Thickness(5, 0, 0, 0);
            colorHex.Width = 110;
            colorHex.TextAlignment = TextAlignment.Left;
            colorHex.FontFamily = new FontFamily("Consolas");
            colorHex.VerticalAlignment = VerticalAlignment.Center;

            stackPanel.Children.Add(label);
            stackPanel.Children.Add(rectangle);
            stackPanel.Children.Add(rectangle2);
            stackPanel.Children.Add(colorHex);

            panel.Children.Add(stackPanel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var data in allData)
            {
                data.colors[0].Set(1, 1, 1, 1);
                data.colors[1].Set(1, 1, 1, 1);

                data.colors[2].Set(1, 1, 1, 0);
                data.colors[3].Set(1, 1, 1, 0);

                data.SaveToFile();
            }
        }
    }
}
