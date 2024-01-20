using AutoMapper;
using Magic_Villa_Api.Data;
using Magic_Villa_Api.DTOs;
using Magic_Villa_Api.Migrations;
using Magic_Villa_Api.Modeles;
using Magic_Villa_Api.Modeles.DTOs;
using Magic_Villa_Api.Repo.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Magic_Villa_Api.Controllers
{
    [Route("api/v{version:apiversion}/VillaNumber")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaNumberController : Controller
    {
        //private readonly applicationDbContext _applicationDbContext;
        private readonly IVillaNumber _context;
        private readonly IVillaRepo _villa;
        private ILogger<VillaNumberController> logger;
        private IMapper _mapper;
        protected APIResponse _apiResponse;

        public VillaNumberController(IVillaNumber context, ILogger<VillaNumberController> _logger, IMapper mapper, IVillaRepo villa/*, applicationDbContext applicationDbContext*/)
        {
            _context = context;
            logger = _logger;
            _mapper = mapper;
            _apiResponse = new();
            _villa = villa;
            //_applicationDbContext = applicationDbContext;
        }
        // GET: VillaNumberController
        [HttpGet]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {

                IEnumerable<VillaNumber> all = await _context.GetAll(includeprops: "Villa");
               
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = _mapper.Map<List<VillaNumberDTO>>(all);
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };

            }
            return _apiResponse;

        }
        [HttpGet("{id:int}", Name = "GetvillaNumber")]
        [ResponseCache(CacheProfileName = "Defualt30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {

                    logger.LogError("id can't be zero");
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return Ok(_apiResponse);

                }
                var villa = await _context.Get(x => x.VillaNo == id ,includeprops: "Villa");
                if (villa == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return Ok(_apiResponse);
                }
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = _mapper.Map<VillaNumberDTO>(villa);
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };

            }
            return _apiResponse;

        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNum)
        {
            try
            {
                if (await _context.Get(x => x.VillaNo == villaNum.VillaNo) != null)
                {
                    ModelState.AddModelError("Errors", "please submit unique Number");

                    return BadRequest(ModelState);
                }
                if(await _villa.Get(x=>x.Id== villaNum.VillaID) == null)
                {
                    ModelState.AddModelError("Errors", "villa id not vallid");
                    return BadRequest(ModelState);
                }
                var model = _mapper.Map<VillaNumber>(villaNum);
                await _context.Add(model);
                await _context.Save();
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Result = _mapper.Map<VillaNumberCreateDTO>(villaNum);

                return CreatedAtRoute("GetvillaNumber",
                    new { id = villaNum.VillaNo }
                    , _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };

            }
            return _apiResponse;

        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}", Name = "DeletevillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {

            try
            {
                if (id == null || id == 0)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return Ok(_apiResponse);

                }
                var thevilla = await _context.Get(x => x.VillaNo == id);
                if (thevilla == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return Ok(_apiResponse);
                }
                await _context.Remove(thevilla);
                await _context.Save();
                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                return Ok(_apiResponse);

            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };

            }
            return _apiResponse;
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaNumberUpdateDTO villa)
        {



            try
            {
                if (id == 0 )
                {
                    return BadRequest();
                }
                //var villafromdb = await _context.Get(x => x.VillaNo== id);
                //if (villafromdb == null)
                //{
                //    return BadRequest();
                //}
                var model = _mapper.Map<VillaNumber>(villa);
                await _context.Update(model);
                await _context.Save();
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };

            }
            return _apiResponse;

        }
        [Authorize(Roles = "admin")]
        [HttpPatch("{id:int}", Name = "patchvillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateVillapatch(int id, JsonPatchDocument<VillaNumberUpdateDTO> patch)
        {

            if (id == 0 || id == null)
            {
                return BadRequest();
            }
            var villafromdb = await _context.Get(x => x.VillaNo == id);
            if (villafromdb == null)
            {
                ModelState.AddModelError("Errors", "Villa ID is Invalid!");
                return BadRequest(ModelState);
            }
            var vdto = _mapper.Map<VillaNumberUpdateDTO>(villafromdb);
            //VillaUpdateDTO vdto = new VillaUpdateDTO
            //{
            //    Amenity = villafromdb.Amenity,
            //    Details = villafromdb.Details,
            //    Id  = villafromdb.Id,
            //    Name = villafromdb.Name,
            //    ImageUrl = villafromdb.ImageUrl,
            //    Occupancy = villafromdb.Occupancy,
            //    Rate = villafromdb.Rate,
            //    Sqft    = villafromdb.Sqft

            //};
            patch.ApplyTo(vdto, ModelState);
            var model = _mapper.Map<VillaNumber>(vdto);
            //Villa vtoupdate = new Villa {
            //    Amenity = vdto.Amenity,
            //    Details = vdto.Details,
            //    Id = vdto.Id,
            //    Name = vdto.Name,
            //    ImageUrl = vdto.ImageUrl,
            //    Occupancy = vdto.Occupancy,
            //    Rate = vdto.Rate,
            //    Sqft = vdto.Sqft
            //};
            await _context.Update(model);
            await _context.Save();
            return NoContent();

        }
    }
}
