using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace bybite_bot
{
   abstract class HTTP
    {
        //Запросы ввиде GET должны быть представлены в виде x-www-form-urlencoded
        public static string Get(string paramstr)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.bybit.com"+paramstr);
            Console.WriteLine("https://api.bybit.com"+paramstr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        //Запросы типа POST  должны быть представлены в виде JSON
        public static string Post(string json, string url)
        {
           // string siteurl = "https://api.bybit.com/" + url;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.bybit.com"+url);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
               return streamReader.ReadToEnd();
            }
        }
    }

    

}
