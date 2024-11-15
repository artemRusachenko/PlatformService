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
                CreateMap<CommandCreateDto, Command>();
                CreateMap<Command, CommandReadDto>();
                CreateMap<PlatformPublishDto, Platform>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
                CreateMap<GrpcPlatformModel, Platform>()
                    .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.PlatformId))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Commands, opt => opt.Ignore());
            }
        }
    }
}