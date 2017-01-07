//
//  HttpValue.cs
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

namespace SocketClusterSharp.Helpers.Web
{
    /// <summary>
    ///     Http query key value pair.
    /// </summary>
    public sealed class HttpValue
    {
        /// <summary>
        ///     Gets or Sets the key in the key/value pair.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        ///     Gets or Sets the value in the key/value pair.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SocketClusterSharp.Helpers.Web.HttpValue" /> class.
        /// </summary>
        public HttpValue()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SocketClusterSharp.Helpers.Web.HttpValue" /> class.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public HttpValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}