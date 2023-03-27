using AutoMapper;
using PadelApp.Model;
using PadelApp.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelApp.Models.Mapper
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<Session, SessionDto>();
            CreateMap<SessionDto, Session>();
        }
    }
}
