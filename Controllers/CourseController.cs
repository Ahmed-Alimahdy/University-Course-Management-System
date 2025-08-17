using Microsoft.AspNetCore.Mvc;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.ModelView;

namespace universityManagementSys.Controllers
{
    public class CourseController : Controller
    {
        Context _context;

        public CourseController(Context context)
        {
            _context = context;
        }
        public IActionResult GetAllCourses()
        {
            var courses = _context.courses.ToList();
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
            return View(courses);
        }
        public IActionResult GetCourseByID(int id)
        {
            var course = _context.courses.FirstOrDefault(c => c.ID == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        public IActionResult GetAllCoursesByStudentID(int id)
        {
            var courses = _context.enrollments
                .Where(e => e.StudentID == id)
                .Select(e => e.Course)
                .ToList();
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
                return View(courses);
        }
        public IActionResult GetAllCoursesBySemesterID(int id)
        {
            var courses = _context.courses
                .Where(c => c.SemesterID == id)
                .ToList();
            if (courses == null || !courses.Any())
            { 
                return NotFound();
            }
                return View(courses);
        }
        public IActionResult GetAllCoursesByDepartmentId(int id)
        {
            var courses = _context.departmentCourses
                .Where(dc => dc.DepartmentID == id)
                .Select(dc => dc.Course)
                .ToList();
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
            return View(courses);
        }
        public IActionResult GetAllCoursesByInstructorID(int id)
        {
            var courses = _context.courses
                .Where(c => c.InstructorID == id)
                .ToList();
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
            return View(courses);
        }
        public IActionResult Create()
        {
            ViewModel modelView = new ViewModel
            {
                PageTitle = "Create Course",
                WelcomeMessage = "Welcome to the Course Creation Page",
            };
            return View(modelView);
        }
        public IActionResult CreateCourse(Course course)
        {
            _context.courses.Add(course);
            _context.SaveChanges();
            TempData["Success"] = "Course added successfully!";
            return RedirectToAction("GetAllCourses");
        }
        public IActionResult Edit(int id)
        {
            var course = _context.courses.FirstOrDefault(s => s.ID == id);
            if (course == null)
            {
                return NotFound();
            }
            return View();
        }
        public IActionResult EditCourse(Course course)
        {
            _context.courses.Update(course);
            _context.SaveChanges();
            TempData["Success"] = "Course updated successfully!";
            return RedirectToAction("GetAllCourses");
        }
        public IActionResult Delete(int id)
        {
            var course = _context.courses.FirstOrDefault(s => s.ID == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        public IActionResult DeleteCourseConfirmed(int id)
        {
            var course = _context.courses.FirstOrDefault(s => s.ID == id);
            if (course == null)
            {
                return NotFound();
            }

            _context.courses.Remove(course);
            _context.SaveChanges();
            TempData["Success"] = "Courses deleted successfully!";
            return RedirectToAction("GetAllCourses");
        }

    }
}
