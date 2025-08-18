using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using universityManagementSys.Data;
using universityManagementSys.Models;

namespace universityManagementSys.Controllers
{
    public class SemesterController : Controller
    {
        Context _context;

        public SemesterController(Context context)
        {
            _context = context;
        }
        public IActionResult GetAllSemesters()
        {
            var semesters = _context.semesters
               .ToList();

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
           _context.semesters.Add(semester);
           _context.SaveChanges();       
           return RedirectToAction("GetAllSemesters");
        }

    }
}
