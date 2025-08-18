using Microsoft.EntityFrameworkCore;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Repositories.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly Context _context;

        public DepartmentRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(Department dept)
        {
            _context.departments.Add(dept);
            await SaveAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dept = await GetByIdAsync(id);
            if (dept == null) return false;

            _context.Remove(dept);
            await SaveAsync();
            return true;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.departments.Include(d => d.Students).ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.departments.Include(d => d.Students).FirstOrDefaultAsync(d => d.ID == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, Department dept)
        {
            var deptToBeUpdated = await GetByIdAsync(id);
            if (deptToBeUpdated == null)
                return false;

            deptToBeUpdated.Name = dept.Name;
            deptToBeUpdated.Description = dept.Description;

            _context.departments.Update(deptToBeUpdated);
            await SaveAsync();
            return true;
        }
    }
}
