using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;

namespace Mephi_data
{
    class Program
    {
        //static private string URL = "https://home.mephi.ru/study_groups/2873/schedule";
        static private string URL = "https://home.mephi.ru/study_groups/";
        static private string html = "";
        static string group = "";


        static void Main(string[] args)
        {            
            string[] data;

            Console.WriteLine("Input group, pls ");
            group = Console.ReadLine();

            html = init();

            Console.WriteLine("Input day, pls ");
            string day = Console.ReadLine();

            Console.Clear();

            data = getTextFromHTML(html);
            data = correctionData(data,day);

            Console.Read();
        }


        static public string init()
        {
            string result = "";
            URL = "https://home.mephi.ru/study_groups";

            HttpWebRequest proxy_request = (HttpWebRequest)WebRequest.Create(URL);
            proxy_request.Method = "GET";
            proxy_request.ContentType = "application/x-www-form-urlencoded";
            proxy_request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.89 Safari/532.5";
            proxy_request.KeepAlive = true;
            HttpWebResponse resp = proxy_request.GetResponse() as HttpWebResponse;
            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("utf-8"))) html = sr.ReadToEnd();

            result = getTitle(html.Trim());

            File.WriteAllText(@"E:\data.txt",result);

            /*
            for (int i = 550; i < 3000; i++)
            {
                if (CheckNet.CheckURL(URL + i + "/schedule"))
                {
                    HttpWebRequest proxy_request = (HttpWebRequest)WebRequest.Create(URL);
                    proxy_request.Method = "GET";
                    proxy_request.ContentType = "application/x-www-form-urlencoded";
                    proxy_request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.89 Safari/532.5";
                    proxy_request.KeepAlive = true;
                    HttpWebResponse resp = proxy_request.GetResponse() as HttpWebResponse;
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("utf-8"))) html = sr.ReadToEnd();

                    result = getTitle(html.Trim(), i);

                    if (result == group)
                    {
                        Console.WriteLine("Group found!");
                        return html.Trim();
                    }

                    Console.WriteLine("Other group");

                }
                else
                {
                    Console.WriteLine(i+" Error: does not exist");
                }
            }
            */
            return "";
        }

        static public string getTitle(string htmlTitle)
        {
            string str1 = "<div class='list-group'>";

            int len = htmlTitle.Length;
     
            string str2 = "</a></div>";
            string result = "";

            //Console.WriteLine(htmlTitle);
            //Console.Read();

            int index1 = htmlTitle.IndexOf(str1);

            htmlTitle = htmlTitle.Substring(index1);

            int index2 = htmlTitle.IndexOf(str2) + index1;



            //Console.WriteLine(htmlTitle + index1 + "   " + index2);
            //Console.Read();

            result = htmlTitle.Substring(index1, index2 - index1);

            Console.Clear();
            Console.WriteLine(result);
            //result = htmlTitle.Substring(index2, );



            if (result == null)
            {
                result = "";
            }

            Console.WriteLine(result);

            return result;
        }



        static public string[] correctionData(string[] data,string day)
        {
            string flag = "";
            int count = 0;

            List<string> monday = new List<string>();
            List<string> tuesday = new List<string>();
            List<string> wednesday = new List<string>();
            List<string> thursday = new List<string>();
            List<string> friday = new List<string>();
            List<string> saturday = new List<string>();

            for (int i = 0; i<data.Count()-2;i++)
            {
                switch (data[i])
                {
                    case ("Понедельник"):
                        flag = "Понедельник";
                        break;
                    case ("Вторник"):
                        flag = "Вторник";
                        break;
                    case ("Среда"):
                        flag = "Среда";
                        break;
                    case ("Четверг"):
                        flag = "Четверг";
                        break;
                    case ("Пятница"):
                        flag = "Пятница";
                        break;
                    case ("Суббота"):
                        flag = "Суббота";
                        break;
                }

                switch (flag)
                {
                    case ("Понедельник"):
                        monday.Add(data[i]);
                        count++;
                        break;
                    case ("Вторник"):
                        thursday.Add(data[i]);
                        count++;
                        break;
                    case ("Среда"):
                        wednesday.Add(data[i]);
                        count++;
                        break;
                    case ("Четверг"):
                        thursday.Add(data[i]);
                        count++;
                        break;
                    case ("Пятница"):
                        friday.Add(data[i]);
                        count++;
                        break;
                    case ("Суббота"):
                        saturday.Add(data[i]);
                        count++;
                        break;
                }
            }


            switch (day)
            {
                case ("monday"):
                    foreach (string p in monday)
                    {
                        Console.WriteLine(p);
                    }
                    break;
                case ("tuesday"):
                    foreach (string p in tuesday)
                    {
                        Console.WriteLine(p);
                    }
                    break;
                case ("wednesday"):
                    foreach (string p in wednesday)
                    {
                        Console.WriteLine(p);
                    }
                    break;
                case ("thursday"):
                    foreach (string p in thursday)
                    {
                        Console.WriteLine(p);
                    }
                    break;
                case ("friday"):
                    foreach (string p in friday)
                    {
                        Console.WriteLine(p);
                    }
                    break;
                case ("saturday"):
                    foreach (string p in saturday)
                    {
                        Console.WriteLine(p);
                    }
                    break;
            }

            return data;
        }


        static public string[] getTextFromHTML(string html)
        {
            int flag = 0;
            int count = 0;
            string str = "";
            string[] text = new string[100000];

            for (int i = 0; i<html.Length-3;i++)
            {
                str = str + html[i];

                if ((html[i] == '<')&&(flag==0))
                {
                    flag = 1;  
                }

                
                if((html[i+1] == '<') && (html[i+2] == '/')){
                    if ((str != "") || (str != " "))
                    {
                        text[count] = str.Trim();
                        str = "";
                        count++;
                    }
                }
                

                if ((html[i] == '>') && (flag != 0))
                {
                    if ((str!="")||(str!=" "))
                    {
                        text[count] = str.Trim();
                        str = "";
                        count++;
                    }
                    flag = 0;
                }
            }

            string[] dataFromHTML = new string[count];
            int countNewText = 0;

            for (int i=0; i<count;i++)
            {
                flag = 1;
                text[i] = text[i].Trim();

                for (int k = 0; k < (text[i].Length)/3; k++)
                {
                    if ((text[i][k] == '<')||(text[i][k] == '>')||(text[i][k] == ';'))
                    {
                        flag = 0;
                        break;
                    }
                }

                if((flag == 1)&&(text[i]!="")&&(text[i] != " "))
                {
                    dataFromHTML[countNewText] = text[i];
                    countNewText++;
                }

            }

            string[] data = new string[countNewText];
            for(int i = 0; i < countNewText; i++)
            {
                flag = 1;
                for(int q = 0;q < dataFromHTML[i].Length; q++)
                {             
                    if (dataFromHTML[i][q] == '<')
                    {
                        flag = 0;
                        data[i] = dataFromHTML[i].Substring(0, q).Trim();
                        break;
                    }             
                }

                if (flag == 1)
                {
                    data[i] = dataFromHTML[i];
                }
            }

            return data;
        }

    }
}
