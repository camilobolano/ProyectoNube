using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using bibliotecaModels.Dto;
using bibliotecaModels.Entity;

namespace bibliotecaAPI.services
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<books,booksDto>().ReverseMap();
            CreateMap<users,usersDto>().ReverseMap();
            CreateMap<loan,loanDto>().ReverseMap();
        }   
    }
}