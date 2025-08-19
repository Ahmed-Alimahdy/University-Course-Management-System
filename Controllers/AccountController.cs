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
                        return RedirectToAction("Index", "Home");
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

        // GET: Manage Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            var users = userManager.Users.ToList();
            var userRoles = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRoles.Add(new UserWithRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return View("ManageUsers", userRoles);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AssignRole()
        {
            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.Select(r => r.Name).ToList();

            var vm = new AssignRoleViewModel
            {
                AvailableUsers = users,
                AvailableRoles = roles
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            // Get current roles
            var currentRoles = await userManager.GetRolesAsync(user);

            // Remove all existing roles
            if (currentRoles.Any())
            {
                var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    TempData["Error"] = "Failed to remove existing roles.";
                    return RedirectToAction("AssignRole");
                }
            }

            // Add the new role
            var result = await userManager.AddToRoleAsync(user, model.SelectedRole);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Failed to assign role.";
                return RedirectToAction("AssignRole");
            }

            TempData["Success"] = $"Role '{model.SelectedRole}' assigned to {user.UserName}!";
            return RedirectToAction("AssignRole");
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            // Validate unique email
            var existingUserByEmail = await userManager.FindByEmailAsync(model.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != model.Id)
            {
                ModelState.AddModelError("Email", "This email is already taken.");
                return View(model);
            }

            // Validate unique username
            var existingUserByName = await userManager.FindByNameAsync(model.UserName);
            if (existingUserByName != null && existingUserByName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "This username is already taken.");
                return View(model);
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "User updated successfully!";
                return RedirectToAction("ManageUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // Prevent admin from deleting themselves
            var currentUser = await userManager.GetUserAsync(User);
            if (user.Id == currentUser.Id)
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction("ManageUsers");
            }

            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "User deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete user.";
            }

            return RedirectToAction("ManageUsers");
        }


    }
}