using universityManagementSys.DTOs.Courses;
using universityManagementSys.Models;

namespace universityManagementSys.Services.Interfaces
{
    public interface ICourseService
    {
        Task<int> CreateCourse(CreateCourseDTO courseDTO);
        Task<bool> UpdateCourse(int courseID,UpdateCourseDTO courseDTO);
        Task<bool> DeleteCourse(int courseID);
        Task<IEnumerable<ReadCourseDTO>> GetAllCourses();
        Task<ReadCourseDTO?> GetByID(int courseID);

    }
}
