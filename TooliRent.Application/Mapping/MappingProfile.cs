using AutoMapper;
using TooliRent.Domain.Models;
using TooliRent.Application.Dto.Tool;
using TooliRent.Application.Dto.Category;
using TooliRent.Application.Dto.Booking;

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

            // category mapping
            CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.Tools, opt => opt.MapFrom(src => src.Tools));

            CreateMap<Category, CategoryWithoutToolsDtos>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            // Booking mapping
            CreateMap<Booking, BookingResponseDto>()
            .ForMember(dest => dest.ToolName,
             opt => opt.MapFrom(src => src.BookingTools
            .Where(bt => bt.Tool != null)
            .Select(bt => bt.Tool.Name)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<BookingReqDto, Booking>()
                .ForMember(dest => dest.BookingTools, opt => opt.Ignore());

            CreateMap<UpdateBookingDto, Booking>()
                .ForMember(dest => dest.BookingTools, opt => opt.Ignore());



        }
    }
}
