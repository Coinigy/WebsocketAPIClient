//
//  SCSocketOptions.cs
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

using System;
using SocketClusterSharp.Helpers.Web;
using SocketClusterSharp.Interfaces;

namespace SocketClusterSharp.Client
{
    /// <summary>
    ///     SocketCluster client options.
    /// </summary>
    public class SCClientOptions : ISCOptions
    {
        /// <summary>
        ///     This is the timeout in milliseconds for getting a response to a SCSocket emit event (when a callback is provided).
        /// </summary>
        public double AckTimeout { get; set; } = 30000D;

        /// <summary>
        ///     A custom engine to use for storing and loading JWT auth tokens on the client side.
        /// </summary>
        public SCAuthEngine AuthEngine { get; set; }

        /// <summary>
        ///     The auto process subscriptions.
        /// </summary>
        public bool AutoProcessSubscriptions { get; set; } = true;

        /// <summary>
        ///     Whether or not to automatically reconnect the socket when it loses the connection.
        /// </summary>
        public bool AutoReconnect { get; set; } = true;

        /// <summary>
        ///     Gets or sets the auto reconnect options.
        /// </summary>
        /// <value>The auto reconnect options.</value>
        public SCAutoReconnectOptions AutoReconnectOptions { get; set; }

        /// <summary>
        ///     The name of the JWT auth token (provided to the authEngine - By default this is the localStorage variable name);
        ///     defaults to 'socketCluster.authToken'.
        /// </summary>
        public string AuthTokenName { get; set; } = "socketCluster.authToken";

        /// <summary>
        ///     Set this to false during debugging - Otherwise client connection will fail when using self-signed certificates.
        /// </summary>
        public string BinaryType { get; set; } = "arraybuffer";

        /// <summary>
        ///     Gets or sets the call identifier generator.
        /// </summary>
        /// <value>The call identifier generator.</value>
        public Func<int> CallIdGenerator { get; set; }

        /// <summary>
        ///     The hostname.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        ///     The path.
        /// </summary>
        public string Path { get; set; } = "/socketcluster/";

        /// <summary>
        ///     The port number.
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        ///     Gets or sets the query string.
        /// </summary>
        /// <value>The query.</value>
        public HttpValueCollection Query { get; set; }

        /// <summary>
        ///     Set this to false during debugging - Otherwise client connection will fail when using self-signed certificates.
        /// </summary>
        /// <value><c>true</c> if reject unauthorized; otherwise, <c>false</c>.</value>
        public bool RejectUnauthorized { get; set; } = true;

        /// <summary>
        ///     Use secure connection.
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        ///     Whether or not to add a timestamp to the WebSocket.
        /// </summary>
        public bool TimestampRequests { get; set; } = false;

        /// <summary>
        ///     The query parameter name to use to hold the timestamp.
        /// </summary>
        public string TimestampParam { get; set; } = "t";

        /// <summary>
        ///     Gets or sets the current logging level <see cref="SocketClusterSharp.Internal.SCLogingLevels" />.
        /// </summary>
        /// <value>The logging.</value>
        public SCLogingLevels Logging { get; set; } = SCLogingLevels.Error;
    }
}