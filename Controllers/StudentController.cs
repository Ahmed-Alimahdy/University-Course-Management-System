using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
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
            if (students == null || !students.Any())
            {
                return NotFound();
            }
            return View(students);
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
            ViewModel modelView = new ViewModel
            {
                PageTitle = "Create Student",
                WelcomeMessage = "Welcome to the Student Creation Page",
            };
            return View(modelView);
        }
        public IActionResult CreateStudent(Student student)
        {
            _context.students.Add(student);
            _context.SaveChanges();
            TempData["Success"] = "Student added successfully!";
            return RedirectToAction("GetAllStudents");
        }
        public IActionResult Edit(int id)
        {
            ViewModel modelView = new ViewModel
            {
                PageTitle = "Edit Student",
                WelcomeMessage = "Welcome to the Student Edit Page",
            };
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
