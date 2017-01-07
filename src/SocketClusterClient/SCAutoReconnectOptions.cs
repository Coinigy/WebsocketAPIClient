//
//  SCAutoReconnectOptions.cs
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

namespace SocketClusterSharp.Client
{
    /// <summary>
    ///     SC auto reconnect options.
    /// </summary>
    public class SCAutoReconnectOptions
    {
        /// <summary>
        ///     Gets or sets the initial delay.
        /// </summary>
        /// <value>The initial delay.</value>
        public double InitialDelay { get; set; } = 10000;

        /// <summary>
        ///     Gets or sets the multiplier.
        /// </summary>
        /// <value>The multiplier.</value>
        public float Multiplier { get; set; } = 1.5F;

        /// <summary>
        ///     Gets or sets the max delay.
        /// </summary>
        /// <value>The max delay.</value>
        public double MaxDelay { get; set; } = 60000;

        /// <summary>
        ///     Gets or sets the randomness.
        /// </summary>
        /// <value>The randomness.</value>
        public double? Randomness { get; set; } = 10000;
    }
}