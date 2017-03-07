using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;//INotifyPropertyChanged

namespace DesktopApplication
{
    public class Authority : INotifyPropertyChanged
    {
        private string file_path; //
        private string authority_number;//权限
        public event PropertyChangedEventHandler PropertyChanged;

        public Authority()
        {
            
        }

        //public Authority(String file_path, String authority_number)
        //{
        //    this.file_path = file_path;
        //    this.authority_number = authority_number;
        //}

        public string File_Path
        {
            get
            {
                return file_path;
            }
            set
            {
                file_path = value;
            }
        }

        public string Authority_Number
        {
            get
            {
                return authority_number;
            }
            set
            {
                authority_number = value;
                //if (this.PropertyChanged != null)//激发事件，参数为Age属性    
                //{
                //    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Age"));
                //}  
            }
        }

        //public string GetFile_path()
        //{
        //    return file_path;
        //}

        //public void SetFile_path(string file_path)
        //{
        //    this.file_path = file_path;
        //}

        //public string GetAuthority_number()
        //{
        //    return authority_number;
        //}

        //public void SetAuthority_number(string authority_number)
        //{
        //    this.authority_number = authority_number;
        //}


        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

    }
}
