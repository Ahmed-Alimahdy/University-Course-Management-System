using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using universityManagementSys.Data;
using universityManagementSys.Filters;
using universityManagementSys.Models;
using universityManagementSys.ModelView;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Controllers
{
    [Authorize]

    public class InstructorController : Controller
    {
        IInstructorRepository _instructorRepository;
        ICourseRepository _courseRepository;
        public InstructorController(Context context, IInstructorRepository instructorRepository,ICourseRepository courseRepository)
        {
            _instructorRepository = instructorRepository;
            _courseRepository = courseRepository;
        }
        
        public IActionResult GetAllInstructors()
        {

            var instructors = _instructorRepository.GetAllAsync().Result;
             

            ViewBag.NoDataMessage = !instructors.Any() ? "No instructors found." : " ";
            return View("GetInstructors", instructors);
        }
        [ServiceFilter(typeof(ValidateModelNotEmptyFilter))]
        public IActionResult GetInstructorById(int id)
        {
            var instructor = _instructorRepository.GetByIdAsync(id).Result;
          
            return View(instructor);
        }
        [ServiceFilter(typeof(ValidateModelNotEmptyFilter))]
        public IActionResult GetInstructorbbyCourseId(int id)
        {  
            var instructor = _courseRepository.GetAllAsync().Result
                .Where(c => c.ID == id)
                .Select(c => c.Instructor)
                .FirstOrDefault();
            
            return View(instructor);
        }
        public IActionResult Create()
        {
            var model = new dataViewModel
            {
                PageTitle = "Add instructor",
                WelcomeMessage = "Please fill in the instructor details.",
                instructor = new Instructor()
            };

            return View("AddInstructor", model);
        }
        public IActionResult CreateInstructor(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _instructorRepository.AddAsync(instructor).Wait();
                _instructorRepository.SaveAsync().Wait();
                TempData["Success"] = "instructor added successfully!";
                return RedirectToAction("GetAllInstructors");
            }
            var model = new dataViewModel
            {
                PageTitle = "Add instructor",
                WelcomeMessage = "Please fill in the instructor details.",
                instructor = instructor
            };

            return View("AddInstructor", instructor);
        }
        public IActionResult Edit(int id)
        {
            var instructor = _instructorRepository.GetByIdAsync(id).Result;

            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }
        public IActionResult EditInstructor(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _instructorRepository.UpdateAsync(instructor).Wait();
                _instructorRepository.SaveAsync().Wait();
                TempData["Success"] = "Instructor updated successfully!";
                return RedirectToAction("GetAllInstructors");
            }
           
                TempData["Error"] = "Invalid instructor data.";
                return View("Edit", instructor);


        }
        public IActionResult Delete(int id)
        {
  
            var instructor = _instructorRepository.GetByIdAsync(id).Result;
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }
        public IActionResult DeleteInastructorConfirmed(int id)
        {
            var instructor = _instructorRepository.GetByIdAsync(id).Result;
            if (instructor == null)
            {
                return NotFound();
            }

            _instructorRepository.DeleteAsync(id).Wait();
            _instructorRepository.SaveAsync().Wait();
            TempData["Success"] = "Instructor deleted successfully!";
            return RedirectToAction("GetAllInstructors");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailUnique(string email, int id = 0)
        {
            var exists = await _instructorRepository.CheckUniqueEmailAsync(email, id);
            return Json(!exists);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsPhoneUnique(string phoneNum, int id = 0)
        {
            var exists = await _instructorRepository.CheckUniquePhoneAsync(phoneNum, id);
            return Json(!exists);
        }


    }
}
