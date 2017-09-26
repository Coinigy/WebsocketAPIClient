using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NewMarketExchangeItem
    {
        [JsonProperty("exch_alert_threshold")]
        public long ExchAlertThreshold { get; set; }

        [JsonProperty("exch_fee_upfront")]
        public long ExchFeeUpfront { get; set; }

        [JsonProperty("exch_agg_priority")]
        public long ExchAggPriority { get; set; }

        [JsonProperty("exch_agg_enabled")]
        public long ExchAggEnabled { get; set; }

        [JsonProperty("exch_added")]
        public string ExchAdded { get; set; }

        [JsonProperty("exch_agg_instance_id")]
        public long ExchAggInstanceId { get; set; }

        [JsonProperty("exch_alert_last_success")]
        public string ExchAlertLastSuccess { get; set; }

        [JsonProperty("exch_alert_last_checked")]
        public string ExchAlertLastChecked { get; set; }

        [JsonProperty("exch_alert_status")]
        public long ExchAlertStatus { get; set; }

        [JsonProperty("exch_enabled")]
        public long ExchEnabled { get; set; }

        [JsonProperty("exch_balances_unavailable")]
        public long ExchBalancesUnavailable { get; set; }

        [JsonProperty("exch_balance_enabled")]
        public long ExchBalanceEnabled { get; set; }

        [JsonProperty("exch_code")]
        public string ExchCode { get; set; }

        [JsonProperty("exch_fee_buy")]
        public long ExchFeeBuy { get; set; }

        [JsonProperty("exch_fee")]
        public double ExchFee { get; set; }

        [JsonProperty("exch_fee_sell")]
        public long ExchFeeSell { get; set; }

        [JsonProperty("exch_name")]
        public string ExchName { get; set; }

        [JsonProperty("exch_markets_inverted")]
        public long ExchMarketsInverted { get; set; }

        [JsonProperty("exch_id")]
        public long ExchId { get; set; }

        [JsonProperty("exch_monitor_enabled")]
        public long ExchMonitorEnabled { get; set; }

        [JsonProperty("exch_time_offset")]
        public string ExchTimeOffset { get; set; }

        [JsonProperty("exch_trading_unavailable")]
        public long ExchTradingUnavailable { get; set; }

        [JsonProperty("exch_prices_indexed")]
        public long ExchPricesIndexed { get; set; }

        [JsonProperty("exch_trade_enabled")]
        public long ExchTradeEnabled { get; set; }

        [JsonProperty("exch_url")]
        public string ExchUrl { get; set; }

        [JsonProperty("exch_ws_orders")]
        public long ExchWsOrders { get; set; }
    }
}
