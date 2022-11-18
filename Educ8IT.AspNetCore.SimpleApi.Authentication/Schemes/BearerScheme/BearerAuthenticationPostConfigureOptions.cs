// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.Extensions.Options;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class BearerAuthenticationPostConfigureOptions : IPostConfigureOptions<BearerAuthenticationOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void PostConfigure(string name, BearerAuthenticationOptions options)
        {

        }
    }
}
