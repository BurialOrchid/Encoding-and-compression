using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace List5Exercise3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow : Window
    {
        private byte[] pictureBytes;

        public MainWindow()
        {
            InitializeComponent();
            LoadFile();
        }

        private void LoadFile()
        {
            string filepath = "../../../Flower_640x500_8bit_Grayscale.raw";
            pictureBytes = File.ReadAllBytes(filepath);
            ImagePanel.Source = LoadImage(640, 500, pictureBytes);
        }

        private BitmapSource LoadImage(int width, int height, byte[] imageData)
        {
            var format = PixelFormats.Gray8;
            var stride = (width * format.BitsPerPixel + 7) / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, imageData, stride);
        }
    }
}