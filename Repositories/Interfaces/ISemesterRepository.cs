using universityManagementSys.Models;

namespace universityManagementSys.Repositories.Interfaces
{
    public interface ISemesterRepository
    {
        Task<Semester?> GetByIdAsync(int id);
        Task<IEnumerable<Semester>> GetAllAsync();
        Task AddAsync(Semester semester);
        Task<bool> UpdateAsync(int id, Semester semester);
        Task<bool> DeleteAsync(int id);
        Task SaveAsync();
    }
}
