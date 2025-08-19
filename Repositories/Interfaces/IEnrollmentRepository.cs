using universityManagementSys.Models;

namespace universityManagementSys.Repositories.Interfaces
{
    public interface IEnrollmentRepository
    {

        Task AddAsync(Enrollment enrollment);
        Task<int> SaveAsync();
        Task<IEnumerable<Enrollment>> GetAllAsync();
    }
}
