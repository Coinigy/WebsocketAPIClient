//
//  HttpUtility.cs
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

namespace SocketClusterSharp.Helpers.Web
{
    internal sealed class HttpUtility
    {
        /// <summary>
        ///     Parses a query string into a <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection"> using UTF8 encoding.
        /// </summary>
        /// <returns>A <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection"> of query parameters and values. </returns>
        /// <param name="query">The query string to parse.</param>
        public static HttpValueCollection ParseQueryString(string query)
        {
            if (query == null) throw new ArgumentNullException("query");

            if (query.Length > 0 && query[0] == '?') query = query.Substring(1);

            return new HttpValueCollection(query, true);
        }
    }
}