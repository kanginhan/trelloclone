using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public BoardRepository(DataContext context, IMapper mapper, IOptions<Config> config)
        {
            _context = context;
            _mapper = mapper;
            _config = config.Value;
        }

        public BoardDto GetBoard(string email, int boardSeq)
        {
            var board = _context.BOARD.Find(email, boardSeq);
            if (board == null) {
                board = this.GetNewBoard(email);
            }

            var cardLists = from cardList in _context.CARD_LIST
                            join card in _context.CARD
                            on new { cardList.EMAIL, cardList.BOARD_SEQ, cardList.LIST_SEQ } equals new { card.EMAIL, card.BOARD_SEQ, card.LIST_SEQ } into cards
                            where cardList.EMAIL == email && cardList.BOARD_SEQ == boardSeq
                            select new CardListResultDto {
                                listSeq = cardList.LIST_SEQ,
                                listTitle = cardList.LIST_TITLE,
                                cards = cards.Select(x => new CardResultDto {
                                    cardSeq = x.CARD_SEQ,
                                    title = x.CARD_TITLE,
                                    description = x.DESCRIPTION
                                })
                            };

            var result = new BoardDto {
                boardSeq = board.BOARD_SEQ,
                boardTitle = board.BOARD_TITLE,
                cardLists = cardLists
            };

            return result;
        }

        public BOARD GetNewBoard(string email, string boardTitle = "Board Title")
        {
            var maxSeq = _context.BOARD.Count() > 0 ? _context.BOARD.Max(x => x.BOARD_SEQ) : -1;
            var entity = new BOARD {
                BOARD_SEQ = maxSeq + 1,
                EMAIL = email,
                BOARD_TITLE = boardTitle
            };
            _context.BOARD.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public void SaveBoardTitle(string email, BoardDto board)
        {
            var findBoard = _context.BOARD.Find(email, board.boardSeq);
            if (findBoard != null) {
                findBoard.BOARD_TITLE = board.boardTitle;
                _context.BOARD.Update(findBoard);
                _context.SaveChanges();
            }
        }

        public void SaveList(string email, BoardSaveListRequestDto cardList)
        {
            var entity = new CARD_LIST {
                EMAIL = email,
                BOARD_SEQ = cardList.boardSeq,
                LIST_SEQ = cardList.listSeq,
                LIST_TITLE = cardList.listTitle
            };
            var findList = _context.CARD_LIST.Find(email, cardList.boardSeq, cardList.listSeq);
            if (findList == null) {
                _context.CARD_LIST.Add(entity);
            }
            else {
                findList.LIST_TITLE = entity.LIST_TITLE;
                _context.CARD_LIST.Update(findList);
            }

            _context.SaveChanges();
        }

        public void SaveCard(string email, BoardSaveCardRequestDto card)
        {
            var entity = new CARD {
                EMAIL = email,
                BOARD_SEQ = card.boardSeq,
                LIST_SEQ = card.listSeq,
                CARD_SEQ = card.cardSeq,
                CARD_TITLE = card.title,
                DESCRIPTION = card.description
            };
            var findCard = _context.CARD.Find(email, entity.BOARD_SEQ, entity.LIST_SEQ, entity.CARD_SEQ);
            if(findCard == null) {
                _context.CARD.Add(entity);
            }
            else {
                findCard.CARD_TITLE = entity.CARD_TITLE;
                findCard.DESCRIPTION = entity.DESCRIPTION;
                _context.CARD.Update(findCard);
            }
            _context.SaveChanges();
        }

        public void DeleteList(string email, BoardSaveListRequestDto list)
        {
            _context.CARD.RemoveRange(_context.CARD.Where(x => x.EMAIL == email && x.BOARD_SEQ == list.boardSeq && x.LIST_SEQ == list.listSeq));
            _context.CARD_LIST.Remove(_context.CARD_LIST.Find(email, list.boardSeq, list.listSeq));
            _context.SaveChanges();
        }

        public void DeleteCard(string email, BoardSaveCardRequestDto card)
        {
            _context.CARD.Remove(_context.CARD.Find(email, card.boardSeq, card.listSeq, card.cardSeq));
            _context.SaveChanges();
        }
    }
}
