//
//  SCChannels.cs
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
using Newtonsoft.Json.Linq;
using SocketClusterSharp.Errors;
using SocketClusterSharp.Interfaces;

namespace SocketClusterSharp.Client
{
    public class SCChannel : ISCChannel
    {
        #region Private Fields

        private readonly SCSocket _socketClient;

        #endregion

        #region Constructors

        public SCChannel(string name, SCSocket client)
        {
            Name = name;
            _socketClient = client;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the channel's name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the channel's state.
        /// </summary>
        /// <value>The state.</value>
        public SCChannelsState State { get; set; } = SCChannelsState.Pending;

        /// <summary>
        ///     Gets or sets the channel's pending subscription cid.
        /// </summary>
        /// <value>The pending subscription cid.</value>
        public int? PendingSubscriptionCid { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs when channel is unsubscribed to.
        /// </summary>
        public event Action Unsubscribed = delegate { };

        /// <summary>
        ///     Occurs when subscription failed.
        /// </summary>
        public event Action<SCError> SubscribeFailed = delegate { };

        /// <summary>
        ///     Occurs when channel is sucessfully subscribed to.
        /// </summary>
        public event Action Subscribed = delegate { };

        /// <summary>
        ///     Occurs when kicked out of when channel.
        /// </summary>
        public event Action<string> KickedOut = delegate { };

        /// <summary>
        ///     Occurs when data is recieved.
        /// </summary>
        public event Action<JToken> DataRecieved = delegate { };

        #endregion

        #region Public Methods

        /// <summary>
        ///     Subscribe to this channel. This function returns an SCChannel object which lets you watch for incoming data
        ///     on this channel.
        /// </summary>
        public async Task ScubscribeAsync()
        {
            await _socketClient.SubscribeAsync(Name);
        }

        /// <summary>
        ///     Unsubscribe from this channel. This makes any associated SCChannel object inactive. You can reactivate this
        ///     SCChannel object by calling SubscribeAsync() again at a later time.
        /// </summary>
        public async Task UnScubscribeAsync()
        {
            await _socketClient.UnsubscribeAsync(Name);
        }

        /// <summary>
        ///     Check if currently subscribed to this channel.
        /// </summary>
        /// <returns><c>true</c> if this instance is subscribed.; otherwise, <c>false</c>.</returns>
        /// <param name="includePending">If includePending is <c>true</c>, it will aslo return true if the subscription is pending.</param>
        public bool IsSubscribed(bool includePending = false)
        {
            return _socketClient.IsSubscribed(Name, includePending);
        }

        /// <summary>
        ///     Publishs the data on given channel.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="data">Data.</param>
        /// <param name="callback">Callback.</param>
        public async Task PublishAsync(JToken data, SCCallback callback = null)
        {
            await _socketClient.PublishAsync(Name, data, callback);
        }

        /// <summary>
        ///     Lets you watch the channel directly. The handler accepts a single data argument which holds the
        ///     data as a <see cref="Newtonsoft.Json.JsonToken" /> which was published to the channel.
        /// </summary>
        public void Watch(Action<JToken> handler)
        {
            DataRecieved += handler;
        }


        /// <summary>
        ///     Stop handling data which is published this specified channel. This is different from unsubscribe in that the socket
        ///     will still receieve channel data but the specified handler will no longer capture it. If the handler argument is
        ///     not
        ///     provided, all handlers for this channel will be removed.
        /// </summary>
        public void UnWatch(Action<JToken> handler = null)
        {
            if (handler == null)
                DataRecieved = delegate { };
            else DataRecieved -= handler;
        }

        /// <summary>
        ///     This will cause SCSocket to unsubscribe this channel and remove any watchers from it. Any SCChannel object which is
        ///     associated with this channel will be disabled permanently (ready to be cleaned up by garbage collector).
        /// </summary>
        public void Destroy()
        {
            _socketClient.DestroyChannelAsync(Name);
        }

        /// <summary>
        ///     Returns a list of event handlers which are watching for data on this channel.
        /// </summary>
        public Delegate[] Watchers()
        {
            return DataRecieved.GetInvocationList();
        }

        /// <summary>
        ///     Triggers the kicked out event.
        /// </summary>
        /// <param name="message">Message.</param>
        public void TriggerKickedOut(string message)
        {
            KickedOut(message);
        }

        /// <summary>
        ///     Triggers the unsubscribed event.
        /// </summary>
        public void TriggerUnsubscribed()
        {
            Unsubscribed();
        }

        /// <summary>
        ///     Triggers the subscribed event.
        /// </summary>
        public void TriggerSubscribed()
        {
            Subscribed();
        }

        /// <summary>
        ///     Triggers the subscribe failed event.
        /// </summary>
        /// <param name="err">Error.</param>
        public void TriggerSubscribeFailed(SCError err)
        {
            SubscribeFailed(err);
        }

        /// <summary>
        ///     Triggers the data recieved event.
        /// </summary>
        /// <param name="data">Data.</param>
        public void TriggerDataRecieved(JToken data)
        {
            DataRecieved(data);
        }

        #endregion
    }
}