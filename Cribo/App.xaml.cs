namespace Cribo
{
    using Cribo.ViewModels;
    using Cribo.Views;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Dictionary<Type, object> instances = new Dictionary<Type, object>();

        public static void Register<T>(T instance)
        {
            if(instances.ContainsKey(typeof(T)))
            {
                throw new Exception(string.Format("Instance of type {0} already exists", typeof(T).Name));
            }

            instances.Add(typeof(T), instance);
        }

        public static void Unregister<T>()
        {
            if (!instances.ContainsKey(typeof(T)))
            {
                throw new Exception(string.Format("No instance of type {0} exists", typeof(T).Name));
            }

            instances.Remove(typeof(T));
        }

        public static T Get<T>()
        {
            if (!instances.ContainsKey(typeof(T)))
            {
                throw new Exception(string.Format("No instance of type {0} exists", typeof(T).Name));
            }

            return (T)instances[typeof(T)];
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Register<DatabaseViewModel>(new DatabaseViewModel());
            Register<MainViewModel>(new MainViewModel());
            var view = new MainView();
            view.DataContext = App.Get<MainViewModel>();
            view.ShowDialog();
        }
    }
}
