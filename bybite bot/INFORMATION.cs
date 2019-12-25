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
    class Program2
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

        static void F1Main7()
        {
            

            string rsi = "";
            double rsi_low = 80.0d;
            double rsi_high = 80.0d;
            string average = "";
            string url = "";


            Authorization authorization = new Authorization(apikey, secret);
            PlaceOrder placeorder = new PlaceOrder();
           // Makejson makejson = new Makejson();

            placeorder.side = "Sell";
            placeorder.symbol = "BTCUSD";
            placeorder.order_type = "Limit";
            placeorder.qty = "10";
            placeorder.price = "8083";
            placeorder.time_in_force = "GoodTillCancel";

            url = placeorder.CreateRequest(authorization);//задаем путь запроса
           // makejson.post = placeorder;
            var ty = HTTP.Post(Makejson.Convert(placeorder), url);

            QueryActiveOrder queryActiveOrder = new QueryActiveOrder();
            queryActiveOrder.symbol = "BTCUSD";
            queryActiveOrder.order_id = "569fbea4-42a9-4ad8-9514-6a1ae7abf7bb";

            var t = HTTP.Get(queryActiveOrder.CreateRequest(authorization));


            //  string url = placeorder.CreateRequest(authorization);//задаем путь запроса
            //создаем объект класса для создания JSON

            // string g = placeorder.sign;



            Parse_RSI parse = new Parse_RSI();
            while (true)
            {
                rsi = parse.GetValueRSI();
                average = parse.GetValueAverage();

                Console.WriteLine(rsi);

                if (double.Parse(rsi, CultureInfo.InvariantCulture) < rsi_low)
                {

                    Console.WriteLine("Отправили ордер");
                    System.Threading.Thread.Sleep(1000);

                };
                System.Threading.Thread.Sleep(100);


            }
            //Console.ReadLine();
            //Login();

        }
    }




}