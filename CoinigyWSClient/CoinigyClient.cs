using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SocketClusterSharp;
using SocketClusterSharp.Client;
using SocketClusterSharp.Errors;

namespace CoinigyWS
{
    public class CoinigyClient
    {
        private readonly SCSocket _scClient;

        public SCConnectionState State = SCConnectionState.Closed;
        private static ConsoleColor DefaultForegroundColor;
        private static APICreds Creds;
        private static bool Ready;
        private static bool Debug { get; set; }

        public CoinigyClient(string apiKey, string apiSecret, bool debug = false, string url = "wss://sc-02.coinigy.com/socketcluster/", int port = 443, bool secure = true)
        {
            Debug = debug;
            DefaultForegroundColor = Console.ForegroundColor;
            Creds = new APICreds {apiKey = apiKey, apiSecret = apiSecret};
            var options = new SCClientOptions
            {
                AutoReconnect = true,
                Hostname = url,
                Logging = Debug ? SCLogingLevels.Debug : SCLogingLevels.None,
                Port = port,
                Secure = secure
            };
            _scClient = new SCSocket(options);
            State = _scClient.State;
            _scClient.ConnectionStateChanged += _scClient_ConnectionStateChanged;
            _scClient.Authenticated += _scClient_Authenticated;
            _scClient.MessageRecieved += _scClient_MessageRecieved;
            _scClient.Subscribed += _scClient_Subscribed;
            _scClient.Connected += _scClient_Connected;
            _scClient.Error += _scClient_Error;
            _scClient.Raw += _scClient_Raw;
        }

        private void _scClient_Authenticated(string obj)
        {
            if (Debug)
                WriteLine("Authenticated: " + obj, ConsoleColor.Green);
            Ready = true;
        }

        private void _scClient_Connected(SCConnectStatus obj)
        {
            if (Debug)
                WriteLine("Connected: " + obj, ConsoleColor.Green);
        }

        private void _scClient_ConnectionStateChanged(SCConnectionState obj)
        {
            if (Debug)
                WriteLine("State Changed: " + obj, ConsoleColor.Green);
            State = obj;
        }

        private void _scClient_Error(SCError obj)
        {
            if (Debug)
                WriteLine("Error: " + obj, ConsoleColor.Red);
        }

        private void _scClient_MessageRecieved(JToken obj)
        {
            if (Debug)
                WriteLine("Message: " + obj, ConsoleColor.Gray);
        }

        private void _scClient_Raw(string obj)
        {
            if (Debug)
                WriteLine("Raw: " + obj, ConsoleColor.Gray);
        }

        private void _scClient_Subscribed(JToken obj)
        {
            if (Debug)
                WriteLine("Subscribed: " + obj, ConsoleColor.Green);

            //_scClient.Watch(obj.ToString(), Handler);
        }

        public async Task Connect()
        {
            await _scClient.ConnectAsync();
            await _scClient.EmitAsync("auth", Creds);
            do
            {
                await Task.Delay(500);
            } while (!Ready);
        }

        public async Task GetChannels()
        {
            await _scClient.EmitAsync("channels");
        }

        private void Handler(JToken jToken, SCResponse scResponse)
        {
            if (Debug)
                WriteLine("Channel Message: " + jToken, ConsoleColor.Blue);
        }

        private void Handler(JToken jToken)
        {
            if (jToken["event"] != null)
                if (jToken["event"].ToString() == "#publish")
                {
                }
            if (Debug)
                WriteLine("Channel Message: " + jToken, ConsoleColor.Yellow);
        }

        public event Message OnMessage;

        public async Task Subscibe(string channel, Action<JToken> dataHandler)
        {
            var res = await _scClient.SubscribeAsync(channel);
            res.DataRecieved += dataHandler;
            ;
        }

        private static void WriteLine(string message, ConsoleColor color)
        {
            if (!Debug) return;
            try
            {
                Console.Write($"{DateTime.Now} | ");
                Console.ForegroundColor = color;
                Console.Write($"{message}\r\n");
                Console.ResetColor();
            }
            catch
            {
                // Ignore
            }
        }
    }
}