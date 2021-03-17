// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authorization;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// With the requirement, the Principle is checked for roles membership
    /// </summary>
    public class RoleRequirement : IAuthorizationRequirement
    {

    }
}
