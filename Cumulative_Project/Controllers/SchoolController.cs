using System;
using Cumulative_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative_Project.Controllers
{
	public class SchoolController : Controller
    {
        // GET: School
        public ActionResult Index()
        {
            return View();
        }

        // GET : /School/StudentList
        public ActionResult StudentList()
        {
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> students = controller.ListStudents();
            return View(students);
        }

        // GET: /School/ClassesList
        public ActionResult ClassesList()
        {
            ClassListDataController controller = new ClassListDataController();
            IEnumerable<Class> classessList = controller.ListClasses();
            return View(classessList);
        }
    }
}

