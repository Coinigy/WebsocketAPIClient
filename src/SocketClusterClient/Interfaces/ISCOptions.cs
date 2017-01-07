//
//  ISCSocketOptions.cs
//
//  Author:
//       Todd Henderson <todd@todd-henderson.me>
//
//  Copyright (c) 2015-2016 Todd Henderson
//
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General  License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General  License for more details.
//
//  You should have received a copy of the GNU Lesser General  License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using SocketClusterSharp.Internal;
using SocketClusterSharp.Helpers.Web;

namespace SocketClusterSharp.Interfaces
{
	/// <summary>
	/// Base SCOptions Intervace
	/// </summary>
	public interface ISCOptions
	{
		/// <summary>
		/// Gets or sets the logging level.
		/// </summary>
		/// <value>The logging.</value>
		SCLogingLevels Logging { get; set; }

	}

}