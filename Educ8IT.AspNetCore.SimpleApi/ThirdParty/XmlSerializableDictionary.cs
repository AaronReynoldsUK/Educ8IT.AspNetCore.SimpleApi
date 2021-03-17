// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.
// Other licenses/rights may apply to derived work below.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.ThirdParty
{
    /// <summary>
    /// Courtesy of Peter welter
    /// https://weblogs.asp.net/pwelter34/444961
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [XmlRoot("dictionary")]
    public class XmlSerializableDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>, IXmlSerializable
    {
        /// <summary>
        /// 
        /// </summary>
        public XmlSerializableDictionary() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        public XmlSerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="comparer"></param>
        public XmlSerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public XmlSerializableDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public XmlSerializableDictionary(int capacity) : base(capacity) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public XmlSerializableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }

        #region IXmlSerializable Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(String.Empty, String.Empty);

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                Type t = this[key].GetType();
                //writer.WriteAttributeString("type", t.FullName);

                if (t == typeof(string[]))
                {
                    var __tmpValue = String.Join(",", this[key] as string[]);
                    valueSerializer.Serialize(writer, __tmpValue, xmlSerializerNamespaces);
                }
                else
                {
                    TValue value = this[key];
                    valueSerializer.Serialize(writer, value, xmlSerializerNamespaces);
                }

                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
