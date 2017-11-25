using Cribo.ViewModels;
using Cribo.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Cribo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var viewmodel = new MainViewModel();
            var view = new MainView();
            view.DataContext = viewmodel;
            view.ShowDialog();
        }
    }
}
