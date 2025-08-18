using AutoMapper;
using universityManagementSys.DTOs.Courses;
using universityManagementSys.DTOs.Instructors;
using universityManagementSys.DTOs.Students;
using universityManagementSys.Models;
namespace universityManagementSys.DTOs
{
    
public class DTOsMapper : Profile
    {
        public DTOsMapper()
        {
            // Students Mapper
            CreateMap<CreateStudentDto, Student>();
            CreateMap<UpdateStudentDto, Student>();

            // Courses Mapper
            CreateMap<CreateCourseDTO, Course>();

            CreateMap<UpdateCourseDTO, Course>();

            CreateMap<Course, ReadCourseDTO>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.FirstName))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.Semester.Name));

            // Instructors Mapper
            CreateMap<CreateInstructorDTO, Instructor>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => 0));

            CreateMap<UpdateInstructorDTO, Instructor>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
