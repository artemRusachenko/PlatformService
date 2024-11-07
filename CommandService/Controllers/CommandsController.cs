using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;  
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId){
            Console.WriteLine($"--> Hit GetCommandsFotPlatform {platformId}");

            if(!_repository.PlatformExists(platformId)){
                return NotFound();
            }

            var commands = _repository.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId){
            Console.WriteLine($"--> Hit GetCommandsFotPlatform {platformId} / {commandId}");

            if(!_repository.PlatformExists(platformId)){
                return NotFound();
            }

            var command = _repository.GetCommand(platformId, commandId);

            if(command == null){
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));

        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPLatform(int platformId, CommandCreateDto commandCreateDto){
            Console.WriteLine($"--> Hit CreateCommandForPLatform {platformId}");

            if(!_repository.PlatformExists(platformId)){
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandCreateDto);

            _repository.CreateCommand(platformId, command);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId, commandId = commandReadDto.Id}, commandReadDto);
        }
    }
}