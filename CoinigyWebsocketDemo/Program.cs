using System;
using System.Threading;
using CoinigyWebsocketClient;
using CoinigyWebsocketClient.Models;
using CoinigyWebsocketClient.Types;
using PureSocketCluster;

namespace CoinigyWebsocketDemo
{
	public class Program
	{
		private static readonly ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
		private static CoinigyWsClient _client;

		public static void Main()
		{
			// We can use the console built into the CoinigyWebsocketClient to ensure we don't lock up our program or cause delays. Even though
			// the normal console will work, be aware that writing to the console is a blocking operation and lots of writes can slow down
			// your program. This is why using the builtin OutpuConsole class is a better option.
			OutputConsole.WriteLine("Starting Coinigy Websocket Demo Client!", ConsoleColor.DarkGreen, ConsoleColor.White);

			// lets create an instance of the client class and set it to debug mode, normally we would not use debug mode since it spits out a lot of information
			_client = new CoinigyWsClient(new CoinigyWebSocketClientOptions { Creds = new Creds { apiKey = "your_api_key", apiSecret = "your_api_secret" }, DebugMode = true });

			// lets hook up some events
			_client.OnClientReady += Client_OnClientReady;
			_client.OnFatality += _client_OnFatality;
			_client.OnClosed += _client_OnClosed;
			_client.OnStateChanged += _client_OnStateChanged;
			// there are many more events for us to use, play around

			// we can handle messages in a few different ways
			// 1. we can specificly handle certain messages such as orders and trades
			_client.OnOrderMessage += _client_OnOrderMessage;
			_client.OnTradeMessage += _client_OnTradeMessage;
			_client.OnBlockMessage += _client_OnBlockMessage;
			_client.OnFavoriteMessage += _client_OnFavoriteMessage;
			_client.OnNewsMessage += _client_OnNewsMessage;
			_client.OnNotificationMessage += _client_OnNotificationMessage;
			// 2. we can handle all messages ourself if we want
			_client.OnMessage += _client_OnMessage;

			// 3. when we subcribe to a channel we can attach a callback as demonstrated in Client_OnClientReady with MyCustomCallback

			// lets try and open our connection, we must remember we are not authorized to do anything until OnClientReady is fired
			var connectionResult = _client.Connect();
			OutputConsole.WriteLine($"Connected = {connectionResult}", ConsoleColor.Green);

			// we don't want our app to close so lets block
			ManualResetEvent.WaitOne();
		}

		private static void _client_OnStateChanged(System.Net.WebSockets.WebSocketState newState, System.Net.WebSockets.WebSocketState prevState)
		{
			// the connection state has changed, is this bad? that is for you to decide if you want to
			OutputConsole.WriteLine($"Socket state changed from {prevState} to {newState}", ConsoleColor.Gray);
		}

		private static void _client_OnClosed(System.Net.WebSockets.WebSocketCloseStatus reason)
		{
			// the connection was closed, decide what you want to do here
			OutputConsole.WriteLine($"Socket Closed reason: {reason}", ConsoleColor.Red);
		}

		private static void _client_OnFatality(string reason)
		{
			// a fatality is extremely severe and we should just give up
			ManualResetEvent.Set();
		}

		private static void _client_OnNotificationMessage(NotificationDataItem notification)
		{
			// this is where we would get personal alerts
			OutputConsole.WriteLine($"{DateTime.UtcNow}: Received notification {notification.Title}", ConsoleColor.Green);
		}

		private static void _client_OnNewsMessage(NewsDataItem news)
		{
			// we received a news article
			OutputConsole.WriteLine($"{DateTime.UtcNow}: Received news item \"{news.Title}\"", ConsoleColor.Green);
		}

		private static void _client_OnFavoriteMessage(FavoriteDataItem[] favorites)
		{
			// a message about our favorited markets
			OutputConsole.WriteLine($"{DateTime.UtcNow}: Received favorites count {favorites.Length}", ConsoleColor.Green);
		}

		[Obsolete]
		private static void _client_OnBlockMessage(string curr, BlockItem block)
		{
			// a new block has been discovered on a chain
			OutputConsole.WriteLine($"{DateTime.UtcNow}: Received new block for {curr} height {block.BlockId}", ConsoleColor.Green);
		}

