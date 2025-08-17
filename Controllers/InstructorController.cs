using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.ModelView;

namespace universityManagementSys.Controllers
{
    public class InstructorController : Controller
    {
        Context _context;

        public InstructorController(Context context)
        {
            _context = context;
        }
        public IActionResult GetAllInstructors()
        {

            var instructors = _context.instructors
              .ToList();
            if (_context.students.IsNullOrEmpty())
            {
                instructors = null;
            }
            ViewBag.NoDataMessage = !instructors.Any() ? "No instructors found." : " ";
            return View("GetInstructors", instructors);
        }
        public IActionResult GetInstructorById(int id)
        {
            var instructor = _context.instructors.FirstOrDefault(i => i.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }
        public IActionResult GetInstructorbbyCourseId(int id)
        {  
            var instructor = _context.courses
                .Where(c => c.ID == id)
                .Select(c => c.Instructor)
                .FirstOrDefault();
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }
        public IActionResult Create()
        {
            var model = new ViewModel
            {
                PageTitle = "Add instructor",
                WelcomeMessage = "Please fill in the instructor details.",
                instructor = new Instructor()
            };

            return View("AddInstructor", model);
        }
        public IActionResult CreateInstructor(Instructor instructor)
        {
            _context.instructors.Add(instructor);
            _context.SaveChanges();
            TempData["Success"] = "instructor added successfully!";
            return RedirectToAction("GetAllInstructors");
        }
        public IActionResult Edit(int id)
        {
            var instructor = _context.instructors.FirstOrDefault(s => s.ID == id);
         
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }
        public IActionResult EditInstructor(Instructor instructor)
        {
            _context.instructors.Update(instructor);
            _context.SaveChanges();
            TempData["Success"] = "Instructor updated successfully!";
            return RedirectToAction("GetAllInstructors");
        }
        public IActionResult Delete(int id)
        {
  
            var instructor = _context.instructors.FirstOrDefault(s => s.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }
        public IActionResult DeleteInastructorConfirmed(int id)
        {
            var instructor = _context.instructors.FirstOrDefault(s => s.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            _context.instructors.Remove(instructor);
            _context.SaveChanges();
            TempData["Success"] = "Instructor deleted successfully!";
            return RedirectToAction("GetAllInstructors");
        }

    }
}
