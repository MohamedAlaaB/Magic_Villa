using AutoMapper;
using Magic_Villa_Api.DTOs;
using Magic_Villa_Api.Modeles;
using Magic_Villa_Api.Modeles.DTOs;

namespace Magic_Villa_Api
{
    public class MappingConfig :Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDTO, Villa>();
            CreateMap<Villa, VillaDTO>();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap(); 
            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<AppUser,UserDto>().ReverseMap();

        }
    }
}
