using AutoMapper;
using Tasks_Handler.Dtos.Assignment;
using Tasks_Handler.Models;

namespace Tasks_Handler
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Assignment, GetAssignmentDto>();
            CreateMap<AddAssignmentDto, Assignment>();
        }
    }
}