using AutoMapper;
using Microsoft.EntityFrameworkCore;
using universityManagementSys.DTOs.Courses;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;
using universityManagementSys.Services.Interfaces;

namespace universityManagementSys.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepo, IMapper mapper)
        {
            _courseRepo = courseRepo;
            _mapper = mapper;
        }

        async public Task<int> CreateCourse(CreateCourseDTO courseDTO)
        {
            var course = _mapper.Map<Course>(courseDTO);

            await _courseRepo.AddAsync(course);

            return course.ID;
        }

        async public Task<bool> DeleteCourse(int courseID)
        {
            return await _courseRepo.DeleteAsync(courseID);
        }

        public async Task<IEnumerable<ReadCourseDTO>> GetAllCourses()
        {
            var courses = await _courseRepo.GetAllAsync();

            return _mapper.Map<IEnumerable<ReadCourseDTO>>(courses);
        }

        public async Task<ReadCourseDTO?> GetByID(int courseID)
        {
            var course = await _courseRepo.GetByIdAsync(courseID);

            return _mapper.Map<ReadCourseDTO?>(course);
        }

        public async Task<bool> UpdateCourse(int courseID, UpdateCourseDTO courseDTO)
        {
            var record = await _courseRepo.GetByIdAsync(courseID);

            if (record == null)
                return false;

            _mapper.Map(courseDTO, record);

            await _courseRepo.UpdateAsync(record);
            return true;
        }
    }
}
