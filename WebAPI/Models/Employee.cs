using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
