using Microsoft.Win32;
using Shade.Objects;
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

namespace Shade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string imagepath = "";
        //private byte[] salt = UnicodeEncoding.UTF8.GetBytes("T]-D`hxf5Ln;a>t#");
        private byte[] salt = UnicodeEncoding.UTF8.GetBytes("a");
        private byte[] buffer;
        private bool isplaintext = true;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OpenImage(string imagepath)
        {
            this.imagepath = imagepath;
            using (FileStream fs = new FileStream(imagepath, FileMode.Open))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = fs;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                this.imageviewer.Source = image;
            }

            byte[] data = ImageStream.GetImageBytes(imagepath);
            this.buffer = ShadeStream.GetShadeBytes(data);
            this.isplaintext = false;
            this.UpdateEditor();
        }

        private void SaveImage(string imagepath)
        {
            if(this.isplaintext)
            {
                MessageBox.Show("You cannot save the data if it is in plaintext mode");
                return;
            }

            byte[] data = ImageStream.GetImageBytes(imagepath);
            ShadeStream.SetShadeBytes(data, this.buffer);
            ImageStream.SetImageBytes(imagepath, data);
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            this.SaveImage(this.imagepath);
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                this.OpenImage(dlg.FileName);
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EncryptClick(object sender, RoutedEventArgs e)
        {
            if (!this.isplaintext)
            {
                MessageBox.Show("Data is already encrypted");
                return;
            }

            this.buffer = Crypto.Encrypt(this.buffer, this.GetHash());
            this.isplaintext = false;
            this.UpdateEditor();
        }

        private void DecryptClick(object sender, RoutedEventArgs e)
        {
            if (this.isplaintext)
            {
                MessageBox.Show("Data is not encrypted");
                return;
            }

            this.buffer = Crypto.Decrypt(this.buffer, this.GetHash());
            this.isplaintext = true;
            this.UpdateEditor();
        }

        private byte[] GetHash()
        {
            byte[] hash = SimpleHash.Create(this.txtPassword.Password, this.salt);
            return hash;
        }

        private void UpdateEditor()
        {
            this.texteditor.Text = UnicodeEncoding.UTF8.GetString(this.buffer);
        }

        private void texteditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.isplaintext)
            {
                this.buffer = UnicodeEncoding.UTF8.GetBytes(this.texteditor.Text);
            }
        }

    }
}
