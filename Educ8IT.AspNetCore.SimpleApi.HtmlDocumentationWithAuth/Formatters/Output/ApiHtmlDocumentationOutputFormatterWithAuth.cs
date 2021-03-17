// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authorisation.TypeDescriptions;
using Educ8IT.AspNetCore.SimpleApi.Formatters;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.HtmlDocumentationWithAuth.Formatters.Output
{
    public class ApiHtmlDocumentationOutputFormatterWithAuth : ApiHtmlDocumentationOutputFormatter
    {
        public ApiHtmlDocumentationOutputFormatterWithAuth()
            : base()
        {

        }

        public override string GetAuthPoliciesSection(IApiMethodItem methodItem)
        {
            if (methodItem is AuthorisationAwareApiMethodItem authorisationAwareApiMethodItem)
            {
                string __html = String.Empty;
                var __authAttr = authorisationAwareApiMethodItem.AuthoriseAttribute;

                if (__authAttr != null)
                {
                    var __policiesHtml = (__authAttr.Policies != null && __authAttr.Policies.Count > 0)
                        ? String.Join(", ", __authAttr.Policies.ToArray())
                        : "None";
                    
                    __html += $"Must pass these policies: {__policiesHtml}<br />";

                    var __requiredRolesHtml = (__authAttr.RequiredRoles != null && __authAttr.RequiredRoles.Count > 0)
                        ? String.Join(", ", __authAttr.RequiredRoles.ToArray())
                        : "None";

                    __html += $"User must have all these roles: {__requiredRolesHtml}<br />";

                    var __roles = (__authAttr.Roles != null && __authAttr.Roles.Count > 0)
                        ? String.Join(", ", __authAttr.Roles.ToArray())
                        : "None";

                    __html += $"User must at least one of these roles: {__roles}<br />";

                    return __html;
                }
            }
            
            return base.GetAuthPoliciesSection(methodItem);
        }
    }
}
