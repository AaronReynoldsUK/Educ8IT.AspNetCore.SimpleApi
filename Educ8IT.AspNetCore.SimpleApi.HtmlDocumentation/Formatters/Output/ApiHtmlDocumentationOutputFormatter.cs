// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiDescriptions;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.Common;
using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.HtmlDocumentation;
using Educ8IT.AspNetCore.SimpleApi.HtmlDocumentation.Properties;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiHtmlDocumentationOutputFormatter : OutputFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiHtmlDocumentationOutputFormatter()
            : base("text/vnd.educ8it.api.documentation+html", false)
        { }

        /// <summary>
        /// 
        /// </summary>
        public IApiDescription ApiDescription { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IHtmlDocumentationOptions HtmlDocumentationOptions { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseObject"></param>
        /// <returns></returns>
        public override ResponseObject FormatResponse(ResponseObject responseObject)
        {
            if (responseObject == null)
                throw new ArgumentNullException(nameof(responseObject));

            if (responseObject.ActionResult.ResultObject == null)
                return responseObject;

            if (ApiDescription == null)
                ApiDescription = responseObject.HttpContext.RequestServices.GetRequiredService<IApiDescription>();

            if (HtmlDocumentationOptions == null)
            {
                var __iOptions = responseObject.HttpContext.RequestServices.GetRequiredService<IOptions<HtmlDocumentationOptions>>();
                HtmlDocumentationOptions = __iOptions.Value;
            }
            
            //var __htmlPageTemplate = Resources.ApiPageTemplate;

            if (responseObject.ActionResult.ResultObject is List<IApiControllerItem> controllers)
            {
                responseObject = FormatResponse_ListOfIApiControllers(responseObject, controllers);
            }
            else if (responseObject.ActionResult.ResultObject is IApiControllerItem controller)
            {
                responseObject = FormatResponse_IApiController(responseObject, controller);
            }
            else if (responseObject.ActionResult.ResultObject is IApiMethodItem methodItem)
            {
                responseObject = FormatResponse_IApiMethodItem(responseObject, methodItem);
            }
            else if (responseObject.ActionResult.ResultObject is TypeDescription typeDescription)
            {
                responseObject = FormatResponse_Type(responseObject, typeDescription);
            }

            //responseObject.FormattedResponseContent = __htmlPageTemplate;
            //responseObject.ContentLength = __htmlPageTemplate.Length;
            //responseObject.ContentType ??= "text/html";//this.SupportedMediaType;

            return responseObject;
        }

        private ResponseObject UpdateFinalResponseObject(ResponseObject responseObject, string htmlParts, string pageTitle)
        {
            var __htmlPageTemplate = Resources.ApiPageTemplate;

            __htmlPageTemplate = __htmlPageTemplate
                .Replace("$$PAGE_CONTENT$$", htmlParts);

            __htmlPageTemplate = __htmlPageTemplate
                .Replace("$$PAGE_TITLE$$", pageTitle)
                .Replace("$$DOCUMENTATION_HOME_URL$$", HtmlDocumentationHelper.GetRootUri(HtmlDocumentationOptions))
                .Replace("$$BRAND$$", HtmlDocumentationOptions.BrandName)
                .Replace("$$BRAND_LINK$$", HtmlDocumentationOptions.BrankLink)
                .Replace("$$COPYRIGHT_NOTICE$$", HtmlDocumentationOptions.CopyrightNotice);

            responseObject.FormattedResponseContent = __htmlPageTemplate;
            responseObject.ContentLength = __htmlPageTemplate.Length;
            responseObject.ContentType ??= "text/html";//this.SupportedMediaType;

            return responseObject;
        }

        private ResponseObject FormatResponse_ListOfIApiControllers(ResponseObject responseObject, List<IApiControllerItem> controllers)
        {
            // Home page

            // Controllers
            var __controllerTemplate = Resources.ApiControllerTemplate;
            var __controllerMethodTemplate = Resources.ApiControllerMethodTemplate;

            var __controllerSectionHtml = String.Empty;
            foreach (var __controller in controllers)
            {
                var __controllerUri = HtmlDocumentationHelper.GetContollerUri(HtmlDocumentationOptions, __controller);

                var __controllerText = __controllerTemplate
                    .Replace("$$GUIDE_CONTROLLER_URI$$", __controllerUri)
                    .Replace("$$CONTROLLER_NAME$$", __controller.Name)
                    .Replace("$$CONTROLLER_DESCRIPTION$$", __controller.Description);

                var __controllerMethods = String.Empty;

                foreach (var __method in __controller.Methods)
                {
                    if (__method.ExcludeFromDocumentation ?? false)
                        continue;

                    var __methodsAllowed = String.Join(",", __method.ActionRoutes.Select(a => a.Method).Distinct());

                    var __methodUri = HtmlDocumentationHelper.GetMethodUri(HtmlDocumentationOptions, __method);

                    var __methodItem = __controllerMethodTemplate
                        .Replace("$$GUIDE_METHOD_URI$$", __methodUri)
                        .Replace("$$HTTP_METHODS$$", __methodsAllowed)
                        .Replace("$$METHOD_NAME$$", __method.Name)
                        .Replace("$$METHOD_DESCRIPTION$$", __method.Description);

                    __controllerMethods += __methodItem;
                }

                if (!String.IsNullOrEmpty(__controllerMethods))
                {
                    __controllerText = __controllerText
                        .Replace("$$CONTROLLER_METHODS$$", __controllerMethods);

                    __controllerSectionHtml += __controllerText;
                }
            }

            return UpdateFinalResponseObject(responseObject, __controllerSectionHtml, "Home");
        }

        private ResponseObject FormatResponse_IApiController(ResponseObject responseObject, IApiControllerItem controller)
        {
            // Contoller Page
            var __controllerTemplate = Resources.ApiControllerTemplate;
            var __controllerMethodTemplate = Resources.ApiControllerMethodTemplate;

            var __controllerSectionHtml = String.Empty;
            var __controller = controller;

            var __controllerUri = HtmlDocumentationHelper.GetContollerUri(HtmlDocumentationOptions, controller);

            var __controllerText = __controllerTemplate
                .Replace("$$GUIDE_CONTROLLER_URI$$", __controllerUri)
                .Replace("$$CONTROLLER_NAME$$", __controller.Name)
                .Replace("$$CONTROLLER_DESCRIPTION$$", __controller.Description);

            var __controllerMethods = String.Empty;

            // Methods
            foreach (var __method in __controller.Methods)
            {
                if (__method.ExcludeFromDocumentation ?? false)
                    continue;

                var __methodsAllowed = String.Join(",", __method.ActionRoutes.Select(a => a.Method).Distinct());

                var __methodUri = HtmlDocumentationHelper.GetMethodUri(HtmlDocumentationOptions, __method);

                var __methodItem = __controllerMethodTemplate
                    .Replace("$$GUIDE_METHOD_URI$$", __methodUri)
                    .Replace("$$HTTP_METHODS$$", __methodsAllowed)
                    .Replace("$$METHOD_NAME$$", __method.Name)
                    .Replace("$$METHOD_DESCRIPTION$$", __method.Description);

                __controllerMethods += __methodItem;
            }

            if (!String.IsNullOrEmpty(__controllerMethods))
            {
                __controllerText = __controllerText
                    .Replace("$$CONTROLLER_METHODS$$", __controllerMethods);

                __controllerSectionHtml += __controllerText;
            }

            return UpdateFinalResponseObject(responseObject, __controllerSectionHtml, $"Controller: {__controller.Name}");
        }

        private ResponseObject FormatResponse_IApiMethodItem(ResponseObject responseObject, IApiMethodItem methodItem)
        {
            // Method page
            if (methodItem.ExcludeFromDocumentation ?? false)
                throw new CustomHttpException("Not Found", System.Net.HttpStatusCode.NotFound);

            // Method
            var __methodTemplate = Resources.ApiMethodTemplate;
                        
            var __controllerUri = HtmlDocumentationHelper.GetContollerUri(HtmlDocumentationOptions, methodItem.ApiControllerItem);

            var __methodSectionHtml = __methodTemplate
                .Replace("$$CONTOLLER_URI$$", __controllerUri)
                .Replace("$$CONTROLLER_NAME$$", methodItem.ApiControllerItem.Name)
                .Replace("$$METHOD_NAME$$", methodItem.Name)
                .Replace("$$METHOD_DESCRIPTION$$", methodItem.Description);

            if (__methodSectionHtml.Contains("$$ACTION_ROUTES$$"))
            {
                __methodSectionHtml = __methodSectionHtml
                    .Replace("$$ACTION_ROUTES$$",
                        GetActionRouteSection(methodItem));
            }

            //var __methodsAllowed = String.Join(",", methodItem.ActionRoutes.Select(a => a.Method));
            //var __methodPatterns = String.Join(",", methodItem.ActionRoutes.Select(a => a.Pattern));

            //var __methodSectionHtml = __methodTemplate
            //    .Replace("$$HTTP_METHOD$$", __methodsAllowed)
            //    .Replace("$$METHOD_PATH$$", __methodPatterns)

            // $$AUTH_POLICIES$$
            if (__methodSectionHtml.Contains("$$AUTH_POLICIES$$"))
            {
                __methodSectionHtml = __methodSectionHtml
                    .Replace("$$AUTH_POLICIES$$",
                        GetAuthPoliciesSection(methodItem));
            }


            // $$HEADER_PARAMETERS$$
            __methodSectionHtml = __methodSectionHtml
                .Replace("$$HEADER_PARAMETERS$$",
                    GetParameterSection(
                        methodItem.MethodParameters.Where(p => p.IsHeaderParameter)));

            //$$ROUTE_PARAMETERS$$
            __methodSectionHtml = __methodSectionHtml
                .Replace("$$ROUTE_PARAMETERS$$",
                    GetParameterSection(
                        methodItem.MethodParameters.Where(p => p.IsRouteParameter)));

            //$$BODY_PARAMETERS$$
            __methodSectionHtml = __methodSectionHtml
                .Replace("$$BODY_PARAMETERS$$",
                    GetParameterSection(
                        methodItem.MethodParameters.Where(p => p.IsBodyParameter)));

            //$$FORM_PARAMETERS$$
            __methodSectionHtml = __methodSectionHtml
                .Replace("$$FORM_PARAMETERS$$",
                    GetParameterSection(
                        methodItem.MethodParameters.Where(p => p.IsFormParameter)));

            //$$URI_PARAMETERS$$
            __methodSectionHtml = __methodSectionHtml
                .Replace("$$URI_PARAMETERS$$",
                    GetParameterSection(
                        methodItem.MethodParameters.Where(p => p.IsQueryParameter)));


            // $$REQUEST_FORMATS$$
            if (methodItem.AllowedRequestContentTypes != null && methodItem.AllowedRequestContentTypes.Count > 0)
            {
                __methodSectionHtml = __methodSectionHtml
                    .Replace("$$REQUEST_FORMATS$$",
                    String.Join("<br />", methodItem.AllowedRequestContentTypes));
            }
            else
            {
                __methodSectionHtml = __methodSectionHtml
                       .Replace("$$REQUEST_FORMATS$$", "<p>None</p>");
            }


            // Body Samples...
            // Output sample sets for all supported Request Types
            // [tab: mime-type] [tab: mime-type]
            // [content]


            //$$RESPONSE_TYPE$$
            var __responseTypeTable = Resources.ApiResponseTypeTable;
            var __responseTypeTableRow = Resources.ApiResponseTypeTableRow;
            var __responseTypeRowsHtml = String.Empty;
            foreach (var responseTypeSet in methodItem.ResponseTypes.OrderBy(o => o.Key))
            {
                 var __statusCode = (System.Net.HttpStatusCode) Enum.Parse(
                     typeof(System.Net.HttpStatusCode), 
                     responseTypeSet.Key.ToString());

                __responseTypeRowsHtml += __responseTypeTableRow
                    .Replace("$$CODE$$", responseTypeSet.Key.ToString())
                    .Replace("$$DESCRIPTION$$", __statusCode.ToString())
                    .Replace("$$TYPE$$", GetHtmlLink(responseTypeSet.Value));
            }
            var __responseTypeContent = (!String.IsNullOrEmpty(__responseTypeRowsHtml))
                ? __responseTypeTable.Replace("$$TABLE_ROWS$$", __responseTypeRowsHtml)
                : "<p>None</p>";

            __methodSectionHtml = __methodSectionHtml
                    .Replace("$$RESPONSE_TYPE$$", __responseTypeContent);


            // $$RESPONSE_FORMATS$$
            if (methodItem.AllowedResponseContentTypes != null && methodItem.AllowedResponseContentTypes.Count > 0)
            {
                __methodSectionHtml = __methodSectionHtml
                    .Replace("$$RESPONSE_FORMATS$$",
                    String.Join("<br />", methodItem.AllowedResponseContentTypes));
            }
            else
            {
                __methodSectionHtml = __methodSectionHtml
                       .Replace("$$RESPONSE_FORMATS$$", "<p>None</p>");
            }


            /*
             * Controller name + route
             * HttpMethod + Routes (include parent route / extract pattern) - still need to Parse correctly
             * Show Auth policies applied (if any)
             * Headers + example
             * Route params + example
             * Query (URI params) + example
             * Body / Form + example
             * 
             * Response
             * show by HttpCode
             * 200 - (clickable) type + example
             * etc
             * 
             * all types are (clickable) if Enum or Complex
             */

            return UpdateFinalResponseObject(responseObject, __methodSectionHtml, $"Method: {methodItem.Name}");
        }

        private string GetActionRouteSection(IApiMethodItem methodItem)
        {
            var __orderedActionRoutes = methodItem.ActionRoutes
                .OrderBy(o => o.Method)
                .ThenBy(o => o.Order)
                .ThenBy(o => o.ToString());

            List<string> __actionRoutesArray = new List<string>();
            foreach (var __actionRoute in __orderedActionRoutes)
            {
                if (__actionRoute.ParentRoutePrefixes != null && __actionRoute.ParentRoutePrefixes.Count > 0)
                {
                    foreach (var __parentRoutePrefix in __actionRoute.ParentRoutePrefixes)
                    {
                        var __defaults = new RouteValueDictionary()
                        {
                            { "controller", methodItem.ApiControllerItem.Name },
                            { "action", methodItem.Name }
                        };
                        var __routePattern = __actionRoute.GetRoutePattern(__parentRoutePrefix, __defaults);

                        var __pattern = __routePattern.RawText
                            .Replace("{controller}", methodItem.ApiControllerItem.Name)
                            .Replace("{action}", methodItem.Name);

                        __actionRoutesArray.Add($"({__actionRoute.Method}) {__pattern}");
                    }
                }
                else
                {
                    __actionRoutesArray.Add($"({__actionRoute.Method}) {__actionRoute.GetRoutePattern()}");
                }
            }

            return String.Join("<br />", __actionRoutesArray.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodItem"></param>
        /// <returns></returns>
        public virtual string GetAuthPoliciesSection(IApiMethodItem methodItem)
        {
            return "<p>N/A</p>";
        }

        private ResponseObject FormatResponse_Type(ResponseObject responseObject, TypeDescription typeDescription)
        {
            /*
             * Show the Type name + description
             * 
             * if an enum, show the enum values table
             * 
             * if a complex type, show the properties table
             * 
             * otherwise, show sub-type list
             * 
             */
            var __typeTemplate = Resources.ApiType;

            var __typeHtml = __typeTemplate
                .Replace("$$TYPE_NAME$$", typeDescription.Name)
                .Replace("$$TYPE_DESCRIPTION$$", typeDescription.Description);

            if (typeDescription is DictionaryDescription dictionaryDescription)
            {
                var __dictionaryTypeTable = Resources.ApiDictionaryTypeTable;

                __dictionaryTypeTable
                    .Replace("$$KEY_TYPE$$", dictionaryDescription.KeyDescription.TypeName)
                    .Replace("$$VALUE_TYPE$$", dictionaryDescription.ValueDescription.TypeName);

                if (!String.IsNullOrEmpty(__dictionaryTypeTable))
                    __typeHtml = __typeHtml.Replace("$$TYPE_PROPERTIES$$",
                        Resources.TypeTable.Replace("$$TABLE_ROWS$$", __dictionaryTypeTable));
            }
            else if (typeDescription is CollectionDescription collectionDescription)
            {
                var __innerTypeDescription = ApiDescription
                    .GetTypeDescription(collectionDescription.ElementDescription.Type);

                
            }
            else if (typeDescription is EnumTypeDescription enumTypeDescription)
            {
                var __enumTableTemplate = Resources.ApiEnumTable;
                var __enumItemValueRows = String.Empty;

                foreach (var enumValueDescription in enumTypeDescription.Values)
                {
                    __enumItemValueRows += GetEnumItemRow(enumValueDescription);
                }

                if (!String.IsNullOrEmpty(__enumItemValueRows))
                    __typeHtml = __typeHtml.Replace("$$TYPE_PROPERTIES$$",
                        __enumTableTemplate.Replace("$$TABLE_ROWS$$", __enumItemValueRows));
            }
            else if (typeDescription is ComplexTypeDescription complexTypeDescription)
            {
                var __propertyRows = String.Empty;
                foreach (var apiPropertyItem in complexTypeDescription.Properties)
                {
                    if (apiPropertyItem.PropertyInfo?.GetCustomAttributes(true).FirstOrDefault(attr => attr is ApiPropertyAttribute) is ApiPropertyAttribute __apiPropertyAttribute)
                    {
                        if (__apiPropertyAttribute.ExcludeFromDocumentation)
                            continue;

                        if ((__apiPropertyAttribute.ExcludeIfParentTypeIn?.Select(p => p.Name) ?? new string[] { }).Contains(typeDescription.Name))
                            continue;
                    }

                    __propertyRows += GetPropertyItemRow(apiPropertyItem);

                    // Check if a Collection, Enum, Complex or Simple...
                    // need to ensure these are correctly setup when processing the type in the first place
                    if (apiPropertyItem.IsComplexType)
                    { }
                }

                if(!String.IsNullOrEmpty(__propertyRows))
                    __typeHtml = __typeHtml.Replace("$$TYPE_PROPERTIES$$",
                        Resources.TypeTable.Replace("$$TABLE_ROWS$$", __propertyRows));
            }
            else
            {

            }

            return UpdateFinalResponseObject(responseObject, __typeHtml, $"Type: {typeDescription.Name}");
        }

        private string GetHtmlLink(Type type)
        {
            if (type == null)
                return String.Empty;

            var __readableName = type.GetReadableTypeName();
            var __uri = HtmlDocumentationHelper.GetTypeUri(HtmlDocumentationOptions, __readableName);

            if (type.IsDictionary())
            {
                var __keyType = type.GetDictionaryKeyType();
                var __valueType = type.GetDictionaryValueType();

                return $"Dictionary&lt;{GetHtmlLink(__keyType)},{GetHtmlLink(__valueType)}&gt;";
            }
            else if (type.IsArray)
            {
                var __valueType = type.GetNonGenericEnumeratedType();

                return $"Array&lt;{GetHtmlLink(__valueType)}&gt;";
            }
            else if (type.IsCollection())
            {
                var __valueType = type.GetGenericEnumeratedType();

                return $"List&lt;{GetHtmlLink(__valueType)}&gt;";
            }            
            else if (type.IsNullable())
            {
                var __nullableType = type.GetNullableType();

                return $"Nullable&lt;{GetHtmlLink(__nullableType)}&gt;";
            }
            else if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                Type[] genericArguments = type.GetGenericArguments();
                string genericTypeName = genericType.Name;

                // Trim the generic parameter counts from the name
                genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
                string[] argumentTypeLinks = genericArguments.Select(t => GetHtmlLink(t)).ToArray();

                return String.Format(
                    CultureInfo.InvariantCulture,
                    "{0}&lt;{1}&gt;", genericTypeName, String.Join(",", argumentTypeLinks));
            }
            else if (ApiDescription.GetTypeByName(type.FullName) != null)
            {
                return $"<a href=\"{__uri}\">{__readableName}</a>";
            }
            else
            {
                return __readableName;
            }
        }

        private string GetParameterSection(
            IEnumerable<ApiParameterItem> parameterItems)
        {
            return GetParameterTable(parameterItems)
                ?? "<p>None</p>";
        }

        private string GetParameterTable(IEnumerable<ApiParameterItem> parameterItems)
        {
            if (parameterItems == null || parameterItems.Count() == 0)
                return null;

            string __rowsHtml = String.Empty;
            foreach (var __param in parameterItems)
            {
                __rowsHtml += GetParameterRow(__param)
                    ?? String.Empty;
            }

            return Resources.TypeTable
                .Replace("$$TABLE_ROWS$$", __rowsHtml);
        }

        private string GetParameterRow(ApiParameterItem apiParameter)
        {
            var __rowHtml = Resources.TypeTableRow;

            // Potentially handle differently if a complex type or enum...

            __rowHtml = __rowHtml
                .Replace("$$PARAMETER_NAME$$", apiParameter.Name)
                .Replace("$$PARAMETER_DESCRIPTION$$", apiParameter.Description)
                .Replace("$$PARAMETER_INFO$$", String.Join("<br />", 
                    apiParameter.Annotations.Select(a => a.Documentation).ToArray()));

            __rowHtml = __rowHtml
                    .Replace("$$PARAMETER_TYPE$$", GetHtmlLink(apiParameter.Type));

            return __rowHtml;
        }

        private string GetEnumItemRow(EnumValueDescription enumValueDescription)
        {
            var __rowHtml = Resources.ApiEnumTableRow;

            __rowHtml = __rowHtml
                .Replace("$$ENUM_NAME$$", enumValueDescription.Name)
                .Replace("$$ENUM_VALUE$$", enumValueDescription.Value)
                .Replace("$$ENUM_DESCRIPTION$$", enumValueDescription.Documentation);

            return __rowHtml;
        }

        private string GetPropertyItemRow(ApiPropertyItem apiPropertyItem)
        {
            var __rowHtml = Resources.TypeTableRow;

            __rowHtml = __rowHtml
                .Replace("$$PARAMETER_NAME$$", apiPropertyItem.Name)
                .Replace("$$PARAMETER_DESCRIPTION$$", apiPropertyItem.Description)
                .Replace("$$PARAMETER_INFO$$", "")
                .Replace("$$PARAMETER_TYPE$$", GetHtmlLink(apiPropertyItem.Type));

            return __rowHtml;
        }
    }
}
