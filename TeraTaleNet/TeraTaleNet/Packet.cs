﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TeraTaleNet
{
    public class Packet
    {
        static Dictionary<Type, int> _indexByType = new Dictionary<Type, int>();
        static Dictionary<int, Type> _typeByIndex = new Dictionary<int, Type>();

        public Header header;
        public ISerializable body;

        static Packet()
        {
            var type = typeof(ISerializable);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract);
            var typeArr = types.ToArray();
            for (int i = 0; i < typeArr.Length; i++)
            {
                _indexByType.Add(typeArr[i], i);
                _typeByIndex.Add(i, typeArr[i]);
            }
        }

        static public int GetIndexByType(Type type)
        {
            return _indexByType[type];
        }

        static public Type GetTypeByIndex(int index)
        {
            return _typeByIndex[index];
        }

        static public Packet Create(Header header, byte[] bytes)
        {
            ISerializable body = (ISerializable)Activator.CreateInstance(GetTypeByIndex(header.type), bytes);
            return new Packet(header, body);
        }

        Packet(Header header, ISerializable body)
        {
            this.header = header;
            this.body = body;
        }

        public Packet(ISerializable body)
        {
            header = body.CreateHeader();
            this.body = body;
        }

        public byte[] Serialize()
        {
            var headerBytes = header.Serialize();
            var bodyBytes = body.Serialize();

            int offset = 0;
            byte[] ret = new byte[headerBytes.Length + bodyBytes.Length];

            headerBytes.CopyTo(ret, offset);
            offset += headerBytes.Length;
            bodyBytes.CopyTo(ret, offset);
            offset += bodyBytes.Length;

            return ret;
        }

        public int SerializedSize()
        {
            return header.SerializedSize() + body.SerializedSize();
        }
    }
}
