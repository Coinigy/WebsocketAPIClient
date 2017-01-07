//
//  DateTimeExtensions.cs
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

namespace SocketClusterSharp.Helpers
{
	/// <summary>
	/// A collection of methods for working with UNIX Timestamps.
	/// </summary>
	public static class UnixTimeStamp
	{

		/// <summary>
		/// Gets the UNIX epoch.
		/// </summary>
		/// <value>The epoch.</value>
		public static DateTime Epoch{ get { return new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); } }

		/// <summary>
		/// Converts a given DateTime into a Unix timestamp
		/// </summary>
		/// <param name="value">Any DateTime</param>
		/// <returns>The given DateTime in Unix timestamp format</returns>
		public static long ToUnixTimeStamp (this DateTime value)
		{
			return (long)Math.Truncate ((value.ToUniversalTime ().Subtract (Epoch)).TotalSeconds);
		}

		/// <summary>
		/// Gets a Unix timestamp representing the current moment
		/// </summary>
		/// <returns>Now expressed as a Unix timestamp</returns>
		public static long Now ()
		{
			return DateTime.UtcNow.ToUnixTimeStamp ();
		}

		/// <summary>
		/// Constructs a DateTime from unix timestamp.
		/// </summary>
		/// <returns>Returns the parsed <see cref="System.DateTime"/></returns>
		/// <param name="unixTimeStamp">Unix time stamp.</param>
		public static DateTime Parse (double unixTimeStamp)
		{
			return Epoch.AddSeconds (unixTimeStamp).ToLocalTime ();
		}

		/// <summary>
		/// Constructs a DateTime from unix timestamp.
		/// </summary>
		/// <returns>Returns the parsed <see cref="System.DateTime"/></returns>
		/// <param name="unixTimeStamp">Unix time stamp.</param>
		public static DateTime Parse (int unixTimeStamp)
		{
			return Epoch.AddSeconds (Convert.ToDouble (unixTimeStamp)).ToLocalTime ();
		}

		/// <summary>
		/// Constructs a DateTime from unix timestamp.
		/// </summary>
		/// <returns>Returns the parsed <see cref="System.DateTime"/></returns>
		/// <param name="unixTimeStamp">Unix time stamp.</param>
		public static DateTime Parse (long unixTimeStamp)
		{
			return Epoch.AddSeconds (Convert.ToDouble (unixTimeStamp)).ToLocalTime ();
		}

		/// <summary>
		/// Constructs a DateTime from unix timestamp.
		/// </summary>
		/// <returns>Returns the parsed <see cref="System.DateTime"/></returns>
		/// <param name="unixTimeStamp">Unix time stamp.</param>
		public static DateTime Parse (string unixTimeStamp)
		{
			return Epoch.AddSeconds (Convert.ToDouble (unixTimeStamp)).ToLocalTime ();
		}
	}
}

