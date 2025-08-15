using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var students = _context.students
             .Include(s => s.Department)
              .ToList();
            if (_context.students.IsNullOrEmpty())
            {
                students = null;
            }
            ViewBag.NoDataMessage = !students.Any() ? "No students found." : " ";
            return View("GetStudents",students);
        }

        public IActionResult GetIdtoSearch()
        {
            StudentViewModel modelView = new StudentViewModel
            {
                PageTitle = "Search Student",
                WelcomeMessage = "Welcome to the search Student Page",
            };
            return View("GetStudentIdtoSearch", modelView);
        }

        public IActionResult GetStudentByID(int id)
        {
            var student = _context.students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                TempData["Error"] = "No student found with this ID.";
                return View("GetStudentByID");
            }
            return View("GetStudentByID",student);
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
            var departments = _context.departments
                .Select(d => new { d.ID, d.Name })
                .ToList();

            ViewBag.Departments = new SelectList(departments, "ID", "Name");

            var model = new StudentViewModel
            {
                PageTitle = "Create Student",
                WelcomeMessage = "Please fill in the student details.",
                student = new Student()
            };

            return View("AddStudent", model);
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
            var student = _context.students
                .Include(s => s.Department)
                .FirstOrDefault(s => s.ID == id);

            var departments = _context.departments
                .Select(d => new { d.ID, d.Name })
                .ToList();

            ViewBag.Departments = new SelectList(departments, "ID", "Name", student.DepartmentID);

            var model = new StudentViewModel
            {
                PageTitle = "Edit Student",
                WelcomeMessage = "Please update the student details.",
                student = student
            };

            return View("EditStudent", model);
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
            return View("DeleteStudent",student);
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
