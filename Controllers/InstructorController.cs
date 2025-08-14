using Microsoft.AspNetCore.Mvc;
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
    
            var instructors = _context.instructors.ToList();
            ViewBag.PageTitle = "Add Instructor";
            ViewBag.WelcomeMessage = "Welcome to the instructor add Page";
            ViewBag.instructors = instructors;
            if (instructors == null || !instructors.Any())
            {
                return NotFound();
            }
            return View(ViewBag);
           
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
            ViewBag.PageTitle = "Add Instructor";
            ViewBag.WelcomeMessage = "Welcome to the instructor add Page";
            return View();
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
