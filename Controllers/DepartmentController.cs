using Microsoft.AspNetCore.Mvc;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.ModelView;

namespace universityManagementSys.wwwroot
{
    public class DepartmentController : Controller
    {
        Context _context;

        public DepartmentController(Context context)
        {
            _context = context;
        }
        public IActionResult GetAllDepartments()
        {
            var departments = _context.departments.ToList();
            if (departments == null || !departments.Any())
            {
                return NotFound();
            }
            return View(departments);
        }
        public IActionResult GetDepartmentByID(int id)
        {
            var department = _context.departments.FirstOrDefault(d => d.ID == id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        public IActionResult GetAllDepartmentByCourseId(int id)
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
        public IActionResult GetDepartmentByStudentId(int id)
        {
            var student = _context.students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            var department = _context.departments.FirstOrDefault(d => d.ID == student.DepartmentID);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        public IActionResult Create()
        {
            ViewModel modelView = new ViewModel
            {
                PageTitle = "Create Department",
                WelcomeMessage = "Welcome to the Department Creation Page",
            };
            return View(modelView);
        }
        public IActionResult CreateDepartment(Department department)
        {
            _context.departments.Add(department);
            _context.SaveChanges();
            TempData["Success"] = "Department added successfully!";
            return RedirectToAction("GetAllDepartments");
        }
        public IActionResult Edit(int id)
        {

            var department = _context.departments.FirstOrDefault(s => s.ID == id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        public IActionResult EditDepartment(Department department)
        {
            _context.departments.Update(department);
            _context.SaveChanges();
            TempData["Success"] = "Department updated successfully!";
            return RedirectToAction("GetAllDepartments");
        }
        public IActionResult Delete(int id)
        {
          
            var department = _context.departments.FirstOrDefault(s => s.ID == id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        public IActionResult DeleteDepartmentConfirmed(int id)
        {
            var department = _context.departments.FirstOrDefault(s => s.ID == id);
            if (department == null)
            {
                return NotFound();
            }

            _context.departments.Remove(department);
            _context.SaveChanges();
            TempData["Success"] = "Department deleted successfully!";
            return RedirectToAction("GetAllDepartments");
        }
    }
}
