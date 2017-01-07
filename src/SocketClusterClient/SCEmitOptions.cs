//
//  SCEmitOptions.cs
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

namespace SocketClusterSharp
{
    /// <summary>
    ///     SocketCLuster options parameters object for emitting messages to servers.
    /// </summary>
    public class SCEmitOptions
    {
        /// <summary>
        ///     Gets or sets a value indicating whether to force and the send.
        /// </summary>
        /// <value><c>true</c> if force; otherwise, <c>false</c>.</value>
        public bool Force { get; set; } = false;

        /// <summary>
        ///     Gets or sets a value indicating whether not to timeout send.
        /// </summary>
        /// <value><c>true</c> if no timeout; otherwise, <c>false</c>.</value>
        public bool NoTimeout { get; set; } = false;
    }
}