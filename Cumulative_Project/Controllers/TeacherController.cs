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
        public ActionResult List(string name = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.ListTeachers(name);
            return View(teachers);
        }

        // GET : /Teacher/FilterMore
        public ActionResult FilterMore(string name = null, string hireDate = null, string salary = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.ListTeachers(name);
            return View(teachers);
        }

        // GET : /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<TeacherLecturesClass> teacherLectureClassess = controller.FindTeacher(id);

            return View(teacherLectureClassess);
        }
    }
}

