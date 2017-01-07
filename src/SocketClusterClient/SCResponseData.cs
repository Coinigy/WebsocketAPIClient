//
//  SCResponceData.cs
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

namespace SocketClusterSharp
{
    /// <summary>
    ///     SocketCluster response data object.
    /// </summary>
    public class SCResponseData
    {
        /// <summary>
        ///     Gets or sets the rid.
        /// </summary>
        /// <value>The rid.</value>
        [JsonProperty(PropertyName = "rid")]
        public string Rid { get; set; }

        /// <summary>
        ///     Gets or sets the response error.
        /// </summary>
        /// <value>The error.</value>
        [JsonProperty(PropertyName = "err", NullValueHandling = NullValueHandling.Ignore)]
        public object Err { get; set; }

        /// <summary>
        ///     Gets or sets the response data.
        /// </summary>
        /// <value>The data.</value>
        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
    }
}