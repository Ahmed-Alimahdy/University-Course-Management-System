using Microsoft.EntityFrameworkCore;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Repositories.Implementations
{
    public class EnrollmentRepository : IEnrollmentRepository
    {

        private readonly Context _context;

        public EnrollmentRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(Enrollment enrollment)
        {
            _context.enrollments.Add(enrollment);
            await SaveAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _context.enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();

        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();

        }
    }
}
