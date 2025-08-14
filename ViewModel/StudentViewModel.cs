using universityManagementSys.Models;

namespace universityManagementSys.ModelView
{
    public class StudentViewModel
    {
        public string PageTitle { get; set; } = string.Empty;
        public string WelcomeMessage { get; set; } = string.Empty;

        public DateTime CurrentDate { get; set; } = DateTime.Now;

        public Student student { get; set; }
    }
}
