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

            FindTreeViewItem(allfilesItem).Focus();
              
            thelist = new ObservableCollection<Authority> {};
            this.mylistview.ItemsSource = thelist;
            

            //leftList.ItemsSource = theleftlist;
            //leftList.SelectedItem

            if (MainWindow.isSigined)
            {
                //ObservableCollection<Authority> mainlist = getFileName(Post.GetAuthorityList());
                ObservableCollection<Authority> mainlist = Post.GetAuthorityList();
                foreach (Authority item in mainlist)
                {
                    item.File_Path.Replace("/", "\\");
                    thelist.Add(item);
                }
            }

            countText.Text = "共 " + thelist.Count() + " 项";
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
            //this.textblock_filepath.Text = m_Dir;

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
                //this.textblock_filepath.Text = openFileDialog.FileName;
                //thelist.Add(new Authority { Name = openFileDialog.SafeFileName, Address = openFileDialog.FileName });
                //thelist[0].Address = "bbbbbbbb";
                //this.mylistview.Items.Add(new FileClass { Name = openFileDialog.SafeFileName, Address = openFileDialog.FileName });

                Post.SendAuthority(openFileDialog.FileName.Replace("\\","/"), "1");

                thelist.Clear();
                if (MainWindow.isSigined)
                {
                    ObservableCollection<Authority> mainlist = Post.GetAuthorityList();
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
                    countText.Text = "共 " + thelist.Count() + " 项";
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
                    countText.Text = "共 " + SelectedFilelist.Count() + " 项";
                    break;

                case "图片":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("jpg") ||
                              authority.File_Path.Split('.').Last().Equals("png")
                        select authority;
                    this.mylistview.ItemsSource = SelectedFilelist;
                    countText.Text = "共 " + SelectedFilelist.Count() + " 项";
                    break;

                case "视频":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("mp4") 
                        select authority;
                    this.mylistview.ItemsSource = SelectedFilelist;
                    countText.Text = "共 " + SelectedFilelist.Count() + " 项";
                    break;

                case "音频":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("mp3")
                        select authority;
                  
                    this.mylistview.ItemsSource = SelectedFilelist;
                    countText.Text = "共 " + SelectedFilelist.Count() + " 项";
                    break;

                case "应用":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("exe")
                        select authority;
                   
                    this.mylistview.ItemsSource = SelectedFilelist;
                    countText.Text = "共 " + SelectedFilelist.Count() + " 项";
                    break;

                case "文件夹":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("")
                        select authority;

                    this.mylistview.ItemsSource = SelectedFilelist;
                    countText.Text = "共 " + SelectedFilelist.Count() + " 项";
                    break;
                case "其他":
                    SelectedFilelist =
                        from authority in thelist
                        where authority.File_Path.Split('.').Last().Equals("")
                        select authority;

                    this.mylistview.ItemsSource = SelectedFilelist;
                    countText.Text = "共 " + SelectedFilelist.Count() + " 项";
                    break;
            }
        }     
       
        private void listview_Selected(object sender, SelectionChangedEventArgs e)
        {
            var authority = this.mylistview.SelectedItem as Authority;
            if (authority != null)
            {
                //this.textblock_filepath.Text = authority.File_Path;
            }
                
        }


        //private TreeViewItem FindTreeViewItem(ItemsControl container, object item)
        //{    
        //    if (null == container || null == item)
        //    {
        //        return null;
        //    }

        //    if (container.DataContext == item)
        //    {
        //        return container as TreeViewItem;
        //    }

        //    int count = container.Items.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        TreeViewItem subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);

        //        if (null == subContainer)
        //        {
        //            continue;
        //        }

        //        // Search the next level for the object.
        //        TreeViewItem resultContainer = FindTreeViewItem(subContainer, item);
        //        if (null != resultContainer)
        //        {
        //            return resultContainer;
        //        }
        //    }

        //    return null;
        //}

        private TreeViewItem FindTreeViewItem(ItemsControl container)
        {
            if (null == container)
            {
                return null;
            }

            if (container.Name.Equals("allfilesItem"))
            {
                return container as TreeViewItem;
            }


            int count = container.Items.Count;
            for (int i = 0; i < count; i++)
            {
                TreeViewItem subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);

                if (null == subContainer)
                {
                    continue;
                }

                // Search the next level for the object.
                TreeViewItem resultContainer = FindTreeViewItem(subContainer);
                if (null != resultContainer)
                {
                    return resultContainer;
                }
            }

            return null;
        }


        /////使用上述方法遍历 ， 然后设置IsSelected属性

        //private static void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    //
        //    SelectedProtocolControl spc = d as SelectedProtocolControl;

        //    if (null != spc)
        //    {
        //        if (e.NewValue != spc.trvSelectedProtocol.SelectedItem)
        //        {
        //            //TreeViewItem tviNew = spc.trvSelectedProtocol.ItemContainerGenerator.ContainerFromItem(e.NewValue) as TreeViewItem;
        //            TreeViewItem tviNew = spc.FindTreeViewItem(spc.trvSelectedProtocol, e.NewValue);
        //            if (null != tviNew)
        //            {
        //                tviNew.IsSelected = true;
        //            }
        //        }

        //        if (null != e.OldValue)
        //        {
        //            //TreeViewItem tviOld = spc.trvSelectedProtocol.ItemContainerGenerator.ContainerFromItem(e.OldValue) as TreeViewItem;
        //            TreeViewItem tviOld = spc.FindTreeViewItem(spc.trvSelectedProtocol, e.OldValue);
        //            if (null != tviOld)
        //            {
        //                tviOld.IsSelected = false;
        //            }
        //        }
        //    }

        //    System.Diagnostics.Debug.WriteLine("SelectedItemChanged...");
        //}



    }
}
