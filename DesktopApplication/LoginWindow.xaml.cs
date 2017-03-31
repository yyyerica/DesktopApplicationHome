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



namespace DesktopApplication
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        //添加一个委托
        public delegate void PassDataBetweenFormHandler(object sender, PassDataWinFormEventArgs e);
        //添加一个PassDataBetweenFormHandler类型的事件
        public event PassDataBetweenFormHandler PassDataBetweenForm;

        //书写子窗体确认按钮的代码如下：
        private void SiginButton_Click(object sender, RoutedEventArgs e)
        {
            String Account,Key;
            Account = AccountTextBox.Text;
            Key = KeyTextBox.Password;

            if(Post.HttpLogin(Account, Key))
            {
                PassDataWinFormEventArgs args = new PassDataWinFormEventArgs(Account, Key);
                PassDataBetweenForm(this, args);
                this.Close();
            }

            
        }

        private void AccountTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                KeyTextBox.Focus();
            }
        }

        private void KeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SiginButton.Focus();
            }
        }
    }
}
