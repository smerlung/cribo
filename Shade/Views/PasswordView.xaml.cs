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

namespace Shade.Views
{
    /// <summary>
    /// Interaction logic for PasswordView.xaml
    /// </summary>
    public partial class PasswordView : Window
    {
        public PasswordView()
        {
            InitializeComponent();
        }

        public static string GetPassword()
        {
            var view = new PasswordView();
            view.DataContext = view;
            bool? result = view.ShowDialog();
            if(result.HasValue && result.Value)
            {
                return view.txtPassword.Password;
            }

            return null;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
