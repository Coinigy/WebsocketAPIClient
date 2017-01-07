//
//  Error.cs
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
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SocketClusterSharp.Errors
{
	/// <summary>
	/// SocketCluster error.
	/// </summary>
	public class SCError
	{
		/// <summary>
		/// Gets or sets the error code.
		/// </summary>
		/// <value>The error code.</value>
		[JsonProperty (PropertyName = "code", NullValueHandling = NullValueHandling.Ignore)]
		public int Code { get; set; }

		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		/// <value>The message.</value>
		[JsonProperty (PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the error name.
		/// </summary>
		/// <value>The name.</value>
		[JsonProperty (PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the error type.
		/// </summary>
		/// <value>The type.</value>
		[JsonProperty (PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the error stack.
		/// </summary>
		/// <value>The stack.</value>
		[JsonProperty (PropertyName = "stack", NullValueHandling = NullValueHandling.Ignore)]
		public string Stack { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SocketClusterSharp.Errors.SCError"/> class.
		/// </summary>
		public SCError ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SocketClusterSharp.Errors.SCError"/> struct.
		/// </summary>
		/// <param name="message">Error Message.</param>
		public SCError (string message)
		{
			Message = message;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SocketClusterSharp.Errors.SCError"/> struct.
		/// </summary>
		/// <param name="type">Error Type.</param>
		/// <param name="message">Error Message.</param>
		public SCError (string type, string message)
		{
			Type = type;
			Message = message;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="SocketClusterSharp.Errors.SCError"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="SocketClusterSharp.Errors.SCError"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[SCError: Code={0}, Message=\"{1}\", Name=\"{2}\", Type=\"{3}\", Stack=\"{4}\"]", Code, Message, Name, Type, Stack);
		}
	
	}
}

