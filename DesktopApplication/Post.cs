using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Collections.ObjectModel;//ObservableCollection

namespace DesktopApplication
{
    public class Post
    {
        public static bool HttpLogin(string name, string password)
        {
            var request = (HttpWebRequest)WebRequest.Create(MyKeys.SENDURL);

            string source = "name=" + name
                        + "&password=" + password;
            source = Encrypt(source);

            var postData = "model=" + PostOptions.LOGIN;
            postData += "&guid=" + UrlEncode(MyKeys.GUID, Encoding.UTF8);
            postData += source;

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
            responseString = Decrypt(responseString);
            Console.WriteLine("responseString:" + responseString);

            string[] sourceStrArray = responseString.Split('&');
            string status = sourceStrArray[0].Split('=')[1].ToString();
            //Console.WriteLine("status:" + status);

            if (status.Equals("OK"))
            {
                string userId = sourceStrArray[1].Split('=')[1].ToString();
                MyKeys.USER_ID = userId;
                //Console.WriteLine(MyKeys.USER_ID);
                //Console.WriteLine("LOGIN");
                return true;
            }

            return false;
        }

        public static Boolean SendAuthority(string file_path, string authority_number)
        {
            var request = (HttpWebRequest)WebRequest.Create(MyKeys.SENDURL);

            string source = "file_path=" + file_path
                        + "&authority_number=" + authority_number;
            source = Encrypt(source);

            var postData = "model=" + PostOptions.SENDAUTHORITY;
            postData += "&user_id=" + UrlEncode(MyKeys.USER_ID, Encoding.UTF8);
            postData += "&guid=" + UrlEncode(MyKeys.GUID, Encoding.UTF8);
            postData += source;

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
            responseString = Decrypt(responseString);
            Console.WriteLine("responseString:" + responseString);

            string[] sourceStrArray = responseString.Split('&');
            string status = sourceStrArray[0].Split('=')[1].ToString();
            Console.WriteLine("status:" + status);

            if (status.Equals("OK"))
            {
                //string content = sourceStrArray[1].Split('=')[1].ToString();
                Console.WriteLine("SEND");
                return true;
            }

            return false;
        }

        public static bool SendCheck(string file_path, string authority_number)
        {
            bool isOpen = false;

            var request = (HttpWebRequest)WebRequest.Create(MyKeys.SENDURL);

            string operate_date = DateTime.Now.ToString("yyyy-MM-dd");
            string operate_time = DateTime.Now.ToLongTimeString().ToString();

            string source = "file_path=" + file_path
                        + "&authority_number=" + authority_number
                        + "&operate_date=" + operate_date
                        + "&operate_time=" + operate_time;

            source = Encrypt(source);

            var postData = "model=" + PostOptions.OPERATE;
            postData += "&user_id=" + UrlEncode(MyKeys.USER_ID, Encoding.UTF8);
            postData += "&guid=" + UrlEncode(MyKeys.GUID, Encoding.UTF8);
            postData += source;

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
            responseString = Decrypt(responseString);
            Console.WriteLine("responseString:" + responseString);

            string[] sourceStrArray = responseString.Split('&');
            string status = sourceStrArray[0].Split('=')[1].ToString();
            Console.WriteLine("status:" + status);

            if (status.Equals("OK"))
            {
                isOpen = true;
                Console.WriteLine("SEND");
            }
            else
            {
                isOpen = false;
                Console.WriteLine("FALSE");
            }

            return isOpen;
        }

        public static ObservableCollection<Authority> GetAuthorityList()
        {
            var request = (HttpWebRequest)WebRequest.Create(MyKeys.SENDURL);

            var postData = "model=" + PostOptions.GETAUTHORITYLIST;
            postData += "&user_id=" + UrlEncode(MyKeys.USER_ID, Encoding.UTF8);
            postData += "&guid=" + UrlEncode(MyKeys.GUID, Encoding.UTF8);

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
            responseString = Decrypt(responseString);
            Console.WriteLine("responseString:" + responseString);

            ObservableCollection<Authority> authorities = new ObservableCollection<Authority>();
            string[] sourceStrArray = responseString.Split('&');
            if (sourceStrArray.Length > 1)
            {
                for (int i = 0; i < sourceStrArray.Length; i += 2)
                {
                    string file_path = sourceStrArray[i].Split('=')[1];
                    string authority_number = sourceStrArray[i + 1].Split('=')[1];
                    //Authority authority = new Authority(guid, file_path);
                    Authority authority = new Authority() { File_Path = file_path, Authority_Number = authority_number };
                    authorities.Add(authority);
                }
            }
            
            return authorities;
        }

