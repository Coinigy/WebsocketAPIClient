//
//  StringExtensions.cs
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
using System.Collections.Generic;

namespace SocketClusterSharp.Helpers
{
	/// <summary>
	/// A collection of helper methods and extensions for <see cref="System.Collections.Generic.ICollection">
	/// </summary>
	public static class CollectionHelper
	{
		/// <summary>
		/// Adds a value to an ICollection and converts a it to a html query string
		/// </summary>
		/// <returns>The query string.</returns>
		/// <param name="query">Query.</param>
		/// <param name="name">parameter name.</param>
		/// <param name="value">parameter value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static String ToQueryString <T> (this ICollection<T> query, string name, string value)
		{

			// decodes urlencoded pairs from uri.Query to HttpValueCollection
			var httpValueCollection = Helpers.Web.HttpUtility.ParseQueryString (String.Empty);

			httpValueCollection.Add (name, value);

			return httpValueCollection.ToString ();
		}
	}
}

