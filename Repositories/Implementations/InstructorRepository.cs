using Microsoft.EntityFrameworkCore;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Repositories.Implementations
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly Context _context;

        public InstructorRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(Instructor instructor)
        {
            _context.instructors.Add(instructor);
            await SaveAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var instructor = await GetByIdAsync(id);
            if (instructor == null) return false;

            _context.Remove(instructor);
            await SaveAsync();

            return true;
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            return await _context.instructors.Include(i => i.Courses).ToListAsync();
        }

        public async Task<Instructor?> GetByIdAsync(int id)
        {
            return await _context.instructors.Include(i => i.Courses).FirstOrDefaultAsync(i => i.ID == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Instructor instructor)
        {
            _context.instructors.Update(instructor);
            await SaveAsync();
        }

        public async Task<bool> UpdateAsync(int id, Instructor instructor)
        {
            var instructorToBeUpdated = await GetByIdAsync(id);

            if(instructorToBeUpdated == null) return false;

            instructorToBeUpdated.Title = instructor.Title;
            instructorToBeUpdated.Email = instructor.Email;
            instructorToBeUpdated.HireDate = instructor.HireDate;
            instructorToBeUpdated.FirstName = instructor.FirstName;
            instructorToBeUpdated.LastName = instructor.LastName;
            instructorToBeUpdated.PhoneNum = instructor.PhoneNum;

            _context.instructors.Update(instructorToBeUpdated);
            await SaveAsync();

            return true;
        }
    }
}
