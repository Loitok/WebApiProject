using AutoMapper;
using BLL.DTOs;
using DAL.Entities;

namespace BLL.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LocationEntity, LocationDTO>();
            CreateMap<ClientEntity, ClientDTO>();
        }
    }
}
