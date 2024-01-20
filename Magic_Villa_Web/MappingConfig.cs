using AutoMapper;
using Magic_Villa_Web.DTOs;
using Magic_Villa_Web.Modeles;
using Magic_Villa_Web.Modeles.DTOs;

namespace Magic_Villa_Web
{
    public class MappingConfig :Profile
    {
        public MappingConfig()
        {
          
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
          

        }
    }
}
