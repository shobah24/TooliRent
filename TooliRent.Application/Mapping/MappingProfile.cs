using AutoMapper;
using TooliRent.Domain.Models;
using TooliRent.Application.Dto.Tool;

namespace TooliRent.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // tool mapping
            CreateMap<Tool, ToolDto>()
           .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
 
            CreateMap<CreateToolDto, Tool>();

            CreateMap<UpdateToolDto, Tool>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            
        }
    }
}
