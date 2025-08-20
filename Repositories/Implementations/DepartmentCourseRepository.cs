using Microsoft.EntityFrameworkCore;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Repositories.Implementations
{
    public class DepartmentCourseRepository : IDepartmentCourseRepository
    {
        private readonly Context _context;

        public DepartmentCourseRepository(Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DepartmentCourse>> GetAllAsync()
        {
            return await _context.departmentCourses.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(DepartmentCourse dept)
        {
            _context.departmentCourses.Add(dept);
            await SaveAsync();
        }
    }
}
