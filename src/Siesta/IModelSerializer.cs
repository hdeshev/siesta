#region License
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2009, Enkari, Ltd.
// Licensed under the Apache License, Version 2.0. See the file LICENSE.txt for details.
#endregion
#region Using Directives
using System;
using System.Collections.Generic;

#endregion

namespace Siesta
{
	public interface IModelSerializer
	{
		IEnumerable<string> ContentTypes { get; }
		bool IsDefault { get; }

		string Serialize(object model, ModelFormatting formatting);
		object Deserialize(Type modelType, string content);
	}
}