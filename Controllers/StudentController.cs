using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.ModelView;

namespace universityManagementSys.Controllers
{
    public class StudentController : Controller
    {
        Context _context;

        public StudentController(Context context)
        {
            _context = context;
        }
        public IActionResult GetAllStudents()
        {
            var students = _context.students.ToList();
            if (_context.students.IsNullOrEmpty())
            {
                students = null;
            }
            ViewBag.NoDataMessage = !students.Any() ? "No students found." : null;
            return View("GetStudents",students);
        }


        public IActionResult GetStudentByID(int id)
        {
            var student = _context.students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        public IActionResult GetAllStudentsByCourseID(int id)
        {
            var students = _context.enrollments
                .Where(e => e.CourseID == id)
                .Select(e => e.Student)
                .ToList();
            if (students == null || !students.Any())
            {
                return NotFound();
            }
            return View(students);
        }
        public IActionResult GetAllStudentsByDepartmentID(int id)
        {
            var students = _context.students
                .Where(s => s.DepartmentID == id)
                .ToList();
            if (students == null || !students.Any())
            {
                return NotFound();
            }
            return View(students);
        }
        public IActionResult GetAllStudentsByGradeId(int id)
        {
            var students = _context.enrollments
                .Where(e => e.GradeID == id)
                .Select(e => e.Student)
                .ToList();
            if (students == null || !students.Any())
            {
                return NotFound();
            }
            return View(students);
        }
        public IActionResult Create()
        {
            StudentViewModel modelView = new StudentViewModel
            {
                PageTitle = "Add Student",
                WelcomeMessage = "Welcome to the add Student Page",
            };
            return View("AddStudent",modelView);
        }
        public IActionResult CreateStudent(StudentViewModel studentViewModel)
        {
           
            _context.students.Add(studentViewModel.student);
            _context.SaveChanges();
            TempData["Success"] = "Student added successfully!";
            return RedirectToAction("GetAllStudents");
        }
        public IActionResult Edit(int id)
        {
          
            var student = _context.students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        public IActionResult EditStudent(Student student)
        {
            _context.students.Update(student);
            _context.SaveChanges();
            TempData["Success"] = "Student updated successfully!";
            return RedirectToAction("GetAllStudents");
        }

        public IActionResult Delete(int id)
        {
            var student = _context.students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        public IActionResult DeleteStudentConfirmed(int id)
        {
            var student = _context.students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            _context.students.Remove(student);
            _context.SaveChanges();
            TempData["Success"] = "Student deleted successfully!";
            return RedirectToAction("GetAllStudents");
        }
    }
}
