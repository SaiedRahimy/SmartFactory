using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Infrastructure.Hubs
{
    public class SignalRAuthorization : IAuthorizationRequirement
    {
    }

    public class SignalrAuthorizationHandler : AuthorizationHandler<SignalRAuthorization>
    {
        readonly IHttpContextAccessor _httpContextAccessor;



        public SignalrAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;


        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SignalRAuthorization requirement)
        {
            var _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var token);

            if (string.IsNullOrEmpty(token))
            {
                token = _httpContextAccessor.HttpContext.Request.QueryString.Value;
            }

            if (!string.IsNullOrEmpty(token))
            {
                var jsonToken = _jwtSecurityTokenHandler.ReadToken(token.ToString().Replace("Bearer ", ""));
                var tokenS = jsonToken as JwtSecurityToken;
                var userId = tokenS.Claims.FirstOrDefault(claim => claim.Type == "sub").Value;
                var clientId = tokenS.Claims.FirstOrDefault(claim => claim.Type == "client_id").Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }


            await Task.CompletedTask;
        }
    }
}
