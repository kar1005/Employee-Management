/*using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Employee_ManagementSystem.Models
{
    public class Bio
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Employee Name")]
        public int EmployeeId { get; set; }
        public required Employee Employee { get; set; }

        [DisplayName("Date of Birth")]
        public required DateOnly DateOfBirth { get; set; } //change datatype to DateTime

        [DisplayName("Aadhar card number")]
        public required string Aadhar { get; set; }

        [DisplayName("Emergency Contact")]
        public string EmergencyContact { get; set; }

        [DisplayName("Educational Qualifications")]
        public string Qualifications { get; set; }

        [DisplayName("Hobbies")]
        public string Hobbies { get; set; }

        [DisplayName("Social Media Links")]
        public string SocialHandles { get; set; }

        
    }
}*/



using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Employee_ManagementSystem.Models
{
    public class Bio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Employee Name")]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        [Required]
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }  // Changed from DateOnly to DateTime

        [Required]
        [DisplayName("Aadhar card number")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhar number must be 12 digits")]
        public string Aadhar { get; set; } = string.Empty;

        [Required]
        [DisplayName("Emergency Contact")]
        [Phone]
        public string EmergencyContact { get; set; } = string.Empty;

        [DisplayName("Educational Qualifications")]
        public string? Qualifications { get; set; }

        [DisplayName("Hobbies")]
        public string? Hobbies { get; set; }

        [DisplayName("Social Media Links")]
        public string? SocialHandles { get; set; }
    }
}
