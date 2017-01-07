//
//  Timer.cs
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
using System.Threading.Tasks;
using System.Threading;

namespace SocketClusterSharp.Helpers
{
	/// <summary>
	/// Generates a recurring events at a set interval while the callback returns true.
	/// </summary>
	public class Timer
	{
		Guid _id = Guid.NewGuid ();

		readonly CancellationTokenSource _cancellationTokenSourse = new CancellationTokenSource ();

		TimeSpan _interval = TimeSpan.FromMilliseconds (25);

		TimeSpan _maxInterval = TimeSpan.FromTicks (Int32.MaxValue);

		Func<bool> _csallback;


		/// <summary>
		/// Starts a recurring timer using the device clock capabilities.
		/// </summary>
		Timer Start ()
		{
			bool success = false;

			_cancellationTokenSourse.CancelAfter (_maxInterval);

			Task.Run (() => {
				while (!success) {
					if (_cancellationTokenSourse.IsCancellationRequested)
						break;

					Task.Delay (_interval).Wait ();
					success = !_csallback ();
				}
			}, _cancellationTokenSourse.Token);
				

			return this;
		}

		/// <summary>
		/// Starts a recurring timer using the device clock capabilities.
		/// </summary>
		/// <remarks>While the callback returns true, the timer will keep recurring.</remarks>
		public static Timer Start (Func<bool> callback)
		{
			var timer = new Timer ();
			timer._csallback = callback;
			return timer.Start ();
		}

		/// <summary>
		/// Starts a recurring timer using the device clock capabilities.
		/// </summary>
		/// <remarks>While the callback returns true, the timer will keep recurring.</remarks>
		/// <param name="callback">The action to run when the timer elapses.</param>
		/// <param name="interval">The interval between invocations of the callback.</param>
		public static Timer Start (Func<bool> callback, TimeSpan interval)
		{
			var timer = new Timer ();
			timer._interval = interval;
			timer._csallback = callback;
			return timer.Start ();
		}

		/// <summary>
		/// Starts a recurring timer using the device clock capabilities.
		/// </summary>
		/// <remarks>While the callback returns true, the timer will keep recurring.</remarks>
		/// <param name="callback">The action to run when the timer elapses.</param>
		/// <param name="interval">The interval in miliseconds between invocations of the callback.</param>
		public static Timer Start (Func<bool> callback, double interval)
		{
			var timer = new Timer ();
			timer._interval = TimeSpan.FromMilliseconds (interval);
			timer._csallback = callback;
			return timer.Start ();
		}

		/// <summary>
		/// Starts a recurring timer using the device clock capabilities.
		/// </summary>
		/// <remarks>While the callback returns true, the timer will keep recurring or the max interval is reached.</remarks>
		/// <param name="callback">The action to run when the timer elapses.</param>
		/// <param name="interval">The interval between invocations of the callback.</param>
		/// <param name = "maxInterval">The maximum interval before timer stops.</param>
		public static Timer Start (Func<bool> callback, TimeSpan interval, TimeSpan maxInterval)
		{
			var timer = new Timer ();
			timer._interval = interval;
			timer._csallback = callback;
			timer._maxInterval = maxInterval;
			return timer.Start ();
		}

		/// <summary>
		/// Starts a recurring timer using the device clock capabilities.
		/// </summary>
		/// <remarks>While the callback returns true, the timer will keep recurring or the max interval is reached.</remarks>
		/// <param name="callback">The action to run when the timer elapses.</param>
		/// <param name="interval">The interval in miliseconds between invocations of the callback.</param>
		/// <param name = "maxInterval">The maximum interval in miliseconds before timer stops.</param>
		public static Timer Start (Func<bool> callback, double interval, double maxInterval)
		{
			var timer = new Timer ();
			timer._interval = TimeSpan.FromMilliseconds (interval);
			timer._csallback = callback;
			timer._maxInterval = TimeSpan.FromMilliseconds (maxInterval);
			return timer.Start ();
		}

		/// <summary>
		/// Manually stops the timer.
		/// </summary>
		public void Stop ()
		{
			_cancellationTokenSourse.Cancel ();
			_csallback = () => {
				return true;
			};
		}
	}
}

