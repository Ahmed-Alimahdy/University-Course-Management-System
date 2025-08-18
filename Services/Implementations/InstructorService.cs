using AutoMapper;
using Microsoft.EntityFrameworkCore;
using universityManagementSys.Data;
using universityManagementSys.DTOs.Instructors;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;
using universityManagementSys.Services.Interfaces;
namespace universityManagementSys.Services.Implementations
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepo;
        private readonly IMapper _mapper;

        public InstructorService(IInstructorRepository instructor, IMapper mapper)
        {
            _instructorRepo = instructor;
            _mapper = mapper;
        }
        async Task<int> IInstructorService.CreateInstructor(CreateInstructorDTO dto)
        {
            var instructor = _mapper.Map<Instructor>(dto);

            await _instructorRepo.AddAsync(instructor);

            return instructor.ID;
        }

        async Task<bool> IInstructorService.UpdateInstructor(int instructorId, UpdateInstructorDTO dto)
        {
            var record = await _instructorRepo.GetByIdAsync(instructorId);

            if (record == null)
                return false;

            _mapper.Map(dto, record);

            await _instructorRepo.UpdateAsync(record);
            return true;
        }

        async Task<bool> IInstructorService.DeleteInstructor(int instructorId)
        {
            return await _instructorRepo.DeleteAsync(instructorId);
        }

        async Task<IEnumerable<Instructor>> IInstructorService.GetAllInstructors()
        {
            return await _instructorRepo.GetAllAsync();
        }

        async Task<Instructor?> IInstructorService.GetById(int instructorId)
        {
            return await _instructorRepo.GetByIdAsync(instructorId);
        }
    }
}
