using System.ComponentModel.DataAnnotations;

namespace universityManagementSys.ViewModel
{
    public class RoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}