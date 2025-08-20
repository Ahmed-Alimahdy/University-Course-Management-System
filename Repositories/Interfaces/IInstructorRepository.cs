using universityManagementSys.Models;

namespace universityManagementSys.Repositories.Interfaces
{
    public interface IInstructorRepository
    {
        Task<Instructor?> GetByIdAsync(int id);
        Task<IEnumerable<Instructor>> GetAllAsync();
        Task<bool> CheckUniqueEmailAsync(string email, int id);
        Task<bool> CheckUniquePhoneAsync(string phone, int id);
        Task<Instructor?> GetByEmailAsync(string email);
        Task AddAsync(Instructor instructor);
        Task UpdateAsync(Instructor instructor);
        Task<bool> UpdateAsync(int id, Instructor instructor);
        Task<bool> DeleteAsync(int id);
        Task SaveAsync();
    }
}
