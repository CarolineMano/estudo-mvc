using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESTUDO.MVC.Data;
using ESTUDO.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESTUDO.MVC.Controllers
{
    public class CoursesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _database;
        public CoursesController(ApplicationDbContext database, UserManager<IdentityUser> userManager)
        {
            _database = database;
            _userManager = userManager;
        }
        public IActionResult List()
        {
            var courses = _database.Courses.ToList();
            return View(courses);
        }
        [Authorize(Policy="UserRoleFaculty")]
        public IActionResult Add()
        {
            return View();
        }
        public IActionResult Save(Course course)
        {
            if(course.Id == 0)
            {
                _database.Courses.Add(course);
            }
            else
            {
                var courseFromDB = _database.Courses.First(registry => registry.Id == course.Id);
                courseFromDB.Name = course.Name;
                courseFromDB.CourseLoad = course.CourseLoad;
                courseFromDB.Price = course.Price;
            }
            _database.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Edit(int id)
        {
            var course = _database.Courses.First(registry => registry.Id == id);
            return View("Add", course);
        }
        public IActionResult Delete(int id)
        {
            var course = _database.Courses.First(registry => registry.Id == id);
            
            try
            {
                _database.Courses.Remove(course);
                _database.SaveChanges();
            }
            catch (System.Exception)
            {
                return View("Error");
            }          
            return RedirectToAction("List");
        }

    }
}