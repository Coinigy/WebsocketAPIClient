using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
	public class FavoriteDataItem
	{
		[DataMember(Name = "exch_code")]
		public string ExchCode { get; set; }

		[DataMember(Name = "last_price")]
		public decimal LastPrice { get; set; }

		[DataMember(Name = "btc_volume_24")]
		public decimal BtcVolume24 { get; set; }

		[DataMember(Name = "base_curr")]
		public string BaseCurr { get; set; }

		[DataMember(Name = "display_name")]
		public string DisplayName { get; set; }

		[DataMember(Name = "exch_name")]
		public string ExchName { get; set; }

		[DataMember(Name = "exch_id")]
		public int ExchId { get; set; }

		[DataMember(Name = "exchmkt_id")]
		public int ExchmktId { get; set; }

		[DataMember(Name = "mkt_name")]
		public string MktName { get; set; }

		[DataMember(Name = "mkt_id")]
		public int MktId { get; set; }

		[DataMember(Name = "primary_curr")]
		public string PrimaryCurr { get; set; }

		[DataMember(Name = "volume_24")]
		public decimal Volume24 { get; set; }

		[DataMember(Name = "percent_24")]
		public decimal Percent24 { get; set; }
	}
}
