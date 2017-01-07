using System;
using System.Threading.Tasks;
using CoinigyWS;
using Newtonsoft.Json.Linq;

namespace Demo
{
    internal class Program
    {
        private static void HandleTradeData(JToken data)
        {
            WriteLine(data.ToString(), ConsoleColor.Cyan);
        }

        private static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            //TODO: replace apiKey and apiSecret with your own from Coinigy.com
            var CClient = new CoinigyClient("apiKey", "apiSecret");
            await CClient.Connect();

            await CClient.Subscibe("TRADE-OK--BTC--CNY", HandleTradeData);
            Console.ReadLine();
        }

        private static void WriteLine(string message, ConsoleColor color)
        {
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