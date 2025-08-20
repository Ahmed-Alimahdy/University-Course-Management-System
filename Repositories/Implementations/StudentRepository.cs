using Microsoft.EntityFrameworkCore;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly Context _context;

        public StudentRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(Student student)
        {
            _context.students.Add(student);

            await SaveAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await GetByIdAsync(id);
            if (student == null) return false;

            _context.Remove(student);
            await SaveAsync();

            return true;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.students.Include(s => s.Department).ToListAsync();
        }

        public async Task<bool> CheckUniqueEmailAsync(string email, int id)
        {
            return await _context.students.AnyAsync(i => i.Email == email && i.ID != id);
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.students
                .Include(s => s.Department)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Instructor)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Semester)
                .FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<Student?> GetByIdForAssignCourseAsync(int id)
        {
            return await _context.students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _context.students
                .Include(s => s.Department)   
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)  
                .FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.students.Update(student);
            await SaveAsync();
        }

        public async Task<bool> UpdateAsync(int id, Student student)
        {
            var studentToBeUpdated = await GetByIdAsync(id);

            if (studentToBeUpdated == null) return false;

            studentToBeUpdated.FirstName = student.FirstName;
            studentToBeUpdated.LastName = student.LastName;
            studentToBeUpdated.DateOfBirth = student.DateOfBirth;
            studentToBeUpdated.Email = student.Email;
            studentToBeUpdated.PhoneNum = student.PhoneNum;
            studentToBeUpdated.DepartmentID = student.DepartmentID;

            _context.students.Update(studentToBeUpdated);
            await SaveAsync();

            return true;
        }

        public async Task<IEnumerable<Student>> GetStudentByCourseIdAsync(int courseId)
        {
            return await _context.enrollments
                .Where(e => e.Course.ID == courseId)
                .Include(e => e.Student)
                    .ThenInclude(s => s.Department)
                .Select(e => e.Student)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentByDepartmentIdAsync(int departmentId)
        {
            return await _context.students
                .Where(s => s.DepartmentID == departmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentByGradeIdAsync(int gradeId)
        {
           return await _context.enrollments
                .Where(e => e.GradeID == gradeId)
                .Select(e => e.Student)
                .ToListAsync();
        }
    }
}
