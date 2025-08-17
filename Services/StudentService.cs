using AutoMapper;
using Microsoft.EntityFrameworkCore;
using universityManagementSys.ApiServices.DTOs;
using universityManagementSys.Data;
using universityManagementSys.DTOs;
using universityManagementSys.Models;

namespace universityManagementSys.Services
{
    public class StudentService : IStudentService
    {
        Context _context;
        IMapper _mapper;
        public StudentService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<int> IStudentService.CreateStudent(CreateStudentDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            _context.students.Add(student);
            await _context.SaveChangesAsync();
            return student.ID;
        }

        async Task<bool> IStudentService.UpdateStudent(int studentId, UpdateStudentDto studentDto)
        {
            var student = await _context.students.FindAsync(studentId);
            if (student == null)
            {
                return false;
            }
            if (student.Email != studentDto.Email) // CHECK IF THERE ARE EMAIL WITH THE SAME EMAIL THAT HAS BEEN EDITED
            { 
                var collision = await _context.students.FirstOrDefaultAsync(u => u.Email == studentDto.Email); // TO USE IT LATER
                if (collision != null) 
                    return false; 
            }
            _mapper.Map(studentDto, student);
            _context.students.Update(student);
            await _context.SaveChangesAsync();
            return true;
        }
        async Task<bool> IStudentService.DeleteStudent(int studentId)
        {
            var student = await _context.students.FindAsync(studentId);
            if (student == null)
            {
                return false;
            }
            _context.students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        async Task<IEnumerable<Student>> IStudentService.GetAllStudents()
        {
            return await _context.students.ToListAsync();
        }
        async Task<Student?> IStudentService.GetById(int id)
        {
            return await _context.students.FindAsync(id);
        }
    }
}
