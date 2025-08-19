using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using universityManagementSys.DTOs.Students;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.ModelView;
using universityManagementSys.Services.Interfaces;

namespace universityManagementSys.APIControllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class StudentApiController : Controller
    {
        Context _context;
        IMapper _mapper;
        IStudentService _studentService;
        public StudentApiController(IStudentService studentService,IMapper mapper,Context context)
        {
            _context = context;
            _studentService = studentService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllUsers()
        {
           return Ok(await _studentService.GetAllStudents());
        }
    
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentById(int id)  // USING ACTIONRESULT TO RETURN DIFFERENT TYPES OF RESPONSES  
        {
            var student = await _studentService.GetById(id);
            if(student == null)
            {
                return BadRequest("There is no user with that id");
      
            }
            return Ok(student);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<Student>> CreateStudent(CreateStudentDto studentdto)
        {
            var userId = await _studentService.CreateStudent(studentdto);
            return Ok(userId);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateStudent(int id,UpdateStudentDto studentDto)
        {

            var student = await _context.students.FindAsync(id);

            if (student == null)
            {
                return BadRequest("There is no user with that id");
            }
            _mapper.Map(studentDto, student);
            await _context.SaveChangesAsync();
            return true;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            var student = await _context.students.FindAsync(id);
            if (student == null)
            {
                return BadRequest("There is no user with that id");
            }
            _context.students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
