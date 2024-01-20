using AutoMapper;
using Magic_Villa_Api.DTOs;
using Magic_Villa_Api.Modeles;
using Magic_Villa_Api.Repo.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Magic_Villa_Api.Controllers
{
    [Route("api/v{version:apiversion}/VillaApi")]

    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class VillaApiController : Controller
    {
        private readonly IVillaRepo _context;
        private ILogger<VillaApiController> logger;
        private IMapper _mapper;
        protected APIResponse _apiResponse;
        public VillaApiController(ILogger<VillaApiController> _logger, IVillaRepo context, IMapper mapper)
        {
            logger = _logger;
            _context = context;
            _mapper = mapper;
            _apiResponse = new();
        }
        //[Authorize]

        [HttpGet(Name = "GetAllVillas")]
        //[ResponseCache(Duration =10)]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name = "filter")] int? Occupancy,
            [FromQuery(Name = "search")] string? search, int pagesize, int pagenumber)
        {
            try
            {
                IEnumerable<Villa> all = new List<Villa>();
                if (Occupancy > 0)
                {
                    all = await _context.GetAll(x => x.Occupancy == Occupancy, pagesize: pagesize, pagenumber: pagenumber);
                }
                else
                {
                    all = await _context.GetAll(pagesize: pagesize, pagenumber: pagenumber);

                }
                if (!string.IsNullOrEmpty(search))
                {
                    all = all.Where(x => x.Name.ToLower().Contains(search));
                }
                Pagination pagination = new() { PageNumber = pagenumber, PageSize = pagesize };
                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(pagination));
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = _mapper.Map<List<VillaDTO>>(all);
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };

            }
            return _apiResponse;

        }
        [HttpGet]

        [MapToApiVersion("2.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { "string 1", "string 2" };
        }


        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<APIResponse>> getvillasv2()
        //{
        //    try
        //    {
        //        IEnumerable<Villa> all = await _context.GetAll(includeprops: null);
        //        _apiResponse.StatusCode = HttpStatusCode.OK;
        //        _apiResponse.Result = _mapper.Map<List<VillaDTO>>(all);
        //        return Ok(_apiResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        _apiResponse.IsSuccess = false;
        //        _apiResponse.Errors = new List<string> { ex.ToString() };

        //    }
        //    return _apiResponse;

        //}

        [HttpGet("{id:int}", Name = "Getvillas")]
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
                var villa = await _context.Get(x => x.Id == id);
                if (villa == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return Ok(_apiResponse);
                }
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = _mapper.Map<VillaDTO>(villa);
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

        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] Villa villa)
        {
            try
            {
                if (await _context.Get(x => x.Name.ToLower() == villa.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("Errors", "please submit unique name");
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Errors = new List<string> { ModelState.ToString() };
                    return Ok(_apiResponse);
                }
                await _context.Add(villa);
                await _context.Save();
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Result = _mapper.Map<VillaCreateDTO>(villa);
                _apiResponse.IsSuccess = true;
                return CreatedAtRoute("Getvillas",
                    new { id = villa.Id }
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
        [HttpDelete("{id:int}", Name = "Deletevillas")]
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
                var thevilla = await _context.Get(x => x.Id == id);
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
        [HttpPut("{id:int}", Name = "updatevillas")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villa)
        {
            try
            {
                if (villa == null || id != villa.Id)
                {
                    return BadRequest();
                }

                var model = _mapper.Map<Villa>(villa);
                await _context.Update(model);
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
        [HttpPatch("{id:int}", Name = "patchvillas")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillapatch(int id, JsonPatchDocument<VillaUpdateDTO> patch)
        {

            if (id == 0 || id == null)
            {
                return BadRequest();
            }
            var villafromdb = await _context.Get(x => x.Id == id);
            if (villafromdb == null)
            {
                ModelState.AddModelError("Errors", "Villa ID is Invalid!");
                return BadRequest(ModelState);
            }
            var vdto = _mapper.Map<VillaUpdateDTO>(villafromdb);
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
            var model = _mapper.Map<Villa>(vdto);
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
