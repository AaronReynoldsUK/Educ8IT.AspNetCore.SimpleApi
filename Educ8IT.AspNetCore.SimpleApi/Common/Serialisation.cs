// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.
// Additional copyright as referenced in URLs below.

using Educ8IT.AspNetCore.SimpleApi.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// Common serialisation classes
    /// </summary>
    public static class Serialisation
    {
        #region XML Serialisation

        /// <summary>
        /// Remove all namespaces from an XML document string.
        /// XML string is parsed into an <see cref="XmlDocument"/> to process.
        /// https://stackoverflow.com/questions/987135/how-to-remove-all-namespaces-from-xml-with-c
        /// </summary>
        /// <param name="xmlDocument">XML document as string to parse</param>
        /// <returns>Cleaned XML document as string</returns>
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        /// <summary>
        /// Remove all namespaces from an <see cref="XElement"/>.
        /// This function works recursively through the document.
        /// https://stackoverflow.com/questions/987135/how-to-remove-all-namespaces-from-xml-with-c
        /// </summary>
        /// <param name="xmlDocument">XML element to parse</param>
        /// <returns>XML element that has been cleaned</returns>
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName)
                {
                    Value = xmlDocument.Value
                };

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        /// <summary>
        /// Asynchronousely serialise a CLR object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the CLR object to serialise</typeparam>
        /// <param name="source">A CLR object</param>
        /// <param name="formatForReading">Pretty-print the output. Default is false</param>
        /// <param name="encoding">Encoding to use when serialising. Default is UTF8.</param>
        /// <returns>XML-serialised version of the object</returns>
        public async static Task<string> SerialiseToXmlAsync<T>(this T source, bool formatForReading = false, Encoding encoding = null)
        {
            string result = null;

            try
            {
                using (Stream stream = new MemoryStream())
                {
                    Type serType = (typeof(T).Name == "Object") ? source.GetType() : typeof(T);
                    XmlSerializer xSer = new XmlSerializer(serType);
                    XmlWriter xWriter = XmlWriter.Create(stream,
                        new XmlWriterSettings()
                        {
                            OmitXmlDeclaration = true,
                            Encoding = encoding ?? Encoding.UTF8,
                            Indent = formatForReading,
                        }); //new XmlTextWriter(stream, Encoding.UTF8);
                    xSer.Serialize(stream, source);

                    using (TextReader reader = new StreamReader(stream, encoding ?? Encoding.UTF8))
                    {
                        stream.Position = 0;
                        //result = reader.ReadToEnd();
                        result = await reader.ReadToEndAsync();
                    }

                    stream.Close();
                }

                result = RemoveAllNamespaces(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result = null;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="stream"></param>
        /// <param name="formatForReading"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async static Task<string> SerialiseToXmlAsync<T>(this T source, MemoryStream stream, bool formatForReading = false, Encoding encoding = null)
        {
            string result = null;

            try
            {
                Type serType = (typeof(T).Name == "Object") ? source.GetType() : typeof(T);
                XmlSerializer xSer = new XmlSerializer(serType);
                XmlWriter xWriter = XmlWriter.Create(stream,
                    new XmlWriterSettings()
                    {
                        Async = true,
                        OmitXmlDeclaration = true,
                        Encoding = encoding ?? Encoding.UTF8,
                        Indent = formatForReading
                    }); //new XmlTextWriter(stream, Encoding.UTF8);
                xSer.Serialize(stream, source);
                await xWriter.FlushAsync();

                using (TextReader reader = new StreamReader(stream, encoding ?? Encoding.UTF8))
                {
                    stream.Position = 0;
                    //result = reader.ReadToEnd();
                    result = await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Synchronousely serialise a CLR object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the CLR object to serialise</typeparam>
        /// <param name="source">A CLR object</param>
        /// <param name="formatForReading">Pretty-print the output. Default is false</param>
        /// <param name="encoding">Encoding to use when serialising. Default is UTF8.</param>
        /// <returns>XML-serialised version of the object</returns>
        public static string SerialiseToXml<T>(this T source, bool formatForReading = false, Encoding encoding = null)
        {
            string result = null;

            try
            {
                using (Stream stream = new MemoryStream())
                {
                    Type serType = (typeof(T).Name == "Object") ? source.GetType() : typeof(T);
                    XmlSerializer xSer = new XmlSerializer(serType);
                    XmlWriter xWriter = XmlWriter.Create(stream,
                        new XmlWriterSettings()
                        {
                            OmitXmlDeclaration = true,
                            Encoding = encoding ?? Encoding.UTF8,
                            Indent = formatForReading
                        }); //new XmlTextWriter(stream, Encoding.UTF8);
                    xSer.Serialize(stream, source);

                    using (TextReader reader = new StreamReader(stream, encoding ?? Encoding.UTF8))
                    {
                        stream.Position = 0;
                        result = reader.ReadToEnd();
                    }

                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result = null;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserialiseFromXml<T>(this string xml)
        {
            T result = default;

            try
            {
                using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    XmlSerializer xSer = new XmlSerializer(typeof(T));
                    result = (T)xSer.Deserialize(stream);
                }
            }
            catch (InvalidOperationException invo_ex)
            {
                throw invo_ex;
            }
            catch
            {
                result = default;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserialiseFromXml(this string xml, Type type)
        {
            object result = default;

            try
            {
                using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    XmlSerializer xSer = new XmlSerializer(type);
                    result = xSer.Deserialize(stream);
                }
            }
            catch (InvalidOperationException invo_ex)
            {
                throw invo_ex;
            }
            catch
            {
                result = default;
            }

            return result;
        }

        #endregion

        #region JSON Serialisation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserialiseFromJson(this string json, Type type)
        {
            object result;

            try
            {
                result = JsonSerializer.Deserialize(json, type);
            }
            catch (JsonException json_ex)
            {
                throw json_ex;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result = default;
            }

            return result;
        }

        #endregion

        #region Base64 Serialisation

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectSource"></param>
        /// <returns></returns>
        public static string SerialiseToBase64<T>(this T objectSource)
        {
            string result = null;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter();
                    ms.Position = 0;

                    formatter.Serialize(ms, objectSource);

                    byte[] bytes = ms.GetBuffer();

                    result = bytes.Length.ToString() + ":" + Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result = null;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static T DeserialiseFromBase64<T>(this string base64)
        {
            T result = default;

            try
            {
                int __marker = base64.IndexOf(":");
                int __length = Convert.ToInt32(base64.Substring(0, __marker));

                byte[] byteArray = Convert.FromBase64String(base64.Substring(__marker + 1));

                using (MemoryStream ms = new MemoryStream(byteArray, 0, __length))
                {
                    IFormatter formatter = new BinaryFormatter();

                    result = (T)(formatter.Deserialize(ms));// as T;
                }
            }
            catch
            {
                result = default;
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SerialiseToUrlEncoded(this object source)
        {
            string result = String.Empty;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            var __properties = source.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            foreach (var __property in __properties)
            {
                var __propertyAlias = __property.GetCustomAttributes(true).Where(attr => attr is PropertyAliasAttribute).FirstOrDefault() as PropertyAliasAttribute;

                keyValuePairs.Add(
                    __propertyAlias?.Alias ?? __property.Name,
                    __property.GetValue(source)?.ToString() ?? String.Empty
                    );
            }

            List<string> __parameters = new List<string>();
            foreach (var kvp in keyValuePairs)
                __parameters.Add(kvp.Key + "=" + System.Web.HttpUtility.UrlEncode(kvp.Value));

            return String.Join("&", __parameters.ToArray());
        }
    }

    #region Legacy

    /// <summary>
    /// 
    /// </summary>
    public class StringWriterWithEncoding : StringWriter
    {
        private Encoding myEncoding;

        /// <summary>
        /// 
        /// </summary>
        public override Encoding Encoding
        {
            get
            {
                return myEncoding;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        public StringWriterWithEncoding(Encoding encoding)
            : base()
        {
            myEncoding = encoding;
        }
    }

    #endregion

}
