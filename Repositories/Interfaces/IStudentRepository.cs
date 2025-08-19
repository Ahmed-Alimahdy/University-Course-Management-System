using universityManagementSys.Models;

namespace universityManagementSys.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(int id);
        Task<Student> GetByIdForAssignCourseAsync(int id);
        Task<bool> CheckUniqueEmailAsync(string email, int id);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByEmailAsync(string email);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task<bool> UpdateAsync(int id, Student student);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Student>> GetStudentByCourseIdAsync(int courseId);
        Task<IEnumerable<Student>> GetStudentByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<Student>> GetStudentByGradeIdAsync(int gradeId);
        Task SaveAsync();
    }
}
