using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bybite_bot
{
    class Get { };

    class QueryActiveOrder : Get//Узнать активные ордера
    {
        public string api_key       { get; set; }
        public string order_id      { get; set; }
        public string order_link_id { get; set; }
        public string symbol        { get; set; }
        public string timestamp     { get; set; }
        public string sign          { get; set; }

        public Authorization authorization;

        public string CreateRequest(Authorization authorization, int TimeValue)
        {
            api_key = authorization.apikey;
            timestamp = GetTimeStamp.ReturnTime(TimeValue);

            //sign = authorization.GetSign(CreateParamStr());

            string paramstring = CreateParamStr();
            sign = authorization.GetSign(paramstring);
            return "/v2/private/order?"+paramstring+"&sign="+sign;
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

    class GetActiveOrder : Get//Узнать активные ордера
    {
        public string api_key       { get; set; }
        public string limit         { get; set; }
        public string order         { get; set; }
        public string order_id      { get; set; }
        public string order_link_id { get; set; }
        public string order_status  { get; set; }
        public string page          { get; set; }
        public string symbol        { get; set; }
        public string timestamp     { get; set; }
        public string sign          { get; set; }

        public Authorization authorization;

        public string CreateRequest(Authorization authorization, int TimeValue)
        {
            api_key = authorization.apikey;
            timestamp = GetTimeStamp.ReturnTime(TimeValue);

            //sign = authorization.GetSign(CreateParamStr());

            string paramstring = CreateParamStr();
            sign = authorization.GetSign(paramstring);
            return "/open-api/order/list?" + paramstring + "&sign=" + sign;
        }
        private string CreateParamStr()
        {
            string s = "";
            s += api_key != null ? "api_key=" + api_key + "&" : "";
            s += limit != null ? "limit=" + limit + "&" : "";
            s += order != null ? "order_id=" + order + "&" : "";
            s += order_id != null ? "order_id=" + order_id + "&" : "";
            s += order_link_id != null ? "order_link_id=" + order_link_id + "&" : "";
            s += order_status != null ? "order_status=" + order_status + "&" : "";
            s += page != null ? "page=" + page + "&" : "";
            s += symbol != null ? "symbol=" + symbol + "&" : "";
            s += timestamp != null ? "timestamp=" + timestamp + "&" : "";
            return s.Substring(0, s.Length - 1);
        }

    }

    class GetActiveOrderRealTime : Get//Узнать активные ордера
    {
        public string api_key       { get; set; } 
        public string order_id      { get; set; }
        public string order_link_id { get; set; }
        public string symbol        { get; set; }
        public string timestamp     { get; set; }
        public string sign          { get; set; }

        public Authorization authorization;

        public string CreateRequest(Authorization authorization, int TimeValue)
        {
            api_key = authorization.apikey;
            timestamp = GetTimeStamp.ReturnTime(TimeValue);

            //sign = authorization.GetSign(CreateParamStr());

            string paramstring = CreateParamStr();
            sign = authorization.GetSign(paramstring);
            return "/v2/private/order?" + paramstring + "&sign=" + sign;
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

    class GetUserLeverage
    {
        public string api_key       { get; set; }
        public string timestamp     { get; set; }
        public string sign          { get; set; }

        public Authorization authorization;

        public string CreateRequest(Authorization authorization, int TimeValue)
        {
            api_key = authorization.apikey;
            timestamp = GetTimeStamp.ReturnTime(TimeValue);

            //sign = authorization.GetSign(CreateParamStr());

            string paramstring = CreateParamStr();
            sign = authorization.GetSign(paramstring);
            return "/user/leverage?" + paramstring + "&sign=" + sign;
        }
        private string CreateParamStr()
        {
            string s = "";
            s += api_key != null ? "api_key=" + api_key + "&" : "";
            s += timestamp != null ? "timestamp=" + timestamp + "&" : "";
            return s.Substring(0, s.Length - 1);


        }
    }

    class GetMyPosition : Get//Узнать активные ордера
    {
        public string api_key       { get; set; }
        public string symbol        { get; set; }
        public string timestamp     { get; set; }
        public string sign          { get; set; }

        public Authorization authorization;

        public string CreateRequest(Authorization authorization, int TimeValue)
        {
            api_key = authorization.apikey;
            timestamp = GetTimeStamp.ReturnTime(TimeValue);

            //sign = authorization.GetSign(CreateParamStr());

            string paramstring = CreateParamStr();
            sign = authorization.GetSign(paramstring);
            return "/v2/private/position/list?" + paramstring + "&sign=" + sign;
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

   
}
