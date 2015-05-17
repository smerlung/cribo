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

        public MainWindow()
        {
            InitializeComponent();
            
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
            string unicode = UnicodeEncoding.UTF8.GetString(ShadeStream.GetShadeBytes(data));
            this.texteditor.Text = unicode;
        }

        private void SaveImage(string imagepath)
        {
            byte[] data = ImageStream.GetImageBytes(imagepath);
            byte[] unicode = UnicodeEncoding.UTF8.GetBytes(this.texteditor.Text);
            ShadeStream.SetShadeBytes(data, unicode);
            ImageStream.SetImageBytes(imagepath, data);
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            this.SaveImage(this.imagepath);
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            this.OpenImage(@"C:\Development\CSharp\Shade\shadeimage.png");
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
