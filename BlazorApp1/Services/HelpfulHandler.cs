﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

public class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public Task HandleAsync(RequestDelegate next,
        HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult) => next(context);
}