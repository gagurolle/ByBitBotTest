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
    class BTC_RSI_Scheme : Constants
    {
        string url = "";
        double rsi = 0;//текущее значение RSI
        double average = 0;//Текущее значение цены
        double price = 0;
        bool positionEnd = false;
        int RSI_TEMP_COUNT_ORDER = 0;
        string TempSide = "";

        public void SetValueStack(string RSI, string AVERAGE, Authorization authorization)
        {


            try
            {
                rsi = double.Parse(RSI, CultureInfo.InvariantCulture);
                average = double.Parse(AVERAGE, CultureInfo.InvariantCulture);
            }
            catch (System.FormatException e)
            {   //Если поймали ошибку считывания с сайта - выходим из метода и пытаемся считать еще раз
                Console.WriteLine("Произошла ошибка обработки значения с tradingview");
                System.Threading.Thread.Sleep(3000);
                return;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Переходим НИЖНЮЮ границу RSI в первый раз
            if (rsi < RSI_LOW && orderflagLow == false && orderflagHigh == false)
            {
                price = PlaceActiveOrders(authorization, "Low");
                LAST_RSI_LOW = rsi;
                LAST_RSI_LOW_AVERAGE = price;
                orderflagLow = true;
                Console.WriteLine("Перешли границу RSI_LOW||" + DateTime.UtcNow.ToString());
                Console.WriteLine("LAST_RSI_LOW = {0}||orderflagLow={1}||LAST_RSI_LOW_AVERAGE={2}", LAST_RSI_LOW, orderflagLow, LAST_RSI_LOW_AVERAGE);
                Console.WriteLine("RSI = " + rsi + ", average = " + average);
                return;
            }

            //Ушли дальше от НИЖНЕЙ границы на заданный шаг
            if (rsi < (LAST_RSI_LOW - RSI_LOW_STEP) && average < LAST_RSI_LOW_AVERAGE && orderflagLow == true)
            {
                price = PlaceActiveOrders(authorization, "Low");
                LAST_RSI_LOW_AVERAGE = price;
                LAST_RSI_LOW = rsi;
                Console.WriteLine("Опустились еще ниже RSI_LOW||" + DateTime.UtcNow.ToString());
                Console.WriteLine("LAST_RSI_LOW = {0}||orderflagLow={1}||LAST_RSI_LOW_AVERAGE={2}", LAST_RSI_LOW, orderflagLow, LAST_RSI_LOW_AVERAGE);

                Console.WriteLine("RSI = " + rsi + ", average = " + average);
                return;
            }
            //Выходим из позиции при достижении цены последнего лимитного ордера, выставленного с НИЖНЕГО RSI
            if (orderflagLow == true && average >= LAST_RSI_LOW_AVERAGE && rsi > LAST_RSI_LOW)
            {
                Console.WriteLine("Продали лимтные ордера с LOW||" + DateTime.UtcNow.ToString());
                Console.WriteLine("LAST_RSI_LOW = {0}||orderflagLow={1}||LAST_RSI_LOW_AVERAGE={2}", LAST_RSI_LOW, orderflagLow, LAST_RSI_LOW_AVERAGE);
                orderflagLow = false;
                LAST_RSI_LOW_AVERAGE = 0;
                LAST_RSI_LOW = 0;
                RSI_HIGH_COUNT_ORDER = 0;
                RSI_LOW_COUNT_ORDER = 0;
                Console.WriteLine("RSI = " + rsi + ", average = " + average);
                return;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////

            //Переходим ВЕРХНЮЮ границу RSI в первый раз
            if (rsi > RSI_HIGH && orderflagHigh == false && orderflagLow == false && RSI_HIGH_COUNT_ORDER == 0)
            {
                price = PlaceActiveOrders(authorization, "High");
                LAST_RSI_HIGH = rsi;
                LAST_RSI_HIGH_AVERAGE = price;
                orderflagHigh = true;

                Console.WriteLine("Перешли границу RSI_HIGH||" + DateTime.UtcNow.ToString());
                Console.WriteLine("LAST_RSI_HIGH = {0}||orderflagHIGH={1}||LAST_RSI_HIGH_AVERAGE={2}||RSI_HIGH_COUNT_ORDER={3}", LAST_RSI_HIGH, orderflagHigh, LAST_RSI_HIGH_AVERAGE, RSI_HIGH_COUNT_ORDER);
                Console.WriteLine("RSI = " + rsi + ", average = " + average);

                return;
            }
            //Ушли дальше от ВЕРХНЕЙ границы на заданный шаг
            if (rsi > (LAST_RSI_HIGH + RSI_HIGH_STEP) && average > LAST_RSI_HIGH_AVERAGE && orderflagHigh == true)
            {
                price = PlaceActiveOrders(authorization, "High");

                LAST_RSI_HIGH_AVERAGE = price;
                LAST_RSI_HIGH = rsi;

                Console.WriteLine("Поднялись выше RSI_HIGH||" + DateTime.UtcNow.ToString());
                Console.WriteLine("LAST_RSI_HIGH = {0}||orderflagHigh={1}||LAST_RSI_HIGH_AVERAGE={2}||RSI_HIGH_COUNT_ORDER={3}", LAST_RSI_HIGH, orderflagHigh, LAST_RSI_HIGH_AVERAGE, RSI_HIGH_COUNT_ORDER);
                Console.WriteLine("RSI = " + rsi + ", average = " + average);

                return;
            }
            //Выходим из позиции при достижении цены последнего лимитного ордера, выставленного с ВЕРХНЕГО RSI
            if (orderflagHigh == true && average <= LAST_RSI_HIGH_AVERAGE && rsi < LAST_RSI_HIGH)
            {

                orderflagHigh = false;

                LAST_RSI_HIGH_AVERAGE = 0;
                LAST_RSI_HIGH = 100;
                RSI_HIGH_COUNT_ORDER = 0;
                RSI_LOW_COUNT_ORDER = 0;

                Console.WriteLine("Купили лимтные ордера с HIGH||" + DateTime.UtcNow.ToString());
                Console.WriteLine("LAST_RSI_HIGH = {0}||orderflagHigh={1}||LAST_RSI_HIGH_AVERAGE={2}||RSI_HIGH_COUNT_ORDER={3}", LAST_RSI_HIGH, orderflagHigh, LAST_RSI_HIGH_AVERAGE, RSI_HIGH_COUNT_ORDER);

                Console.WriteLine("RSI = " + rsi + ", average = " + average);
                return;
            }

        }

        
        public double PlaceActiveOrders(Authorization authorization, string position)
        {
            Console.WriteLine();
            SetPositionConstants(position);

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
            double Price = OrderRoot.result.price;

            //Если выставляем лимитный ордер не в первый раз - отменяем сначала все активные ордера
            if (RSI_TEMP_COUNT_ORDER > 0)
            {
               
                CancelAllActiveOrder cancelorder = new CancelAllActiveOrder();
                url = cancelorder.CreateRequest(authorization, TimeValue);//задаем путь запроса
                response = HTTP.Post(Makejson.Convert(cancelorder), url);
                Console.WriteLine("Отменили все ордера");
            }

            RSI_TEMP_COUNT_ORDER++;//Увеличили количество выставленных ордеров на 1, потому что сейчас выставим ордера

           
            Price = SetPrice(Price, FirstContract, position, 1);//Выставляем ордер от той цены позиции 


            for (int i = 0; i < RSI_TEMP_COUNT_ORDER; i++)//Пытаемся выставить лимитные ордера
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

                    Console.WriteLine("Не успеели выставить ,идем еще ниже - "+Count);

                    if (ResultGetOrder.ret_code != 0)
                    {
                        againplaceorder = true;
                        throw new Exception("Что-то пошло не так при получении значения ActiveOrderRealTime. отправленный запрос - " + getOrderjson + " ; полученный запрос - " + getOrderResponse);
                    }

                    OrderStatus = ResultGetOrder.result.order_status;

                    Console.WriteLine("Цена ордера, который мы выставили - " + ResultGetOrder.result.price);
                    if (OrderStatus == "Cancelled")
                    {
                        Console.WriteLine("не успели выставить ордер, идем с шагом 0.5");
                        againplaceorder = true;
                        Price = SetPrice(Price, 0.5, position, 2);
                        Count++;
                    }
                }
                if (Count >= 20)
                {
                    ClosePosition(authorization);

                    Console.WriteLine("Больше 20 итераций");
                    //return Price - ContractStep;
                    return SetPrice(Price, ContractStep, position, 2);
                }
                //Price += ContractStep;
                Price = SetPrice(Price, ContractStep, position, 1);
                Console.WriteLine("--итерация выставленного ордера:" + i + ";Всего нужно выставить:" + RSI_TEMP_COUNT_ORDER);
            }
            //  System.Threading.Thread.Sleep(1000);

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
            Console.WriteLine("RSI_LOW : " + RSI_LOW);
            Console.WriteLine("LAST_RSI_LOW : " + LAST_RSI_LOW);
            Console.WriteLine("RSI_LOW_COUNT_ORDER : " + RSI_LOW_COUNT_ORDER);
            Console.WriteLine("RSI_HIGH_COUNT_ORDER : " + RSI_HIGH_COUNT_ORDER);
            Console.WriteLine("LAST_RSI_LOW_AVERAGE : " + LAST_RSI_LOW_AVERAGE);
            Console.WriteLine("LAST_RSI_LOW_PRICE_BUY : " + LAST_RSI_LOW_PRICE_BUY);
            Console.WriteLine("RSI_LOW_STEP : " + RSI_LOW_STEP);
            Console.WriteLine("RSI_HIGH : " + RSI_HIGH);
            Console.WriteLine("LAST_RSI_HIGH : " + LAST_RSI_HIGH);
            Console.WriteLine("LAST_RSI_HIGH_AVERAGE  : " + LAST_RSI_HIGH_AVERAGE);
            Console.WriteLine("LAST_RSI_HIGH_PRICE_BUY : " + LAST_RSI_HIGH_PRICE_BUY);
            Console.WriteLine("RSI_HIGH_STEP : " + RSI_HIGH_STEP);
            Console.WriteLine("LastLongValue : " + LastLongValue);
            Console.WriteLine("LastShortValue : " + LastShortValue);
            Console.WriteLine("symbol : " + symbol);
            Console.WriteLine("orderflagLow : " + orderflagLow);
            Console.WriteLine("orderflagHigh : " + orderflagHigh);
        }

        public void TestResponse(Authorization authorization, string Price, Constants constants)
        {

            string OrderStatus = "";
            string url = "";

            placeorder = new PlaceOrder
            {
                api_key = api,
                qty = (ContractQty).ToString(),
                side = "Sell",
                symbol = symbol,
                price = Price,
                order_type = "Limit",
                time_in_force = "PostOnly",
                sign = sign
            };

            url = placeorder.CreateRequest(authorization, TimeValue);//задаем путь запроса

            var response = HTTP.Post(Makejson.Convert(placeorder), url);

            PlaceOrderRoot result = Makeclass<PlaceOrderRoot>.Get(response);


            var temp_order_id = result.result.order_id;

            GetActiveOrderRealTime getOrder = new GetActiveOrderRealTime
            {
                api_key = api,
                order_id = temp_order_id,
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
            LAST_RSI_HIGH_AVERAGE = 0;
            LAST_RSI_HIGH = 100;
            RSI_HIGH_COUNT_ORDER = 0;
            RSI_LOW_COUNT_ORDER = 0;
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
                RSI_TEMP_COUNT_ORDER = RSI_LOW_COUNT_ORDER;
                TempSide = "Buy";
                positionEnd = true;
            }
            if (position == "High" && positionEnd == false)
            {
                RSI_TEMP_COUNT_ORDER = RSI_HIGH_COUNT_ORDER;
                TempSide = "Sell";
                positionEnd = true;
            }
            if (position == "Low" && positionEnd == true)
            {
                RSI_LOW_COUNT_ORDER = RSI_TEMP_COUNT_ORDER;
                positionEnd = false;
            }
            if (position == "High" && positionEnd == true)
            {
                RSI_HIGH_COUNT_ORDER = RSI_TEMP_COUNT_ORDER;
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