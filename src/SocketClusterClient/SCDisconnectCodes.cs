//
//  SCStatusCodes.cs
//
//  Author:
//       Todd Henderson <todd@todd-henderson.me>
//
//  Copyright (c) 2015-2016 Todd Henderson
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//   
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//   
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN

using System.Collections.Generic;

namespace SocketClusterSharp
{
    public static class SCDisconnectCodes
    {
        #region Public static Properties

        /// <summary>
        ///     The error statuses.
        /// </summary>
        public static Dictionary<int, string> Error = new Dictionary<int, string>
        {
            {1001, "Socket was disconnected"},
            {1002, "A WebSocket protocol error was encountered"},
            {1003, "Server terminated socket because it received invalid data"},
            {1005, "Socket closed without status code"},
            {1006, "Socket hung up"},
            {1007, "Message format was incorrect"},
            {1008, "Encountered a policy violation"},
            {1009, "Message was too big to process"},
            {1010, "Client ended the connection because the server did not comply with extension requirements"},
            {1011, "Server encountered an unexpected fatal condition"},
            {4000, "Server ping timed out"},
            {4001, "Client pong timed out"},
            {4002, "Server failed to sign auth token"},
            {4003, "Failed to complete handshake"},
            {4004, "Client failed to save auth token"}
        };

        /// <summary>
        ///     The ignore statuses.
        /// </summary>
        public static Dictionary<int, string> Ignore = new Dictionary<int, string>
        {
            {1000, "Socket closed normally"},
            {1001, "Socket hung up"}
        };

        #endregion
    }
}