		private static void _client_OnTradeMessage(string exchange, string curr1, string curr2, TradeItem trade)
		{
			// handle trade messages
			OutputConsole.WriteLine($"{DateTime.UtcNow}: Received new trade for {exchange} market {curr1}/{curr2} price {trade.Price}", ConsoleColor.Green);
		}

		private static void _client_OnMessage(MessageType messageType, string data)
		{
			// handle all message types
			OutputConsole.WriteLine($"{DateTime.UtcNow}: Received {messageType} message : {data}", ConsoleColor.Yellow);
		}

		private static void _client_OnOrderMessage(string exchange, string curr1, string curr2, OrderItem[] orders)
		{
			// handle order messages
			OutputConsole.WriteLine($"{DateTime.UtcNow}: Received orders for {exchange} market {curr1}/{curr2} count {orders.Length}", ConsoleColor.Green);
		}

		private static void Client_OnClientReady()
		{
			// we can now do whatever we want since we have an authorized connection, do not try to do anything before this state is reached

			// Some calls are simply informational, these have been wrapped up for you into a simple method and will return the results to you
			// lets get the list of all available channels
			//var channels = _client.GetChannels();
			//foreach (var channel in channels)
			//{
			//    OutputConsole.WriteLine(channel, ConsoleColor.Blue);
			//}

			// lets get all channels for BTRX
			//var markets = _client.GetChannels("BTRX");
			//foreach (var market in markets)
			//{
			//	OutputConsole.WriteLine(market, ConsoleColor.Blue);
			//}

			// lets get all exchanges
			//var exchanges = _client.GetExchanges();
			//foreach (var exchange in exchanges)
			//{
			//	OutputConsole.WriteLine(exchange.ExchCode, ConsoleColor.Blue);
			//}

			// Unlike the calls above Subscribtion calls provide streaming data and will received data as it is pushed from the server so we must
			// have some way of handling when data is received. As shown in the Main method we have several options for this.
			// lets subscribe to a channel
			//_client.SubscribeToChannel("TRADE-BITF--ETH--BTC");
			// this can also be achieved by calling
			//_client.SubscribeToTradeChannel("BITF", "ETH", "BTC");

			// this is how to subscribe to your private channel or any other channel or any call that is not predefined for you
			//_client.SubscribeToChannel("44444444-4444-4444-4444-444444444444");

			//_client.SubscribeToChannel("NEWMARKET");
			//_client.SubscribeToChannel("NEWS");

			// we can also subscribe to a channel and supply our own callback instead of using the builtin events
			_client.SubscribeToChannel("TRADE-BITF--ETH--BTC", MyCustomCallback);
			//_client.SubscribeToChannel("ORDER-BITF--ETH--BTC", MyCustomCallback);
		}

		private static void MyCustomCallback(MessageType messageType, string data)
		{
			// this is our custom callback we can do whatever we want with the data here
			OutputConsole.WriteLine($"{DateTime.UtcNow}: Received {messageType} message in private channel callback", ConsoleColor.Green);

			switch (messageType)
			{
				// if you want to deserialize into one of thhe premade classes you can do so quite easily by checking the MessageType and deserializing into the apropriate response class
				case MessageType.TradeData:
					try
					{
						var deserializedClass = TradeResponse.FromJson(_client.Options.Serializer, data);
						OutputConsole.WriteLine($"{DateTime.UtcNow}: We have deserialized a trade in our custom callback and the price in that trade is {deserializedClass.TradeData.Trade.Price}", ConsoleColor.Green);
					}
					catch (Exception ex)
					{
						OutputConsole.WriteLine(ex.Message);
					}

					break;
				case MessageType.OrderData:
					try
					{
						var deserializedClass = OrderResponse.FromJson(_client.Options.Serializer, data);
						OutputConsole.WriteLine($"{DateTime.UtcNow}: We have deserialized an orderbook in our custom callback and the first price in that data is {deserializedClass.OrderData.Orders[0].Price}", ConsoleColor.Green);
					}
					catch (Exception ex)
					{
						OutputConsole.WriteLine(ex.Message);
					}

					break;
				case MessageType.NewsData:
					break;
				case MessageType.BlockData:
					break;
				case MessageType.FavoriteData:
					break;
				case MessageType.NewMarket:
					break;
				case MessageType.NotificationData:
					break;
				case MessageType.Unknown:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
			}
		}
	}
}
