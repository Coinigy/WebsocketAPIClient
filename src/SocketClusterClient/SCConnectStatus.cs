//
//  SCConnectEventArgs.cs
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

using Newtonsoft.Json;
using SocketClusterSharp.Errors;

namespace SocketClusterSharp
{
    /// <summary>
    ///     SocketCluster connect status.
    /// </summary>
    public class SCConnectStatus
    {
        /// <summary>
        ///     Gets or sets the socket's id.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not the current socket is authenticated with
        ///     the server (has a valid auth token).
        /// </summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "isAuthenticated", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAuthenticated { get; set; }

        /// <summary>
        ///     Gets or sets the ping timeout.
        /// </summary>
        /// <value>The ping timeout.</value>
        [JsonProperty(PropertyName = "pingTimeout", NullValueHandling = NullValueHandling.Ignore)]
        public double PingTimeout { get; set; }

        /// <summary>
        ///     Gets or sets the auth error.
        /// </summary>
        /// <value>The auth error.</value>
        [JsonProperty(PropertyName = "authError", NullValueHandling = NullValueHandling.Ignore)]
        public SCAuthError AuthError { get; set; }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents the current
        ///     <see cref="SocketClusterSharp.SCConnectStatus" />.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents the current <see cref="SocketClusterSharp.SCConnectStatus" />.</returns>
        public override string ToString()
        {
            return string.Format("[SCConnectStatus: Id={0}, IsAuthenticated={1}, PingTimeout={2}, AuthError={3}]", Id,
                IsAuthenticated, PingTimeout, AuthError);
        }
    }
}