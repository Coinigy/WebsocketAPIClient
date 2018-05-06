using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class TradeItem
    {
		[DataMember(Name = "market_history_id")]
	    public long MarketHistoryId { get; set; }

	    [DataMember(Name = "exchange")]
	    public string Exchange { get; set; }

	    [DataMember(Name = "marketid")]
	    public int Marketid { get; set; }

	    [DataMember(Name = "label")]
	    public string Label { get; set; }

	    [DataMember(Name = "tradeid")]
	    public string Tradeid { get; set; }

	    [DataMember(Name = "time")]
	    public string Time { get; set; }

	    [DataMember(Name = "price")]
	    public decimal Price { get; set; }

	    [DataMember(Name = "quantity")]
	    public decimal Quantity { get; set; }

	    [DataMember(Name = "total")]
	    public decimal Total { get; set; }

	    [DataMember(Name = "timestamp")]
	    public string Timestamp { get; set; }

	    [DataMember(Name = "time_local")]
	    public string TimeLocal { get; set; }

	    [DataMember(Name = "type")]
	    public string Type { get; set; }

	    [DataMember(Name = "exchId")]
	    public int ExchId { get; set; }

	    [DataMember(Name = "channel")]
	    public string Channel { get; set; }
	}
}
