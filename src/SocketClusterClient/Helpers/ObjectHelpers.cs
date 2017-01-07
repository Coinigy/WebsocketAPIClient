//
//  ObjectExtensions.cs
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
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SocketClusterSharp.Helpers
{
	static public class ObjectHelpers
	{
		/// <summary>
		/// Merge the specified target and source.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="source">Source.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Merge<T> (this T target, T source)
		{
			var typeOfT = typeof(T);
			if (typeOfT.GetTypeInfo ().IsValueType) {
				target = source;
			} else if (source != null) {
				var properties = typeOfT
					.GetRuntimeProperties ()
					.Select ((PropertyInfo x) => new KeyValuePair<PropertyInfo, object> (x, x.GetValue (source, null)))
					.Where ((KeyValuePair<PropertyInfo, object> x) => x.Value != null).ToList ();

				foreach (var property in properties) {
					property.Key.SetValue (target, property.Value);
				}
			}

			//return the modified copy of Target
			return target;
		}

		/// <summary>
		/// Returns a JSON string that represents the current object.
		/// </summary>
		/// <returns>JSON String.</returns>
		/// <param name="source">Source.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static string ToJSON<T> (this T source)
		{
			return JsonConvert.SerializeObject (source);
		}

		/// <summary>
		/// Returns a <see cref="Newtonsoft.Json.Linq.JToken"/> that represents the current object.
		/// </summary>
		/// <returns><see cref="Newtonsoft.Json.Linq.JToken"/></returns>
		/// <param name="source">Source.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static JToken ToJToken<T> (this T source)
		{
			return JToken.FromObject (source);
		}
	}
}

