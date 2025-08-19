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

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        //Register
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
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
                ApplicationUser user = new ApplicationUser
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber
                };

                IdentityResult result = await userManager.CreateAsync(user, newUser.Password);

                if (result.Succeeded)
                {
                    // Add selected role (Student or Instructor)
                    if (await roleManager.RoleExistsAsync(newUser.Role))
                    {
                        await userManager.AddToRoleAsync(user, newUser.Role);
                    }

                    await signInManager.SignInAsync(user, false);

                    if (newUser.Role == "Admin")
                        return RedirectToAction("AdminDashBoard");

                    return RedirectToAction("Index", "Home"); // or your default page
                    return RedirectToAction("Index","AccountController");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View("Register", newUser);
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