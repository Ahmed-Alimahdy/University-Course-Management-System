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

            ViewBag.NoDataMessage = !students.Any() ? "No students found." : " ";
            return View("GetStudents",students);
        }

        public IActionResult GetIdtoSearch()
        {
            ViewBag.functionName = "GetStudentByID";
            ViewModel modelView = new ViewModel
            {
                PageTitle = "Search Student",
                WelcomeMessage = "Welcome to the search Student Page",
            };
            return View("GetStudentIdtoSearch", modelView);
        }
        public IActionResult GetStudentByID(int id)
        {
            var student = _context.students
    .Include(s => s.Department)
    .Include(s => s.Enrollments)
        .ThenInclude(e => e.Course)
            .ThenInclude(c => c.Instructor)
    .Include(s => s.Enrollments)
        .ThenInclude(e => e.Course.Semester)
    .FirstOrDefault(s => s.ID == id);

            if (student == null)
            {
                TempData["Error"] = "No student found with this ID.";
                return View("GetStudentByID");
            }
            return View("GetStudentByID",student);
        }
        public IActionResult GetIdtoAssign()
        {
            ViewBag.functionName = "AssignCourseToStudent";
            ViewModel modelView = new ViewModel
            {
                PageTitle = "Search Student",
                WelcomeMessage = "Welcome to the search Student Page",
            };
            return View("GetStudentIdtoSearch", modelView);
        }
        public IActionResult AssignCourseToStudent(int id)
        {
            var student = _context.students
                .Include(s => s.Enrollments)
                .FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            var courses = _context.courses
                .Select(c => new { c.ID, c.Name })
                .ToList();
            ViewBag.Courses = new SelectList(courses, "ID", "Name");
            var model = new ViewModel
            {
                PageTitle = "Assign Course to Student",
                WelcomeMessage = "Please select a course to assign to the student.",
                student = student
            };
            return View("AssignCourse", model);
        }
        public IActionResult AssignCourseToStudentPost(int StudentID, int CourseID)
        {
            var enrollment = new Enrollment
            {
                StudentID = StudentID,
                CourseID = CourseID
            };

            _context.enrollments.Add(enrollment);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                TempData["Error"] = "This course is already assigned to the student!";
                return RedirectToAction("AssignCourseToStudent", new { id = StudentID });
            }

            TempData["Success"] = "Course assigned successfully!";
            return RedirectToAction("GetAllStudents");
        }
        public IActionResult GetCourseId()
        {
            return View("GetCourseId");
        }
        public IActionResult GetAllStudentsByCourseID(int id)
        {
            var course = _context.courses
                .Include(c => c.Instructor)
                .Include(c => c.Semester)
                .FirstOrDefault(c => c.ID == id);

            if (course == null)
            {
                TempData["Error"] = "No course found with this ID.";
                return RedirectToAction("GetCourseId");
            }

            var students = _context.enrollments
    .Where(e => e.CourseID == id)
    .Include(e => e.Student)
        .ThenInclude(s => s.Department)
    .Select(e => e.Student)
    .ToList();


            var vm = new ViewModel
            {
                course = course,
                students = students
            };

            return View("GetStudentByCourseId", vm);
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

            var model = new ViewModel
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

            var model = new ViewModel
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
