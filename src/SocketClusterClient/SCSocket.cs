//
//  SCSocket.cs
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
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SocketClusterSharp.Errors;
using SocketClusterSharp.Helpers;
using SocketClusterSharp.Internal;

namespace SocketClusterSharp.Client
{
    /// <summary>
    ///     SocketCluster socket.
    /// </summary>
    public class SCSocket : ICanLog
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SCSocket" /> class.
        /// </summary>
        /// <param name="options">Options.</param>
        public SCSocket(SCClientOptions options)
        {
            Options = Options.Merge(options);

            _ackTimeout = Options.AckTimeout;

            // pingTimeout will be ackTimeout at the start, but it will
            // be updated with values provided by the "connect" event
            _pingTimeout = _ackTimeout;

            //Verify Durations
            if (_ackTimeout > _maxTimeout)
                throw new Exception("The AckTimeout value provided exceeded the maximum amount allowed.");

            if (_pingTimeout > _maxTimeout)
                throw new Exception("The PingTimeout value provided exceeded the maximum amount allowed.");


            Options.CallIdGenerator = () => _cid++;

            if (Options.AutoReconnect)
            {
                if (Options.AutoReconnectOptions == null) Options.AutoReconnectOptions = new SCAutoReconnectOptions();

                var reconnectOptions = Options.AutoReconnectOptions.Merge(new SCAutoReconnectOptions
                {
                    InitialDelay = 10000,
                    Randomness = 10000,
                    Multiplier = 1.5F,
                    MaxDelay = 60000
                });
            }

            //TODO: Come Back to this. Not in Api Documentation.
//			if (Options.subscriptionRetryOptions == null) {
//				Options.subscriptionRetryOptions = {};
//			}

            if (Options.AuthEngine != null) _auth = Options.AuthEngine;
            else _auth = new SCAuthEngine();

            Options.Path = Options.Path != null ? Options.Path.TrimEnd('/') + "/" : "";

            //TODO: Come Back to this. Not in Api Documentation.
//			Options.query = opts.query || {};
//			if (typeof Options.query == 'string') {
//				Options.query = querystring.parse(Options.query);
//			}

            Options.Port = Options.Port ?? (Options.Secure ? 443 : 80);
        }

        #endregion

        #region ICanLog implementation

        /// <summary>
        ///     Gets or sets the logging level.
        /// </summary>
        /// <value>The logging.</value>
        SCLogingLevels ICanLog.Logging
        {
            get { return Options.Logging; }
            set { Options.Logging = value; }
        }

        #endregion

        #region Private Fields

        private readonly double _ackTimeout;

        private readonly SCAuthEngine _auth;

        private readonly Dictionary<string, SCChannel> _channels = new Dictionary<string, SCChannel>();

        private int _cid = 1;

        private int _connectAttempts;

        private readonly List<SCEventObject> _emitBuffer = new List<SCEventObject>();

        private readonly Dictionary<string, List<Action<JToken, SCResponse>>> _eventHandlers =
            new Dictionary<string, List<Action<JToken, SCResponse>>>();

        private readonly double _maxTimeout = Math.Pow(2, 31) - 1;

        private bool _pendingConnectCallback;

        private double _pingTimeout;

        private Timer _reconnectTimeoutTimer;

