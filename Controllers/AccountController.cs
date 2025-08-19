using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using universityManagementSys.Models;
using universityManagementSys.ViewModel;

namespace universityManagementSys.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

        }

        [HttpGet]

        //Register
        public IActionResult Register()
        {
            return View("RegisterView");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashBoard()
        {
            return View("AdminDashBoard");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(RegisterViewModel newUser)
        {

            
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = newUser.UserName;
                user.address = newUser.Address;
                IdentityResult Result = await userManager.CreateAsync(user, newUser.Password);
                if (Result.Succeeded)
                {
                    if(await userManager.IsInRoleAsync(user, "Admin"))
                        {
                          return RedirectToAction("AdminDashBoard");
                        }
                    await userManager.AddToRoleAsync(user, "User");
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index","AccountController");
                }
                foreach (var Error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, Error.Description);
                }
            }
            return View("RegisterView", newUser);
        }

        [HttpGet]

        //Login
        public IActionResult Login()
        {
            return View("LoginView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByNameAsync(newUser.UserName);
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, newUser.Password);
                    if (found)
                    {
                        await signInManager.SignInAsync(user, newUser.RememberMe);
                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("password", "password is incorrect.");
                }

                ModelState.AddModelError("username", "Username is incorrect.");
            }
            return View("LoginView", newUser);
        }

        //LogOut
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        
        [Authorize(Roles = "Admin")]
        
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Failed to assign role.";
                return RedirectToAction("ManageUsers");
            }

            TempData["Success"] = "Role assigned successfully!";
            return RedirectToAction("ManageUsers");
        }
    }
}