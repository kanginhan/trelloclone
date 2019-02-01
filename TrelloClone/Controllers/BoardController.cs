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
    public class BoardController : TCControllerBase
    {
        private IBoardRepository _boardReository;

        #region [Creator]
        public BoardController(IMapper mapper, ILogger<BoardController> logger, IOptions<Config> config, IBoardRepository boardReository)
            : base(mapper, logger, config)
        {
            _boardReository = boardReository;
        }
        #endregion

        [HttpGet("getBoard/{boardSeq?}")]
        public ActionResult GetBoard(int? boardSeq)
        {
            var board = _boardReository.GetBoard(User.Identity.Name, boardSeq??0);
            return Ok(board);
        }

        [HttpPost("saveList")]
        public ActionResult SaveList([FromBody]BoardSaveListRequestDto cardList)
        {
            _boardReository.SaveList(User.Identity.Name, cardList);
            return Ok();
        }

        [HttpPost("saveBoardTitle")]
        public ActionResult SaveBoardTitle([FromBody]BoardDto board)
        {
            _boardReository.SaveBoardTitle(User.Identity.Name, board);
            return Ok();
        }

        [HttpPost("saveCard")]
        public ActionResult SaveCard ([FromBody]BoardSaveCardRequestDto card)
        {
            _boardReository.SaveCard(User.Identity.Name, card);
            return Ok();
        }

        [HttpPost("deleteList")]
        public ActionResult DeleteList([FromBody]BoardSaveListRequestDto list)
        {
            _boardReository.DeleteList(User.Identity.Name, list);
            return Ok();
        }

        [HttpPost("deleteCard")]
        public ActionResult DeleteCard([FromBody]BoardSaveCardRequestDto card)
        {
            _boardReository.DeleteCard(User.Identity.Name, card);
            return Ok();
        }
    }
}
