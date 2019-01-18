using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TrelloClone.Dtos;
using TrelloClone.Entities;
using TrelloClone.Infra;
using TrelloClone.Infra.Extensions;
using TrelloClone.Interfaces;

namespace TrelloClone.Controllers
{
    /// <summary>
    /// 권한 Controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : TCControllerBase
    {
        private IUsersRepository _usersRepository;

        #region [Creator]
        public AuthController(IMapper mapper, ILogger<AuthController> logger, IOptions<Config> config, IUsersRepository usersRepository)
            : base(mapper, logger, config)
        {
            _usersRepository = usersRepository;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody]UsersRegisterRequestDto user)
        {
            if (user == null) {
                return BadRequest();
            }
            if (ModelState.IsValid == false) {
                return BadRequest();
            }

            if (_usersRepository.CreateUser(user) == false) {
                return StatusCode(500);
            }

            // 토큰발급
            Response.Cookies.AppendToken(_config, user.EMAIL, HttpContext);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody]UsersLoginRequestDto user)
        {
            if (user == null) {
                return BadRequest();
            }
            if (ModelState.IsValid == false) {
                return BadRequest();
            }

            // 계정확인
            var findUser = _usersRepository.Authenticate(user);
            if(findUser == null) {
                return BadRequest();
            }

            // 토큰발급
            Response.Cookies.AppendToken(_config, findUser.EMAIL, HttpContext);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("duplication")]
        public ActionResult CheckDuplication([FromBody]UsersDuplicateRequestDto user)
        {
            if (user == null) {
                return BadRequest();
            }
            if (ModelState.IsValid == false) {
                return BadRequest();
            }
            
            return Ok(_usersRepository.GetUser(user.EMAIL)?.EMAIL);
        }

        [HttpGet("checkToken")]
        public ActionResult CheckToken()
        {
            return Ok();
        }

        [HttpGet("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete(_config.TokenCookie, new CookieOptions { Path = "/" });
            return Ok();
        }

        

    }
}
