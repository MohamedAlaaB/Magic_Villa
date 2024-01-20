using AutoMapper;
using Magic_Villa_Utility;
using Magic_Villa_Web.DTOs;
using Magic_Villa_Web.Modeles;
using Magic_Villa_Web.Modeles.DTOs;
using Magic_Villa_Web.Models.ViewModels;
using Magic_Villa_Web.Services;
using Magic_Villa_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Magic_Villa_Web.Controllers
{
    
    public class VillaNumberController : Controller
    {
        private IVillaNumberService _VillaNumberService;
        private IVillaService villaService;
        private IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            this._mapper = mapper;
            this._VillaNumberService = villaNumberService;
            this.villaService = villaService;

        }
        // GET: VillaController
        public async Task<IActionResult> Index()
        {
            List<VillaNumberDTO> list = new List<VillaNumberDTO>();
            var response = await _VillaNumberService.GetAllAsync<APIResponse>(token: HttpContext.Session.GetString(SD.session));
            if (response.IsSuccess && response != null)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        // GET: VillaController/Details/5


        // GET: VillaController/Create
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            var response = await villaService.GetAllAsync<APIResponse>(token: HttpContext.Session.GetString(SD.session));

            var villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            var villaNames = villas.ConvertAll(x =>
            {
                return new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = false
                };
            });
            var villanumberVm = new VillaNumberVm { VillasNames = villaNames };
            return View(villanumberVm);
            
        }

        // POST: VillaController/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( VillaNumberVm villanumberVm)
        {
            var vresponse = await villaService.GetAllAsync<APIResponse>(token: HttpContext.Session.GetString(SD.session));

            var villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(vresponse.Result));
            var villcdto = villanumberVm.VillaNumberCreateDTO;
            var villaNames = villas.ConvertAll(x =>
            {
                return new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = false
                };
            });
            villanumberVm.VillasNames = villaNames;
            if (ModelState.IsValid)
            {
                var response = await _VillaNumberService.CreateAsync<APIResponse>(villcdto, token: HttpContext.Session.GetString(SD.session));
                if (response.IsSuccess & response != null)
                {
                    TempData["Success"] = "Created Successfully";
                    return RedirectToAction(nameof(Index));

                }
                else if (response.Errors.Count > 0 )
                {
                    ModelState.AddModelError("error message", response.Errors.FirstOrDefault());
                    TempData["Error"] = response.Errors.FirstOrDefault();
                }                        
            }
            return View(villanumberVm);

        }

        // GET: VillaController/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update(int villaNo)
        {
            var res = await _VillaNumberService.GetAsync<APIResponse>(villaNo, token: HttpContext.Session.GetString(SD.session));
            var villaUpdateDTO = _mapper.Map<VillaNumberUpdateDTO>( JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(res.Result)));
            var response = await villaService.GetAllAsync<APIResponse>(token: HttpContext.Session.GetString(SD.session));

            var villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            var villaNames = villas.ConvertAll(x =>
            {
                return new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = false
                };
            });
            var villanumberVm = new VillaNumberVm { VillasNames = villaNames , VillaNumberUpdateDTO = villaUpdateDTO };
            return View(villanumberVm);
        }

        // POST: VillaController/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost(Name ="Updatepost")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(VillaNumberVm villanumberVm)
        {
            var oldvillatoupdate = villanumberVm.VillaNumberUpdateDTO;
            //Console.WriteLine(updated.ToString());
            var res = await _VillaNumberService.UpdateAsync<APIResponse>(oldvillatoupdate, token: HttpContext.Session.GetString(SD.session));
            if (oldvillatoupdate != null )
            {
                
                
                TempData["Success"] = "Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] =res.Errors.FirstOrDefault() ;
            return View(oldvillatoupdate);
        }

        //GET: VillaController/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int villaNo)
        {
            var res = await _VillaNumberService.GetAsync<APIResponse>(villaNo, token: HttpContext.Session.GetString(SD.session));
            var villaUpdateDTO = _mapper.Map<VillaNumberUpdateDTO>(JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(res.Result)));
            var response = await villaService.GetAllAsync<APIResponse>(token: HttpContext.Session.GetString(SD.session));

            var villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            var villaNames = villas.ConvertAll(x =>
            {
                return new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = false
                };
            });
            var villanumberVm = new VillaNumberVm { VillasNames = villaNames, VillaNumberUpdateDTO = villaUpdateDTO };
            return View(villanumberVm);
        }

        //POST: VillaController/Delete/5

        [Authorize(Roles = "admin")]
        [HttpPost(Name = "Deletepost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(VillaNumberVm villanumberVm)
        {
            var response = await _VillaNumberService.DeleteAsync<APIResponse>(villanumberVm.VillaNumberUpdateDTO.VillaNo , token: HttpContext.Session.GetString(SD.session));
            if (response.IsSuccess && response != null)
            {
                TempData["Success"] = "Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = response.Errors.FirstOrDefault();
            return View(villanumberVm);
        }
    }
}
