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
        static void Main(string[] args)
        {
            string URL = "https://home.mephi.ru/study_groups/2873/schedule";
            string path = @"D:\text.txt";
            string data = @"D:\data.txt";
            string html = "";


            if (CheckNet.CheckURL(URL))
            {
                Console.WriteLine("Ok");
            }
            else
            {
                Console.WriteLine("Error");
                Console.Read();
                return;
            }
 

            HttpWebRequest proxy_request = (HttpWebRequest)WebRequest.Create(URL);
            proxy_request.Method = "GET";
            proxy_request.ContentType = "application/x-www-form-urlencoded";
            proxy_request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.89 Safari/532.5";
            proxy_request.KeepAlive = true;


            HttpWebResponse resp = proxy_request.GetResponse() as HttpWebResponse;

            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("utf-8"))) html = sr.ReadToEnd();
            html = html.Trim();
            //File.WriteAllText(path, html + Environment.NewLine);


            //string[] arr = new string[0];
            //arr[0] = html;

            //Console.WriteLine(arr[0]);

            //упорядочить хтмл по строкам

            html = normalizeHTML(html);

            Console.Read();//
            return;//


            //поиск подстроки в строке 


            //File.WriteAllText(data, "");
            foreach (string line in File.ReadLines(path))
            {
                if (line.Contains("Расписание занятий группы"))
                {
                    string title = getTextFromHTML(line);
                    Console.WriteLine(title); 
                }


                if (line.Contains("tutors")|| line.Contains("label label-default label-lesson")|| line.Contains("lesson-time") || line.Contains("rooms"))
                {
                    string textData = getTextFromHTML(line);
                    Console.WriteLine(textData);

                    if (line.Contains("tutors"))
                    {
                        Console.WriteLine("<-------------------------->");
                    }

                    //File.AppendAllText(data, textData + Environment.NewLine);
                    //Console.WriteLine(line);
                }
            }

            Console.Read();
        }



        static public string normalizeHTML(string html)
        {
            int flag = 0;
            int count = 0;
            string str = "";

            string[] text = new string[55000];

            for (int i = 0; i<html.Length-3;i++)
            //for (int i = 0; i < 31000; i++)
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

            Console.Clear();
            File.WriteAllText(@"D:\data.txt", "");

            string[] dataFromHTML = new string[count];
            int countNewText = 0;

            for (int i=0; i<count;i++)
            {
                //Console.WriteLine(i+"   "+text[i]);
                //File.AppendAllText(@"D:\data.txt", text[i]+ Environment.NewLine);
                flag = 1;
                text[i] = text[i].Trim();
                for (int k = 0; k < text[i].Length; k++)
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


            for(int i = 0; i < countNewText; i++)
            {
                File.AppendAllText(@"D:\data.txt", dataFromHTML[i] + Environment.NewLine);
                Console.WriteLine(dataFromHTML[i]);
            }

            Console.WriteLine("End");
            return html;
        }


        static public string getTextFromHTML(string html)
        {
            for (int i=0;i<html.Length;i++)
            {
                if ((html[i] == '<')||(html[i] == '>')||(html[i] == '/'))
                {
                    int index1 = html.IndexOf(">");
                    int index2 = html.IndexOf("</");
                    return html.Substring(index1 + 1, index2 - index1 - 1);
                }
            }

            return "";
        }

    }
}
