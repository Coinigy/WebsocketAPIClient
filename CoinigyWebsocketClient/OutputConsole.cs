using System;
using System.Collections.Concurrent;
using System.Threading;

namespace CoinigyWebsocketClient
{
    public static class OutputConsole
    {
        private static readonly BlockingCollection<Tuple<string, ConsoleColor?, ConsoleColor?>> MQueue = new BlockingCollection<Tuple<string, ConsoleColor?, ConsoleColor?>>();

        static OutputConsole()
        {
            try
            {
                var thread = new Thread(
                        () =>
                        {
                            while (true)
                            {
                                var item = MQueue.Take();
                                try
                                {
                                    if (item.Item2 != null)
                                        Console.ForegroundColor = (ConsoleColor)item.Item2;
                                    if (item.Item3 != null)
                                        Console.BackgroundColor = (ConsoleColor)item.Item3;
                                }
                                catch
                                {
                                    // ignore
                                }
                                Console.WriteLine(item.Item1);
                                try
                                {
                                    Console.ResetColor();
                                }
                                catch
                                {
                                    // ignore
                                }
                            }
	                        // ReSharper disable once FunctionNeverReturns
                        })
                { IsBackground = true };
                thread.Start();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static void WriteLine(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            MQueue.Add(new Tuple<string, ConsoleColor?, ConsoleColor?>(value, foregroundColor, backgroundColor));
        }
    }
}
