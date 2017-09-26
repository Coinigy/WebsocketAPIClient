using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using CoinigyWebsocketClient.Interfaces;
using CoinigyWebsocketClient.Models;
using CoinigyWebsocketClient.Types;
using Newtonsoft.Json.Linq;
using PureSocketCluster;
using PureWebSockets;

namespace CoinigyWebsocketClient
{
    public class CoinigyWsClient
    {
        private readonly bool _debugMode;
        private readonly PureSocketClusterSocket _scClient;
        private readonly IApiCredentials _apiCredentials;
        private bool _ready;
        private readonly List<KeyValuePair<string, Message>> SubscribeCallbacs = new List<KeyValuePair<string, Message>>();

        public bool ConnectionDebug
        {
            get => _scClient.DebugMode;
            set => _scClient.DebugMode = value;
        }

        public event ClientIsReady OnClientReady;
        public event StateChanged OnStateChanged;
        public event Closed OnClosed;
        public event Error OnError;
        public event Fatality OnFatality;
        public event SendFailed OnSendFailed;
        public event OrderMessage OnOrderMessage;
        public event TradeMessage OnTradeMessage;
        public event BlockMessage OnBlockMessage;
        public event FavoriteMessage OnFavoriteMessage;
        public event NewMarketMessage OnNewMarketMessage;
        public event NotificationMessage OnNotificationMessage;
        public event NewsMessage OnNewsMessage;
        public event Message OnMessage;

        public CoinigyWsClient(IApiCredentials credentials, bool debug = false, string url = "wss://sc-02.coinigy.com/socketcluster/")
        {
            _debugMode = debug;
            _apiCredentials = credentials;
            _scClient = new PureSocketClusterSocket(url);

            _scClient.OnOpened += _scClient_OnOpened;
            _scClient.OnMessage += _scClient_OnMessage;
            _scClient.OnStateChanged += _scClient_OnStateChanged;
            _scClient.OnClosed += _scClient_OnClosed;
            _scClient.OnError += _scClient_OnError;
            _scClient.OnFatality += _scClient_OnFatality;
            _scClient.OnSendFailed += _scClient_OnSendFailed;
        }

        private void _scClient_OnSendFailed(string data, Exception ex) => OnSendFailed?.Invoke(data, ex);

        private void _scClient_OnFatality(string reason) => OnFatality?.Invoke(reason);

        private void _scClient_OnError(Exception ex) => OnError?.Invoke(ex);

        private void _scClient_OnClosed(WebSocketCloseStatus reason) => OnClosed?.Invoke(reason);

        private void _scClient_OnStateChanged(WebSocketState newState, WebSocketState prevState) => OnStateChanged?.Invoke(newState, prevState);

        private void _scClient_OnMessage(string message)
        {
            Debug.WriteLine(message);
            // check if this is a publish message
            var PublishPattern = @"^{""event""*.:*.""#publish""";
            var m = Regex.Match(message, PublishPattern);
            if (!m.Success) return;

            // get the channel

            var jobj = JObject.Parse(message);

            var channel = jobj["data"]["channel"].ToString();



            string reqtype;
            if (channel.Contains('-') && !Guid.TryParse(channel, out var guid))
            {
                reqtype = channel.Substring(0, channel.IndexOf('-'));
            }
            else
            {
                reqtype = channel;
                if (Guid.TryParse(channel, out var cguid))
                    if (cguid.ToString().ToLower() == channel.ToLower())
                        // ok we have a private channel so lets find out what data we have
                        reqtype = jobj["data"]["data"]["MessageType"].ToString();
            }

            var hascb = (from c in SubscribeCallbacs where c.Key == channel select c).Any();
            if (hascb)
            {
                var cbs = (from c in SubscribeCallbacs where c.Key == channel select c.Value);
                foreach (var cb in cbs)
                {
                    cb.Invoke(GetMessageType(reqtype), message);
                }
            }

            switch (reqtype.ToUpper())
            {
                case "ORDER":
                    var ominfo = ParseMarketInfo(channel);
                    var orders = OrderResponse.FromJson(message);
                    OnOrderMessage?.Invoke(ominfo.Exchange, ominfo.Curr1, ominfo.Curr2, orders.OrderData.Orders);
                    OnMessage?.Invoke(MessageType.OrderData, message);
                    break;
                case "TRADE":
                    var tminfo = ParseMarketInfo(channel);
                    var trade = TradeResponse.FromJson(message);
                    OnTradeMessage?.Invoke(tminfo.Exchange, tminfo.Curr1, tminfo.Curr2, trade.TradeData.Trade);
                    OnMessage?.Invoke(MessageType.TradeData, message);
                    break;
                case "BLOCK":
                    OnBlockMessage?.Invoke(ParseBlockInfo(channel), new BlockItem());
                    OnMessage?.Invoke(MessageType.BlockData, message);
                    break;
                case "FAVORITE":
                    OnFavoriteMessage?.Invoke(FavoriteResponse.FromJson(message).Data.FavoritesDataData.Favorites);
                    OnMessage?.Invoke(MessageType.FavoriteData, message);
                    break;
                case "NOTIFICATION":
                    OnNotificationMessage?.Invoke(NotificationResponse.FromJson(message).NotificationData.NotificationDataData.NotificationDataItem);
                    OnMessage?.Invoke(MessageType.NotificationData, message);
                    break;
                case "NEWS":
                    OnNewsMessage?.Invoke(NewsResponse.FromJson(message).NewsData.NewsDataItem);
                    OnMessage?.Invoke(MessageType.NewsData, message);
                    break;
                case "NEWMARKET":
                    OnNewMarketMessage?.Invoke(NewMarketResponse.FromJson(message).NewMarketData.NewMarketDataData);
                    OnMessage?.Invoke(MessageType.NewMarket, message);
                    break;
                default:
                    OnMessage?.Invoke(MessageType.Unknown, message);
                    break;
            }
        }

