using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    
    //private static TestDBContext tc = new TestDBContext();
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                MainWindow wnd = new MainWindow(e.Args[0]);
                wnd.Show();
            }
            else
            {
                MainWindow wnd = new MainWindow();
                wnd.Show();
            }
        }
    }

    //public static TestDBContext TestDBContext
    //{
    //    get {return TestDBContext;}
    //}
}
