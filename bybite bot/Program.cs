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
{
    class Program
    {        
        static void Main(string[] args)
        {
            
            string rsi = "";
            string average = "";
            string url = "";
           
            BTC_RSI_Scheme scheme = new BTC_RSI_Scheme();

            scheme.ReadFile();
            
            string temp = scheme.api;
            Authorization authorization = new Authorization(scheme.api, scheme.secret);
            scheme.SetTimeValue(authorization);
            try
            {
                scheme.ClosePosition(authorization);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }

            //Для теста
            //scheme.TestResponse(authorization);
            //Для теста

             Parse_RSI parse = new Parse_RSI();

              Console.WriteLine("");
              Console.WriteLine("");
              Console.WriteLine("");
              Console.WriteLine("BOT V0.812|TestByBit");
              Console.WriteLine("");
              Console.WriteLine("");

              System.Threading.Thread.Sleep(5000);

              int k = 0;
              int l = 0;          

                  while (true)
            {
                try {
                    try
                    {
                          rsi = parse.GetValueRSI();
                         average = parse.GetValueAverage();

                       // Console.WriteLine("rsi - ");
                      //  rsi = Console.ReadLine();
                      //  Console.WriteLine("average - ");
                      //  average = Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Произошла очередная ошибка считывания с сайта||" + DateTime.UtcNow.ToString());
                        continue;
                    }
                    if (k == 2000)
                    {
                        //Console.WriteLine(rsi + "||" + average + "||" + DateTime.UtcNow.ToString());
                        k = 0;
                    }
                    if (l == scheme.ReloadPageTime && scheme.ReloadPage)
                    {
                        parse.ReloadPage();
                        Console.WriteLine("Страница была перезагружена||" + DateTime.UtcNow.ToString());
                        System.Threading.Thread.Sleep(100);
                        l = 0;
                    }
                    scheme.SetValueStack(rsi, average, authorization);
                    //scheme.
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



