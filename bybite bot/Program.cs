using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace bybite_bot
{// const string url = ;
    class Program
    {       
        public static string TimeStamp;
        public static string apikey = "7r3TSAIjPbhCwrUV0c";
        public static string secret = "tL4nqJ5pjkoiXnVh0piupPTSUWgPg2iEcHmK";
        public static string GetString;
        public static string side;
        public static string symbol;
        public static string order_type;
        public static string qty;
        public static string price;
        public static string time_in_force;

        static void Main(string[] args)
        {

            string rsi = "";
            string average = "";
            string url = "";

            //Constants constants = new Constants();
           
            BTC_RSI_Scheme scheme = new BTC_RSI_Scheme();

            scheme.ReadFile();
            
            string temp = scheme.api;
            Authorization authorization = new Authorization(scheme.api, scheme.secret);
            try
            {
                scheme.ClosePosition(authorization);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
         //       return;
            }
            // scheme.TestResponse(authorization, "7566");
             Parse_RSI parse = new Parse_RSI();

            Console.WriteLine("");
              Console.WriteLine("");
              Console.WriteLine("");
              Console.WriteLine("BOT V0.5|TestByBit");
              Console.WriteLine("");
              Console.WriteLine("");
              System.Threading.Thread.Sleep(5000);
              int k = 0;
              int l = 0;          
                  while (true)
            {
                try {
                    //throw new Exception("Тестовая ошибка");
                    try
                    {
                         rsi = parse.GetValueRSI();
                         average = parse.GetValueAverage();
                       // Console.WriteLine("rsi - ");
                        //rsi = Console.ReadLine();
                        //Console.WriteLine("average - ");
                        //average = Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Произошла очередная ошибка считывания с сайта||" + DateTime.UtcNow.ToString());
                        continue;
                    }
                    if (k == 300)
                    {
                        Console.WriteLine(rsi + "||" + average + "||" + DateTime.UtcNow.ToString());
                        k = 0;
                    }
                    if (l == 6001)
                    {
                     //   parse.ReloadPage();
                        Console.WriteLine("Страница была перезагружена||" + DateTime.UtcNow.ToString());
                        System.Threading.Thread.Sleep(100);
                        l = 0;
                    }
                    scheme.SetValueStack(rsi, average, authorization);
                    System.Threading.Thread.Sleep(scheme.SleepValue);
                    k++;
                    l++;
                }
                catch (Exception e)
                  {
                    scheme.ClosePosition(authorization);
                    Console.WriteLine("..............................................");
                   Console.WriteLine("Произошла ошибка!!!! ");
                  Console.WriteLine("Время ошибки: " + DateTime.UtcNow.ToString());
                  Console.WriteLine("Суть ошибки: " + e.Message);
                   Console.WriteLine("..............................................");
                  scheme.PrintConstants();
                  }
                }
           //   Console.ReadLine();
             // Login();  
             
        }
    }
}



