using Magic_Villa_Web.Modeles.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Magic_Villa_Web.Models.ViewModels
{
    public class VillaNumberVm
    {
        public VillaNumberCreateDTO? VillaNumberCreateDTO { get; set; }
        public VillaNumberUpdateDTO? VillaNumberUpdateDTO { get; set; }
        public List<SelectListItem> VillasNames = new List<SelectListItem>();
    }
}