using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }  //  
        [EmailAddress(ErrorMessage ="Email adresi uygun formatta değil")]
        public string Email { get; set; }
    }
}