        private SCTransport _transport;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; private set; }

        private SCConnectionState _state = SCConnectionState.Closed;

        /// <summary>
        ///     Gets or sets the connection state.
        /// </summary>
        /// <value>The state.</value>
        public SCConnectionState State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    ConnectionStateChanged(_state);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the Socket Connection's options.
        /// </summary>
        /// <value>The options.</value>
        public SCClientOptions Options { get; set; } = new SCClientOptions
        {
            AutoReconnect = true,
            AutoProcessSubscriptions = true,
            AckTimeout = 10000,
            Path = "/socketcluster/",
            TimestampRequests = false,
            TimestampParam = "t",
            AuthTokenName = "socketCluster.authToken",
            BinaryType = "arraybuffer"
        };

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs whenever the client is successfully authenticated by the server -
        ///     The event argument passed along with this event is the encrypted auth token as a string.
        /// </summary>
        public event Action<string> Authenticated = delegate { };

        /// <summary>
        ///     Occurs when the authToken is removed.
        /// </summary>
        public event Action AuthTokenRemoved = delegate { };

        /// <summary>
        ///     Occurs whenever the socket connects to the server (includes reconnections).
        /// </summary>
        public event Action<SCConnectStatus> Connected = delegate { };

        /// <summary>
        ///     Occurs when the connection state changes.
        /// </summary>
        public event Action<SCConnectionState> ConnectionStateChanged = delegate { };

        /// <summary>
        ///     Occurs when connection is aborted.
        /// </summary>
        public event Action<int, JToken> ConnectionAborted = delegate { };

        /// <summary>
        ///     Occurs when this socket becomes disconnected from the server.
        /// </summary>
        public event Action<int, JToken> Disconnected = delegate { };

        /// <summary>
        ///     Occurs when an error occurs on this socket.
        /// </summary>
        public event Action<SCError> Error = delegate { };

        /// <summary>
        ///     Occurs when this socket is kicked out of a particular channel by the backend. (message, channel)
        /// </summary>
        public event Action<string, string> KickedOut = delegate { };

        /// <summary>
        ///     Occurs whenever the client is successfully authenticated by the server -
        ///     The event argument passed along with this event is the encrypted auth token as a string.
        /// </summary>
        public event Action<JToken> MessageRecieved = delegate { };

        /// <summary>
        ///     Occurs whenever the server socket on the other side calls socket.send(...).
        /// </summary>
        public event Action<string> Raw = delegate { };

        /// <summary>
        ///     Occurs when the subscription succeeds.
        /// </summary>
        public event Action<JToken> Subscribed = delegate { };

        /// <summary>
        ///     Occurs when the subscription fails.
        /// </summary>
        public event SCCallback SubscribeFailed = delegate { };

        /// <summary>
        ///     Occurs when the socket becomes unsubscribed from a channel.
        /// </summary>
        public event Action<JToken> Unsubscribed = delegate { };

        #endregion

        #region Public Methods

        /// <summary>
        ///     Connect the client socket to its origin server.
        /// </summary>
        public async Task ConnectAsync()
        {
            if (State == SCConnectionState.Closed)
            {
                State = SCConnectionState.Connecting;

                _transport?.Off();

                _transport = new SCTransport(_auth, Options);

                _transport.Opened += Transport_Opened;
                _transport.Error += Transport_Error;
                _transport.Closed += (code, data) => Transport_Closed(code, data);
                _transport.OpenAborted += (code, data) => Transport_Closed(code, data, true);
                _transport.EventReceived += Transport_EventReceived;

                await _transport.OpenAsync();
            }
        }

        /// <summary>
        ///     Gets the state of the socket.
        /// </summary>
        /// <returns>The state.</returns>
        public SCConnectionState GetState()
        {
            return State;
        }

        /// <summary>
        ///     Disconnect this socket from the server.
        /// </summary>
        public async Task DisconnectAsync(int code = 1000, JToken data = null)
        {
            var closed = false;

            if (State == SCConnectionState.Open)
            {
                var packet = new {code}.ToJToken() as JObject;
                packet.Add("data", data);


                await _transport.EmitAsync("#disconnect", packet, async (error, response) =>
                {
                    if (error != null)
                        Error(error);

                    await _transport.CloseAsync();
                    closed = true;
                });
            }

            Timer.Start(() =>
            {
                if (State != SCConnectionState.Closed && !closed) _transport.CloseAsync();
                return false;
            }, 500D);
        }

        /// <summary>
        ///     Emit the specified event on the corresponding server-side socket. Note that you cannot emit
        ///     any of the reserved SCSocket events.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="socketEvent">Socket event.</param>
        public async Task EmitAsync(string socketEvent)
        {
            await EmitAsync(socketEvent, null, null);
        }

        /// <summary>
        ///     Emit the specified event on the corresponding server-side socket. Note that you cannot emit
        ///     any of the reserved SCSocket events.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="socketEvent">Socket event.</param>
        /// <param name="data">Data.</param>
        public async Task EmitAsync(string socketEvent, JToken data)
        {
            await EmitAsync(socketEvent, data, null);
        }

        /// <summary>
        ///     Emit the specified event on the corresponding server-side socket. Note that you cannot emit
        ///     any of the reserved SCSocket events.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="socketEvent">Socket event.</param>
        /// <param name="data">Data.</param>
        public async Task EmitAsync(string socketEvent, object data)
        {
            await EmitAsync(socketEvent, data.ToJToken(), null);
        }

        /// <summary>
        ///     Emit the specified event on the corresponding server-side socket. Note that you cannot emit
        ///     any of the reserved SCSocket events.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="socketEvent">Socket event.</param>
        /// <param name="data">Data.</param>
        /// <param name="callback">Callback.</param>
        public async Task EmitAsync(string socketEvent, object data, SCCallback callback)
        {
            await EmitAsync(socketEvent, data.ToJToken(), callback);
        }

        /// <summary>
        ///     Emit the specified event on the corresponding server-side socket. Note that you cannot emit
        ///     any of the reserved SCSocket events.
        /// </summary>
        /// <param name="socketEvent">Socket event.</param>
        /// <param name="data">Data.</param>
        /// <param name="callback">Callback.</param>
        public async Task EmitAsync(string socketEvent, JToken data, SCCallback callback)
        {
//			switch (socketEvent) {     
//			case "connect":
//				Connected (data.ToObject<SCConnectStatus> ());
//				break;
//
//			case "connectAbort":
//				await ConnectionAborted (data);
//				break;
//
//			case "disconnect":
//				Disconnected (data);
//				break;
//
//			case "message":
//				MessageRecieved (data);
//				break;
//
//			case "error":
//				Error (new SCError (data));
//				break;
//
//			case "raw":
//				Raw (data.ToString ());
//				break;
//
//			case "fail":
//				SubscribeFailed (data);
//				break;
//
//			case "kickOut":
//				KickedOut ((string)data ["message"], (string)data ["channel"]);
//				break;
//
//			case "subscribe":
//				Subscribed (data);
//				break;
//
//			case "unsubscribe":
//				Unsubscribed (data);
//				break;
//
//			case "authenticate":
//				Authenticated (data.ToString ());
//				break;
//			
//			case "removeAuthToken":
//				AuthTokenRemoved ();
//				break;
//
//			default:
            if (State == SCConnectionState.Closed)
                await ConnectAsync();

            var eventObject = new SCEventObject
            {
                Event = socketEvent,
                Data = data,
                Callback = callback
            };

            if (callback == null)
                eventObject.Timeout = Timer.Start(() =>
                {
                    HandleEventAckTimeout(eventObject);
                    return false;
                }, _ackTimeout);

            _emitBuffer.Add(eventObject);

            if (State == SCConnectionState.Open)
                await FlushEmitBufferAsync();
//				break;
//			}
        }

        /// <summary>
        ///     Add a handler for a particular event (those emitted from a corresponding socket on the server).
        ///     The handler is a function in the form: handler(data, res) - The res argument is a function which
        ///     can be used to send a response to the server socket which emitted the event (assuming that the
        ///     server is expecting a response - I.e. A callback was provided to the emit method). The res
        ///     function is in the form: res(err, message) - To send back an error, you can do either:
        ///     res('This is an error') or res(1234, 'This is the error message for error code 1234').
        ///     To send back a normal non-error response: res(null, 'This is a normal response message').
        ///     Note that both arguments provided to res can be of any JSON-compatible type.
        /// </summary>
        /// <param name="name">Name of the envent handler</param>
        /// <param name="handler">Handler.</param>
        public void On(string name, Action<JToken, SCResponse> handler)
        {
            if (!string.IsNullOrEmpty(name))
                if (_eventHandlers.ContainsKey(name))
                    _eventHandlers[name].Add(handler);
                else
                    _eventHandlers[name] = new List<Action<JToken, SCResponse>> {handler};
        }

        /// <summary>
        ///     Unbind a previously attached event handler.
        /// </summary>
        /// <param name="name"></param>
        public void Off(string name)
        {
            if (!string.IsNullOrEmpty(name)) _eventHandlers.Remove(name);
        }

        /// <summary>
        ///     Send some raw data to the server. This will trigger a 'raw' event on the server which will carry the provided data.
        /// </summary>
        /// <param name="data">Data.</param>
        public async Task SendAsync(string data)
        {
            await _transport.SendAsync(data);
        }

        /// <summary>
        ///     Perform client-initiated authentication - This is useful if you already have a valid encrypted auth token string
        ///     and would like to use it to authenticate directly with the server (without having to ask the user to login
        ///     details).
        ///     Typically, you should perform server-initiated authentication using the socket.setAuthToken() method from the
        ///     server side. This method is useful if, for example, you received the token from a different browser tab via
        ///     localStorage and you want to immediately authenticate the current socket without having to reconnect the socket.
        ///     It may also be useful if you're getting the token from a third-party JWT-based system and you're using the same
        ///     authKey (see boot options for SocketCluster) - In which case they should be compatible.
        /// </summary>
        /// <param name="encryptedAuthToken">Encrypted token string.</param>
        /// <param name="callback">Callback.</param>
        public async Task AuthenticateAsync(string encryptedAuthToken, SCCallback callback = null)
        {
            await EmitAsync("#authenticate", encryptedAuthToken, (error, authStatus) =>
            {
                if (error != null)
                {
                    if (callback != null)
                        callback(error, authStatus);
                }
                else
                {
                    _auth.SaveToken(Options.AuthTokenName, encryptedAuthToken, null, (err, data) =>
                    {
                        if (callback != null)
                            callback(err, authStatus);

                        if (err != null) Transport_Error(err);
                        else if (authStatus["isAuthenticated"] != null && (bool) authStatus["isAuthenticated"])
                            Authenticated(encryptedAuthToken);
                    });
                }
            });
        }

        /// <summary>
        ///     Publishs the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="data">Data.</param>
        /// <param name="callback">Callback.</param>
        public async Task PublishAsync(JToken data, SCCallback callback = null)
        {
            await EmitAsync("#publish", data, callback);
        }

        /// <summary>
        ///     Publish data to the specified channelName. The data argument can be any JSON-compatible object/array or primitive.
        ///     The callback lets you check that the publish action reached the backend successfully. Callback function is in the
        ///     form callback(err) - On success, the err argument will be undefined.
        /// </summary>
        /// <param name="channelName">Channel name.</param>
        /// <param name="data">Data.</param>
        /// <param name="callback">Callback.</param>
        public async Task PublishAsync(string channelName, JToken data, SCCallback callback = null)
        {
            var pubData = new
            {
                channel = channelName,
                data
            };

            await EmitAsync("#publish", pubData.ToJToken(), callback);
        }

        /// <summary>
        ///     Subscribe to a particular channel. This function returns an SCChannel object which lets you watch for incoming data
        ///     on that channel.
        /// </summary>
        /// <param name="channelName">Channel name.</param>
        public async Task<SCChannel> SubscribeAsync(string channelName)
        {
            SCChannel channel;
            if (!_channels.TryGetValue(channelName, out channel))
            {
                channel = new SCChannel(channelName, this);
                _channels.Add(channelName, channel);
            }

            if (channel.State != SCChannelsState.Subscribed)
            {
                channel.State = SCChannelsState.Pending;
                await TryScubscribeAsync(channel);
            }

            return channel;
        }

        /// <summary>
        ///     Unsubscribe from the specified channel. This makes any associated SCChannel object inactive. You can reactivate the
        ///     SCChannel object by calling SubscribeAsync(channelName) again at a later time.
        /// </summary>
        /// <param name="channelName">Channel name.</param>
        public async Task UnsubscribeAsync(string channelName)
        {
            SCChannel channel;
            if (_channels.TryGetValue(channelName, out channel))
                if (channel.State != SCChannelsState.UnSubscribed)
                {
                    TriggerChannelUnsubscribe(channel);
                    await TryUnsubscribeAsync(channel);
                }
        }

        /// <summary>
        ///     Returns an SCChannel instance. This is different from subscribe() in that it will not try to subscribe to that
        ///     channel.
        ///     The returned channel will be inactive initially. You can call channel.subscribe() later to activate that channel
        ///     when
        ///     required.
        /// </summary>
        /// <param name="channelName">Channel name.</param>
        public SCChannel Channel(string channelName)
        {
            SCChannel channel;
            if (_channels.TryGetValue(channelName, out channel)) return channel;
            return new SCChannel(channelName, this);
        }

        /// <summary>
        ///     Lets you watch a channel directly from the SCSocket object. The handler accepts a single data argument which holds
        ///     the
        ///     data which was published to the channel.
        /// </summary>
        /// <param name="channelName">Channel name.</param>
        /// <param name="handler">Handler.</param>
        public void Watch(string channelName, Action<JToken> handler)
        {
            SCChannel channel;
            if (_channels.TryGetValue(channelName, out channel)) channel.DataRecieved += handler;
        }

        /// <summary>
        ///     Stop handling data which is published on the specified channel. This is different from unsubscribe in that the
        ///     socket
        ///     will still receieve channel data but the specified handler will no longer capture it. If the handler argument is
        ///     not
        ///     provided, all handlers for that channelName will be removed.
        /// </summary>
        /// <param name="channelName">Channel name.</param>
        /// <param name="handler">Handler.</param>
        public void Unwatch(string channelName, Action<JToken> handler = null)
        {
            SCChannel channel;
            if (_channels.TryGetValue(channelName, out channel)) channel.UnWatch(handler);
        }

        /// <summary>
        ///     Returns a list of event handlers which are watching for data on the specified channel.
        /// </summary>
        /// <param name="channelName">Channel name.</param>
        public Delegate[] Watchers(string channelName)
        {
            SCChannel channel;
            if (_channels.TryGetValue(channelName, out channel)) return channel.Watchers();
            return null;
        }

        /// <summary>
        ///     This will cause SCSocket to unsubscribe that channel and remove any watchers from it. Any SCChannel object which is
        ///     associated with that channelName will be disabled permanently (ready to be cleaned up by garbage collector).
        /// </summary>
        /// <param name="channelName">Channel name.</param>
        public async Task DestroyChannelAsync(string channelName)
        {
            SCChannel channel;
            if (_channels.TryGetValue(channelName, out channel))
            {
                channel.UnWatch();
                await channel.UnScubscribeAsync();
                _channels.Remove(channelName);
            }
        }

        /// <summary>
        ///     Returns an array of active channel subscriptions which this socket is bound to. If includePending is true, pending
        ///     subscriptions will also be included in the list.
        /// </summary>
        /// <param name="includePending">If set to <c>true</c> include pending.</param>
        public SCChannel[] Subscriptions(bool includePending = false)
        {
            if (includePending)
                return _channels
                    .Where(
                        channel =>
                            channel.Value.State == SCChannelsState.Pending ||
                            channel.Value.State == SCChannelsState.Subscribed)
                    .Select(Channel => Channel.Value).ToArray();
            return _channels
                .Where(channel => channel.Value.State == SCChannelsState.Subscribed)
                .Select(Channel => Channel.Value).ToArray();
        }

        /// <summary>
        ///     Check if socket is subscribed to channelName. If includePending is true, pending subscriptions will also be
        ///     included in the list.
        /// </summary>
        /// <returns><c>true</c> if this instance is subscribed the specified channelName includePending; otherwise, <c>false</c>.</returns>
        /// <param name="channelName">Channel name.</param>
        /// <param name="includePending">If includePending is <c>true</c>, pending subscriptions will also be included in the list.</param>
        public bool IsSubscribed(string channelName, bool includePending = false)
        {
            if (includePending)
                return _channels.ContainsKey(channelName) &&
                       _channels[channelName].State != SCChannelsState.UnSubscribed;
            return _channels.ContainsKey(channelName) && _channels[channelName].State == SCChannelsState.Subscribed;
        }

        /// <summary>
        ///     Deletes the auth token from this client if it has one.
        /// </summary>
        public void RemoveAuthToken(SCCallback callback = null)
        {
            _auth.RemoveToken(Options.AuthTokenName, (err, data) =>
            {
                if (err != null)
                {
                    // Non-fatal error - Do not close the connection
                    Error(err);
                    if (callback != null)
                        callback(err);
                }
                else
                {
                    AuthTokenRemoved();
                    if (callback != null)
                        callback(null);
                }
            });
        }

        #endregion

        #region Private Methods

        private void CancelPendingSubscribeCallback(SCChannel channel)
        {
            if (channel.PendingSubscriptionCid != null)
            {
                _transport.CancelPendingResponse(channel.PendingSubscriptionCid);
                channel.PendingSubscriptionCid = null;
            }
        }

        private async Task FlushEmitBufferAsync()
        {
            var buffer = new List<SCEventObject>(_emitBuffer);
            _emitBuffer.Clear();

            foreach (var item in buffer) await _transport.EmitRawAsync(item);
        }

        private void HandleEventAckTimeout(SCEventObject eventObject)
        {
            var error = new SCError("timeout", "Event response for '" + eventObject.Event + "' timed out");
            error.Stack = new Exception().StackTrace;

            if (eventObject.Callback == null) return;

            var callback = eventObject.Callback;
            eventObject.Callback = null;

            _emitBuffer.Remove(eventObject);

            callback(error, eventObject.ToJToken());

            Transport_Error(error);
        }


        private async Task ProcessPendingSubscriptionsAsync()
        {
            _pendingConnectCallback = false;

            foreach (var channel in _channels)
                if (channel.Value.State == SCChannelsState.Pending) await TryScubscribeAsync(channel.Value);
        }

        private void SuspendSubscriptions()
        {
            foreach (var channel in _channels)
            {
                SCChannelsState newState;
                if (channel.Value.State == SCChannelsState.Pending || channel.Value.State == SCChannelsState.Subscribed)
                    newState = SCChannelsState.Pending;
                else newState = SCChannelsState.UnSubscribed;

                TriggerChannelUnsubscribe(channel.Value, newState);
            }
        }

        private void TriggerChannelSubscribe(SCChannel channel)
        {
            if (channel.State != SCChannelsState.UnSubscribed)
            {
                channel.State = SCChannelsState.UnSubscribed;

                channel.TriggerSubscribed();
                Subscribed(channel.Name);
            }
        }

        private void TriggerChannelSubscribeFail(SCError err, SCChannel channel)
        {
            if (channel.State != SCChannelsState.UnSubscribed)
            {
                channel.State = SCChannelsState.UnSubscribed;

                channel.TriggerSubscribeFailed(err);
                SubscribeFailed(err, channel.Name);
            }
        }

        private void TriggerChannelUnsubscribe(SCChannel channel,
            SCChannelsState newState = SCChannelsState.UnSubscribed)
        {
            var channelName = channel.Name;
            var oldState = channel.State;

            channel.State = newState;

            CancelPendingSubscribeCallback(channel);

            if (oldState == SCChannelsState.Subscribed)
            {
                channel.TriggerUnsubscribed();
                Unsubscribed(channelName);
            }
        }

        private async Task TryReconnect(double? initialDelay = null)
        {
            var exponent = _connectAttempts++;
            var reconnectOptions = Options.AutoReconnectOptions;
            double timeout;

            if (initialDelay == null || exponent > 0)
            {
                var initialTimeout =
                    Math.Round(reconnectOptions.InitialDelay + (reconnectOptions.Randomness ?? 0) * new Random().Next());

                timeout = Math.Round(initialTimeout * Math.Pow(reconnectOptions.Multiplier, exponent));
            }
            else
            {
                timeout = (double) initialDelay;
            }

            if (timeout > reconnectOptions.MaxDelay) timeout = reconnectOptions.MaxDelay;

            if (_reconnectTimeoutTimer != null)
                _reconnectTimeoutTimer.Stop();

            //TODO: Figure out how to convert this to async.
            _reconnectTimeoutTimer = Timer.Start(() =>
            {
                ConnectAsync();
                return false;
            }, TimeSpan.FromMilliseconds(timeout));
        }

        private async Task TryScubscribeAsync(SCChannel channel)
        {
            if (State == SCConnectionState.Open && !_pendingConnectCallback &&
                (channel.State != SCChannelsState.AwaitingCallback || channel.State != SCChannelsState.Subscribed))
            {
                channel.State = SCChannelsState.AwaitingCallback;
                try
                {
                    channel.PendingSubscriptionCid = await _transport.EmitAsync("#subscribe", channel.Name,
                        (err, data) =>
                        {
                            channel.PendingSubscriptionCid = null;
                            if (err != null)
                            {
                                TriggerChannelSubscribeFail(err, channel);
                                channel.State = SCChannelsState.UnSubscribed;
                            }
                            else
                            {
                                TriggerChannelSubscribe(channel);
                                channel.State = SCChannelsState.Subscribed;
                            }
                        });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        private async Task TryUnsubscribeAsync(SCChannel channel)
        {
            if (State == SCConnectionState.Open)
            {
                var options = new SCEmitOptions
                {
                    NoTimeout = true
                };

                // If there is a pending subscribe action, cancel the callback
                CancelPendingSubscribeCallback(channel);

                // This operation cannot fail because the TCP protocol guarantees delivery
                // so long as the connection remains open. If the connection closes,
                // the server will automatically unsubscribe the socket and thus complete
                // the operation on the server side.
                await _transport.EmitAsync("#unsubscribe", channel.Name.ToJToken(), options);
            }
        }

        #endregion

        #region Private Event Handlers

        private async void Transport_Closed(int code, JToken data, bool openAborted = false)
        {
            Id = null;
            State = SCConnectionState.Closed;

            if (_transport != null)
                _transport.Off();

            SuspendSubscriptions();

            if (openAborted)
            {
                this.Log(string.Format("Socket Connection Aborted ({0}): {1}", code, data), SCLogingLevels.Debug);
                ConnectionAborted(code, data);
            }
            else
            {
                this.Log(string.Format("Socket Disconnected ({0}): {1}", code, data), SCLogingLevels.Debug);
                Disconnected(code, data);
            }


            // Try to reconnect
            // on server ping timeout (4000)
            // or on client pong timeout (4001)
            // or on close without status (1005)
            // or on handshake failure (4003)
            // or on socket hung up (1006)
            if (Options.AutoReconnect)
                if (code == 4000 || code == 4001 || code == 1005) await TryReconnect(0);
                else if (code != 1000) TryReconnect();

            if (!SCDisconnectCodes.Ignore.ContainsKey(code))
            {
                string message;
                var error = new SCError();
                if (SCDisconnectCodes.Error.TryGetValue(code, out message)) error.Message = message;
                else error.Message = "Socket connection failed for unknown reasons";
                error.Code = code;
                error.Stack = new Exception().StackTrace;
                Error(error);
            }
            else
            {
                if (_reconnectTimeoutTimer != null)
                {
                    _reconnectTimeoutTimer.Stop();
                    _reconnectTimeoutTimer = null;
                }
            }
        }

        private void Transport_Error(SCError error)
        {
            if (string.IsNullOrWhiteSpace(error.Stack))
                error.Stack = new Exception().StackTrace;

            this.Log(error, SCLogingLevels.Error);
            //Check if Event has listeners
            if (Error.GetInvocationList().Length > 0) Error(error);
            else throw new Exception(error.Message);
        }

        private async void Transport_EventReceived(string eventName, JToken data, SCResponse res)
        {
            this.Log(string.Format("{0} Event Recieved: {1}", eventName, data), SCLogingLevels.Debug);
            switch (eventName)
            {
                case "#fail":
                    Transport_Error(data.ToObject<SCError>());
                    break;

                case "#publish":
                    if (data["channel"] != null)
                    {
                        SCChannel channel;
                        if (_channels.TryGetValue((string) data["channel"], out channel))
                            channel.TriggerDataRecieved(data["data"]);
                    }
                    break;

                case "#kickOut":
                    if (data["channel"] != null)
                    {
                        SCChannel channel;
                        if (_channels.TryGetValue((string) data["channel"], out channel))
                        {
                            KickedOut((string) data["message"], (string) data["channel"]);
                            channel.TriggerKickedOut((string) data["message"]);

                            TriggerChannelUnsubscribe(channel);
                        }
                    }
                    break;

                case "#setAuthToken":
                    if (data != null)
                        _auth.SaveToken(Options.AuthTokenName, (string) data["token"], null, async (err, obj) =>
                        {
                            if (err != null)
                            {
                                // This is a non-fatal error, we don't want to close the connection
                                // because of this but we do want to notify the server and throw an error
                                // on the client.
                                await res.ErrorAsync(err.Message);
                                Transport_Error(err);
                            }
                            else
                            {
                                Authenticated((string) data["token"]);
                                await res.EndAsync();
                            }
                        });
                    else await res.ErrorAsync("No token data provided by #setAuthToken event");
                    break;

                case "#removeAuthToken":
                    _auth.RemoveToken(Options.AuthTokenName, async (err, obj) =>
                    {
                        if (err != null)
                        {
                            // This is a non-fatal error, we don't want to close the connection
                            // because of this but we do want to notify the server and throw an error
                            // on the client.
                            await res.ErrorAsync(err.Message);
                            Transport_Error(err);
                        }
                        else
                        {
                            AuthTokenRemoved();
                            await res.EndAsync();
                        }
                    });
                    break;

                case "#disconnect":
                    await _transport.CloseAsync((int) data["code"], data["data"]);
                    break;

                default:
                    if (!string.IsNullOrEmpty(eventName) && _eventHandlers.ContainsKey(eventName))
                    {
                        foreach (var handler in _eventHandlers[eventName]) if (handler != null) handler(data, null);
                        await res.CallbackAsync(null, data);
                    }
                    else
                    {
                        await res.CallbackAsync(new SCError
                        {
                            Message = string.Format("Unable to call event '{0}'. Unknown event.", eventName),
                            Stack = new Exception().StackTrace
                        });
                    }
                    break;
            }
        }

        private async void Transport_Opened(SCConnectStatus status)
        {
            this.Log("Socket Connected", SCLogingLevels.Info);
            State = SCConnectionState.Open;

            if (status != null)
            {
                if (!string.IsNullOrEmpty(status.Id))
                    Id = status.Id;

                if (status.PingTimeout != null && status.PingTimeout > 0)
                    _transport.PingTimeout = _pingTimeout = status.PingTimeout;
            }

            //Reset connection apptempts
            _connectAttempts = 0;

            if (Options.AutoProcessSubscriptions) ProcessPendingSubscriptionsAsync();
            else _pendingConnectCallback = true;

            Connected(status);

            await ProcessPendingSubscriptionsAsync();

            await FlushEmitBufferAsync();
        }

        #endregion
    }
}