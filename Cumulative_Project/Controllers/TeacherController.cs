using System;
using Cumulative_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative_Project.Controllers
{
	public class TeacherController : Controller
	{
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        // GET : /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.ListTeachers(SearchKey);
            return View(teachers);
        }

        // GET : /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.FindTeacher(id);

            return View(teacher);
        }
    }
}

