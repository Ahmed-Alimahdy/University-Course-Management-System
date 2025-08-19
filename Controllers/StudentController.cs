using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.ModelView;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Controllers
{
    public class StudentController : Controller
    {
        IStudentRepository _studentRepository;
        ICourseRepository _courseRepository;
        IEnrollmentRepository _enrollmentRepository;
        IDepartmentRepository _departmentRepository;

        public StudentController(IStudentRepository studentRepository, ICourseRepository courseRepository,IEnrollmentRepository enrollmentRepository, IDepartmentRepository departmentRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
            _departmentRepository = departmentRepository;
        }
       
        public IActionResult GetAllStudents()
        {
           var students = _studentRepository.GetAllAsync();

            return View("GetStudents",students);
        }

        public IActionResult GetIdtoSearch()
        {
            ViewBag.functionName = "GetStudentByID";
            dataViewModel modelView = new dataViewModel
            {
                PageTitle = "Search Student",
                WelcomeMessage = "Welcome to the search Student Page",
            };
            return View("GetStudentIdtoSearch", modelView);
        }
        public IActionResult GetStudentByID(int id)
        {
            var student = _studentRepository.GetByIdAsync(id);
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
            dataViewModel modelView = new dataViewModel
            {
                PageTitle = "Search Student",
                WelcomeMessage = "Welcome to the search Student Page",
            };
            return View("GetStudentIdtoSearch", modelView);
        }
        public IActionResult AssignCourseToStudent(int id)
        {
            var student = _studentRepository.GetByIdForAssignCourseAsync(id).Result;
            if (student == null)
            {
                return NotFound();
            }
            if (HttpContext.Items.ContainsKey("ErrorMessage"))
            {
                TempData["Error"] = HttpContext.Items["ErrorMessage"];
            }
            var courses = _courseRepository.GetCoursesForDropDownLists().Result;
            ViewBag.Courses = new SelectList(courses, "ID", "Name");
            var model = new dataViewModel
            {
                PageTitle = "Assign Course to Student",
                WelcomeMessage = "Please select a course to assign to the student.",
                student = student
            };
            return View("AssignCourse", model);
        }
        [ServiceFilter(typeof(DbExceptionFilter))]
        public IActionResult AssignCourseToStudentPost(int StudentID, int CourseID)
        {
            var enrollment = new Enrollment
            {
                StudentID = StudentID,
                CourseID = CourseID
            };

            _enrollmentRepository.AddAsync(enrollment);
   
            _enrollmentRepository.SaveAsync().Wait();

            TempData["Success"] = "Course assigned successfully!";
            return RedirectToAction("GetAllStudents");
        }
        public IActionResult GetCourse()
        {
            var courses =  _courseRepository.GetCoursesForDropDownLists().Result;   
            ViewBag.Courses = new SelectList(courses, "ID", "Name");
            return View("GetStudentByCourseId");
        }

        public async Task<IActionResult> GetStudentsByCourseId(int courseId)
        {
            var students = _studentRepository.GetStudentByCourseIdAsync(courseId).Result;

            return PartialView("StudentTablePartialV", students);
        }
        public IActionResult GetAllStudentsByDepartmentID(int id)
        {
            var students = _studentRepository.GetStudentByDepartmentIdAsync(id).Result;
            if (students == null || !students.Any())
            {
                return NotFound();
            }
            return View(students);
        }
        public IActionResult GetAllStudentsByGradeId(int id)
        {
            var students = _studentRepository.GetStudentByGradeIdAsync(id).Result;
            if (students == null || !students.Any())
            {
                return NotFound();
            }
            return View(students);
        }
        public IActionResult Create()
        {
            var departments = _departmentRepository.GetAllAsync().Result
                .Select(d => new { d.ID, d.Name })
                .ToList();

            ViewBag.Departments = new SelectList(departments, "ID", "Name");

            var model = new dataViewModel
            {
                PageTitle = "Create Student",
                WelcomeMessage = "Please fill in the student details.",
                student = new Student()
            };

            return View("AddStudent", model);
        }

        public IActionResult CreateStudent(Student student)
        {
                _studentRepository.AddAsync(student);
             _studentRepository.SaveAsync().Wait();
            TempData["Success"] = "Student added successfully!";
                return RedirectToAction("GetAllStudents");
        }
        public IActionResult Edit(int id)
        {
            var student = _studentRepository.GetByIdAsync(id).Result;

            var departments = _departmentRepository.GetAllAsync().Result
                .Select(d => new { d.ID, d.Name })
                .ToList();

            ViewBag.Departments = new SelectList(departments, "ID", "Name", student?.DepartmentID);

            var model = new dataViewModel
            {
                PageTitle = "Edit Student",
                WelcomeMessage = "Please update the student details.",
                student = student
            };

            return View("EditStudent", model);
        }

        public IActionResult EditStudent(Student student)
        {
            _studentRepository.UpdateAsync(student).Wait();
            _studentRepository.SaveAsync().Wait();
            TempData["Success"] = "Student updated successfully!";
            return RedirectToAction("GetAllStudents");
        }

        public IActionResult Delete(int id)
        {
            var student = _studentRepository.GetByIdAsync(id).Result;
            if (student == null)
            {
                return NotFound();
            }
            return View("DeleteStudent",student);
        }
        public IActionResult DeleteStudentConfirmed(int id)
        {
            var student = _studentRepository.GetByIdAsync(id).Result;
            if (student == null)
            {
                return NotFound();
            }

            _studentRepository.DeleteAsync(id).Wait();
            _studentRepository.SaveAsync().Wait();
            TempData["Success"] = "Student deleted successfully!";
            return RedirectToAction("GetAllStudents");
        }
    }
}
