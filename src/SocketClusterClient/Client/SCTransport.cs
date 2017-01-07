﻿//
//  SCTransport.cs
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
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SocketClusterSharp.Errors;
using SocketClusterSharp.Helpers;
using SocketClusterSharp.Helpers.Web;
using SocketClusterSharp.Internal;

using WebSocket.Portable;
using WebSocket.Portable.Interfaces;

namespace SocketClusterSharp.Client
{
	public class SCTransport : ICanLog
	{
		const string _wss = "wss";

		const string _ws = "ws";

		Timer _pingTimeoutTimer;

		static Dictionary <int?,SCEventObject> _eventObjects = new Dictionary<int?,SCEventObject> ();


		WebSocketClient _socket;

		int _socketClosedCode = 1005;

		JToken _socketClosedData;

		#region Public Properties

		/// <summary>
		/// Gets or sets the auth.
		/// </summary>
		/// <value>The auth.</value>
		public SCAuthEngine Auth { get; set; }

		/// <summary>
		/// Gets or sets the call identifier generator.
		/// </summary>
		/// <value>The call identifier generator.</value>
		public object CallIdGenerator { get; set; }

		/// <summary>
		/// Gets or sets the options.
		/// </summary>
		/// <value>The options.</value>
		public SCClientOptions Options{ get; set; } = new SCClientOptions ();

		/// <summary>
		/// Gets or sets the ping timeout.
		/// </summary>
		/// <value>The ping timeout.</value>
		public double PingTimeout { get; set; }

		SCConnectionState _state = SCConnectionState.Closed;

		/// <summary>
		/// Gets or sets the connection state.
		/// </summary>
		/// <value>The state.</value>
		public SCConnectionState State { 
			get { return _state; } 
			set { 
				if (value != _state) {
					_state = value;
					ConnectionStateChanged (_state);
				}
			} 
		}

		#endregion

		#region Public Events

		/// <summary>
		/// Occurs when socket connection is opened.
		/// </summary>
		public event Action<SCConnectStatus> Opened = delegate {};

		/// <summary>
		/// Occurs when socket connection is open is aborted.
		/// </summary>
		public event Action<int, JToken> OpenAborted = delegate {};

		/// <summary>
		/// Occurs when the connection state changes.
		/// </summary>
		public event Action<SCConnectionState> ConnectionStateChanged = delegate {};

		/// <summary>
		/// Occurs when socket connection is closed.
		/// </summary>
		public event Action<int, JToken> Closed = delegate {};

		/// <summary>
		/// Occurs when there is socket connections error.
		/// </summary>
		public event Action<SCError> Error = delegate {};

		/// <summary>
		/// Occurs when the socket connections recieves a message.
		/// </summary>
		public event Action<IWebSocketMessage> MessageReceived = delegate {};
	
		/// <summary>
		/// Occurs when the socket connections recieves an event.
		/// </summary>
		public event Action<string, JToken, SCResponse> EventReceived = delegate {};

		/// <summary>
		/// Occurs when the socket connections recieves a raw event.
		/// </summary>
		public event Action<string> RawEventReceived = delegate {};

		#endregion

		#region ICanLog implementation

