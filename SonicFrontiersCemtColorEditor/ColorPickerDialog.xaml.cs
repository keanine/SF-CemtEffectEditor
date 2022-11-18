using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SonicFrontiersCemtColorEditor
{
    /// <summary>
    /// Interaction logic for ColorPickerDialog.xaml
    /// </summary>
    public partial class ColorPickerDialog : Window
    {
        public ColorPickerDialog()
        {
            InitializeComponent();
        }

        public ColorPickerDialog(Color initialColor)
        {
            InitializeComponent();
            Color = initialColor;
        }

        private Color color;

        public Color Color
        {
            get
            {
                color = new Color();
                color.ScR = (float)picker.ColorState.RGB_R;
                color.ScG = (float)picker.ColorState.RGB_G;
                color.ScB = (float)picker.ColorState.RGB_B;
                color.ScA = (float)picker.ColorState.A;
                return color;
            }
            set
            {
                picker.ColorState.SetARGB(value.A, value.R, value.G, value.B);
            }
        }

        private void SelectColor_Click(object sender, RoutedEventArgs e)
        {
            color = Color;
            this.DialogResult = true;
            this.Close();
        }

        private void picker_ColorChanged(object sender, RoutedEventArgs e)
        {
            colorPreview.Fill = new SolidColorBrush(Color);
        }
    }
}
