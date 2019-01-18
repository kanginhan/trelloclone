using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrelloClone.Infra
{
    public class Config
    {
        public string JWTSecurityKey { get; set; }
        public string TokenCookie { get; set; }
        public string IPClaimType { get; set; }
        public double TokenExpire { get; set; }
    }
}
