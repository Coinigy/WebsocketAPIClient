//
//  Response.cs
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
using SocketClusterSharp.Errors;
using SocketClusterSharp.Helpers;
using WebSocket4Net;

namespace SocketClusterSharp
{
    /// <summary>
    ///     SocketCluster responce object.
    /// </summary>
    public class SCResponse
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SCResponse" /> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="socket">Web socket client.</param>
        public SCResponse(string id, WebSocket socket)
        {
            Socket = socket;
            Id = id;
        }

        #endregion

        #region Private Methods

        private async Task RespondAsync(SCResponseData data)
        {
            Socket.Send(data.ToJSON());
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the socket client.
        /// </summary>
        /// <value>The socket.</value>
        public WebSocket Socket { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Ends the response.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task EndAsync()
        {
            if (!string.IsNullOrWhiteSpace(Id)) await RespondAsync(new SCResponseData {Rid = Id});
        }

        /// <summary>
        ///     Ends the response.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="data">Data.</param>
        public async Task EndAsync(object data)
        {
            if (!string.IsNullOrWhiteSpace(Id)) await RespondAsync(new SCResponseData {Rid = Id, Data = data});
        }

        /// <summary>
        ///     Sends error responce asyncronously.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="error">Error.</param>
        public async Task ErrorAsync(string error)
        {
            await ErrorAsync(new SCError(error), null);
        }

        /// <summary>
        ///     Sends error responce asyncronously.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="error">Error.</param>
        public async Task ErrorAsync(SCError error)
        {
            await ErrorAsync(error, null);
        }

        /// <summary>
        ///     Sends error responce asyncronously.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="exception">Error.</param>
        public async Task ErrorAsync(Exception exception)
        {
            var error = new SCError(exception.Message);
            error.Stack = exception.StackTrace;


            if (exception.InnerException != null)
            {
                var data = new
                {
                    innerException = new
                    {
                        message = exception.InnerException.Message,
                        stack = exception.InnerException.StackTrace
                    }
                };
                await ErrorAsync(error, data);
            }
            else
            {
                await ErrorAsync(error, null);
            }
        }

        /// <summary>
        ///     Sends error responce asyncronously.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="error">Error.</param>
        /// <param name="data"></param>
        public async Task ErrorAsync(string error, object data)
        {
            await ErrorAsync(new SCError(error), data);
        }

        /// <summary>
        ///     Sends error responce asyncronously.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="exception">Error.</param>
        /// <param name="data"></param>
        public async Task ErrorAsync(Exception exception, object data)
        {
            var error = new SCError(exception.Message);
            error.Stack = exception.StackTrace;

            if (exception.InnerException != null)
                data = data.Merge(new
                {
                    innerException = new
                    {
                        message = exception.InnerException.Message,
                        stack = exception.InnerException.StackTrace
                    }
                });
            await ErrorAsync(error, data);
        }

        /// <summary>
        ///     Sends error responce asyncronously.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="error">Error.</param>
        /// <param name="data">Data.</param>
        public async Task ErrorAsync(SCError error, object data)
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                var response = new SCResponseData
                {
                    Rid = Id,
                    Data = data,
                    Err = error
                };

                await RespondAsync(response);
            }
        }

        /// <summary>
        ///     The asyncronous callback.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="error">Error.</param>
        /// <param name="data">Data.</param>
        public async Task CallbackAsync(SCError error, object data = null)
        {
            if (error != null) await ErrorAsync(error, data);
            else await EndAsync(data);
        }

        #endregion
    }
}