using universityManagementSys.Models;

namespace universityManagementSys.ModelView
{
    public class DataViewModel

    {
        public string PageTitle { get; set; } = string.Empty;
        public string WelcomeMessage { get; set; } = string.Empty;

        public DateTime CurrentDate { get; set; } = DateTime.Now;

        public Student student { get; set; }
        public Instructor instructor { get; set; }
        public Semester semester { get; set; }
        public Course course { get; set; }
        public Department department { get; set; }
        public Grade grade { get; set; }

        public ICollection<Student> students { get; set; } = new List<Student>();
        public ICollection<Instructor> instructors { get; set; } = new List<Instructor>();
        public ICollection<Semester> semesters { get; set; } = new List<Semester>();
        public ICollection<Course> courses { get; set; } = new List<Course>();
        public ICollection<Department> departments { get; set; } = new List<Department>();
        public ICollection<Grade> grades { get; set; } = new List<Grade>();


    }
}
