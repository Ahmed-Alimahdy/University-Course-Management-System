using Microsoft.AspNetCore.Mvc;
using universityManagementSys.DTOs.Students;
using universityManagementSys.Models;

namespace universityManagementSys.Services.Interfaces
{
    public interface IStudentService
    {
        Task<int> CreateStudent(CreateStudentDto student);
        Task<bool> UpdateStudent(int studentId, UpdateStudentDto student);
        Task<bool> DeleteStudent(int studentId);
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student?> GetById(int studentId);
    }
}
