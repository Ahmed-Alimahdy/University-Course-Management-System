using AutoMapper;
using universityManagementSys.DTOs.Students;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;
using universityManagementSys.Services.Interfaces;

namespace universityManagementSys.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository student, IMapper mapper)
        {
            _studentRepo = student;
            _mapper = mapper;
        }

        async Task<int> IStudentService.CreateStudent(CreateStudentDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);

            await _studentRepo.AddAsync(student);
            return student.ID;
        }

        async Task<bool> IStudentService.UpdateStudent(int studentId, UpdateStudentDto studentDto)
        {
            var student = await _studentRepo.GetByIdAsync(studentId);

            if (student == null)
            {
                return false;
            }
            if (student.Email != studentDto.Email) // CHECK IF THERE ARE EMAIL WITH THE SAME EMAIL THAT HAS BEEN EDITED
            {
                var collision = await _studentRepo.GetByEmailAsync(studentDto.Email); // TO USE IT LATER
                if (collision != null) 
                    return false; 
            }

            _mapper.Map(studentDto, student);
            
            await _studentRepo.UpdateAsync(student);

            return true;
        }
        async Task<bool> IStudentService.DeleteStudent(int studentId)
        {
            return await _studentRepo.DeleteAsync(studentId);
        }

        async Task<IEnumerable<ReadStudentDTO>> IStudentService.GetAllStudents()
        {
            var students = await _studentRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ReadStudentDTO>>(students);
        }
        async Task<Student?> IStudentService.GetById(int id)
        {
            return await _studentRepo.GetByIdAsync(id);
        }
    }
}
