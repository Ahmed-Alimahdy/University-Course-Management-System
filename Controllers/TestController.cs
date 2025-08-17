using Microsoft.AspNetCore.Mvc;
using universityManagementSys.Data;

namespace universityManagementSys.Controllers
{
    public class TestController : Controller
    {
        Context _context ;

        public TestController(Context context)
        {
            _context = context;
        }
        public IActionResult GetAllCourses()
        {
            var courses = _context.courses.ToList();
            return View(courses);
        }
        public IActionResult GetAllStudents()
        {
            var students = _context.students.ToList();
            return View(students);
        }
        public IActionResult GetAllDepartments()
        {
            var departments = _context.departments.ToList();
            return View(departments);
        }
        public IActionResult GetAllInstructors()
        {
            var instructors = _context.instructors.ToList();
            return View(instructors);
        }
        public IActionResult GetAllCoursesByStudentID(int id)
        {
            var courses = _context.enrollments
                .Where(e => e.StudentID == id)
                .Select(e => e.Course)
                .ToList();
            return View(courses);
        }
        public IActionResult GetGradeByStudentID(int id)
        {
            var grades = _context.enrollments
                .Where(e => e.StudentID == id)
                .Select(e => e.Grade)
                .ToList();
            return View(grades);
        }
        public IActionResult GetAllCoursesBySemesterID(int id)
        {
            var courses = _context.courses
                .Where(c => c.SemesterID == id)
                .ToList();
            return View(courses);
        }
        public IActionResult GetAllCoursesByDepartmentId(int id)
        {
            var courses = _context.departmentCourses
                .Where(dc => dc.DepartmentID == id)
                .Select(dc => dc.Course)
                .ToList();
            return View(courses);
        }

       
        public IActionResult GetAllCoursesByInstructorID(int id)
        {
            var courses = _context.courses
                .Where(c => c.InstructorID == id)
                .ToList();
            return View(courses);
        }

        public IActionResult GetAllStudentsByCourseID(int id)
        {
            var students = _context.enrollments
                .Where(e => e.CourseID == id)
                .Select(e => e.Student)
                .ToList();
            return View(students);
        }
        

    }
}
