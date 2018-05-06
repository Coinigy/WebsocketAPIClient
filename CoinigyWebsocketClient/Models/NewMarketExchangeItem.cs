using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class NewMarketExchangeItem
    {
        [DataMember(Name = "exch_alert_threshold")]
        public long ExchAlertThreshold { get; set; }

        [DataMember(Name = "exch_fee_upfront")]
        public long ExchFeeUpfront { get; set; }

        [DataMember(Name = "exch_agg_priority")]
        public long ExchAggPriority { get; set; }

        [DataMember(Name = "exch_agg_enabled")]
        public long ExchAggEnabled { get; set; }

        [DataMember(Name = "exch_added")]
        public string ExchAdded { get; set; }

        [DataMember(Name = "exch_agg_instance_id")]
        public long ExchAggInstanceId { get; set; }

        [DataMember(Name = "exch_alert_last_success")]
        public string ExchAlertLastSuccess { get; set; }

        [DataMember(Name = "exch_alert_last_checked")]
        public string ExchAlertLastChecked { get; set; }

        [DataMember(Name = "exch_alert_status")]
        public long ExchAlertStatus { get; set; }

        [DataMember(Name = "exch_enabled")]
        public long ExchEnabled { get; set; }

        [DataMember(Name = "exch_balances_unavailable")]
        public long ExchBalancesUnavailable { get; set; }

        [DataMember(Name = "exch_balance_enabled")]
        public long ExchBalanceEnabled { get; set; }

        [DataMember(Name = "exch_code")]
        public string ExchCode { get; set; }

        [DataMember(Name = "exch_fee_buy")]
        public long ExchFeeBuy { get; set; }

        [DataMember(Name = "exch_fee")]
        public double ExchFee { get; set; }

        [DataMember(Name = "exch_fee_sell")]
        public long ExchFeeSell { get; set; }

        [DataMember(Name = "exch_name")]
        public string ExchName { get; set; }

        [DataMember(Name = "exch_markets_inverted")]
        public long ExchMarketsInverted { get; set; }

        [DataMember(Name = "exch_id")]
        public long ExchId { get; set; }

        [DataMember(Name = "exch_monitor_enabled")]
        public long ExchMonitorEnabled { get; set; }

        [DataMember(Name = "exch_time_offset")]
        public string ExchTimeOffset { get; set; }

        [DataMember(Name = "exch_trading_unavailable")]
        public long ExchTradingUnavailable { get; set; }

        [DataMember(Name = "exch_prices_indexed")]
        public long ExchPricesIndexed { get; set; }

        [DataMember(Name = "exch_trade_enabled")]
        public long ExchTradeEnabled { get; set; }

        [DataMember(Name = "exch_url")]
        public string ExchUrl { get; set; }

        [DataMember(Name = "exch_ws_orders")]
        public long ExchWsOrders { get; set; }
    }
}
