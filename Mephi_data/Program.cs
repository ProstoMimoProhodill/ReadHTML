﻿using System;
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
            string html = "";
            string[] data;

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

            data = getTextFromHTML(html);

            for (int i = 0;i<data.Count();i++)
            {
                Console.WriteLine(data[i]);
            }

            Console.Read();
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
