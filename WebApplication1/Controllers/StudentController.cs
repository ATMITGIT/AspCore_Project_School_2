using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationContext db;
        public StudentController(ApplicationContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View("register");
        }
        public IActionResult Registr(string Name, string Email, string Password, string Confirm)
        {
            if (Password != Confirm)
            {
                ViewData["con"] = "password is not equal confrim password";
                return View("register");
            }
            else
            {
                Student s = new Student { Name = Name, Email = Email, Password = Password };
                db.Add(s);
                db.SaveChanges();
                return RedirectPermanent("~/Student/Course");
            }

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email,string Password,string Confrim)
        {
            bool t = db.Students.Any(x => x.Email == Email && x.Password == Password);
            if (Password != Confrim)
            {
                ViewData["con"] = "password is not equal confrim password";
                return View("login");
            }
            else
            {
                return RedirectPermanent("~/Student/Course");
            }
          
        }
        public IActionResult Course()
        {
            return View();
        }
    }
    

}