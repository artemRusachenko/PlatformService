using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataService.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController(ICommandDataClient commandDataClient, IPlatformRepo repository, IMapper mapper, IMessageBusClient messageBusClient) : ControllerBase
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
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto){
            var plaformModel = mapper.Map<Platform>(platformCreateDto);
            repository.CreatePlatform(plaformModel);
            repository.SaveChanges();
            
            var platformReadDto = mapper.Map<PlatformReadDto>(plaformModel);

            //Send Sync Message
            try{
                await commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex){
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            //Send Async Message
            try{
                var platformPublishedDto = mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";
                messageBusClient.PublishNewPlatform(platformPublishedDto);
            }catch(Exception ex){
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPLatformById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }

}