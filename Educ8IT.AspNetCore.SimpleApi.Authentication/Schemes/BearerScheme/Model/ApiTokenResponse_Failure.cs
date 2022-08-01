// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Newtonsoft.Json;
using System;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme.Model
{
    /// <summary>
    /// Returned when authentication isn't successful, token expires, invalid token used etc. 
    /// Should be in JSON but will return in the expected format (determined from "format" querystring parameter or Accept Header or request content type). 
    /// Refer to https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/ for further details.
    /// </summary>
    [JsonObject("Failure response")]
    [Obsolete()]
    public class ApiTokenResponse_Failure
    {
        /// <summary>
        /// specific error type
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>
        /// Circumstance of the error
        /// </summary>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Optional Uri to Help Documentation
        /// </summary>
        [JsonProperty("error_uri")]
        public string ErrorUri { get; set; }

        /// <summary>
        /// Error-specific HTTP response code
        /// </summary>
        [JsonIgnore()]
        public int HttpErrorCode { get; set; } = 400;


        /// <summary>
        /// The request is missing a parameter so the server can’t proceed with the request. 
        /// This may also be returned if the request includes an unsupported parameter or repeats a parameter.
        /// </summary>
        public const string Error_InvalidRequest = "invalid_request";

        /// <summary>
        /// Client authentication failed, such as if the request contains an invalid client ID or secret. 
        /// Send an HTTP 401 response in this case.
        /// </summary>
        public const string Error_InvalidClient = "invalid_client"; // 401

        /// <summary>
        /// The authorization code (or user’s password for the password grant type) is invalid or expired. 
        /// This is also the error you would return if the redirect URL given in the authorization grant does not match the URL provided in this access token request.
        /// </summary>
        public const string Error_InvalidGrant = "invalid_grant";

        /// <summary>
        /// For access token requests that include a scope (password or client_credentials grants), this error indicates an invalid scope value in the request.
        /// </summary>
        public const string Error_InvalidScope = "invalid_scope";

        /// <summary>
        /// This client is not authorized to use the requested grant type. 
        /// For example, if you restrict which applications can use the Implicit grant, you would return this error for the other apps.
        /// </summary>
        public const string Error_UnauthorisedClient = "unauthorized_client";

        /// <summary>
        /// If a grant type is requested that the authorization server doesn’t recognize, use this code. 
        /// Note that unknown grant types also use this specific error code rather than using the invalid_request above.
        /// </summary>
        public const string Error_UnsupportedGrantType = "unsupported_grant_type";
    }
}
