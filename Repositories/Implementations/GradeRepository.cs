using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace universityManagementSys.Repositories.Implementations
{
    public class GradeRepository : IGradeRepository
    {
        private readonly Context _context;

        public GradeRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(Grade grade)
        {
            _context.grades.Add(grade);
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

        public async Task<IEnumerable<Grade>> GetAllAsync()
        {
            return await _context.grades.ToListAsync();
        }

        public async Task<Grade?> GetByIdAsync(int id)
        {
            return await _context.grades.FirstOrDefaultAsync(g => g.ID == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Grade grade)
        {
            _context.grades.Update(grade);
            await SaveAsync();
            return true;
        }
    }
}
