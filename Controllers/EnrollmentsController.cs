using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESTUDO.MVC.Data;
using ESTUDO.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESTUDO.MVC___backup.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _database;
        public EnrollmentsController(ApplicationDbContext database, UserManager<IdentityUser> userManager)
        {
            _database = database;
            _userManager = userManager;
        }

        [Authorize(Policy="UserRoleStudent")]
        public async Task<IActionResult> Enroll(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var enrollmentsFromDB = _database.Enrollments.Where(e => e.User == user).Include(e => e.Course).ToList();

            foreach (var enrolled in enrollmentsFromDB)
            {
                if(enrolled.Course.Id == id)
                {
                    return View("Error", enrolled.Course.Name);
                }
            }

            Enrollment newEnroll = new Enrollment();
            newEnroll.User = user;
            newEnroll.Course = _database.Courses.First(registry => registry.Id == id);

            _database.Enrollments.Add(newEnroll);
            _database.SaveChanges();

            return RedirectToAction("EnrollList");
        }

        public async Task<IActionResult> EnrollList()
        {
            var user = await _userManager.GetUserAsync(User);
            var enrollmentsFromDB = _database.Enrollments.Where(e => e.User == user).Include(e => e.Course).ToList();

            List<Course> coursesEnrolled = new List<Course>();

            foreach (Enrollment enrolled in enrollmentsFromDB)
            {
                coursesEnrolled.Add(enrolled.Course); 
            }
            return View("EnrollList", coursesEnrolled);
        }

        public async Task<IActionResult> Unenroll(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var enrolled = _database.Enrollments.Where(e => e.User == user).Include(e => e.Course).First(registry => registry.Course.Id == id);
            _database.Enrollments.Remove(enrolled);
            _database.SaveChanges();
            return RedirectToAction("EnrollList");
        }
    }
}