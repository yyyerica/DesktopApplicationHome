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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;//ObservableCollection

using System.Web;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Data.OleDb;
using System.Data;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Security;
using System.Management;
using System.Collections;
using System.Reflection;

namespace DesktopApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {   
            InitializeComponent();
            init(); 
        }

        public static void init()
        {
            MyKeys.CPUID = GetCpuID();
            Console.WriteLine("MyKeys.CPUID", MyKeys.CPUID);
            string[] key = MyEncrypt.GenerateKey();
            MyKeys.CLIENT_PUBLIC_KEY = key[0];
            MyKeys.CLIENT_PRIVATE_KEY = key[1];
            MyKeys.SERVER_PUBLIC_KEY = getServerKey();
            string directory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string filePath = System.IO.Path.Combine(directory, "serverPublicKeyString.txt");
            while (!File.Exists(filePath)) ;
        }
        public static string getServerKey()
        {
            string directory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string filePath = System.IO.Path.Combine(directory, "serverPublicKeyString.txt");
            if (!File.Exists(filePath))
            {
                var request = (HttpWebRequest)WebRequest.Create(MyKeys.SENDURL);

                var postData = "model=" + PostOptions.GETSERVERKEY;
                postData += "&publicKey=" + Post.UrlEncode(MyKeys.CLIENT_PUBLIC_KEY, Encoding.UTF8);
                postData += "&cpuId=" + Post.UrlEncode(MyKeys.CPUID, Encoding.UTF8);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var josnObj = Newtonsoft.Json.Linq.JObject.Parse(responseString);
                string status = josnObj["status"].ToString();
                if (status.Equals("OK"))
                {
                    string publicKey = josnObj["publicKey"].ToString();
                    File.WriteAllText("serverPublicKeyString.txt", publicKey, Encoding.UTF8);
                    Console.WriteLine("GET");
                    return publicKey;
                }
            }
            else
            {
                string publicKey = File.ReadAllText("serverPublicKeyString.txt");
                return publicKey;
            }
            return "";
        }
        public static string GetCpuID()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "";
            }
        }

        public static bool isSigined = false;

        PageMain pm = new PageMain();

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            this.myFrame.Source = new Uri("/PageHistory.xaml", UriKind.Relative);
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            this.myFrame.Source = new Uri("/PageMain.xaml", UriKind.Relative);
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            //目标
            this.contextMenu.PlacementTarget = this.SettingButton;
            //位置
            //this.contextMenu.Placement = PlacementMode.Top;
            //显示菜单
            this.contextMenu.IsOpen = true;
        }

        private void btnMenu_Initialized(object sender, EventArgs e)
        {
             //设置右键菜单为null
             this.SettingButton.ContextMenu = null;
        }

        private void SiginSigoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow win = new LoginWindow();
            win.PassDataBetweenForm += new LoginWindow.PassDataBetweenFormHandler(Child_PassDataBetweenForm);
            win.Show();
            win.Activate();
        }

        //回调
        private void Child_PassDataBetweenForm(object sender, PassDataWinFormEventArgs e)
        {
            SiginButton.Content = e.Account;
            //ObservableCollection<Authority> authorities = Post.GetAuthorityList();
            //PageMain.thelist = getFileName(Post.GetAuthorityList());
            
            isSigined = true;

            if (MainWindow.isSigined)
            {
                ObservableCollection<Authority> mainlist = getFileName(Post.GetAuthorityList());
                foreach (Authority item in mainlist)
                {
                    PageMain.thelist.Add(item);
                }
            }

            //PageHistory.refreshchart();

            //Console.WriteLine("thelist.COUNT" + PageMain.thelist.Count + "");
        }
        
        public ObservableCollection<Authority> getFileName(ObservableCollection<Authority> list)
        {
            ObservableCollection<Authority> filenames = new ObservableCollection<Authority>();
            foreach (Authority a in list)
            {
                string[] filename = a.File_Path.Split('/');
                Authority one = new Authority()
                {
                    File_Path = filename.Last(),
                    Authority_Number = a.Authority_Number
                };
                filenames.Add(one);
            }
            return filenames;
        }
        
    }
}
