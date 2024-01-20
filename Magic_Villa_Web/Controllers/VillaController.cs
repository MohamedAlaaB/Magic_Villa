using AutoMapper;
using Magic_Villa_Utility;
using Magic_Villa_Web.DTOs;
using Magic_Villa_Web.Modeles;
using Magic_Villa_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Magic_Villa_Web.Controllers
{
    
    public class VillaController : Controller
    {
        private IVillaService _VillaService;
        private IMapper _mapper;

        public VillaController( IVillaService villaService , IMapper mapper)
        {
            this._mapper = mapper;
            this._VillaService = villaService;
        }
        // GET: VillaController
        public async Task<IActionResult> Index()
        {
            List<VillaDTO> list = new List<VillaDTO>();
            var response = await _VillaService.GetAllAsync<APIResponse>(token: HttpContext.Session.GetString(SD.session));
            if (response.IsSuccess && response != null)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        // GET: VillaController/Details/5


        // GET: VillaController/Create
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Create()
        {
            //var response = await _VillaService.GetAsync<APIResponse>(id);
            //var villaCreateDTO = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            //return View(villaCreateDTO);
            return View();
        }

        // POST: VillaController/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( VillaCreateDTO villaDTO)
        {
            var response = await _VillaService.CreateAsync<APIResponse>(villaDTO, token: HttpContext.Session.GetString(SD.session));
            if (ModelState.IsValid)
            {
               
                if (response.IsSuccess & response != null)
                {
                    TempData["Success"] = "Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["Error"] = response.Errors.FirstOrDefault();
            return View(villaDTO);

        }

        // GET: VillaController/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update(int villaId)
        {
            var res = await _VillaService.GetAsync<APIResponse>(villaId, token: HttpContext.Session.GetString(SD.session));
            var villaUpdateDTO = _mapper.Map<VillaUpdateDTO>( JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(res.Result)));
            return View(villaUpdateDTO);
        }

        // POST: VillaController/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost(Name ="Updatepost")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(VillaUpdateDTO oldvillatoupdate )
        {

            //Console.WriteLine(updated.ToString());
            var res = await _VillaService.UpdateAsync<APIResponse>(oldvillatoupdate, token: HttpContext.Session.GetString(SD.session));
            if (oldvillatoupdate != null )
            {
                
                TempData["Success"] = "Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = res.Errors.FirstOrDefault();
            return View(oldvillatoupdate);
        }

        //GET: VillaController/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int villaId)
        {
            var res = await _VillaService.GetAsync<APIResponse>(villaId, token: HttpContext.Session.GetString(SD.session));
            var villaDTO = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(res.Result));
            return View(villaDTO);
        }

        //POST: VillaController/Delete/5

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(VillaDTO villaDTO)
        {
            var response = await _VillaService.DeleteAsync<APIResponse>(villaDTO.Id, token: HttpContext.Session.GetString(SD.session));
            if (response.IsSuccess && response !=null)
            {
                TempData["Success"] = "Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = response.Errors.FirstOrDefault();
            return View(villaDTO);
        }
    }
}
