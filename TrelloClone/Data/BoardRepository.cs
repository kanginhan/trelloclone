using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrelloClone.Dtos;
using TrelloClone.Entities;
using TrelloClone.Infra;
using TrelloClone.Interfaces;

namespace TrelloClone.Data
{
    public class BoardRepository : IBoardRepository
    {
        private DataContext _context;
        private IMapper _mapper;
        private Config _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string UserEmail
        {
            get {
                return this._httpContextAccessor.HttpContext.User.Identity.Name;
            }
        }

        public BoardRepository(DataContext context, IMapper mapper, IOptions<Config> config, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _config = config.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public BoardDto GetBoard(string email, int? boardSeq)
        {
            var board = _context.BOARD.Find(email, boardSeq);
            if (board == null) {
                return null;
            }

            // private시 사용자 체크
            if (board.PUBLIC_TF == false && this.UserEmail != email) {
                return null;
            }

            var cardLists = from cardList in _context.CARD_LIST
                            join card in _context.CARD
                            on new { cardList.EMAIL, cardList.BOARD_SEQ, cardList.LIST_SEQ } equals new { card.EMAIL, card.BOARD_SEQ, card.LIST_SEQ } into cards
                            where cardList.EMAIL == email && cardList.BOARD_SEQ == boardSeq
                            select new CardListResultDto {
                                seq = cardList.LIST_SEQ,
                                listTitle = cardList.LIST_TITLE,
                                prevSeq = cardList.PREV_LIST,
                                cards = cards.Select(x => new CardResultDto {
                                    seq = x.CARD_SEQ,
                                    title = x.CARD_TITLE,
                                    description = x.DESCRIPTION,
                                    prevSeq = x.PREV_CARD
                                }).SetTreeOrder()
                            };

            var result = new BoardDto {
                boardSeq = board.BOARD_SEQ,
                boardTitle = board.BOARD_TITLE,
                isPublic = board.PUBLIC_TF,
                canEditing = this.UserEmail == email,
                cardLists = cardLists.SetTreeOrder()
            };

            return result;
        }

        public IEnumerable<BoardDto> GetBoardList()
        {
            return _context.BOARD.Where(x => x.EMAIL == this.UserEmail)
                .Select(x => new BoardDto {
                    hashId = CardUtil.GetHashId(this.UserEmail, x.BOARD_SEQ),
                    boardSeq = x.BOARD_SEQ,
                    boardTitle = x.BOARD_TITLE
                });
        }

        public void SaveBoard(BoardDto board)
        {
            var entity = new BOARD {
                EMAIL = this.UserEmail,
                BOARD_SEQ = board.boardSeq,
                BOARD_TITLE = board.boardTitle
            };
            _context.BOARD.Add(entity);
            _context.SaveChanges();
        }

        public void SaveBoardTitle(BoardDto board)
        {
            var findBoard = _context.BOARD.Find(this.UserEmail, board.boardSeq);
            if (findBoard != null) {
                findBoard.BOARD_TITLE = board.boardTitle;
                _context.BOARD.Update(findBoard);
                _context.SaveChanges();
            }
        }

        public void SaveList(BoardSaveListRequestDto cardList)
        {
            var entity = new CARD_LIST {
                EMAIL = this.UserEmail,
                BOARD_SEQ = cardList.boardSeq,
                LIST_SEQ = cardList.seq,
                LIST_TITLE = cardList.listTitle,
                PREV_LIST = cardList.prevSeq,
            };
            var findList = _context.CARD_LIST.Find(this.UserEmail, cardList.boardSeq, cardList.seq);
            if (findList == null) {
                _context.CARD_LIST.Add(entity);
            }
            else {
                findList.LIST_TITLE = entity.LIST_TITLE;
                findList.PREV_LIST = entity.PREV_LIST;
                _context.CARD_LIST.Update(findList);
            }

            _context.SaveChanges();
        }

        public void SaveListTitle(BoardSaveListRequestDto cardList)
        {
            var findList = _context.CARD_LIST.Find(this.UserEmail, cardList.boardSeq, cardList.seq);
            if (findList != null) {
                findList.LIST_TITLE = cardList.listTitle;
                _context.CARD_LIST.Update(findList);
                _context.SaveChanges();
            }
        }

        public void SaveCard(BoardSaveCardRequestDto card)
        {
            var entity = new CARD {
                EMAIL = this.UserEmail,
                BOARD_SEQ = card.boardSeq,
                LIST_SEQ = card.listSeq,
                CARD_SEQ = card.seq,
                CARD_TITLE = card.title,
                DESCRIPTION = card.description,
                PREV_CARD = card.prevSeq
            };
            var findCard = _context.CARD.Find(this.UserEmail, entity.BOARD_SEQ, entity.LIST_SEQ, entity.CARD_SEQ);
            if (findCard == null) {
                _context.CARD.Add(entity);
            }
            else {
                findCard.CARD_TITLE = entity.CARD_TITLE;
                findCard.DESCRIPTION = entity.DESCRIPTION;
                findCard.PREV_CARD = entity.PREV_CARD;
                _context.CARD.Update(findCard);
            }
            _context.SaveChanges();
        }

        public void SaveCardContent(BoardSaveCardRequestDto card)
        {
            var findCard = _context.CARD.Find(this.UserEmail, card.boardSeq, card.listSeq, card.seq);
            if (findCard != null) {
                findCard.CARD_TITLE = card.title;
                findCard.DESCRIPTION = card.description;
                _context.CARD.Update(findCard);
                _context.SaveChanges();
            }

        }

        public void DeleteBoard(BoardDto board)
        {
            _context.CARD.RemoveRange(_context.CARD.Where(x => x.EMAIL == this.UserEmail && x.BOARD_SEQ == board.boardSeq));
            _context.CARD_LIST.RemoveRange(_context.CARD_LIST.Where(x => x.EMAIL == this.UserEmail && x.BOARD_SEQ == board.boardSeq));
            _context.BOARD.Remove(_context.BOARD.Find(this.UserEmail, board.boardSeq));
            _context.SaveChanges();
        }

        public void DeleteList(BoardSaveListRequestDto list)
        {
            _context.CARD.RemoveRange(_context.CARD.Where(x => x.EMAIL == this.UserEmail && x.BOARD_SEQ == list.boardSeq && x.LIST_SEQ == list.seq));
            _context.CARD_LIST.Remove(_context.CARD_LIST.Find(this.UserEmail, list.boardSeq, list.seq));
            _context.SaveChanges();
        }

        public void DeleteCard(BoardSaveCardRequestDto card)
        {
            _context.CARD.Remove(_context.CARD.Find(this.UserEmail, card.boardSeq, card.listSeq, card.seq));
            _context.SaveChanges();
        }

        public void UpdateListPrevSeq(List<BoardUpdateListPrevSeqRequestDto> updates)
        {
            updates.ForEach(x => {
                var findList = _context.CARD_LIST.Find(this.UserEmail, x.boardSeq, x.seq);
                if (findList != null) {
                    findList.PREV_LIST = x.prevSeq;
                }
                _context.CARD_LIST.Update(findList);
            });
            _context.SaveChanges();
        }

        public void UpdateCardPrevSeq(List<BoardUpdateCardPrevSeqRequestDto> updates)
        {
            updates.ForEach(x => {
                _context.Database.ExecuteSqlCommand($"UPDATE CARD SET LIST_SEQ = {x.listSeq}, CARD_SEQ = {x.seq}, PREV_CARD = {x.prevSeq} WHERE EMAIL = {this.UserEmail} AND BOARD_SEQ = {x.boardSeq} AND LIST_SEQ = {x.fromListSeq ?? x.listSeq} AND CARD_SEQ = {x.fromSeq ?? x.seq};");
            });
        }

        public void UpdateIsPublic(BoardDto board)
        {
            var findBoard = _context.BOARD.Find(this.UserEmail, board.boardSeq);
            if (findBoard != null) {
                findBoard.PUBLIC_TF = board.isPublic;
                _context.BOARD.Update(findBoard);
                _context.SaveChanges();
            }
        }
    }
}
