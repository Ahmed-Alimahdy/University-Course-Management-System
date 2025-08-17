using Microsoft.AspNetCore.Mvc;
using universityManagementSys.ApiServices.DTOs;
using universityManagementSys.DTOs;
using universityManagementSys.Models;

namespace universityManagementSys.Services
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
