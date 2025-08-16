namespace universityManagementSys.ModelView
{
    public class ViewModel
    {
        public string PageTitle { get; set; } = string.Empty;
        public string WelcomeMessage { get; set; } = string.Empty;

        public DateTime CurrentDate { get; set; } = DateTime.Now;
    }
}
