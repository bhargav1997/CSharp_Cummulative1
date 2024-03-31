using System;
using System.Diagnostics;
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

        //GET : /Teacher/New
        public ActionResult New()
        {
            return View();
        }

        //POST : /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string TeacherHireDate, string TeacherEmployeeNumber, string TeacherSalary)
        {
            //Identify that this method is running
            //Identify the inputs provided from the form

            Debug.WriteLine("I have accessed the Create Method!");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(TeacherHireDate);

            Teacher teacher = new Teacher();
            teacher.teacherFname = TeacherFname;
            teacher.teacherLname = TeacherLname;
            teacher.teacherHireDate = TeacherHireDate;
            teacher.teacherEmployeeNumber = TeacherEmployeeNumber;
            teacher.salary = Convert.ToDecimal(TeacherSalary);

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(teacher);

            return RedirectToAction("List");
        }

        // GET : /Teacher/List
        public ActionResult List(string name = null, string hireDate = null, string salary = null)
        {
            Debug.WriteLine(name);
            Debug.WriteLine(hireDate);
            Debug.WriteLine(salary);
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.ListTeachers(name, hireDate, salary);
            return View(teachers);
        }

        // GET : /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<TeacherLecturesClass> teacherLectureClassess = controller.FindTeacher(id);

            return View(teacherLectureClassess);
        }

        //GET : /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.SearchTeacherById(id);

            return View(NewTeacher);
        }


        //POST : /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }
    }
}

