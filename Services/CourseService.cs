using AutoMapper;
using Microsoft.EntityFrameworkCore;
using universityManagementSys.Data;
using universityManagementSys.DTOs;
using universityManagementSys.Models;

namespace universityManagementSys.Services
{
    public class CourseService : ICourseService
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public CourseService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async public Task<int> CreateCourse(CreateCourseDTO courseDTO)
        {
            var course = _mapper.Map<Course>(courseDTO);

            _context.courses.Add(course);
            await _context.SaveChangesAsync();

            return course.ID;
        }

        async public Task<bool> DeleteCourse(int courseID)
        {
            var record = await _context.courses.FindAsync(courseID);

            if (record == null) 
                return false;

            _context.courses.Remove(record);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ReadCourseDTO>> GetAllCourses()
        {
            var courses = await _context.courses
                .Include(c => c.Instructor)
                .Include(c => c.Semester)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ReadCourseDTO>>(courses);
        }

        public async Task<ReadCourseDTO?> GetByID(int courseID)
        {
            var course = await _context.courses
                .Include(c => c.Instructor)
                .Include(c => c.Semester)
                .FirstOrDefaultAsync(c => c.ID == courseID);

            return _mapper.Map<ReadCourseDTO?>(course);
        }

        public async Task<bool> UpdateCourse(int courseID, UpdateCourseDTO courseDTO)
        {
            var record = await _context.courses.FindAsync(courseID);

            if (record == null)
                return false;

            _mapper.Map(courseDTO, record);

            _context.courses.Update(record);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
