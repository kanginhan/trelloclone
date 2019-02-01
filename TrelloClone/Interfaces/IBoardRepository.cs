using System.Net;
using TrelloClone.Dtos;
using TrelloClone.Entities;

namespace TrelloClone.Interfaces
{
    public interface IBoardRepository
    {
        BoardDto GetBoard(string email, int boardSeq);
        void SaveList(string email, BoardSaveListRequestDto cardList);
        void SaveBoardTitle(string email, BoardDto board);
        void SaveCard(string email, BoardSaveCardRequestDto card);
        void DeleteList(string email, BoardSaveListRequestDto list);
        void DeleteCard(string email, BoardSaveCardRequestDto card);
    }
}