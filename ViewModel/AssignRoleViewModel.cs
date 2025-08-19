using universityManagementSys.Models;

namespace universityManagementSys.ViewModel
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string SelectedRole { get; set; }

        public List<string> AvailableRoles { get; set; }
        public List<ApplicationUser> AvailableUsers { get; set; }
    }
}

