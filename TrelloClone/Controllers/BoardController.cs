using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HashidsNet;
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
    /// Board Controller
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

        [AllowAnonymous]
        [HttpGet("getBoard/{hashId?}")]
        public ActionResult GetBoard(string hashId)
        {
            var boardKey = CardUtil.DecodeHashId(hashId);
            var board = _boardReository.GetBoard(boardKey.email, boardKey.boardSeq);
            if (board == null) {
                return BadRequest();
            }
            return Ok(board);
        }

        [HttpGet("getBoardList")]
        public ActionResult GetBoardList()
        {
            var boardList = _boardReository.GetBoardList();
            return Ok(boardList);
        }

        [HttpPost("saveBoard")]
        public ActionResult SaveBoard([FromBody]BoardDto board)
        {
            _boardReository.SaveBoard(board);
            var hashId = CardUtil.GetHashId(User.Identity.Name, board.boardSeq);
            return Ok(hashId);
        }

        [HttpPost("saveList")]
        public ActionResult SaveList([FromBody]BoardSaveListRequestDto cardList)
        {
            _boardReository.SaveList(cardList);
            return Ok();
        }

        [HttpPost("saveBoardTitle")]
        public ActionResult SaveBoardTitle([FromBody]BoardDto board)
        {
            _boardReository.SaveBoardTitle(board);
            return Ok();
        }

        [HttpPost("saveListTitle")]
        public ActionResult SaveListTitle([FromBody]BoardSaveListRequestDto cardList)
        {
            _boardReository.SaveListTitle(cardList);
            return Ok();
        }

        [HttpPost("saveCard")]
        public ActionResult SaveCard([FromBody]BoardSaveCardRequestDto card)
        {
            _boardReository.SaveCard(card);
            return Ok();
        }

        [HttpPost("saveCardContent")]
        public ActionResult SaveCardContent([FromBody]BoardSaveCardRequestDto card)
        {
            _boardReository.SaveCardContent(card);
            return Ok();
        }

        [HttpPost("deleteBoard")]
        public ActionResult DeleteBoard([FromBody]BoardDto board)
        {
            _boardReository.DeleteBoard(board);
            return Ok();
        }

        [HttpPost("deleteList")]
        public ActionResult DeleteList([FromBody]BoardSaveListRequestDto list)
        {
            _boardReository.DeleteList(list);
            return Ok();
        }

        [HttpPost("deleteCard")]
        public ActionResult DeleteCard([FromBody]BoardSaveCardRequestDto card)
        {
            _boardReository.DeleteCard(card);
            return Ok();
        }

        [HttpPost("updateListPrevSeq")]
        public ActionResult UpdateListPrevSeq([FromBody]List<BoardUpdateListPrevSeqRequestDto> updates)
        {
            _boardReository.UpdateListPrevSeq(updates);
            return Ok();
        }

        [HttpPost("updateCardPrevSeq")]
        public ActionResult UpdateCardPrevSeq([FromBody]List<BoardUpdateCardPrevSeqRequestDto> updates)
        {
            _boardReository.UpdateCardPrevSeq(updates);
            return Ok();
        }

        [HttpPost("updateIsPublic")]
        public ActionResult UpdateIsPublic([FromBody]BoardDto board)
        {
            _boardReository.UpdateIsPublic(board);
            return Ok();
        }
    }
}
