using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrelloClone.Infra
{
    public class JWTInHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private Config _config;

        public JWTInHeaderMiddleware(RequestDelegate next, IOptions<Config> config)
        {
            _next = next;
            _config = config.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            // refreshToken 도 발급하도록 작업해야해
            var cookie = context.Request.Cookies[_config.TokenCookie];

            if (cookie != null) {
                if (context.Request.Headers.ContainsKey("Authorization") == false) {
                    context.Request.Headers.Append("Authorization", "Bearer " + cookie);
                }
            }

            await _next.Invoke(context);
        }
    }
}
