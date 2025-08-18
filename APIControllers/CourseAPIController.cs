using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using universityManagementSys.DTOs.Courses;
using universityManagementSys.Models;
using universityManagementSys.Services;
using universityManagementSys.Services.Interfaces;

namespace universityManagementSys.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public CourseAPIController(ICourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }

        [HttpPost]
        async public Task<ActionResult<int>> CreateCourse(CreateCourseDTO courseDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var courseID = await _courseService.CreateCourse(courseDTO);

            return Ok(courseID);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseDTO courseDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _courseService.UpdateCourse(id, courseDTO);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var deleted = await _courseService.DeleteCourse(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            return Ok(await _courseService.GetAllCourses());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Course>> GetById(int id)
        {
            var course = await _courseService.GetByID(id);

            if (course == null)
                return NotFound();

            return Ok(course);
        }
    }
}
