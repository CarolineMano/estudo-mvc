using Microsoft.AspNetCore.Identity;

namespace ESTUDO.MVC.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public Course Course { get; set; }
        public IdentityUser User { get; set; }
    }
}