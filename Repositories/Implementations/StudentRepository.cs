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

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.students.Include(s => s.Department).FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _context.students.FirstOrDefaultAsync(s => s.Email.Equals(email));
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
    }
}
