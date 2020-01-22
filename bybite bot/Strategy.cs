using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace bybite_bot
{
    class BTC_MAC_Scheme : Constants
    {
        string url = "";
        double rsi = 0;//текущее значение MAC
        double average = 0;//Текущее значение цены
        double price = 0;
        bool positionEnd = false;
        int MAC_TEMP_COUNT_ORDER = 0;
        string TempSide = "";

        public void SetValueStack(string MAC, string AVERAGE, Authorization authorization)
        {
            try
            {
                rsi = double.Parse(MAC, CultureInfo.InvariantCulture);
                average = double.Parse(AVERAGE, CultureInfo.InvariantCulture);
            }
            catch (System.FormatException e)
            {   //Если поймали ошибку считывания с сайта - выходим из метода и пытаемся считать еще раз
                Console.WriteLine("Произошла ошибка обработки значения с tradingview");
                System.Threading.Thread.Sleep(3000);
                return;
            }

            //////////////////////////////////////////////
            //ЗДЕСЬ БЫЛА ОПРЕДЕЛЕННАЯ ЛОГИКА ПРИЛОЖЕНИЯ//
            /////////////////////////////////////////////
            

        }

        
        public double PlaceActiveOrders(Authorization authorization, string position)//Выставить активный ордер
        {
            Console.WriteLine();
            SetPositionConstants(position);

            bool againplaceorder = true;
            int Count = 0;
            var OrderStatus = "";
            string LastLimitPrice = "";
            //Выставляем контракт по рынку
            placeorder = new PlaceOrder
            {
                api_key = api,
                qty = ContractQty.ToString(),
                side = TempSide,
                symbol = symbol,
                order_type = "Market",
                time_in_force = "GoodTillCancel",
                sign = sign
            };

            url = placeorder.CreateRequest(authorization, TimeValue);//задаем путь запроса
            //получаем ответ от выставленных контрактов
            var response = HTTP.Post(Makejson.Convert(placeorder), url);

            PlaceOrderRoot OrderRoot = Makeclass<PlaceOrderRoot>.Get(response);

            //Получаем цену ,по которой вошли в позицию
            double Price = OrderRoot.result.price;

            //Если выставляем лимитный ордер не в первый раз - отменяем сначала все активные ордера
            if (MAC_TEMP_COUNT_ORDER > 0)
            {
               
                CancelAllActiveOrder cancelorder = new CancelAllActiveOrder();
                url = cancelorder.CreateRequest(authorization, TimeValue);//задаем путь запроса
                response = HTTP.Post(Makejson.Convert(cancelorder), url);
                Console.WriteLine("Отменили все ордера");
            }

            MAC_TEMP_COUNT_ORDER++;//Увеличили количество выставленных ордеров на 1, потому что сейчас выставим ордера

           
            Price = SetPrice(Price, FirstContract, position, 1);//Выставляем ордер от той цены позиции 


            for (int i = 0; i < MAC_TEMP_COUNT_ORDER; i++)//Пытаемся выставить лимитные ордера
            {
                while (againplaceorder && Count < 20)//Если цена пошла быстрее чем мы выставили ордер, то начинаем спамить с определенным шагом
                {
                    againplaceorder = false;
                    placeorder = new PlaceOrder
                    {
                        api_key = api,
                        qty = (ContractQty).ToString(),
                        side = TempSide == "Sell" ? "Buy" : "Sell",
                        symbol = symbol,
                        price = Price.ToString(),
                        order_type = "Limit",
                        time_in_force = "PostOnly",
                        sign = sign
                    };

                    url = placeorder.CreateRequest(authorization, TimeValue);//задаем путь запроса
                    response = HTTP.Post(Makejson.Convert(placeorder), url);

                    PlaceOrderRoot placeOrderRoot = Makeclass<PlaceOrderRoot>.Get(response);


                    var temp_order_id = placeOrderRoot.result.order_id;

                    GetActiveOrderRealTime getOrder = new GetActiveOrderRealTime
                    {
                        api_key = api,
                        order_id = temp_order_id,
                        symbol = symbol,
                        sign = sign
                    };
                    string getOrderjson = getOrder.CreateRequest(authorization, TimeValue);
                    var getOrderResponse = HTTP.Get(getOrderjson);
                    GetActiveOrderRealTimeRoot ResultGetOrder = Makeclass<GetActiveOrderRealTimeRoot>.Get(getOrderResponse);
                    if (Count != 0) {
                        Console.WriteLine("Не успеели выставить ,идем еще ниже - " + Count);
                    }
                    if (ResultGetOrder.ret_code != 0)
                    {
                        againplaceorder = true;
                        throw new Exception("Что-то пошло не так при получении значения ActiveOrderRealTime. отправленный запрос - " + getOrderjson + " ; полученный запрос - " + getOrderResponse);
                    }

                    OrderStatus = ResultGetOrder.result.order_status;
                    

                    Console.WriteLine("Цена ордера, который мы выставили - " + ResultGetOrder.result.price);
                    Console.WriteLine("Статус выставленного оордера - " + OrderStatus);
                    if (OrderStatus == "Cancelled")
                    {
                        Console.WriteLine("не успели выставить ордер, идем с шагом 0.5");
                        againplaceorder = true;
                        Price = SetPrice(Price, 0.5, position, 2);
                        Count++;
                    }
                    LastLimitPrice = ResultGetOrder.result.price;
                }
                if (Count >= 20)
                {
                    ClosePosition(authorization);

                    Console.WriteLine("Больше 20 итераций");
                    //return Price - ContractStep;
                    return SetPrice(Price, ContractStep, position, 2);
                }
                else
                {
                    Console.WriteLine("Могла бы быть такая цена - " + SetPrice(Price, ContractStep, position, 1));
                    Price = double.Parse(LastLimitPrice, CultureInfo.InvariantCulture);
                    Console.WriteLine("Но сейчас такая цена последняя выставленного ордера по запросу GET - "+Price);
                }
                
               // Price = SetPrice(Price, ContractStep, position, 1);
                Console.WriteLine("--итерация выставленного ордера:" + i + ";Всего нужно выставить:" + MAC_TEMP_COUNT_ORDER);
            }


            SetPositionConstants(position);
            Console.WriteLine();
            return SetPrice(Price, ContractStep, position, 2);
        }


        public void PrintConstants()
        {
            Console.WriteLine("api : " + api);
            Console.WriteLine("sign  : " + sign);
            Console.WriteLine("valueStack  : " + valueStack);
            Console.WriteLine("leverage : " + leverage);
            Console.WriteLine("AvailableBalance : " + AvailableBalance);
            Console.WriteLine("ContractStep : " + ContractStep);
            Console.WriteLine("ContractQty : " + ContractQty);
            Console.WriteLine("MAC_LOW : " + MAC_LOW);
            Console.WriteLine("LAST_MAC_LOW : " + LAST_MAC_LOW);
            Console.WriteLine("MAC_LOW_COUNT_ORDER : " + MAC_LOW_COUNT_ORDER);
            Console.WriteLine("MAC_HIGH_COUNT_ORDER : " + MAC_HIGH_COUNT_ORDER);
            Console.WriteLine("LAST_MAC_LOW_AVERAGE : " + LAST_MAC_LOW_AVERAGE);
            Console.WriteLine("LAST_MAC_LOW_PRICE_BUY : " + LAST_MAC_LOW_PRICE_BUY);
            Console.WriteLine("MAC_LOW_STEP : " + MAC_LOW_STEP);
            Console.WriteLine("MAC_HIGH : " + MAC_HIGH);
            Console.WriteLine("LAST_MAC_HIGH : " + LAST_MAC_HIGH);
            Console.WriteLine("LAST_MAC_HIGH_AVERAGE  : " + LAST_MAC_HIGH_AVERAGE);
            Console.WriteLine("LAST_MAC_HIGH_PRICE_BUY : " + LAST_MAC_HIGH_PRICE_BUY);
            Console.WriteLine("MAC_HIGH_STEP : " + MAC_HIGH_STEP);
            Console.WriteLine("LastLongValue : " + LastLongValue);
            Console.WriteLine("LastShortValue : " + LastShortValue);
            Console.WriteLine("symbol : " + symbol);
            Console.WriteLine("orderflagLow : " + orderflagLow);
            Console.WriteLine("orderflagHigh : " + orderflagHigh);
        }

        public void TestResponse(Authorization authorization)
        {

            Console.WriteLine();
            GetMyPosition getPosition = new GetMyPosition { symbol = symbol };
            string getPositionRequest = getPosition.CreateRequest(authorization, TimeValue);
            string getPositionResponse = HTTP.Get(getPositionRequest);
            GetMyPositionRoot ResultGetPosition = Makeclass<GetMyPositionRoot>.Get(getPositionResponse);
            Console.ReadLine();
            bool againplaceorder = true;
            int Count = 0;
            var OrderStatus = "";
            //Выставляем контракт по рынку
            placeorder = new PlaceOrder
            {
                api_key = api,
                qty = ContractQty.ToString(),
                side = TempSide,
                symbol = symbol,
                order_type = "Market",
                time_in_force = "GoodTillCancel",
                sign = sign
            };

            url = placeorder.CreateRequest(authorization, TimeValue);//задаем путь запроса
            //получаем ответ от выставленных контрактов
            var response = HTTP.Post(Makejson.Convert(placeorder), url);

            PlaceOrderRoot OrderRoot = Makeclass<PlaceOrderRoot>.Get(response);

            //Получаем цену ,по которой вошли в позицию
         //   double Price = OrderRoot.result.price;



            GetActiveOrderRealTime getOrder = new GetActiveOrderRealTime
            {
                api_key = api,
       //         order_id = temp_order_id,
                symbol = symbol,
                sign = sign
            };

            var p = HTTP.Get(getOrder.CreateRequest(authorization, TimeValue));
            GetActiveOrderRealTimeRoot ResultGetOrder = Makeclass<GetActiveOrderRealTimeRoot>.Get(p);
            OrderStatus = ResultGetOrder.result.order_status;

            if (OrderStatus == "Cancelled")
            {
                Console.WriteLine("STOP");
            }
        }

        public void ClosePosition(Authorization authorization)//Закрытие позиции
        {
            //Makejson makejson = new Makejson();
            //Обнуляем переменные
            orderflagHigh = false;
            LAST_MAC_HIGH_AVERAGE = 0;
            LAST_MAC_HIGH = 100;
            MAC_HIGH_COUNT_ORDER = 0;
            MAC_LOW_COUNT_ORDER = 0;
            //отменяем все ордера
            CancelAllActiveOrder cancelorder = new CancelAllActiveOrder();
            url = cancelorder.CreateRequest(authorization, TimeValue);//задаем путь запроса

            string response = HTTP.Post(Makejson.Convert(cancelorder), url);
            //Получаем значение позиции
            GetMyPosition getPosition = new GetMyPosition { symbol = symbol };
            string getPositionRequest = getPosition.CreateRequest(authorization, TimeValue);
            string getPositionResponse = HTTP.Get(getPositionRequest);
            GetMyPositionRoot ResultGetPosition = Makeclass<GetMyPositionRoot>.Get(getPositionResponse);
            if (ResultGetPosition.ret_code != 0)
            {
                throw new Exception("Ошибка в закрытии позиции. RetCode = " + ResultGetPosition.ret_code + "Отправленный запрос - " + getPositionRequest + "; полученный запрос - " + getPositionResponse);
            }
            int _qty = ResultGetPosition.result.size;//Количество контрактов
            string _side = ResultGetPosition.result.side;//Long or Short             
            //Выставляем рыночный ордер, чтобы закрыть позицию
            placeorder = new PlaceOrder
            {
                api_key = api,
                qty = _qty.ToString(),
                side = _side == "Sell" ? "Buy" : "Sell",
                symbol = symbol,
                order_type = "Market",
                time_in_force = "GoodTillCancel",
                sign = sign
            };

            url = placeorder.CreateRequest(authorization, TimeValue);//задаем путь запроса

            var responseOrder = HTTP.Post(Makejson.Convert(placeorder), url);
            PlaceOrderRoot OrderTemp1 = Makeclass<PlaceOrderRoot>.Get(responseOrder);
            if (OrderTemp1.ret_code == 0)
            {
                Console.WriteLine("закрыли позицию без ошибок");
            }
            else
            {
                Console.WriteLine("что-то пошло не так при закрытии ошибки");
            };
        }

        void SetPositionConstants(string position)
        {
            if (position == "Low" && positionEnd == false)
            {
                MAC_TEMP_COUNT_ORDER = MAC_LOW_COUNT_ORDER;
                TempSide = "Buy";
                positionEnd = true;
            }
            if (position == "High" && positionEnd == false)
            {
                MAC_TEMP_COUNT_ORDER = MAC_HIGH_COUNT_ORDER;
                TempSide = "Sell";
                positionEnd = true;
            }
            if (position == "Low" && positionEnd == true)
            {
                MAC_LOW_COUNT_ORDER = MAC_TEMP_COUNT_ORDER;
                positionEnd = false;
            }
            if (position == "High" && positionEnd == true)
            {
                MAC_HIGH_COUNT_ORDER = MAC_TEMP_COUNT_ORDER;
                positionEnd = false;
            }

        }

        double SetPrice(double main, double add, string position, int situation)
        {
            if (position == "Low" && situation == 1)
            {
                return main + add;
            }
            if (position == "High" && situation == 1)
            {
                return main - add;
            }
            if (position == "Low" && situation == 2)
            {
                return main - add;
            }
            if (position == "High" && situation == 2)
            {
                return main + add;
            }
            return 1;
        }

        

    }

}