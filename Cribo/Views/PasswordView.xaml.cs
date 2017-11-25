using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cribo.Views
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
            if (result.HasValue && result.Value)
            {
                return view.txtPassword.Password;
            }

            return null;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            var state = Keyboard.GetKeyStates(Key.CapsLock);
            switch (state)
            {
                case KeyStates.Down | KeyStates.Toggled:
                    this.lblCapsLockWarning.Visibility = Visibility.Visible;
                    break;
                case KeyStates.None:
                    this.lblCapsLockWarning.Visibility = Visibility.Collapsed;

                    break;
                case KeyStates.Toggled:
                    this.lblCapsLockWarning.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}
