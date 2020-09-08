using AutoMapper;
using ServiceCollectionCoreApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCollectionCoreApp.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>();
        }
    }
}
