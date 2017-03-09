using System;
using System.Collections;
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
using System.Windows.Forms;
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
using System.Reflection;

namespace DesktopApplication
{
    /// <summary>
    /// Interaction logic for PageMain.xaml
    /// </summary>
    public partial class PageMain : Page
    {
        public static ObservableCollection<Authority> thelist;

        IEnumerable<Authority> SelectedFilelist;
       

        public PageMain()
        {
            InitializeComponent();
            thelist = new ObservableCollection<Authority> {};
            this.mylistview.ItemsSource = thelist;

            //leftList.ItemsSource = theleftlist;
            //leftList.SelectedItem

            if (MainWindow.isSigined)
            {
                ObservableCollection<Authority> mainlist = getFileName(Post.GetAuthorityList());
                foreach (Authority item in mainlist)
                {
                    thelist.Add(item);
                }
            }
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

        //选择文件夹
        private void button_runScript_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
            DialogResult result = m_Dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            string m_Dir = m_Dialog.SelectedPath.Trim();
            this.textblock_filepath.Text = m_Dir;

            Post.SendAuthority(m_Dir.Replace("\\", "/"), "1");

            thelist.Clear();
            if (MainWindow.isSigined)
            {
                ObservableCollection<Authority> mainlist = getFileName(Post.GetAuthorityList());
                foreach (Authority item in mainlist)
                {
                    thelist.Add(item);
                }
            }
        }

        //选择文件
        private void button_chose_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                //Filter = "Excel Files (*.sql)|*.sql|图片文件(*.jpg,*.png)|*.jpg;*.png"  
                Filter = "All Files|*.*"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                this.textblock_filepath.Text = openFileDialog.FileName;
                //thelist.Add(new Authority { Name = openFileDialog.SafeFileName, Address = openFileDialog.FileName });
                //thelist[0].Address = "bbbbbbbb";
                //this.mylistview.Items.Add(new FileClass { Name = openFileDialog.SafeFileName, Address = openFileDialog.FileName });

                Post.SendAuthority(openFileDialog.FileName.Replace("\\","/"), "1");

                thelist.Clear();
                if (MainWindow.isSigined)
                {
                    ObservableCollection<Authority> mainlist = getFileName(Post.GetAuthorityList());
                    foreach (Authority item in mainlist)
                    {
                        thelist.Add(item);
                    }
                }
            }
        }

        private void btnMenu_Initialized(object sender, EventArgs e)
        {
            //设置右键菜单为null
            this.AddButton.ContextMenu = null;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //目标
            this.contextMenu.PlacementTarget = this.AddButton;
            //位置
            //this.contextMenu.Placement = PlacementMode.Top;
            //显示菜单
            this.contextMenu.IsOpen = true;

            //this.textblock_filepath.Text = MyKeys.FILE_PATH;
        }
  
        private void treeview_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem a = e.OriginalSource as TreeViewItem;
            switch(a.Name.ToString())
            {
                case "全部文件":
                    this.mylistview.ItemsSource = thelist;
                    break;

                case "文档":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("doc") || 
                              authority.File_Path.Split('.').Last().Equals("docx") || 
                              authority.File_Path.Split('.').Last().Equals("txt") || 
                              authority.File_Path.Split('.').Last().Equals("pdf")
                        select authority;
                  
                    this.mylistview.ItemsSource = SelectedFilelist;
                    break;

                case "图片":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("jpg") ||
                              authority.File_Path.Split('.').Last().Equals("png")
                        select authority;
                 
                    this.mylistview.ItemsSource = SelectedFilelist;
                    break;

                case "视频":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("mp4") 
                        select authority;
                  
                    this.mylistview.ItemsSource = SelectedFilelist;
                    break;

                case "音频":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("mp3")
                        select authority;
                  
                    this.mylistview.ItemsSource = SelectedFilelist;
                    break;

                case "应用":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("exe")
                        select authority;
                   
                    this.mylistview.ItemsSource = SelectedFilelist;
                    break;

                case "文件夹":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("")
                        select authority;

                    this.mylistview.ItemsSource = SelectedFilelist;
                    break;
                case "其他":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("")
                        select authority;

                    this.mylistview.ItemsSource = SelectedFilelist;
                    break;
            }
        }     
       
        private void listview_Selected(object sender, SelectionChangedEventArgs e)
        {
            var authority = this.mylistview.SelectedItem as Authority;
            if (authority != null)
                this.textblock_filepath.Text = authority.File_Path;
        }

    }
}
