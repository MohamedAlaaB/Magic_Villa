using AutoMapper;
using Magic_Villa_Utility;
using Magic_Villa_Web.DTOs;
using Magic_Villa_Web.Modeles;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Magic_Villa_Web.Controllers
{
    public class HomeController : Controller
    {
        private IVillaService _VillaService;
        private readonly ILogger<HomeController> _logger;
        private IMapper _mapper;
        public HomeController(ILogger<HomeController> logger, IVillaService villaService, IMapper mapper)
        {
            _logger = logger;
            this._mapper = mapper;
            this._VillaService = villaService;
        }

        public async Task<IActionResult> Index()
        {
            List<VillaDTO> list = new List<VillaDTO>();
            var response = await _VillaService.GetAllAsync<APIResponse>(token:HttpContext.Session.GetString(SD.session));
            if (  response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}