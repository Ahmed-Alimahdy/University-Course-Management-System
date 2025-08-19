namespace universityManagementSys.DTOs.Students
{
    public class ReadStudentDTO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string departmentName { get; set; }
    }
}
