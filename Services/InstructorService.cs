using AutoMapper;
using Microsoft.EntityFrameworkCore;
using universityManagementSys.ApiService.DTOs;
using universityManagementSys.Data;
using universityManagementSys.Models;
namespace universityManagementSys.ApiService.Service
{
    public class InstructorService : IInstructorService
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public InstructorService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        async Task<int> IInstructorService.CreateInstructor(CreateInstructorDTO dto)
        {
            var instructor = _mapper.Map<Instructor>(dto);

            _context.instructors.Add(instructor);
            await _context.SaveChangesAsync();

            return instructor.ID;
        }

        async Task<bool> IInstructorService.UpdateInstructor(int instructorId, UpdateInstructorDTO dto)
        {
            var existing = await _context.instructors.FindAsync(instructorId);

            if (existing == null)
                return false;

            _mapper.Map(dto, existing);

            _context.instructors.Update(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        async Task<bool> IInstructorService.DeleteInstructor(int instructorId)
        {
            var instructor = await _context.instructors.FindAsync(instructorId);

            if (instructor == null)
                return false;

            _context.instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return true;
        }

        async Task<IEnumerable<Instructor>> IInstructorService.GetAllInstructors()
        {
            return await _context.instructors.ToListAsync();
        }

        async Task<Instructor?> IInstructorService.GetById(int instructorId)
        {
            return await _context.instructors.FindAsync(instructorId);
        }
    }
}
