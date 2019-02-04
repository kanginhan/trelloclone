using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using TrelloClone.Dtos;
using TrelloClone.Entities;

namespace TrelloClone.Interfaces
{
    public interface IBoardRepository
    {
        BoardDto GetBoard(string email, int? boardSeq);
        void SaveList(BoardSaveListRequestDto cardList);
        void SaveBoardTitle(BoardDto board);
        void SaveCard(BoardSaveCardRequestDto card);
        void DeleteList(BoardSaveListRequestDto list);
        void DeleteCard(BoardSaveCardRequestDto card);
        void UpdateListPrevSeq(List<BoardUpdateListPrevSeqRequestDto> updates);
        void UpdateCardPrevSeq(List<BoardUpdateCardPrevSeqRequestDto> updates);
        void SaveListTitle(BoardSaveListRequestDto cardList);
        void SaveCardContent(BoardSaveCardRequestDto card);
        IEnumerable<BoardDto> GetBoardList();
        void DeleteBoard(BoardDto board);
        void SaveBoard(BoardDto board);
        void UpdateIsPublic(BoardDto board);
    }
}