using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Dtos;
using TrelloClone.Entities;

namespace TrelloClone.Infra
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<USERS, UsersRegisterRequestDto>().ReverseMap();
        }
    }
}
