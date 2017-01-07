//
//  SCEventObject.cs
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
using Newtonsoft.Json.Linq;
using SocketClusterSharp.Helpers;

namespace SocketClusterSharp
{
    /// <summary>
    ///     SocketCluster event object.
    /// </summary>
    public class SCEventObject
    {
        /// <summary>
        ///     Gets or sets the event.
        /// </summary>
        /// <value>The event.</value>
        [JsonProperty(PropertyName = "event")]
        public string Event { get; set; }

        /// <summary>
        ///     Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [JsonProperty(PropertyName = "data")]
        public JToken Data { get; set; }

        /// <summary>
        ///     Gets or sets the cid.
        /// </summary>
        /// <value>The cid.</value>
        [JsonProperty(PropertyName = "cid")]
        public int Cid { get; set; }

        /// <summary>
        ///     Gets or sets the callback.
        /// </summary>
        /// <value>The callback.</value>
        [JsonIgnore]
        public SCCallback Callback { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has recieved a response.
        /// </summary>
        /// <value><c>true</c> if this instance has recieved response; otherwise, <c>false</c>.</value>
        [JsonIgnore]
        public bool HasRecievedResponse { get; set; } = false;

        /// <summary>
        ///     Gets or sets the timeout timer.
        /// </summary>
        /// <value>The timeout.</value>
        [JsonIgnore]
        public Timer Timeout { get; set; }
    }
}