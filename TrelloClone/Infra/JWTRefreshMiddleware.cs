using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrelloClone.Infra.Extensions;
using TrelloClone.Interfaces;

namespace TrelloClone.Infra
{
    public class JWTRefreshMiddleware
    {
        private readonly RequestDelegate _next;
        private Config _config;

        public JWTRefreshMiddleware(RequestDelegate next, IOptions<Config> config)
        {
            _next = next;
            _config = config.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated) {
                long expirelong = 0;
                long.TryParse(context.User.FindFirst("exp").Value, out expirelong);

                if (expirelong > 0) {
                    var expireDate = expirelong.FromUnixTime();
                    var nowDate = DateTime.UtcNow;

                    // 토큰 재발급 (만료기간 절반이내로 남은경우)
                    if (expireDate > nowDate && expireDate < nowDate.AddMinutes(_config.TokenExpire * 0.5)) {
                        context.Response.Cookies.AppendToken(_config, context.User.Identity.Name, context);
                    }
                }
            }
            
            await _next.Invoke(context);
        }
    }
}