        private static MessageType GetMessageType(string reqType)
        {
            switch (reqType.ToUpper())
            {
                case "ORDER":
                    return MessageType.OrderData;
                case "TRADE":
                    return MessageType.TradeData;
                case "BLOCK":
                    return MessageType.BlockData;
                case "FAVORITE":
                    return MessageType.FavoriteData;
                case "NOTIFICATION":
                    return MessageType.NotificationData;
                case "NEWS":
                    return MessageType.NewsData;
                case "NEWMARKET":
                    return MessageType.NewMarket;
                default:
                    return MessageType.Unknown;
            }
        }

        private static MarketInfo ParseMarketInfo(string data)
        {
            var tmp = data.Replace("--", "-");
            var arr = tmp.Split('-');
            return new MarketInfo() { Exchange = arr[1], Curr1 = arr[2], Curr2 = arr[3] };
        }

        private static string ParseBlockInfo(string data) => data.Split('-')[1];

        public bool Connect()
        {
            if (_scClient == null || _scClient.SocketState == WebSocketState.Connecting || _scClient.SocketState == WebSocketState.Open)
                return false;

            return _scClient.Connect();
        }

        public IEnumerable<string> GetChannels()
        {
            if (_scClient == null || _scClient.SocketState != WebSocketState.Open || !_ready) return null;

            var results = new List<string>();
            var ackReceived = false;
            _scClient.Emit("channels", "", ack: (string name, object error, object data) =>
            {
                var res = (JArray)data;
                foreach (var r in res)
                {
                    foreach (var c in r)
                    {
                        try
                        {
                            results.Add(c.ElementAt(0).Children().ElementAt(0).ToString());
                        }
                        catch
                        {
                            // ignore stupid items
                        }
                    }
                }
                ackReceived = true;
            });
            var st = DateTime.UtcNow;
            do
            {
                // just wait a few for results if needed
                if (ackReceived)
                    break;
            } while (st.AddSeconds(20000) > DateTime.UtcNow);

            // remove depreciated
            if (results.Contains("TICKER"))
                results.Remove("TICKER");
            if (results.Contains("CHATMSG"))
                results.Remove("CHATMSG");
            if (results.Contains("NOTIFICATION"))
                results.Remove("NOTIFICATION");

            return results;
        }

        public IEnumerable<string> GetChannels(string exchangeCode)
        {
            if (_scClient == null || _scClient.SocketState != WebSocketState.Open || !_ready) return null;

            var results = new List<string>();
            var ackReceived = false;
            _scClient.Emit("channels", exchangeCode, ack: (string name, object error, object data) =>
            {
                var res = (JArray)data;
                foreach (var r in res)
                {
                    foreach (var c in r)
                    {
                        try
                        {
                            results.Add(c.ElementAt(0).Children().ElementAt(0).ToString());
                        }
                        catch
                        {
                            // ignore stupid items
                        }
                    }
                }
                ackReceived = true;
            });
            var st = DateTime.UtcNow;
            do
            {
                // just wait a few for results if needed
                if (ackReceived)
                    break;
            } while (st.AddSeconds(20000) > DateTime.UtcNow);

            return results;
        }

        public IEnumerable<ExchangeItem> GetExchanges()
        {
            if (_scClient == null || _scClient.SocketState != WebSocketState.Open || !_ready) return null;

            var results = new List<ExchangeItem>();
            var ackReceived = false;
            _scClient.Emit("exchanges", "", ack: (string name, object error, object data) =>
            {
                var res = (JArray)data;
                foreach (var r in res)
                {
                    foreach (var c in r)
                    {
                        try
                        {
                            results.Add(ExchangeItem.FromJson(c.ToString()));
                        }
                        catch
                        {
                            // ignore stupid items
                        }
                    }
                }
                ackReceived = true;
            });
            var st = DateTime.UtcNow;
            do
            {
                // just wait a few for results if needed
                if (ackReceived)
                    break;
            } while (st.AddSeconds(20000) > DateTime.UtcNow);

            return results;
        }

        public bool SubscribeToChannel(string channelName) => _scClient.Subscribe(channelName);

        public bool SubscribeToTradeChannel(string exchange, string curr1, string curr2) => _scClient.Subscribe($"TRADE-{exchange}--{curr1}--{curr2}");

        public bool SubscribeToOrderChannel(string exchange, string curr1, string curr2) => _scClient.Subscribe($"ORDER-{exchange}--{curr1}--{curr2}");

        public bool SubscribeToNewsChannel() => _scClient.Subscribe("NEWS");

        public bool SubscribeToBlockChannel(string curr) => _scClient.Subscribe($"BLOCK-{curr}");

        public bool SubscribeToNewMarketChannel() => _scClient.Subscribe("NEWMARKET");

        public bool SubscribeToChannel(string channelName, Message callback)
        {
            SubscribeCallbacs.Add(new KeyValuePair<string, Message>(channelName, callback));

            return _scClient.Subscribe(channelName);
        }

        private void _scClient_OnOpened()
        {
            _scClient.Emit("auth", _apiCredentials, ack: (string name, object error, object data) =>
            {
                WriteDebugLine("Client is ready, Auth completed.", ConsoleColor.DarkGreen);
                _ready = true;
                OnClientReady?.Invoke();
            });
        }

        private void WriteDebugLine(string msg, ConsoleColor? foreColor = null, ConsoleColor? backColor = null)
        {
            if (!_debugMode) return;

            OutputConsole.WriteLine(msg, foreColor, backColor);
        }
    }
}
