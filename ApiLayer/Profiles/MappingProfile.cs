using ApiLayer.DTOs;
using AutoMapper;
using BLL.Models;
using DAL.Entities;

namespace ApiLayer.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LocationEntity, LocationModel>();
            CreateMap<ClientEntity, ClientModel>();
            CreateMap<LocationModel, LocationDTO>();
            CreateMap<ClientModel, ClientDTO>();
        }
    }
}
