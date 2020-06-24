using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace List5Exercise3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow : Window
    {
        private byte[] pictureBytes;
        private byte[] differenceBytes;
        private int pictureHeight;
        private int pictureWidth;

        public MainWindow()
        {
            InitializeComponent();
        }

        private BitmapSource LoadImage(int width, int height, byte[] imageData)
        {
            var format = PixelFormats.Gray8;
            var stride = (width * format.BitsPerPixel + 7) / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, imageData, stride);
        }

        private void LoadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select RAW FILE",
                Filter = "RAW files|*.raw"
            };
            if (openFileDialog.ShowDialog() == true)
                pictureBytes = File.ReadAllBytes(openFileDialog.FileName);
            FileNameValueLbl.Content = openFileDialog.SafeFileName;

            differenceBytes = CalculateDifferenceBytes(pictureBytes);
        }

        private void SizeConfBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Int32.TryParse(HeightTxtBox.Text, out pictureHeight))
            {
                HeightTxtBox.Foreground = Brushes.Red;
            }
            if (!Int32.TryParse(WidthTxtBox.Text, out pictureWidth))
            {
                WidthTxtBox.Foreground = Brushes.Red;
            }

            SizeValueLbl.Content = $"{pictureWidth}x{pictureHeight}";
        }

        private void TxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Foreground = Brushes.Black;
        }

        private void ShowPictureBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pictureHeight <= 0 || pictureWidth <= 0)
            {
                MessageBox.Show("Height or width isn't set", "List 5 Exercise 3 Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (pictureBytes == null)
            {
                MessageBox.Show("Picture not Selected", "List 5 Exercise 3 Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else { ImagePanel.Source = LoadImage(pictureWidth, pictureHeight, pictureBytes); }
        }

        private void DifferenceImageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pictureHeight <= 0 || pictureWidth <= 0)
            {
                MessageBox.Show("Height or width isn't set", "List 5 Exercise 3 Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (differenceBytes == null)
            {
                MessageBox.Show("Picture not Selected", "List 5 Exercise 3 Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ImagePanel.Source = LoadImage(pictureWidth, pictureHeight, differenceBytes);
            }
        }

        private byte[] CalculateDifferenceBytes(byte[] picture)
        {
            byte[] DifferenceTable = new byte[picture.Length];
            DifferenceTable[0] = picture[0];
            for (int i = 1; i < picture.Length; i++)
            {
                int difference = (int)picture[i] - (int)picture[i - 1];
                if (difference < 0) { difference += 256; }
                DifferenceTable[i] = Convert.ToByte(difference);
            }
            return DifferenceTable;
        }

        private byte[] CalculateOriginalBytes(byte[] difference)
        {
            byte[] OriginalTable = new byte[difference.Length];
            OriginalTable[0] = difference[0];
            for (int i = 1; i < difference.Length; i++)
            {
                int pixel = (int)OriginalTable[i - 1] + (int)difference[i];
                if (pixel > 255) { pixel -= 256; }
                OriginalTable[i] = Convert.ToByte(pixel);
            }
            return OriginalTable;
        }

        private void OriginalImageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pictureHeight <= 0 || pictureWidth <= 0)
            {
                MessageBox.Show("Height or width isn't set", "List 5 Exercise 3 Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (differenceBytes == null)
            {
                MessageBox.Show("Picture not Selected", "List 5 Exercise 3 Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                byte[] originalImage = CalculateOriginalBytes(differenceBytes);
                ImagePanel.Source = LoadImage(pictureWidth, pictureHeight, originalImage);
            }
        }

        private void SaveDifferenceBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "RAW files|*.raw",
                Title = "Save Difference Table"
            };
            saveFileDialog.ShowDialog();
            if (string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }
            File.WriteAllBytes(saveFileDialog.FileName, differenceBytes);
        }
    }
}