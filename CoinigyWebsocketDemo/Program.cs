using System;
using System.Threading;
using CoinigyWebsocketClient;
using CoinigyWebsocketClient.Models;
using CoinigyWebsocketClient.Types;

namespace CoinigyWebsocketDemo
{
    class Program
    {
        private static readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private static CoinigyWsClient _client;

        static void Main(string[] args)
        {
            // We can use the console built into the CoinigyWebsocketClient to ensure we don't lock up our program or cause delays. Even though
            // the normal console will work, be aware that writing to the console is a blocking operation and lots of writes can slow down
            // your program. This is why using the builtin OutpuConsole class is a better option.
            OutputConsole.WriteLine("Starting Coinigy Websocket Demo Client!", ConsoleColor.DarkGreen, ConsoleColor.White);

            // lets setup our credentials
            var creds = new ApiCredentials("ApiKey", "ApiSecret");

            // lets  create an instance of the client class and set it to debug mode, normally we would not use any debug mode
            _client = new CoinigyWsClient(creds, true);
            // if we want in depth debug information we can set the debug mode to on for the underlying connection
            // _client.ConnectionDebug = true;

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
            _manualResetEvent.WaitOne();
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
            _manualResetEvent.Set();
        }

        private static void _client_OnNotificationMessage(NotificationDataItem notification)
        {
            // this is where we would get personal alerts
            OutputConsole.WriteLine($"{DateTime.UtcNow}: Received notification {notification.Title}", ConsoleColor.Green);
        }

        private static void _client_OnNewsMessage(NewsDataItem news)
        {
            OutputConsole.WriteLine($"{DateTime.UtcNow}: Received news item \"{news.Title}\"", ConsoleColor.Green);
        }

        private static void _client_OnFavoriteMessage(FavoriteDataItem[] favorites)
        {
            OutputConsole.WriteLine($"{DateTime.UtcNow}: Received favorites count {favorites.Length}", ConsoleColor.Green);
        }

        private static void _client_OnBlockMessage(string curr, BlockItem block)
        {
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
            // we can now do whatever we want now since we have an authorized connection, do not try to do anythign before this state is reached

            // Some calls are simply informational, these have been wrapped up for you into a simple method and will return the results to you
            // lets get the list of all available channels
            var channels = _client.GetChannels();
            foreach (var channel in channels)
            {
                OutputConsole.WriteLine(channel, ConsoleColor.Blue);
            }

            // lets get all channels for BTRX
            //var markets = _client.GetChannels("BTRX");
            //foreach (var market in markets)
            //{
            //    OutputConsole.WriteLine(market, ConsoleColor.Blue);
            //}

            // lets get all exchanges
            //var exchanges = _client.GetExchanges();
            //foreach (var exchange in exchanges)
            //{
            //    OutputConsole.WriteLine(exchange.ExchCode, ConsoleColor.Blue);
            //}

            // Unlike the calls above Subscribtion calls provide streaming data and will received data as it is pushed from the server so we must
            // have some way of handling when data is received. As shown in the Main method we have several options for this.
            // lets subscribe to a channel
            //_client.SubscribeToChannel("TRADE-BITF--ETH--BTC");
            // this can also be achieved by calling
            _client.SubscribeToTradeChannel("BITF", "ETH", "BTC");

            // this is how to subscribe to your private channel or any other channel or any call that is not predefined for you
            //_client.SubscribeToChannel("44444444-4444-4444-4444-444444444444");
            //_client.SubscribeToChannel("NEWMARKET");

            // we can also subscribe to a channel and supply our own callback instead of using the builtin events
            _client.SubscribeToChannel("TRADE-BITF--ETH--BTC", MyCustomCallback);
        }

        private static void MyCustomCallback(MessageType messageType, string data)
        {
            // this is our custom callback we can do whatever we want with the data here
            OutputConsole.WriteLine($"{DateTime.UtcNow}: Received {messageType} message in private channel callback", ConsoleColor.Green);

            // if you want to deserialize into one of thhe premade classes you can do so quite easily by checking the MessageType and deserializing into the apropriate response class
            if (messageType == MessageType.TradeData)
            {
                var deserializedClass = TradeResponse.FromJson(data);
                OutputConsole.WriteLine($"{DateTime.UtcNow}: We have deserialized a trade in our custom callbeck and the price in that trade is {deserializedClass.TradeData.Trade.Price}", ConsoleColor.Green);
            }

        }
    }
}
