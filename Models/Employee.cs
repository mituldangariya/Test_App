using System.ComponentModel.DataAnnotations;

namespace Test_App.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public decimal? Salary { get; set; }

        public string? Photo { get; set; }
        public IFormFile PhotoFile { get; set; }
    }
}
