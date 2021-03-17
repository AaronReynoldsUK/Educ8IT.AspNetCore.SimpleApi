// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.
// Includes content adapted from ASP.NET API

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Educ8IT.AspNetCore.SimpleApi.DocumentationProviders
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlDocumentationProvider : IDocumentationProvider
    {

        private readonly XPathNavigator _documentNavigator;
        //private readonly string _xmlDocumentationFilePath;

        private const string TypeExpression = "/doc/members/member[@name='T:{0}']";
        private const string MethodExpression = "/doc/members/member[@name='M:{0}']";
        private const string PropertyExpression = "/doc/members/member[@name='P:{0}']";
        private const string FieldExpression = "/doc/members/member[@name='F:{0}']";
        private const string ParameterExpression = "param[@name='{0}']";

        /// <summary>
        /// 
        /// </summary>
        public Assembly ApiAssembly { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string DocumentationPath { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiAssembly"></param>
        /// <param name="xmlDocumentationFilePath"></param>
        public XmlDocumentationProvider(Assembly apiAssembly, string xmlDocumentationFilePath)
        {
            ApiAssembly = apiAssembly ??
                throw new ArgumentNullException(nameof(apiAssembly));

            DocumentationPath = xmlDocumentationFilePath ?? 
                throw new ArgumentNullException(nameof(xmlDocumentationFilePath));

            if (!File.Exists(DocumentationPath))
                throw new FileNotFoundException("File not found: " + DocumentationPath);

            try
            {
                XPathDocument xPathDocument = new XPathDocument(DocumentationPath);
                _documentNavigator = xPathDocument.CreateNavigator();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Used for Controllers / Classes and Parameters
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetSummaryOrDescription(Type type)
        {
            XPathNavigator typeNode = GetTypeNode(type);
            return GetTagValue(typeNode, "summary");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public string GetSummaryOrDescription(MethodInfo methodInfo)
        {
            XPathNavigator methodNode = GetMethodNode(methodInfo);
            return GetTagValue(methodNode, "summary");

            //string memberName = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", GetTypeName(method.DeclaringType), method.Name);
            //string expression = MethodExpression;
            //string selectExpression = String.Format(CultureInfo.InvariantCulture, expression, memberName);
            //XPathNavigator propertyNode = _documentNavigator.SelectSingleNode(selectExpression);
            //return GetTagValue(propertyNode, "summary");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public string GetSummaryOrDescription(MemberInfo memberInfo)
        {
            string memberName = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", GetTypeName(memberInfo.DeclaringType), memberInfo.Name);
            string expression = memberInfo.MemberType == MemberTypes.Field ? FieldExpression : PropertyExpression;
            string selectExpression = String.Format(CultureInfo.InvariantCulture, expression, memberName);
            XPathNavigator propertyNode = _documentNavigator.SelectSingleNode(selectExpression);
            return GetTagValue(propertyNode, "summary");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        public string GetSummaryOrDescription(ParameterInfo parameterInfo)
        {
            if (parameterInfo != null)
            {
                MethodInfo methodInfo = parameterInfo.Member as MethodInfo;
                XPathNavigator methodNode = GetMethodNode(methodInfo);
                if (methodNode != null)
                {
                    string parameterName = parameterInfo.Name;
                    XPathNavigator parameterNode = methodNode.SelectSingleNode(String.Format(CultureInfo.InvariantCulture, ParameterExpression, parameterName));
                    if (parameterNode != null)
                    {
                        return parameterNode.Value.Trim();
                    }
                }
            }

            return null;
        }

        #region Helper Methods

        private XPathNavigator GetMethodNode(MethodInfo methodInfo)
        {
            if (methodInfo != null)
            {
                string selectExpression = String.Format(CultureInfo.InvariantCulture, MethodExpression, GetMemberName(methodInfo));
                return _documentNavigator.SelectSingleNode(selectExpression);
            }

            return null;
        }

        private string GetTagValue(XPathNavigator parentNode, string tagName)
        {
            if (parentNode != null)
            {
                XPathNavigator node = parentNode.SelectSingleNode(tagName);
                if (node != null)
                {
                    return node.Value.Trim();
                }
            }

            return null;
        }

        private XPathNavigator GetTypeNode(Type type)
        {
            string controllerTypeName = GetTypeName(type);
            string selectExpression = String.Format(CultureInfo.InvariantCulture, TypeExpression, controllerTypeName);
            return _documentNavigator.SelectSingleNode(selectExpression);
        }

        private string GetTypeName(Type type)
        {
            string name = type.FullName;
            if (type.IsGenericType)
            {
                // Format the generic type name to something like: Generic{System.Int32,System.String}
                Type genericType = type.GetGenericTypeDefinition();
                Type[] genericArguments = type.GetGenericArguments();
                string genericTypeName = genericType.FullName;

                // Trim the generic parameter counts from the name
                genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
                string[] argumentTypeNames = genericArguments.Select(t => GetTypeName(t)).ToArray();
                name = String.Format(CultureInfo.InvariantCulture, "{0}{{{1}}}", genericTypeName, String.Join(",", argumentTypeNames));
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                name = genericType.FullName;
            }
            if (type.IsNested)
            {
                // Changing the nested type name from OuterType+InnerType to OuterType.InnerType to match the XML documentation syntax.
                name = name.Replace("+", ".");
            }

            return name;
        }

        private string GetMemberName(MethodInfo method)
        {
            string name = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", GetTypeName(method.DeclaringType), method.Name);
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length != 0)
            {
                string[] parameterTypeNames = parameters.Select(param => GetTypeName(param.ParameterType)).ToArray();
                name += String.Format(CultureInfo.InvariantCulture, "({0})", String.Join(",", parameterTypeNames));
            }

            return name;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is XmlDocumentationProvider dp)
            {
                if (this.ApiAssembly.FullName == null)
                    throw new InvalidOperationException("XmlDocumentationProvider does not have a valid ApiAssembly");

                if (this.ApiAssembly.FullName != dp.ApiAssembly.FullName)
                    return false;

                if (this.DocumentationPath == null)
                    throw new InvalidOperationException("XmlDocumentationProvider does not have a valid DocumentationPath");

                if (this.DocumentationPath != dp.DocumentationPath)
                    return false;

                return true;
            }

            return false;
        }
    }
}
