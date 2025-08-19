using Microsoft.AspNetCore.Mvc;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.ModelView;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.wwwroot
{
    public class DepartmentController : Controller
    {
       
        IDepartmentRepository _departmentRepository;
        IDepartmentCourseRepository _departmentCourseRepository;
        IStudentRepository _studentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository, IDepartmentCourseRepository departmentCourseRepository, IStudentRepository studentRepository)
        {
            _departmentRepository = departmentRepository;
            _departmentCourseRepository = departmentCourseRepository;
            _studentRepository = studentRepository;
        }
        public IActionResult GetAllDepartments()
        {
            var departments = _departmentRepository.GetAllAsync().Result;
            if (departments == null || !departments.Any())
            {
                return NotFound();
            }
            return View("GetDepartments",departments);
        }
        public IActionResult GetDepartmentByID(int id)
        {
            var department = _departmentRepository.GetByIdAsync(id).Result;
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        public IActionResult GetAllDepartmentByCourseId(int id)
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
        public IActionResult GetDepartmentByStudentId(int id)
        {
            var student = _studentRepository.GetByIdAsync(id).Result;
            if (student == null)
            {
                return NotFound();
            }
            var department = _departmentRepository.GetAllAsync().Result.FirstOrDefault(d => d.ID == student.DepartmentID);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        public IActionResult Create()
        {
            return View("AddDepartment");
        }
        public IActionResult CreateDepartment(Department department)
        {
            _departmentRepository.AddAsync(department).Wait();
            _departmentRepository.SaveAsync().Wait();
            TempData["Success"] = "Department added successfully!";
            return RedirectToAction("GetAllDepartments");
        }
        public IActionResult Edit(int id)
        {

            var department = _departmentRepository.GetByIdAsync(id).Result;
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        public IActionResult EditDepartment(Department department)
        {
            _departmentRepository.UpdateAsync(department.ID, department).Wait();
            _departmentRepository.SaveAsync().Wait();
            TempData["Success"] = "Department updated successfully!";
            return RedirectToAction("GetAllDepartments");
        }
        public IActionResult Delete(int id)
        {
          
            var department =_departmentRepository.GetByIdAsync(id).Result;
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        public IActionResult DeleteDepartmentConfirmed(int id)
        {
            var department = _departmentRepository.GetByIdAsync(id).Result;
            if (department == null)
            {
                return NotFound();
            }

            _departmentRepository.DeleteAsync(id).Wait();
            _departmentRepository.SaveAsync().Wait();
            TempData["Success"] = "Department deleted successfully!";
            return RedirectToAction("GetAllDepartments");
        }
    }
}
