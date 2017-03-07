using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;//INotifyPropertyChanged

namespace DesktopApplication
{
    public class FileClass : INotifyPropertyChanged
    {
        public String Name { get; set; }

        public String Address
        {
            get;
            set;
        }

        //public String Type { get; set; }
        //public FileClass(String Name, String Address)
        //{
        //    this.Name = Name;
        //    this.Address = Address;
        //}

        public FileClass()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}