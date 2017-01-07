//
//  SocketCluster.cs
//
//  Author:
//       Todd Henderson <todd@todd-henderson.me>
//
//  Copyright (c) 2015 Talk Fusion, Inc
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
using System.Reflection;

namespace SocketClusterSharp.Client
{
	/// <summary>
	/// Socket cluster.
	/// </summary>
	public static class SocketCluster
	{
		#region Public Properties

		static string _version;

		/// <summary>
		/// Gets the current version number of this assembly.
		/// </summary>
		/// <value>The version.</value>
		public static string Version {
			get {
				if (_version == null) {
					var assembly = typeof(SocketCluster).GetTypeInfo ().Assembly;
					// In some PCL profiles the above line is: var assembly = typeof(MyType).Assembly;
					var assemblyName = new AssemblyName (assembly.FullName);
					_version = assemblyName.Version.Major + "." + assemblyName.Version.Minor;
				}
				return _version;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Creates and returns a new socket connection to the specified host (or origin if not specified). 
		/// The options argument is optional - If omitted, the socket will try to connect to the origin server 
		/// on the current port. For cross domain requests, a typical options object might look like this 
		/// (example over HTTPS/WSS):
		/// </summary>
		/// <param name="options">Options.</param>
		public static SCSocket Connection (SCClientOptions options = null)
		{
			var Socket = new SCSocket (options);
			return Socket;
		}

		#endregion
	}
}

