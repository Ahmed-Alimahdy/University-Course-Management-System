using Microsoft.EntityFrameworkCore;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Repositories.Implementations
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly Context _context;

        public SemesterRepository(Context context)
        {
            _context = context;
        }
        public async Task AddAsync(Semester semester)
        {
            _context.semesters.Add(semester);
            await SaveAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var semester = await GetByIdAsync(id);
            if (semester == null) return false;

            _context.Remove(semester);
            await SaveAsync();

            return true;
        }

        public async Task<IEnumerable<Semester>> GetAllAsync()
        {
            return await _context.semesters.Include(i => i.Courses).ToListAsync();
        }

        public async Task<Semester?> GetByIdAsync(int id)
        {
            return await _context.semesters.Include(i => i.Courses).FirstOrDefaultAsync(i => i.ID == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, Semester semester)
        {
            var semesterToBeUpdated = await GetByIdAsync(id);

            if (semesterToBeUpdated == null) return false;

            semesterToBeUpdated.Name = semester.Name;
            semesterToBeUpdated.StartDate = semester.StartDate;
            semesterToBeUpdated.EndDate = semester.EndDate;

            await SaveAsync();

            return true;
        }
    }
}
