using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace List3Exercise5b
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //LoadedFileTxtBlck - for file url
        //LoadedCodesTxtBlck - for codes file url
        private string FileUrl { get; set; }

        public string FileName { get; set; }
        public HuffmanCode HuffmanCode { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            HuffmanCode = new HuffmanCode();
            ;
        }

        private void LoadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select FILE",
            };
            if (openFileDialog.ShowDialog() != true) return;
            FileUrl = openFileDialog.FileName;
            FileName = openFileDialog.SafeFileName;
            LoadedFileTxtBlck.Text = FileName;
        }

        private void CreateCodesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FileUrl))
            {
                MessageBox.Show("File not selected", "Select File", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                HuffmanCode.CreateHuffmanCodes(FileUrl);
                HuffmanCode.BinaryCodesName = $"{FileName}HCodes";
                LoadedCodesTxtBlck.Text = HuffmanCode.BinaryCodesName;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Create Codes Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ;
                throw;
            }
        }

        private void ImportCodesBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select FILE",
                Filter = "JSON files|*.JSON",
            };
            if (openFileDialog.ShowDialog() != true) return;
            try
            {
                if (HuffmanCode.ImportHuffmanCodes(openFileDialog.FileName))
                {
                    MessageBox.Show($"Codes Imported", "Import Completed",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Codes Import Failed", "Import Failed",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                HuffmanCode.BinaryCodesName = openFileDialog.SafeFileName;
                LoadedCodesTxtBlck.Text = HuffmanCode.BinaryCodesName;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void ExportCodesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (HuffmanCode.BinaryCodes == null)
            {
                MessageBox.Show($"Codes not Selected\nSelect codes or click 'CreateCodesBtn'", "Export Codes Error",
                      MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files|*.JSON",
                Title = "Save Binary Codes",
                FileName = HuffmanCode.BinaryCodesName
            };

            saveFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                MessageBox.Show($"File name can't be empty", "Export Codes Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (HuffmanCode.ExportHuffmanCodes(saveFileDialog.FileName))
                MessageBox.Show($"Codes Exported", "Export Completed",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                MessageBox.Show($"Codes Export Failed", "Export Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CompressBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FileUrl))
                MessageBox.Show($"File not selected", "Select File",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else if (LoadedCodesTxtBlck.Text == "none")
                MessageBox.Show($"Codes not selected", "Select Codes",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                byte[] compressedFile = HuffmanCoderMethods.CompressFile(HuffmanCode, FileUrl);
                if (compressedFile != null)
                {
                    MessageBox.Show($"File Compressed", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Huff files|*.huff",
                        Title = "Save Compressed File",
                        FileName = $"{FileName}.huff"
                    };
                    saveFileDialog.ShowDialog();
                    if (string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        return;
                    }
                    File.WriteAllBytes(saveFileDialog.FileName, compressedFile);
                }
                else
                    MessageBox.Show($"Compression Failed", "Fail",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DecompressBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FileUrl))
                MessageBox.Show($"File not selected", "Select File",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else if (LoadedCodesTxtBlck.Text == "none")
                MessageBox.Show($"Codes not selected", "Select Codes",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else if (Path.GetExtension(FileUrl) != ".huff") MessageBox.Show($"Wrong File Extension", "Wrong Extension",
                MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                byte[] DecompressedFile = HuffmanCoderMethods.DecompressFile(HuffmanCode, FileUrl);
                if (DecompressedFile != null)
                {
                    MessageBox.Show($"File Decompressed", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Title = "Save Decompressed File",
                        FileName = $"{FileName}"
                    };
                    saveFileDialog.ShowDialog();
                    if (string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        return;
                    }
                    File.WriteAllBytes(saveFileDialog.FileName, DecompressedFile);
                }
                else
                    MessageBox.Show($"Decompression Failed", "Fail",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}