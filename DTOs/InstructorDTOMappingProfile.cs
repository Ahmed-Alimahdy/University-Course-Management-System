using AutoMapper;
using universityManagementSys.Models;

namespace universityManagementSys.ApiService.DTOs
{
    public class InstructorDTOMappingProfile : Profile
    {
        public InstructorDTOMappingProfile()
        {
            CreateMap<CreateInstructorDTO, Instructor>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => 0));

            CreateMap<UpdateInstructorDTO, Instructor>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}