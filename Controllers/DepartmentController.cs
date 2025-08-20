using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using universityManagementSys.Models;
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
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentRepository.GetAllAsync();
            
            return View("GetDepartments", departments);
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
        public async Task<IActionResult> CreateDepartment(Department department)
        {
            await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveAsync();
            TempData["Success"] = "Department added successfully!";
            return RedirectToAction("GetAllDepartments");
        }


        public async Task<IActionResult> Edit(int id)
        {

            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View("EditDepartment", department);
        }
        public async Task<IActionResult> EditDepartment(Department department)
        {
            await _departmentRepository.UpdateAsync(department.ID, department);
            await _departmentRepository.SaveAsync();
            TempData["Success"] = "Department updated successfully!";
            return RedirectToAction("GetAllDepartments");
        }
        public async Task<IActionResult> Delete(int id)
        {
          
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View("DeleteDepartment", department);
        }
        public async Task<IActionResult> DeleteDepartmentConfirmed(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            await _departmentRepository.DeleteAsync(id);
            await _departmentRepository.SaveAsync();
            TempData["Success"] = "Department deleted successfully!";
            return RedirectToAction("GetAllDepartments");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsDepartmentNameUnique(string name, int id = 0)
        {
            var exists = await _departmentRepository.CheckUniqueNameAsync(name, id);
            return Json(!exists);
        }

    }
}
