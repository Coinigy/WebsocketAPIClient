//
//  AuthEngine.cs
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

namespace SocketClusterSharp.Client
{
	/// <summary>
	/// Socket Cluster autherorization engine.
	/// </summary>
	public class SCAuthEngine
	{
		Dictionary<string, string> _localStorage { get; set; } = new Dictionary<string, string>();

		/// <summary>
		/// Saves a token.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="token">Token.</param>
		/// <param name="options">Options.</param>
		/// <param name="callback">Callback.</param>
		public void SaveToken (string name, string token, object options = null, SCCallback callback = null)
		{
			_localStorage.Add (name, token);
			if (callback != null)
				callback (null);
		}

		/// <summary>
		/// Removes a token.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="callback">Callback.</param>
		public void RemoveToken (string name, SCCallback callback = null)
		{
			_localStorage.Remove (name);
			if (callback != null)
				callback (null);
		}

		/// <summary>
		/// Loads a token.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="callback">Callback.</param>
		public void LoadToken (string name, Action<Errors.SCError, string> callback)
		{
			string value;
			if (_localStorage.TryGetValue (name, out value)) {
				callback (null, value);
			} else {
				callback (null, null);
			}
		}

	}
}

