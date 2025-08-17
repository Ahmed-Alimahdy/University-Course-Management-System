using AutoMapper;
using universityManagementSys.Models;

namespace universityManagementSys.DTOs
{
    public class CoursesMapper: Profile
    {
        public CoursesMapper()
        {
            CreateMap<CreateCourseDTO, Course>();

            CreateMap<UpdateCourseDTO, Course>();

            CreateMap<Course, ReadCourseDTO>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.FirstName))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.Semester.Name));


        }
    }
}
