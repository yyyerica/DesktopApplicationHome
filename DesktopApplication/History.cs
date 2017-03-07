using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;//INotifyPropertyChanged

namespace DesktopApplication
{
    public class History : INotifyPropertyChanged
    {
        private string file_path;
        private string authority_number;
        private string operate_date;
        private string operate_time;
        private string isPermit;
        public event PropertyChangedEventHandler PropertyChanged;
        //public History(string file_path, string authority_number, String operate_time, String isPermit)
        //{
        //    this.file_path = file_path;
        //    this.authority_number = authority_number;
        //    this.operate_time = operate_time;
        //    this.isPermit = isPermit;
        //}

        //public History(string file_path, string authority_number, string operate_date, string operate_time, string isPermit)
        //{
        //    this.file_path = file_path;
        //    this.authority_number = authority_number;
        //    this.operate_date = operate_date;
        //    this.operate_time = operate_time;
        //    this.isPermit = isPermit;
        //}

        public History()
        {

        }

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
            }
        }

        public string Operate_Date
        {
            get
            {
                return operate_date;
            }
            set
            {
                operate_date = value;
            }
        }

        public string Operate_Time
        {
            get
            {
                return operate_time;
            }
            set
            {
                operate_time = value;
            }
        }

        public string IsPermit
        {
            get
            {
                return isPermit;
            }
            set
            {
                isPermit = value;
            }
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        //public string getFile_path()
        //{
        //    return file_path;
        //}

        //public void setFile_path(string file_path)
        //{
        //    this.file_path = file_path;
        //}

        //public string getAuthority_number()
        //{
        //    return authority_number;
        //}

        //public void setAuthority_number(string authority_number)
        //{
        //    this.authority_number = authority_number;
        //}

        //public string getOperate_time()
        //{
        //    return operate_time;
        //}

        //public void setOperate_time(string operate_time)
        //{
        //    this.operate_time = operate_time;
        //}

        //public string getIsPermit()
        //{
        //    return isPermit;
        //}

        //public void setIsPermit(string isPermit)
        //{
        //    this.isPermit = isPermit;
        //}

        //public string getOperate_date()
        //{
        //    return operate_date;
        //}

        //public void setOperate_date(string operate_date)
        //{
        //    this.operate_date = operate_date;
        //}
    }
}
