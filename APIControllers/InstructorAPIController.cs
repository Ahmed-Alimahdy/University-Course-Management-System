using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using universityManagementSys.DTOs.Instructors;
using universityManagementSys.Models;
using universityManagementSys.Services.Interfaces;

namespace universityManagementSys.APIControllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AppInstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        private readonly IMapper _mapper;

        public AppInstructorController(IInstructorService instructorService, IMapper mapper)
        {
            _instructorService = instructorService;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin,Instructor,Student")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instructor>>> GetAllInstructors()
        {
            var instructors = await _instructorService.GetAllInstructors();
            if (!instructors.Any())
                return NotFound("No instructors found.");

            return Ok(instructors);
        }

        [Authorize(Roles = "Admin,Instructor,Student")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Instructor>> GetInstructorById(int id)
        {
            var instructor = await _instructorService.GetById(id);
            if (instructor == null)
                return NotFound($"Instructor with ID {id} not found.");

            return Ok(instructor);
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> CreateInstructor(CreateInstructorDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var instructorId = await _instructorService.CreateInstructor(dto);
            return CreatedAtAction(nameof(GetInstructorById), new { id = instructorId }, dto);
        }

        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateInstructor(int id, UpdateInstructorDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _instructorService.UpdateInstructor(id, dto);
            if (!result)
                return NotFound($"Instructor with ID {id} not found.");

            return NoContent();
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteInstructor(int id)
        {
            var result = await _instructorService.DeleteInstructor(id);
            if (!result)
                return NotFound($"Instructor with ID {id} not found.");

            return NoContent();
        }
    }
}
