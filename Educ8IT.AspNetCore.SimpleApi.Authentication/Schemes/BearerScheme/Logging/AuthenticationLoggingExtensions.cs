// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    internal static class AuthenticationLoggingExtensions
    {
        private static Action<ILogger, string, Exception?> _authenticationSchemeSignedIn;
        private static Action<ILogger, string, Exception?> _authenticationSchemeSignedOut;

        static AuthenticationLoggingExtensions()
        {
            _authenticationSchemeSignedIn = LoggerMessage.Define<string>(
                eventId: new EventId(10, "AuthenticationSchemeSignedIn"),
                logLevel: LogLevel.Information,
                formatString: "AuthenticationScheme: {AuthenticationScheme} signed in.");

            _authenticationSchemeSignedOut = LoggerMessage.Define<string>(
                eventId: new EventId(11, "AuthenticationSchemeSignedOut"),
                logLevel: LogLevel.Information,
                formatString: "AuthenticationScheme: {AuthenticationScheme} signed out.");
        }

        public static void AuthenticationSchemeSignedIn(this ILogger logger, string authenticationScheme)
        {
            _authenticationSchemeSignedIn(logger, authenticationScheme, null);
        }

        public static void AuthenticationSchemeSignedOut(this ILogger logger, string authenticationScheme)
        {
            _authenticationSchemeSignedOut(logger, authenticationScheme, null);
        }
    }
}
