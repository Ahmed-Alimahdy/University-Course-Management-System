using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using universityManagementSys.Data;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Implementations;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Controllers
{
    public class SemesterController : Controller
    {
       
        ISemesterRepository _semesterRepository;

        public SemesterController(ISemesterRepository semesterRepository)
        {
            
            _semesterRepository = semesterRepository;
        }
        public IActionResult GetAllSemesters()
        {
            var semesters = _semesterRepository.GetAllAsync().Result;
               
            ViewBag.NoDataMessage = !semesters.Any() ? "No Semesters found." : " ";
            return View("GetSemesters", semesters);
        }
        public IActionResult AddView()
        {
            ViewBag.PageTitle = "Add Semester";
            ViewBag.WelcomeMessage = "Welcome to the semester add Page";
            return View("AddSemester");
        }
        public IActionResult AddSemester(Semester semester)
        {     
           _semesterRepository.AddAsync(semester).Wait();
            _semesterRepository.SaveAsync().Wait();
            return RedirectToAction("GetAllSemesters");
        }
        public IActionResult GetSemesterByID(int id)
        {
            var semester = _semesterRepository.GetByIdAsync(id).Result;
            if (semester == null)
            {
                return NotFound();
            }
            return View(semester);
        }
        public IActionResult Edit(int id)
        {
            var semester = _semesterRepository.GetByIdAsync(id).Result;
            if (semester == null)
            {
                return NotFound();
            }
            return View(semester);
        }
        public IActionResult EditSemester(Semester semester)
        {
            _semesterRepository.UpdateAsync(semester.ID,semester).Wait();
            _semesterRepository.SaveAsync().Wait();
            TempData["Success"] = "Semester updated successfully!";
            return RedirectToAction("GetAllSemesters");
        }
        public IActionResult Delete(int id)
        {
            var semester = _semesterRepository.GetByIdAsync(id).Result;
            if (semester == null)
            {
                return NotFound();
            }
            return View(semester);
        }
        public IActionResult DeleteSemesterConfirmed(int id)
        {
            var semester = _semesterRepository.GetByIdAsync(id).Result;
            if (semester == null)
            {
                return NotFound();
            }

            _semesterRepository.DeleteAsync(id).Wait();
            _semesterRepository.SaveAsync().Wait();
            TempData["Success"] = "Semester deleted successfully!";
            return RedirectToAction("GetAllSemesters");

        }
        }
}
