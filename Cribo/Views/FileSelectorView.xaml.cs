using Microsoft.Win32;
using Cribo.Enumerations;
using Cribo.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Cribo.Views
{
    /// <summary>
    /// Interaction logic for FileSelectorView.xaml
    /// </summary>
    public partial class FileSelectorView : Window
    {
        public FileSelectorView()
        {
            InitializeComponent();
        }

        public RecentFile File { get { return this.DataContext as RecentFile; } }

        private void rdoLocalFile(object sender, RoutedEventArgs e)
        {
            this.gridFtp.Visibility = Visibility.Collapsed;
            this.gridLocal.Visibility = Visibility.Visible;
            this.File.Type = RecentFileType.Local;
        }

        private void rdoFtpFile(object sender, RoutedEventArgs e)
        {
            this.gridFtp.Visibility = Visibility.Visible;
            this.gridLocal.Visibility = Visibility.Collapsed;
            this.File.Type = RecentFileType.Ftp;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                this.File.FileDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
                this.File.FileName = System.IO.Path.GetFileName(dlg.FileName);
                this.File.FirePropertyChanged();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.rdolocal.IsChecked = true;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
