using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bybite_bot
{
    public class Root { }
    public class RootObject
    {
        public int ret_code { get; set; }
        public string ret_msg { get; set; }
        public string ext_code { get; set; }

        public object ext_info { get; set; }
        public string time_now { get; set; }
        public int rate_limit_status { get; set; }
        public long rate_limit_reset_ms { get; set; }
        public string rate_limit { get; set; }
    }

    //PlaceOrder
    public class PlaceOrderRoot : RootObject
    {
        public PlaceOrderResult result { get; set; }
    }

    public class PlaceOrderResult
    {
        public int user_id { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public string order_type { get; set; }
        public double price { get; set; }
        public int qty { get; set; }
        public string time_in_force { get; set; }
        public string order_status { get; set; }
        public PlaceOrderExtFields ext_fields { get; set; }
        public int leaves_qty { get; set; }
        public double leaves_value { get; set; }
        public object reject_reason { get; set; }
        public int cross_seq { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string last_exec_time { get; set; }
        public int last_exec_price { get; set; }
        public string order_id { get; set; }
    }
    public class PlaceOrderExtFields
    {
        public string op_from { get; set; }
        // public int o_req_num { get; set; }
        public string xreq_type { get; set; }
        public int xreq_offset { get; set; }
        public string cross_status { get; set; }
    }
    //
    //
    //GetOrder
   
     public class GetActiveOrderRealTimeRoot : RootObject
    {
        public GetActiveOrderRealTimeResult result { get; set; }
    }

    public class GetActiveOrderRealTimeResult: Root
    {
        public int user_id { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public string order_type { get; set; }
        public string price { get; set; }
        public int qty { get; set; }
        public string time_in_force { get; set; }
        public string order_status { get; set; }
        public GetActiveOrderRealTimeExtFields ext_fields { get; set; }
        public int leaves_qty { get; set; }
        public string leaves_value { get; set; }
        public int cum_exec_qty { get; set; }
        public string reject_reason { get; set; }
        public string order_link_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string order_id { get; set; }
    }

    public class GetActiveOrderRealTimeExtFields
    {
       // public int o_req_num { get; set; }
        public string xreq_type { get; set; }
        public int xreq_offset { get; set; }
    }

    public class GetMyPositionResult
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int risk_id { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public int size { get; set; }
        public string position_value { get; set; }
        public string entry_price { get; set; }
        public int auto_add_margin { get; set; }
        public string leverage { get; set; }
        public string position_margin { get; set; }
        public string liq_price { get; set; }
        public string bust_price { get; set; }
        public string occ_closing_fee { get; set; }
        public string occ_funding_fee { get; set; }
        public string take_profit { get; set; }
        public string stop_loss { get; set; }
        public string trailing_stop { get; set; }
        public string position_status { get; set; }
        public int deleverage_indicator { get; set; }
        public string oc_calc_data { get; set; }
        public string order_margin { get; set; }
        public string wallet_balance { get; set; }
        public string realised_pnl { get; set; }
        public int unrealised_pnl { get; set; }
        public string cum_realised_pnl { get; set; }
        public int cross_seq { get; set; }
        public int position_seq { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class GetMyPositionRoot: RootObject
    {
        public GetMyPositionResult result { get; set; }
    }
}
