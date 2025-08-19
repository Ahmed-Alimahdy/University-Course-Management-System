using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using universityManagementSys.Models;
using universityManagementSys.ModelView;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentRepository.GetAllAsync();

            return View("GetStudents", students);
        }

        public IActionResult GetIdtoSearch()
        {
            ViewBag.functionName = "GetStudentByID";
            DataViewModel modelView = new DataViewModel
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
            DataViewModel modelView = new DataViewModel
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
            var model = new DataViewModel
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

            var model = new DataViewModel
            {
                PageTitle = "Create Student",
                WelcomeMessage = "Please fill in the student details.",
                student = new Student()
            };

            return View("AddStudent", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Security measure to prevent CSRF attacks
        public async Task<IActionResult> CreateStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentRepository.AddAsync(student);
                await _studentRepository.SaveAsync();

                TempData["Success"] = "Student added successfully!";
                return RedirectToAction("GetAllStudents");
            }

            var departments = (await _departmentRepository.GetAllAsync())
                .Select(d => new { d.ID, d.Name })
                .ToList();

            ViewBag.Departments = new SelectList(departments, "ID", "Name");

            var model = new DataViewModel
            {
                PageTitle = "Create Student",
                WelcomeMessage = "Please fill in the student details.",
                student = student
            };

            TempData["Error"] = "Student data is not valid";
            return View("AddStudent", model);
        }

        public IActionResult Edit(int id)
        {
            var student = _studentRepository.GetByIdAsync(id).Result;

            var departments = _departmentRepository.GetAllAsync().Result
                .Select(d => new { d.ID, d.Name })
                .ToList();

            ViewBag.Departments = new SelectList(departments, "ID", "Name", student?.DepartmentID);

            var model = new DataViewModel
            {
                PageTitle = "Edit Student",
                WelcomeMessage = "Please update the student details.",
                student = student
            };

            return View("EditStudent", model);
        }

        public IActionResult EditStudent(Student student)
        {
            if (student == null || !ModelState.IsValid)
            {
                TempData["Error"] = "Invalid student data.";
                var departments = _departmentRepository.GetAllAsync().Result
                    .Select(d => new { d.ID, d.Name })
                    .ToList();
                ViewBag.Departments = new SelectList(departments, "ID", "Name", student?.DepartmentID);

                var model = new DataViewModel
                {
                    PageTitle = "Edit Student",
                    WelcomeMessage = "Please update the student details.",
                    student = student
                };

                return View("EditStudent", model);

            }
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

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailUnique(string email, int id = 0)
        {
            var exists = await _studentRepository.CheckUniqueEmailAsync(email, id);
            return Json(!exists);
        }
    }
}
