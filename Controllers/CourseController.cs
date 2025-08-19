using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using universityManagementSys.Data;
using universityManagementSys.Filters;
using universityManagementSys.Models;
using universityManagementSys.ModelView;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
      
        ICourseRepository _courseRepository;
        IEnrollmentRepository _enrollmentRepository;
        IInstructorRepository _instructorRepository;
        ISemesterRepository _semesterRepository;    
        IDepartmentCourseRepository _departmentCourseRepository;
        public CourseController(Context context,ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository, IInstructorRepository instructorRepository,ISemesterRepository semesterRepository,IDepartmentCourseRepository departmentCourseRepository)
        {
            
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
            _semesterRepository = semesterRepository;
            _enrollmentRepository = enrollmentRepository;
            _departmentCourseRepository = departmentCourseRepository;
        }
        public IActionResult GetAllCourses()
        {
            
            var courses = _courseRepository.GetAllAsync().Result;
            ViewData["PageTitle"] = "Get all courses";
            ViewData["Courses"] = courses;
            //if (_context.students.)
            //{
            //    courses = null;
            //}
            return View("AllCourses",courses);
        }
        public IActionResult GetCourseByID(int id)
        {
            var course = _courseRepository.GetByIdAsync(id).Result;
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        [ServiceFilter(typeof(ValidateModelNotEmptyFilter))]
        public IActionResult GetAllCoursesByStudentID(int id)
        {
            var courses = _enrollmentRepository.GetAllAsync().Result
                .Where(e => e.StudentID == id)
                .Select(e => e.Course)
                .ToList();
       
                return View(courses);
        }
        public IActionResult GetAllCoursesBySemesterID(int id)
        {
            var courses = _courseRepository.GetAllAsync().Result
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
            var courses = _departmentCourseRepository.GetAllAsync().Result
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
            var courses = _courseRepository.GetAllAsync().Result
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
            var instructors = _instructorRepository.GetAllAsync().Result
                 .Select(d => new { d.ID, d.FirstName,d.LastName })
                 .ToList();

            var semesters = _semesterRepository.GetAllAsync().Result    
                 .Select(d => new { d.ID, d.Name })
                 .ToList();

            ViewBag.Instructors = new SelectList(
         instructors.Select(i => new {
             i.ID,
             FullName = i.FirstName + " " + i.LastName
         }),
         "ID",
         "FullName"
     );

            ViewBag.Semester = new SelectList(semesters, "ID", "Name");


            DataViewModel viewModel = new DataViewModel
            {
                PageTitle = "Add Course",
                WelcomeMessage = "Please fill in the course details.",
                course = new Course()
            };
            return View("AddCourse", viewModel);
        }
        public IActionResult CreateCourse(Course course)
        {
            _courseRepository.AddAsync(course).Wait();
            _courseRepository.SaveAsync().Wait();
            TempData["Success"] = "Course added successfully!";
            return RedirectToAction("GetAllCourses");
        }
        public IActionResult Edit(int id)
        {
            var course = _courseRepository.GetByIdAsync(id).Result;
            if (course == null)
            {
                return NotFound();
            }
            return View();
        }
        public IActionResult EditCourse(Course course)
        {
            _courseRepository.UpdateAsync(course).Wait();
            _courseRepository.SaveAsync().Wait();
            TempData["Success"] = "Course updated successfully!";
            return RedirectToAction("GetAllCourses");
        }
        public IActionResult Delete(int id)
        {
            var course = _courseRepository.GetByIdAsync(id).Result;
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        public IActionResult DeleteCourseConfirmed(int id)
        {
            var course = _courseRepository.GetByIdAsync(id).Result;
            if (course == null)
            {
                return NotFound();
            }

            _courseRepository.DeleteAsync(id).Wait();
            _courseRepository.SaveAsync().Wait();
            TempData["Success"] = "Courses deleted successfully!";
            return RedirectToAction("GetAllCourses");
        }

    }
}
