using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class Copmmandsprofile
    {
        public class CommandsProfile : Profile{
            public CommandsProfile(){
                //Source -> Target
                CreateMap<Platform, PlatfromReadDto>();
                CreateMap<Command, CommandCreateDto>();
                CreateMap<Command, CommandReadDto>();
            }
        }
    }
}