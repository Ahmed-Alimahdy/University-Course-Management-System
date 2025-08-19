using universityManagementSys.Models;

namespace universityManagementSys.Repositories.Interfaces
{
    public interface IDepartmentCourseRepository
    {
        Task<IEnumerable<DepartmentCourse>> GetAllAsync();
        
    }
}
