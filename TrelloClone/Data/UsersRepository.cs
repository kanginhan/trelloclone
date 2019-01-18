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
    public class UsersRepository : IUsersRepository
    {
        private DataContext _context;
        private IMapper _mapper;
        private Config _config;

        public UsersRepository(DataContext context, IMapper mapper, IOptions<Config> config)
        {
            _context = context;
            _mapper = mapper;
            _config = config.Value;
        }

        #region [Utils]
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt)) {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.ASCII.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        #endregion

        public bool CreateUser(UsersRegisterRequestDto user)
        {
            // 중복체크
            if (_context.USERS.Any(x => x.EMAIL == user.EMAIL)) {
                return false;
            }
            
            var entity = _mapper.Map<USERS>(user);
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) {
                entity.PASSWORD_SALT = hmac.Key;
                entity.PASSWORD_HASH = hmac.ComputeHash(System.Text.Encoding.ASCII.GetBytes(user.PASSWORD));
            }

            _context.USERS.Add(entity);
            _context.SaveChanges();
            return true;
        }

        public USERS Authenticate(UsersLoginRequestDto user)
        {
            var findUser = this.GetUser(user.EMAIL);

            // 계정확인
            if(findUser == null) {
                return null;
            }

            // 비밀번호 확인
            if (VerifyPasswordHash(user.PASSWORD, findUser.PASSWORD_HASH, findUser.PASSWORD_SALT) == false) {
                return null;
            }

            return findUser;
        }

        public USERS GetUser(string email)
        {
            return _context.USERS.SingleOrDefault(x => x.EMAIL == email);
        }

        
    }
}
