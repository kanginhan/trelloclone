using System.Net;
using TrelloClone.Dtos;
using TrelloClone.Entities;

namespace TrelloClone.Interfaces
{
    public interface IUsersRepository
    {
        bool CreateUser(UsersRegisterRequestDto user);
        USERS Authenticate(UsersLoginRequestDto user);
        USERS GetUser(string email);
    }
}