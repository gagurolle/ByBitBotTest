using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;


namespace bybite_bot

{
  
    class ConfigFile
    {
        async void ReadFile(Makejson makejson)
        {
            string json = "";
            try
            {
                string path = @"\config.txt";
                using (StreamReader sr = new StreamReader(path))
                {
                    Console.WriteLine(sr.ReadToEnd());
                }
                // асинхронное чтение
                using (StreamReader sr = new StreamReader(path))
                {
                        json = await sr.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Constants ty = (Constants)JsonConvert.DeserializeObject(json, typeof(Constants));
           // return ty;
        }
    }

    class Constants//Константы
    {
        public string api                { get; set; }
        public string secret            { get; set; }
        public string sign              { get; set; }
        public int    valueStack        { get; set; }
        public string leverage           { get; set; }
        public double ContractStep      { get; set; }
        public int       ContractQty        { get; set; }
        public double RSI_LOW            { get; set; }
        public double RSI_HIGH          { get; set; }
        public  double RSI_LOW_STEP     { get; set; }
        public  double RSI_HIGH_STEP    { get; set; }
        public  string symbol           { get; set; }
        public int           SleepValue           { get; set; }
        public int           FirstContract { get; set; }
        public static string AvailableBalance { get; set; }
        public static double LastLongValue { get; set; }
        public static double LastShortValue { get; set; }
        public static double LAST_RSI_LOW                    = 0;
        public static double LAST_RSI_LOW_AVERAGE            = 0;
        public static double LAST_RSI_LOW_PRICE_BUY          = 0;
        public static int    RSI_LOW_COUNT_ORDER             = 0;
        public int           RSI_HIGH_COUNT_ORDER            = 0;
        public static double LAST_RSI_HIGH                   = 0;
        public static double LAST_RSI_HIGH_AVERAGE           = 0;
        public static double LAST_RSI_HIGH_PRICE_BUY         = 0;        
        public static bool      orderflagLow             = false;
        public static bool      orderflagHigh           = false;
        public static bool ReloadPage = false;
        public static int ReloadPageTime = 10000;
        public int TimeValue = 0;

        public static PlaceOrder placeorder;

        public void SetTimeValue(Authorization authorization)
        {
            for (int i = 0; i < 10; i++) {
                GetMyPosition getPosition = new GetMyPosition { symbol = symbol };
                string getPositionRequest = getPosition.CreateRequest(authorization, i);
                string getPositionResponse = HTTP.Get(getPositionRequest);
                GetMyPositionRoot ResultGetPosition = Makeclass<GetMyPositionRoot>.Get(getPositionResponse);
                if (ResultGetPosition.ret_code == 0)
                {
                    TimeValue = i;
                    Console.WriteLine("Шаг времени = " + i);
                    return;
                }
            }
            throw new Exception("Ошибка синхонизации времени с сервером");
        }

        public void ReadFile()
        {
            string json = "";
            try
            {
                string path = @"C:\config.txt";
                using (StreamReader sr = new StreamReader(path))
                {
                    json = sr.ReadToEnd();
                    Console.WriteLine(json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Constants ty = (Constants)JsonConvert.DeserializeObject(json, typeof(Constants));
            api = ty.api;
            valueStack = ty.valueStack;
            leverage = ty.leverage;
                          
            ContractStep = ty.ContractStep;
            ContractQty = ty.ContractQty;
            RSI_LOW = ty.RSI_LOW;
            RSI_HIGH = ty.RSI_HIGH;
                          
            RSI_LOW_STEP = ty.RSI_LOW_STEP;
            RSI_HIGH_STEP = ty.RSI_HIGH_STEP;
            symbol = ty.symbol;
            SleepValue = ty.SleepValue;
            secret = ty.secret;
            FirstContract = ty.FirstContract;
        }
    }

    abstract class GetTimeStamp//Получение текущего времени
    {
        public static string TimeStamp { get; set; }
        public static string ReturnTime(int TimeValue)
        {
            int TimeStamps = Convert.ToInt32((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds) + TimeValue;
            TimeStamp = TimeStamps + "000";
            return TimeStamp;
        }
    }

    class Authorization //Авторизация.
    {
        public string apikey { get; set; }
        public string secret { get; set; }
        // public string timestamp;
        public static string sign { get; set; }
        public Authorization(string _apikey, string _secret)
        {
            apikey = _apikey;
            secret = _secret;
            // timestamp = TimeStamp;
        }
        public string GetSign(string paramstr)
        {
            Test test = new Test();
            return test.CreateSignature(secret, paramstr);
        }
        public string GetApiKey() { return apikey; }
        public string GetSecret() { return secret; }
    }

    public class Test//создание SIGN
    {

        public string CreateSignature(string secret, string message)
        {
            var signatureBytes = Hmacsha256(Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(message));

            return ByteArrayToString(signatureBytes);
        }

        private static byte[] Hmacsha256(byte[] keyByte, byte[] messageBytes)
        {
            using (var hash = new HMACSHA256(keyByte))
            {
                return hash.ComputeHash(messageBytes);
            }
        }


        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);

            foreach (var b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }

    abstract class Makejson//Делаем Json из класса
    {
       // public Post post { get; set; }

        public static string Convert(Post post)
        {
       
            string json = JsonConvert.SerializeObject(post, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore//Не учитывая элементы null
            });
            return (json);
        }
    }

    abstract class Makeclass<T>//Делаем класс из Jsona
    {
        static public T Get(string json)
        {
           // var y = (T)JsonConvert.DeserializeObject(json, typeof(T));
            return (T)JsonConvert.DeserializeObject(json, typeof(T));

        }

    }

}
