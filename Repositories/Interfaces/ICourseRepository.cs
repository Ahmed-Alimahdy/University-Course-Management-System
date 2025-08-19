using universityManagementSys.Models;

namespace universityManagementSys.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(int id);
        Task<IEnumerable<object>> GetCoursesForDropDownLists();
        Task<IEnumerable<Course>> GetAllAsync();
        Task<bool> CheckUniqueNameAsync(string name, int id = 0);
        Task AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task<bool> UpdateAsync(int id, Course course);
        Task<bool> DeleteAsync(int id);
        Task SaveAsync();
    }
}
