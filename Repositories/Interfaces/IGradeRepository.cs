using universityManagementSys.Models;

namespace universityManagementSys.Repositories.Interfaces
{
    public interface IGradeRepository
    {
        Task<Grade?> GetByIdAsync(int id);
        Task<IEnumerable<Grade>> GetAllAsync();
        Task AddAsync(Grade grade);
        Task<bool> UpdateAsync(Grade grade);
        Task<bool> DeleteAsync(int id);
        Task SaveAsync();
    }
}
