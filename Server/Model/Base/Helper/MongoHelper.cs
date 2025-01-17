﻿using System;
using System.IO;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ETModel
{
    public static class MongoHelper
    {
        static MongoHelper()
        {
            Type bsonClassMap = typeof(BsonClassMap);
            MethodInfo methodInfo = bsonClassMap.GetMethod("RegisterClassMap", new Type[]
            {
            });

            Type[] types = typeof(Game).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(Component)))
                {
                    continue;
                }

                if (type == typeof(ComponentWithId) || type == typeof(Component) || type == typeof(Entity))
                {
                    continue;
                }
                methodInfo.MakeGenericMethod(type).Invoke(null, null);
            }

            BsonSerializer.RegisterSerializer(new EnumSerializer<NumericType>(BsonType.String));
        }

        public static string ToJson(object obj)
        {
            return obj.ToJson();
        }

        public static string ToJson(object obj, JsonWriterSettings settings)
        {
            return obj.ToJson(settings);
        }

        public static T FromJson<T>(string str)
        {
            return BsonSerializer.Deserialize<T>(str);
        }

        public static object FromJson(Type type, string str)
        {
            return BsonSerializer.Deserialize(str, type);
        }

        public static byte[] ToBson(object obj)
        {
            return obj.ToBson();
        }

        public static void ToBson(object obj, MemoryStream stream)
        {
            using (BsonBinaryWriter bsonWriter = new BsonBinaryWriter(stream, BsonBinaryWriterSettings.Defaults))
            {
                BsonSerializationContext context = BsonSerializationContext.CreateRoot(bsonWriter);
                BsonSerializationArgs args = default(BsonSerializationArgs);
                args.NominalType = typeof(object);
                IBsonSerializer serializer = BsonSerializer.LookupSerializer(args.NominalType);
                serializer.Serialize(context, args, obj);
            }
        }

        public static object FromBson(Type type, byte[] bytes)
        {
            return BsonSerializer.Deserialize(bytes, type);
        }

        public static object FromBson(Type type, byte[] bytes, int index, int count)
        {
            using (MemoryStream memoryStream = new MemoryStream(bytes, index, count))
            {
                return BsonSerializer.Deserialize(memoryStream, type);
            }
        }

        public static object FromBson(object instance, byte[] bytes, int index, int count)
        {
            using (MemoryStream memoryStream = new MemoryStream(bytes, index, count))
            {
                return BsonSerializer.Deserialize(memoryStream, instance.GetType());
            }
        }

        public static object FromBson(object instance, Stream stream)
        {
            return BsonSerializer.Deserialize(stream, instance.GetType());
        }

        public static object FromStream(Type type, Stream stream)
        {
            return BsonSerializer.Deserialize(stream, type);
        }

        public static T FromBson<T>(byte[] bytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                return (T)BsonSerializer.Deserialize(memoryStream, typeof(T));
            }
        }

        public static T FromBson<T>(byte[] bytes, int index, int count)
        {
            return (T)FromBson(typeof(T), bytes, index, count);
        }

        public static T Clone<T>(T t)
        {
            return FromBson<T>(ToBson(t));
        }
    }
}