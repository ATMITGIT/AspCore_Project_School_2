using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminCourseController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private ApplicationContext db;
        public AdminCourseController(IHostingEnvironment IHostingEnvironment,ApplicationContext t)
        {
            _environment = IHostingEnvironment;
            db = t;
        }
        public IActionResult Index()
        {
            return View("admincourse");
        }
        public IActionResult What()
        {
            ViewData["course"] = db.Courses.ToList();
            return View("whadminadd");
        }
        public IActionResult Level()
        {
            return View("bglevel");
        }
          public IActionResult Video()
        {
            return View("addVideo");
        }
        public IActionResult addCourse(string title,string shortt)
        {
            var newFileName = string.Empty;

            if (HttpContext.Request.Form.Files != null)
            {
                var fileName = string.Empty;
                string PathDB = string.Empty;

                var files = HttpContext.Request.Form.Files;

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //Getting FileName
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var FileExtension = Path.GetExtension(fileName);

                        // concating  FileName + FileExtension
                        newFileName = myUniqueFileName + FileExtension;

                        // Combines two strings into a path.
                        fileName = Path.Combine(_environment.WebRootPath, "Files") + $@"\{newFileName}";

                        // if you want to store path of folder in database
                        PathDB = "Files/" + newFileName;

                        using (FileStream fs = System.IO.File.Create(fileName))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                        /*    FileModel m = new FileModel { Path = PathDB };
                            a.Add(m);
                            a.SaveChanges();*/
                        Course c = new Course {CourseImage=PathDB,CourseTitle=title,CourseDescription=shortt };
                        db.Courses.Add(c);
                        db.SaveChanges();
                    }
                }


            }
            return Redirect("Index");
        }
    }
}