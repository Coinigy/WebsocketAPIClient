//
//  HttpValueCollection.cs
//
//  Author:
//       Muhammad Rehan Saeed 
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
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;

namespace SocketClusterSharp.Helpers.Web
{
    /// <summary>
    ///     A collection of <see cref="SocketClusterSharp.Helpers.Web.HttpValue" />.
    /// </summary>
    public class HttpValueCollection : Collection<HttpValue>
    {
        #region Parameters

        /// <summary>
        ///     Gets or sets the <see cref="SocketClusterSharp.Helpers.Web.HttpValue" /> with the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public string this[string key]
        {
            get { return this.First(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase)).Value; }
            set { this.First(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase)).Value = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Fills from string.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <param name="urlencoded">If set to <c>true</c> urlencoded.</param>
        private void FillFromString(string query, bool urlencoded)
        {
            var num = query != null ? query.Length : 0;
            for (var i = 0; i < num; i++)
            {
                var startIndex = i;
                var num4 = -1;
                while (i < num)
                {
                    var ch = query[i];
                    if (ch == '=')
                    {
                        if (num4 < 0) num4 = i;
                    }
                    else if (ch == '&')
                    {
                        break;
                    }
                    i++;
                }
                string str = null;
                string str2 = null;
                if (num4 >= 0)
                {
                    str = query.Substring(startIndex, num4 - startIndex);
                    str2 = query.Substring(num4 + 1, i - num4 - 1);
                }
                else
                {
                    str2 = query.Substring(startIndex, i - startIndex);
                }

                if (urlencoded) Add(Uri.UnescapeDataString(str), Uri.UnescapeDataString(str2));
                else Add(str, str2);

                if (i == num - 1 && query[i] == '&') Add(null, string.Empty);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" /> class.
        /// </summary>
        public HttpValueCollection()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" /> class.
        /// </summary>
        /// <param name="query">Query.</param>
        public HttpValueCollection(string query) : this(query, true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" /> class.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <param name="urlencoded">If set to <c>true</c> urlencoded.</param>
        public HttpValueCollection(string query, bool urlencoded)
        {
            if (!string.IsNullOrEmpty(query)) FillFromString(query, urlencoded);
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Add a new <see cref="SocketClusterSharp.Helpers.Web.HttpValue" /> with the specified key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Add(string key, string value)
        {
            Add(new HttpValue(key, value));
        }

        /// <summary>
        ///     Determines whether the <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" /> contains an element with
        ///     the specified key.
        /// </summary>
        /// <returns>
        ///     <c>true</c>, if the <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" /> contains an element with the
        ///     specified
        ///     key; otherwise <c>false</c> otherwise.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" />.</param>
        public bool ContainsKey(string key)
        {
            return this.Any(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///     Gets the values.
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="key">Key.</param>
        public string[] GetValues(string key)
        {
            return
                this.Where(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Value)
                    .ToArray();
        }

        /// <Docs>The <see cref="SocketClusterSharp.Helpers.Web.HttpValue" /> to remove from the current collection.</Docs>
        /// <para>
        ///     Removes all occurrence of <see cref="SocketClusterSharp.Helpers.Web.HttpValue" /> from the current collection
        ///     with the given key.
        /// </para>
        /// <summary>
        ///     Remove the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public void Remove(string key)
        {
            var items = this.Where(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var item in items) Remove(item);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents the current
        ///     <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents the current
        ///     <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" />.
        /// </returns>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents the current
        ///     <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents the current
        ///     <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" />.
        /// </returns>
        /// <param name="urlencoded">If set to <c>true</c> the returns string is url encoded.</param>
        public virtual string ToString(bool urlencoded)
        {
            return ToString(urlencoded, null);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents the current
        ///     <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents the current
        ///     <see cref="SocketClusterSharp.Helpers.Web.HttpValueCollection" />.
        /// </returns>
        /// <param name="urlencoded">If set to <c>true</c> the returns string is url encoded.</param>
        /// <param name="excludeKeys">Keys to exclude.</param>
        public virtual string ToString(bool urlencoded, IDictionary excludeKeys)
        {
            if (Count == 0) return string.Empty;

            var stringBuilder = new StringBuilder();

            foreach (var item in this)
            {
                var key = item.Key;

                if (excludeKeys == null || !excludeKeys.Contains(key))
                {
                    var value = item.Value;

                    if (urlencoded) key = WebUtility.UrlDecode(key);

                    if (stringBuilder.Length > 0) stringBuilder.Append('&');

                    stringBuilder.Append(key != null ? key + "=" : string.Empty);

                    if (value != null && value.Length > 0)
                    {
                        if (urlencoded) value = Uri.EscapeDataString(value);

                        stringBuilder.Append(value);
                    }
                }
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}