        public static ObservableCollection<History> GetHistoryList(string operate_date)
        {
            var request = (HttpWebRequest)WebRequest.Create(MyKeys.SENDURL);

            string source = "operate_date=" + operate_date;

            source = Encrypt(source);

            var postData = "model=" + PostOptions.GETHISTORYLIST;
            postData += "&user_id=" + UrlEncode(MyKeys.USER_ID, Encoding.UTF8);
            postData += "&guid=" + UrlEncode(MyKeys.GUID, Encoding.UTF8);
            postData += source;

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
            responseString = Decrypt(responseString);
            Console.WriteLine("responseString:" + responseString);

            ObservableCollection<History> histories = new ObservableCollection<History>();
            string[] sourceStrArray = responseString.Split('&');
            for (int i = 0; i < sourceStrArray.Length; i += 4)
            {
                string file_path = sourceStrArray[i].Split('=')[1];
                string authority_number = sourceStrArray[i + 1].Split('=')[1];
                string operate_time = sourceStrArray[i + 2].Split('=')[1];
                string isPermit = sourceStrArray[i + 3].Split('=')[1];
                History history = new History() { File_Path = file_path, Authority_Number = authority_number, Operate_Time = operate_time, IsPermit = isPermit };
                histories.Add(history);
            }
            return histories;
        }

        public static ObservableCollection<History> GetAllHistoryList()
        {
            var request = (HttpWebRequest)WebRequest.Create(MyKeys.SENDURL);

            var postData = "model=" + PostOptions.GETALLHISTORYLIST;
            postData += "&user_id=" + UrlEncode(MyKeys.USER_ID, Encoding.UTF8);
            postData += "&guid=" + UrlEncode(MyKeys.GUID, Encoding.UTF8);

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
            responseString = Decrypt(responseString);
            Console.WriteLine("responseString:" + responseString);

            ObservableCollection<History> histories = new ObservableCollection<History>();
            string[] sourceStrArray = responseString.Split('&');
            if (sourceStrArray.Length > 1)
            {
                for (int i = 0; i < sourceStrArray.Length; i += 5)
                {
                    string file_path = sourceStrArray[i].Split('=')[1];
                    string authority_number = sourceStrArray[i + 1].Split('=')[1];
                    string operate_date = sourceStrArray[i + 2].Split('=')[1];
                    string operate_time = sourceStrArray[i + 3].Split('=')[1];
                    string isPermit = sourceStrArray[i + 4].Split('=')[1];
                    History history = new History() { File_Path = file_path, Authority_Number = authority_number, Operate_Date = operate_date, Operate_Time = operate_time, IsPermit = isPermit };
                    histories.Add(history);
                }
            }
            return histories;
        }

        public static string UrlEncode(string temp, Encoding encoding)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                string t = temp[i].ToString();
                string k = HttpUtility.UrlEncode(t, encoding);
                if (t == k)
                {
                    stringBuilder.Append(t);
                }
                else
                {
                    stringBuilder.Append(k.ToUpper());
                }
            }
            return stringBuilder.ToString();
        }

        public static string Encrypt(string source)
        {
            string result = "";
            source = source.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            string X = MyEncrypt.SHA1(UrlEncode(source, Encoding.UTF8));
            string character = MyEncrypt.EncryptRSA(X, MyKeys.CLIENT_PRIVATE_KEY, 2);
            string C = "{\"source\":\"" + source
                        + "\",\"character\":\"" + character + "\"}";
            string Q = "12345678";
            string D = MyEncrypt.EncryptDES(C, Q);
            string P = MyEncrypt.EncryptRSA(Q, MyKeys.SERVER_PUBLIC_KEY, 1);
            result = "&text=" + UrlEncode(D, Encoding.UTF8)
                + "&password=" + UrlEncode(P, Encoding.UTF8);
            return result;
        }

        public static string Decrypt(string encrypt)
        {
            encrypt = encrypt.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            Console.WriteLine("responseString:" + encrypt);
            var josnObj = Newtonsoft.Json.Linq.JObject.Parse(encrypt);
            string text = josnObj["text"].ToString();
            string password = josnObj["password"].ToString();
            password = MyEncrypt.DecryptRSA(password, MyKeys.CLIENT_PRIVATE_KEY, 2);
            password = password.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            Console.WriteLine("password:" + password);
            text = text.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            text = MyEncrypt.DecryptDES(text, password);
            text = text.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(text);
            string character = jsonObject["character"].ToString();
            string source = jsonObject["source"].ToString().Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            character = MyEncrypt.DecryptRSA(character, MyKeys.SERVER_PUBLIC_KEY, 1);
            character = character.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            String X = MyEncrypt.SHA1(UrlEncode(source, Encoding.UTF8));
            Console.WriteLine("source:" + source);
            Console.WriteLine("character:" + character);
            Console.WriteLine("X:" + X);
            if (X.Equals(character.ToUpper())) return source;
            else return "";
        }
    }
}
