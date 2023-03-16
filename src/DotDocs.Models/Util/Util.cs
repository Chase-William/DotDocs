// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using Newtonsoft.Json;

namespace DotDocs.Models.Util
{
    // Copyright (c) Philipp Wagner. All rights reserved.
    // Licensed under the MIT license. See LICENSE file in the project root for full license information.

    public static class ParameterSerializer
    {
        public static IList<Dictionary<string, object>> ToDictionary<TSourceType>(IList<TSourceType> source)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(source, settings);

            return JsonConvert.DeserializeObject<IList<Dictionary<string, object>>>(json, new CustomDictionaryConverter());
        }
    }    

    /// <summary>
    /// This Converter is only a slightly modified converter from the JSON Extension library. 
    /// 
    /// All Credit goes to Oskar Gewalli (https://github.com/wallymathieu) and the Makrill Project (https://github.com/NewtonsoftJsonExt/makrill).
    /// </summary>
    public class CustomDictionaryConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ExpectObject(reader);
        }

        private static object ExpectDictionaryOrArrayOrPrimitive(JsonReader reader)
        {
            reader.Read();
            var startToken = reader.TokenType;
            switch (startToken)
            {
                case JsonToken.String:
                case JsonToken.Integer:
                case JsonToken.Boolean:
                case JsonToken.Bytes:
                case JsonToken.Date:
                case JsonToken.Float:
                case JsonToken.Null:
                    return reader.Value;
                case JsonToken.StartObject:
                    return ExpectObject(reader);
                case JsonToken.StartArray:
                    return ExpectArray(reader);
            }
            throw new JsonSerializationException($"Unrecognized token: {reader.TokenType}");
        }

        private static object ExpectObject(JsonReader reader)
        {
            var dic = new Dictionary<string, object>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:
                        break;
                    case JsonToken.PropertyName:
                        dic.Add(reader.Value.ToString(), ExpectDictionaryOrArrayOrPrimitive(reader));
                        break;
                    case JsonToken.EndObject:
                        return dic;
                    default:
                        throw new JsonSerializationException($"Unrecognized token: {reader.TokenType}");
                }
            }
            throw new JsonSerializationException("Missing End Token");
        }

        private static object ExpectArray(JsonReader reader)
        {
            var array = new List<Object>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                    case JsonToken.Integer:
                    case JsonToken.Boolean:
                    case JsonToken.Bytes:
                    case JsonToken.Date:
                    case JsonToken.Float:
                    case JsonToken.Null:
                        array.Add(reader.Value);
                        break;
                    case JsonToken.Comment:
                        break;
                    case JsonToken.StartObject:
                        array.Add(ExpectObject(reader));
                        break;
                    case JsonToken.StartArray:
                        array.Add(ExpectArray(reader));
                        break;
                    case JsonToken.EndArray:
                        return array.ToArray();
                    default:
                        throw new JsonSerializationException($"Unrecognized token: {reader.TokenType}");
                }
            }
            throw new JsonSerializationException("Missing End Token");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, object>);
        }
    }
}