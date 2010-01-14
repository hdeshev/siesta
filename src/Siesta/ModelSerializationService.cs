#region License
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2009, Enkari, Ltd.
// Licensed under the Apache License, Version 2.0. See the file LICENSE.txt for details.
#endregion
#region Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Siesta
{
	public class ModelSerializationService : IModelSerializationService
	{
		public IDictionary<string, IModelSerializer> Serializers { get; private set; }

		public IEnumerable<string> SupportedContentTypes
		{
			get 
			{
				return from serializer in Serializers.Values
					   from contentType in serializer.ContentTypes
					   select contentType;
			}
		}

		public string DefaultContentType
		{
			get 
			{ 
				return Serializers.Values.Where(s => s.IsDefault).Select(s => s.ContentTypes.First()).Single(); 
			}
		}

		public ModelSerializationService(IEnumerable<IModelSerializer> serializers)
		{
			var flattened = from serializer in serializers
						  from contentType in serializer.ContentTypes
						  select new { ContentType = contentType, Serializer = serializer };
			Serializers = flattened.ToDictionary(item => item.ContentType, item => item.Serializer);
		}

		public string Serialize(object model, string contentType, ModelFormatting formatting)
		{
			if (!Serializers.ContainsKey(contentType))
				throw new NotSupportedException(String.Format("Don't know how to serialize response with MIME type '{0}'", contentType));

			return Serializers[contentType].Serialize(model, formatting);
		}

		public object Deserialize(Type modelType, string content, string contentType)
		{
			if (!Serializers.ContainsKey(contentType))
				throw new NotSupportedException(String.Format("Don't know how to deserialize request with MIME type '{0}'", contentType));

			return Serializers[contentType].Deserialize(modelType, content);
		}
	}
}