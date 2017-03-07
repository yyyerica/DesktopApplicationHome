using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication {
    public class PassDataWinFormEventArgs : EventArgs
    {
        public PassDataWinFormEventArgs()
        {
            //
        }
        public PassDataWinFormEventArgs(string account, string key)
        {
            this.account = account;
            this.key = key;
        }

        private string account;
        private string key;

        public string Account
        {
            get { return account; }
            set { account = value; }
        }
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
    
    }
}