		/// <summary>
		/// Gets or sets the logging level.
		/// </summary>
		/// <value>The logging.</value>
		SCLogingLevels ICanLog.Logging {
			get {
				return Options.Logging;
			}
			set {
				Options.Logging = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SocketClusterSharp.SCTransport"/> class.
		/// </summary>
		/// <param name="authEngine">Auth engine.</param>
		/// <param name="options">Options.</param>
		public SCTransport (SCAuthEngine authEngine, SCClientOptions options)
		{
			Auth = authEngine;
			Options = options;
			PingTimeout = options.AckTimeout;
			CallIdGenerator = options.CallIdGenerator;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Removes all event handlers.
		/// </summary>
		public void Off ()
		{
			Opened = delegate {
			};
			OpenAborted = delegate {
			};
			ConnectionStateChanged = delegate {
			};
			Closed = delegate {
			};
			EventReceived = delegate {
			};
			RawEventReceived = delegate {
			};
			MessageReceived = delegate {
			};
			Error = delegate {
			};
		}

		/// <summary>
		/// Open a socket connection asynchronously. 
		/// </summary>
		/// <returns>The async.</returns>
		public async Task OpenAsync ()
		{
			State = SCConnectionState.Connecting;

			_socket = new WebSocketClient ();

			_socket.Opened += Socket_Opened;
			_socket.Closed += Socket_Closed;
			_socket.Error += Socket_Error;
			_socket.FrameReceived += Socket_FrameReceived;
			_socket.MessageReceived += Socket_MessageReceived;

			var url = Url ();
			await _socket.OpenAsync (url);

			ResetPingTimeout ();
		}

		/// <summary>
		/// Emits a socket event asynchronously. 
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="eventName">Event name.</param>
		/// <param name="data">Data.</param>
		public async Task<int?> EmitAsync (string eventName, JToken data)
		{
			return await EmitAsync (eventName, data, null, null);
		}

		/// <summary>
		/// Emits a socket event asynchronously. 
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="eventName">Event name.</param>
		/// <param name="data">Data.</param>
		/// <param name="callback">Callback.</param>
		public async Task<int?> EmitAsync (string eventName, JToken data, SCCallback callback)
		{
			return await EmitAsync (eventName, data, callback, null);
		}

		/// <summary>
		/// Emits a socket event asynchronously. 
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="eventName">Event name.</param>
		/// <param name="data">Data.</param>
		/// <param name="options">Options.</param>
		public async Task<int?> EmitAsync (string eventName, JToken data, SCEmitOptions options)
		{
			return await EmitAsync (eventName, data, null, options);
		}

		/// <summary>
		/// Emits a socket event asynchronously. 
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="eventName">Event Name</param>
		/// <param name="data">Data.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="options">Options.</param>
		public async Task<int?> EmitAsync (string eventName, JToken data, SCCallback  callback, SCEmitOptions options)
		{
			options = options ?? new SCEmitOptions ();

			var eventObject = new SCEventObject () {
				Event = eventName,
				Data = data,
				Callback = callback
			};

			if (callback != null && !options.NoTimeout) {
				Timer.Start (() => {
					HandleEventAckTimeout (eventObject);
					return false;
				}, TimeSpan.FromMilliseconds (Options.AckTimeout));
			}

			if (State == SCConnectionState.Open || options.Force) {
				return await EmitRawAsync (eventObject);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Emits a raw socket event asynchronously. 
		/// </summary>
		/// <returns>The raw async.</returns>
		/// <param name="eventObject">Event object.</param>
		public async Task<int?> EmitRawAsync (SCEventObject eventObject)
		{
			eventObject.Cid = Options.CallIdGenerator ();

			if (eventObject.Callback != null && !_eventObjects.ContainsKey (eventObject.Cid))
				_eventObjects.Add (eventObject.Cid, eventObject);

			await SendObjectAsync (eventObject);

			return eventObject.Cid;
		}

		/// <summary>
		/// Sends a string over the socket connection asynchronously. 
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="data">Data.</param>
		public async Task SendAsync (string data)
		{
			try {
				await _socket.SendAsync (data);
				this.Log (String.Format ("Sending: {0}", data), SCLogingLevels.Debug);
			} catch (Exception e) {
				Error (new SCError { Message = e.Message, Stack = e.StackTrace });
			}
		}

		/// <summary>
		/// Converts an object to a JSON string then sends it over the socket connection asynchronously. 
		/// </summary>
		/// <returns>The object async.</returns>
		/// <param name="obj">Object.</param>
		public async Task SendObjectAsync (object obj)
		{
			string data;

			try {
				data = JsonConvert.SerializeObject (obj);
				await SendAsync (data);
			} catch (Exception e) {
				Error (new SCError { Message = e.Message, Stack = e.StackTrace });
			}
		}

		/// <summary>
		/// Closes the socket connection asynchronously.
		/// </summary>
		/// <returns>The async.</returns>
		public async Task CloseAsync (int code = 1005, JToken data = null)
		{
			_socketClosedCode = code;
			_socketClosedData = data;
			await _socket.CloseAsync ();
		}

		/// <summary>
		/// Cancels a socket event's responce.
		/// </summary>
		/// <param name="cid">Cid.</param>
		public void CancelPendingResponse (int? cid)
		{
			_eventObjects.Remove (cid);
		}

		#endregion

		#region Private Methods

		void HandleEventAckTimeout (SCEventObject eventObject)
		{
			var errorMessage = String.Format ("Event response for '{0}' timed out", eventObject.Event);
			var error = new SCError { Message = errorMessage, Stack = new Exception ().StackTrace };
			error.Type = "timeout";
				
			if (eventObject.Cid != null) {
				if (_eventObjects.ContainsKey (eventObject.Cid)) {
					var callback = _eventObjects [eventObject.Cid].Callback;
					_eventObjects [eventObject.Cid].Callback = null;

					callback (error, JToken.FromObject (eventObject));
					Error (error);

					//Remove from eventList
					_eventObjects.Remove (eventObject.Cid);
				}
			} else {
				var callback = eventObject.Callback;
				eventObject.Callback = null;

				callback (error, JToken.FromObject (eventObject));
				Error (error);
			}
		}

		void HandShake (SCCallback  callback)
		{
			Auth.LoadToken (Options.AuthTokenName, (async (err, token) => {
				if (err != null) {
					callback (err, null);
				} else {
					// Don't wait for this.state to be 'open'.
					// The underlying WebSocket (this.socket) is already open.
					var options = new SCEmitOptions () {
						Force = true
					};

					JObject data = new JObject ();
					data.Add ("authToken", token);

					await EmitAsync ("#handshake", data, callback, options);
				}
			}));
		}

		void ResetPingTimeout ()
		{
			if (_pingTimeoutTimer != null) {
				_pingTimeoutTimer.Stop ();
				_pingTimeoutTimer = null;
			}

			_pingTimeoutTimer = Timer.Start (() => {
				this.Log ("Ping Timed Out", SCLogingLevels.Debug);
				Closed (4000, null);
				_socket.CloseAsync ();
				return false;
			}, TimeSpan.FromMilliseconds (PingTimeout));

			this.Log (String.Format ("Ping Timer Reset"), SCLogingLevels.Trace);
		}

		string Url ()
		{
			var uri = new Uri (Options.Hostname);
			if (uri.Scheme.Contains (_wss) || uri.Scheme.Contains ("https"))
				Options.Secure = true;
			
			if (!uri.IsDefaultPort)
				Options.Port = uri.Port;

			var query = Options.Query ?? new HttpValueCollection (uri.Query);
			var path = (Options.Path.StartsWith ("/")) ? Options.Path : "/" + Options.Path;
			var schema = Options.Secure ? _wss : _ws;
			var hostName = uri.Host.TrimEnd ('/');
			var port = "";

			if (Options.Port != null && ((schema == _wss && Options.Port != 443) || (_ws == schema && Options.Port != 80))) {
				port = String.Format (":{0}", Options.Port);
			} 

			if (Options.TimestampRequests) {
				query [Options.TimestampParam] = UnixTimeStamp.Now ().ToString ();
			}

			var queryString = query.ToString ();

			if (!String.IsNullOrWhiteSpace (queryString))
				queryString = "?" + queryString;


			return String.Format ("{0}://{1}{2}{3}{4}", schema, hostName, port, path, queryString).TrimEnd ('/');
		}


		#endregion

		#region Private Event Handlers

		async void Socket_MessageReceived (IWebSocketMessage socketMessage)
		{
			this.Log (String.Format ("Message Recieved: {0}", socketMessage), SCLogingLevels.Trace);
			MessageReceived (socketMessage);

			if (socketMessage.ToString () == "1") {
				this.Log (String.Format ("Recieved: PING"), SCLogingLevels.Debug);

				await _socket.SendAsync ("2");

				this.Log ("Sending: PONG", SCLogingLevels.Debug);
				ResetPingTimeout ();

			} else {
				try {
					var jsonObject = JToken.Parse (socketMessage.ToString ());

					if (!jsonObject ["event"].IsNullOrEmpty ()) {
						this.Log (String.Format ("Event Recieved: {0}", jsonObject), SCLogingLevels.Debug);
						EventReceived ((string)jsonObject ["event"], jsonObject ["data"], new SCResponse ((string)jsonObject ["cid"], _socket));

					} else if (!jsonObject ["rid"].IsNullOrEmpty ()) {
						var rid = (int?)jsonObject ["rid"];

						this.Log (String.Format ("Response Recieved: {0}", jsonObject), SCLogingLevels.Debug);

						SCEventObject eventObj;
						if (_eventObjects.TryGetValue (rid, out eventObj)) {
							//Remove tmeout
							if (eventObj.Callback != null) {
								SCError error = null;
								if (!jsonObject ["error"].IsNullOrEmpty ())
									error = jsonObject ["error"].ToObject<SCError> ();
								
								eventObj.Callback (error, jsonObject ["data"]);
							}

							//Remove from list.
							_eventObjects.Remove (rid);
						}
					} else {
						this.Log (String.Format ("Raw Recieved: {0}", socketMessage), SCLogingLevels.Debug);
						RawEventReceived (socketMessage.ToString ());
					}

				} catch (JsonReaderException ex) {
					this.Log (String.Format ("Raw Recieved: {0}", socketMessage), SCLogingLevels.Debug);
					RawEventReceived (socketMessage.ToString ());
				}
			}
		}

		void Socket_Error (Exception exception)
		{
			var scError = new SCError { Message = exception.Message, Stack = exception.StackTrace };
			scError.Stack = exception.StackTrace;

			this.Log (exception, SCLogingLevels.Error);
			Error (scError);
		}

		void Socket_FrameReceived (IWebSocketFrame socketFrame)
		{
			this.Log (String.Format ("Frame Reieved: {0}", socketFrame), SCLogingLevels.Trace);
		}

		void Socket_Closed ()
		{
			switch (State) {
			case SCConnectionState.Connecting:
				OpenAborted ((int)_socketClosedCode, _socketClosedData);
				this.Log (String.Format ("TRANSPORT CONNECTION ABORTED ({0}): {1}", _socketClosedCode, _socketClosedData), SCLogingLevels.Debug);
				break;

			default:
				Closed ((int)_socketClosedCode, _socketClosedData);
				this.Log (String.Format ("TRANSPORT DISCONNECTED ({0}): {1}", _socketClosedCode, _socketClosedData), SCLogingLevels.Debug);
				break;
			}

			State = SCConnectionState.Closed;
			_socketClosedCode = 1005;
			_socketClosedData = null;

			//Removes event handlers.
			Off ();
		}

		void Socket_Opened ()
		{
			this.Log ("Socket Connection Opened", SCLogingLevels.Debug);

			ResetPingTimeout ();

			HandShake (async (err, status) => {
				this.Log (String.Format ("HandShake Recieved: {0},  {1}", err, status), SCLogingLevels.Debug);
				if (err != null) {
					Error (err);
					_socketClosedCode = 4003;
					this.Log (String.Format ("HandShake Failed: {0}", err), SCLogingLevels.Error);
					await _socket.CloseAsync ();
				} else if (!status.IsNullOrEmpty ()) {
					var statusObj = status.ToObject<SCConnectStatus> ();

					if (statusObj.AuthError != null) {
						Error (statusObj.AuthError);
						_socketClosedCode = 4003;
						await _socket.CloseAsync ();
					} else {
						State = SCConnectionState.Open;

						Opened (statusObj);
						this.Log (String.Format ("TRANSPORT CONNECTED: {0}", status), SCLogingLevels.Debug);
						ResetPingTimeout ();
					}
					
				} else {
					Error (new SCError { Message = "Handshake Failed", Stack = new Exception ().StackTrace });
					this.Log (String.Format ("HandShake Failed: "), SCLogingLevels.Error);
					_socketClosedCode = 4003;
					await _socket.CloseAsync ();
				}
			});
		}

		#endregion
	}
}

