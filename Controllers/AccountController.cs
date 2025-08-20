using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Interfaces;
using universityManagementSys.ViewModel;

namespace universityManagementSys.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IStudentRepository studentRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IInstructorRepository instructorRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ICourseRepository courseRepository;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IStudentRepository studentRepository,IInstructorRepository instructorRepository, IDepartmentRepository departmentRepository,IEnrollmentRepository enrollmentRepository,ICourseRepository courseRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.courseRepository = courseRepository;
            this.studentRepository = studentRepository;
            this.instructorRepository = instructorRepository;
            this.departmentRepository = departmentRepository;
            this._enrollmentRepository = enrollmentRepository;
        }


        //Register
       

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashBoard()
        {
            return View("AdminDashBoard");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(RegisterViewModel newUser)
        {
            
            var user = new ApplicationUser
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, newUser.Password);

            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Register", newUser);
            }

          
            if (await roleManager.RoleExistsAsync(newUser.Role))
            {
                await userManager.AddToRoleAsync(user, newUser.Role);
            }


            if (newUser.Role == "Student" && newUser.student != null)
            {
                var student = new Student
                {
                    FirstName = newUser.Firstname,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    DateOfBirth = newUser.student.DateOfBirth,
                    PhoneNum = newUser.PhoneNumber,
                    DepartmentID = newUser.student.DepartmentID
                };

                await studentRepository.AddAsync(student);

                
                var savedStudent = await studentRepository.GetByEmailAsync(newUser.Email);

                return View("StudentProfile", savedStudent);
            }

            else if (newUser.Role == "Instructor" && newUser.instructor != null)
            {
                var instructor = new Instructor
                {
                    FirstName = newUser.Firstname,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    HireDate = newUser.instructor.HireDate,
                    Title = newUser.instructor.Title,
                    PhoneNum = newUser.PhoneNumber
                };
                await instructorRepository.AddAsync(instructor);
                var savedInstructor = await instructorRepository.GetByEmailAsync(newUser.Email);
                return View("InstructorProfile", instructor);
            }

          
            if (newUser.Role == "Admin")
            {
                return RedirectToAction("AdminDashBoard");
            }
            if (newUser.Role =="Student")
            {
                return RedirectToAction("GetStudentProfile",newUser.student?.ID);
            }
            if(newUser.Role =="Instructor")
            {
                return RedirectToAction("GetInstructionProfile", newUser.instructor?.ID);
            }

            TempData["Success"] = "Account created successfully. Please login.";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> GetStudentForm() 
        {   
            var departments = await departmentRepository.GetAllAsync(); 
            ViewBag.Departments = new SelectList(departments, "ID", "Name"); 
            return PartialView("StudentRegisterForm", new RegisterViewModel { student = new Student(), Role = "Student" });
        }
        
        public async Task<IActionResult> GetStudentProfile(int id)
        {
            var student = await studentRepository.GetByIdAsync(id);

            return RedirectToAction("GetStudentProfile", new { id = student.ID });
        }

       
        public async Task<IActionResult> GetIDProfile(int id)
        {
            var student = await studentRepository.GetByIdAsync(id);
            
            ViewBag.Departments = new SelectList(await departmentRepository.GetAllAsync(), "ID", "Name");

            return View("EditStudentByProfile", student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Student student)
        {
            await studentRepository.UpdateAsync(student);

            return RedirectToAction("GetStudentProfile", new { id = student.ID });
        }


        public async Task<IActionResult> AssignCourseToStudentProfile(int id)
        {
            var student = studentRepository.GetByIdForAssignCourseAsync(id).Result;
            if (student == null)
            {
                return NotFound();
            }
            if (HttpContext.Items.ContainsKey("ErrorMessage"))
            {
                TempData["Error"] = HttpContext.Items["ErrorMessage"];
            }
            var courses = courseRepository.GetCoursesForDropDownLists().Result;
            ViewBag.Courses = new SelectList(courses, "ID", "Name");
          
            return View("AssignCourseByProfile", student);
        }
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
            return RedirectToAction("GetStudentProfile");
        }
       
        public async Task<IActionResult> GetInstructorForm() { return PartialView("InstructorRegisterForm", new RegisterViewModel { instructor = new Instructor(), Role = "Instructor" });
        }
        public async Task<IActionResult> GetInstructorProfile(int id)
        {
            var instructor = await instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View("InstructorProfile", instructor);
        }

        // GET Edit
        public async Task<IActionResult> Edit(int id)
        {
            var instructor = await instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            ViewBag.Departments = new SelectList(await departmentRepository.GetAllAsync(), "ID", "Name");
            return View("EditInstructorByProfile", instructor);
        }

        // POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstructor(Instructor instructor)
        {
          

            await instructorRepository.UpdateAsync(instructor);

            return RedirectToAction("GetInstructorProfile", new { id = instructor.ID });
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
                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("AdminDashBoard");
                        }
                      
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