using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController(IPlatformRepo repository, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms/./....");

            var platformItem = repository.GetAllPlatforms();

            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

        [HttpGet("{id}", Name ="GetPLatformById")]
        public ActionResult<PlatformReadDto> GetPLatformById(int id)
        {
            var platformItem = repository.GetPlatformById(id);

            if (platformItem != null)
            {
                return Ok(mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto){
            var plaformModel = mapper.Map<Platform>(platformCreateDto);
            repository.CreatePlatform(plaformModel);
            repository.SaveChanges();
            
            var platformReadDto = mapper.Map<PlatformReadDto>(plaformModel);

            return CreatedAtRoute(nameof(GetPLatformById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }

}