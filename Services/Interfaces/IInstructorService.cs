using universityManagementSys.DTOs.Instructors;
using universityManagementSys.Models;

namespace universityManagementSys.Services.Interfaces
{
    public interface IInstructorService
    {
        Task<int> CreateInstructor(CreateInstructorDTO instructor);
        Task<bool> UpdateInstructor(int instructorId, UpdateInstructorDTO instructor);
        Task<bool> DeleteInstructor(int instructorId);
        Task<IEnumerable<Instructor>> GetAllInstructors();
        Task<Instructor?> GetById(int instructorId);
    }
}
