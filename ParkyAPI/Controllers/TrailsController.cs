using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
   
    //[Route("api/Trails")]
    [Route("api/v{version.apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings (GroupName = "ParkyOpenAPISpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// Get List of national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet ]
        [ProducesResponseType (200,Type =typeof (List <TrailDto >))]
       
         public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
                
            }

            return Ok(objDto);
        }
        /// <summary>
        /// Get Individual national park
        /// </summary>
        /// <param name="trailId">The Id of the Trail</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name= "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
       
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepo.GetTrail(trailId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);
        }

        [HttpGet("GetTrailInNationalPark/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]

        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _trailRepo.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
           
            return Ok(objDto);
        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto  trailDto)
        {
            if(trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if(_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }
            
            var trailObj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute ("GetTrail",new { trailId = trailObj.Id}, trailObj);
        }
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound )]
        
        [ProducesResponseType(500)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null )
            {
                return BadRequest(ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepo.TrailExists (trailId))
            {
                return NotFound();
            }

            var trailObj = _trailRepo.GetTrail(trailId);
            if (!_trailRepo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deletinging the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}