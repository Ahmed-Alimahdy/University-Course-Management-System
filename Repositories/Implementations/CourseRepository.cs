using Microsoft.EntityFrameworkCore;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Repositories.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private readonly Context _context;

        public CourseRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(Course course)
        {
            _context.courses.Add(course);
            await SaveAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await GetByIdAsync(id);
            if (course == null) return false;

            _context.Remove(course);
            await SaveAsync();
            return true;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.courses.Include(c => c.Instructor).Include(c => c.Semester).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetCoursesForDropDownLists()
        {
            return await _context.courses
                .Select(c => new { c.ID, c.Name })
                .ToListAsync();
        }
        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.courses.Include(c => c.Instructor).Include(c => c.Semester).FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Course course)
        {
            _context.courses.Update(course);
            await SaveAsync();
        }

        public async Task<bool> UpdateAsync(int id, Course course)
        {
            var courseToBeUpdated = await GetByIdAsync(id);
            if (courseToBeUpdated == null)
                return false;

            courseToBeUpdated.Name = course.Name;
            courseToBeUpdated.Description = course.Description;
            courseToBeUpdated.CreditHours = course.CreditHours;
            courseToBeUpdated.SemesterID = course.SemesterID;
            courseToBeUpdated.InstructorID = course.InstructorID;

            _context.courses.Update(courseToBeUpdated);
            await SaveAsync();

            return true;
        }
    }
}
