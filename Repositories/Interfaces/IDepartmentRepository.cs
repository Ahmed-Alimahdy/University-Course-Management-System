using universityManagementSys.Models;

namespace universityManagementSys.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetByIdAsync(int id);
        Task<IEnumerable<Department>> GetAllAsync();
        Task AddAsync(Department dept);
        Task<bool> UpdateAsync(int id, Department dept);
        Task<bool> DeleteAsync(int id);
        Task SaveAsync();
    }
}
