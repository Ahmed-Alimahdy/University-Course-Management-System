using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using universityManagementSys.Models;
using universityManagementSys.ModelView;
using universityManagementSys.Repositories.Interfaces;

namespace universityManagementSys.Controllers
{
    [Authorize]

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
            var vm = new dataViewModel
            {
                PageTitle = "Add Semester",
                WelcomeMessage = "Welcome to the semester add Page",
                semester = new Semester()
            };

            return View("AddSemester", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSemester(Semester semester)
        {

            await _semesterRepository.AddAsync(semester);
            await _semesterRepository.SaveAsync();
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
        public async Task<IActionResult> Edit(int id)
        {
            var semester = await _semesterRepository.GetByIdAsync(id);
            if (semester == null)
            {
                return NotFound();
            }
            return View("EditSemester", semester);
        }

        public async Task<IActionResult> EditSemester(Semester semester)
        {
            await _semesterRepository.UpdateAsync(semester.ID, semester);
            await _semesterRepository.SaveAsync();
            TempData["Success"] = "Semester updated successfully!";
            return RedirectToAction("GetAllSemesters");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var semester = await _semesterRepository.GetByIdAsync(id);
            if (semester == null)
            {
                return NotFound();
            }
            return View("DeleteSemester", semester);
        }

        public async Task<IActionResult> DeleteSemesterConfirmed(int id)
        {
            var semester = await _semesterRepository.GetByIdAsync(id);
            if (semester == null)
            {
                return NotFound();
            }

            await _semesterRepository.DeleteAsync(id);
            await _semesterRepository.SaveAsync();
            TempData["Success"] = "Semester deleted successfully!";
            return RedirectToAction("GetAllSemesters");

        }
        }
}
