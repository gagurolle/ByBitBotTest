using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace bybite_bot
{
    //хуй
    class Post//Родительский класс для POST запросов
    {
        
    }

    class PlaceLeverage:Post//Отправить ордер
    {
        public string api_key            { get; set; }
        public string leverage               { get; set; }
        public string symbol              { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }

    public Authorization authorization;

        public string CreateRequest(Authorization authorization, int TimeValue)
        {
            api_key = authorization.apikey;
            timestamp = GetTimeStamp.ReturnTime(TimeValue);

            //sign = authorization.GetSign(CreateParamStr());
            sign = authorization.GetSign(CreateParamStr());
            return "/open-api/order/create";
        }
        private string CreateParamStr()
        {
            string s = "";
            s += api_key != null ? "api_key=" + api_key+"&" : "";
            s += leverage != null ? "leverage=" + leverage + "&" : "";
            s += symbol != null ? "symbol=" + symbol + "&" : "";
            s += timestamp != null ? "timestamp=" + timestamp + "&" : "";
         
            return s.Substring(0, s.Length - 1);
                        
        }
     
    }

    class PlaceOrder : Post//Отправить ордер
    {
        public string api_key           { get; set; }
        public string qty               { get; set; }
        public string side              { get; set; }
        public string symbol            { get; set; }
        public string order_type        { get; set; }
        public string price             { get; set; }
        public string time_in_force     { get; set; }
        public string take_profit       { get; set; }
        public string stop_loss         { get; set; }
        public string reduce_only       { get; set; }
        public string close_on_trigger  { get; set; }
        public string order_link_id     { get; set; }
        public string timestamp         { get; set; }
        public string trailing_stop     { get; set; }
        public string sign              { get; set; }

        public Authorization authorization;

        public string CreateRequest(Authorization authorization, int TimeValue)
        {
            api_key = authorization.apikey;
            timestamp = GetTimeStamp.ReturnTime(TimeValue);

            sign = authorization.GetSign(CreateParamStr());
            return "/v2/private/order/create";
        }
        private string CreateParamStr()
        {
            string s = "";
            s += api_key != null ? "api_key=" + api_key + "&" : "";
            s += close_on_trigger != null ? "close_on_trigger=" + close_on_trigger + "&" : "";
            s += order_type != null ? "order_type=" + order_type + "&" : "";
            s += order_link_id != null ? "order_link_id =" + order_link_id + "&" : "";
            s += price != null ? "price=" + price + "&" : "";
            s += qty != null ? "qty=" + qty + "&" : "";
            s += reduce_only != null ? "reduce_only=" + reduce_only + "&" : "";
            s += side != null ? "side=" + side + "&" : "";
            s += symbol != null ? "symbol=" + symbol + "&" : "";
            s += take_profit != null ? "take_profit=" + take_profit + "&" : "";
            s += time_in_force != null ? "time_in_force=" + time_in_force + "&" : "";
            s += timestamp != null ? "timestamp=" + timestamp + "&" : "";
            s += trailing_stop != null ? "trailing_stop=" + trailing_stop + "&" : "";
            s += stop_loss != null ? "stop_loss=" + stop_loss + "&" : "";
            return s.Substring(0, s.Length - 1);
        }

    }

  

    class CancelActiveOrder : Post//Отправить ордер
    {
        public string api_key { get; set; }
        public string order_id { get; set; }
        public string order_link_id { get; set; }
        public string symbol { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }

        public Authorization authorization;

        public string CreateRequest(Authorization authorization, int TimeValue)
        {
            api_key = authorization.apikey;
            timestamp = GetTimeStamp.ReturnTime(TimeValue);

            //sign = authorization.GetSign(CreateParamStr());
            sign = authorization.GetSign(CreateParamStr());
            return "/open-api/order/cancel";
        }
        private string CreateParamStr()
        {
            string s = "";
            s += api_key != null ? "api_key=" + api_key + "&" : "";
            s += order_id != null ? "order_id=" + order_id + "&" : "";
            s += order_link_id != null ? "order_link_id=" + order_link_id + "&" : "";
            s += symbol != null ? "symbol=" + symbol + "&" : "";
            s += timestamp != null ? "timestamp=" + timestamp + "&" : "";
            return s.Substring(0, s.Length - 1);
        }

    }

    class CancelAllActiveOrder : Post//Отправить ордер
    {
        public string api_key { get; set; }
        public string symbol = "BTCUSD";
        public string timestamp { get; set; }
        public string sign { get; set; }

        public Authorization authorization;

        public string CreateRequest(Authorization authorization, int TimeValue)
        {
            api_key = authorization.apikey;
            timestamp = GetTimeStamp.ReturnTime(TimeValue);

            //sign = authorization.GetSign(CreateParamStr());
            sign = authorization.GetSign(CreateParamStr());
            return "/v2/private/order/cancelAll";
        }
        private string CreateParamStr()
        {
            string s = "";
            s += api_key != null ? "api_key=" + api_key + "&" : "";
            s += symbol != null ? "symbol=" + symbol + "&" : "";
            s += timestamp != null ? "timestamp=" + timestamp + "&" : "";

            return s.Substring(0, s.Length - 1);


        }

    }

    class ReplaceOrder : Post//Отправить ордер
    {
        public string api_key { get; set; }
        public string order_id { get; set; }
        public string p_r_price { get; set; }
        public string p_r_qty { get; set; }
        public string symbol { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }

        public Authorization authorization;

        public string CreateRequest(Authorization authorization, int TimeValue)
        {
            api_key = authorization.apikey;
            timestamp = GetTimeStamp.ReturnTime(TimeValue);

            //sign = authorization.GetSign(CreateParamStr());
            sign = authorization.GetSign(CreateParamStr());
            return "/open-api/order/replace";
        }
        private string CreateParamStr()
        {
            string s = "";
            s += api_key != null ? "api_key=" + api_key + "&" : "";
            s += order_id != null ? "order_id=" + order_id + "&" : "";
            s += p_r_price != null ? "p_r_price=" + p_r_price + "&" : "";
            s += p_r_qty != null ? "p_r_qty=" + p_r_qty + "&" : "";
            s += symbol != null ? "symbol=" + symbol + "&" : "";
            s += timestamp != null ? "timestamp=" + timestamp + "&" : "";

            return s.Substring(0, s.Length - 1);


        }

    }

   

}
