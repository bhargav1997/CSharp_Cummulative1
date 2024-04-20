using System;
using System.Diagnostics;
using System.Web.Http.Cors;
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
            ViewData["teacherId"] = id;
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

        /// <summary>
        /// Routes to a dynamically generated "Teacher Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Teacher</param>
        /// <returns>A dynamic "Update Teacher" webpage which provides the current information of the Teacher and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Teacher/Update/5</example>
        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.SearchTeacherById(id);

            return View(teacher);
        }

        /// <summary>
        /// Receives a POST request containing information about an existing Teacher in the system, with new values. Conveys this information to the API, and redirects to the "Teacher Show" page of our updated Teacher.
        /// </summary>
        /// <param name="id">Id of the Teacher to update</param>
        /// <param name="TeacherFname">The updated first name of the Teacher</param>
        /// <param name="TeacherLname">The updated last name of the Teacher</param>
        /// <param name="TeacherBio">The updated bio of the Teacher.</param>
        /// <param name="TeacherEmail">The updated email of the Teacher.</param>
        /// <returns>A dynamic webpage which provides the current information of the Teacher.</returns>
        /// <example>
        /// POST : /Teacher/Edit/10
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Bhargav",
        ///	"TeacherLname":"Suthar",
        ///	"TeacherBio":"Enjoy Coding!",
        ///	"TeacherEmail":"bhargav@gmail.com"
        /// }
        /// </example>
        [HttpPost]
        public ActionResult Edit(int id, string TeacherFname, string TeacherLname, string TeacherHireDate, string TeacherEmployeeNumber, string TeacherSalary)
        {
            Teacher teacher = new Teacher();
            teacher.teacherFname = TeacherFname;
            teacher.teacherLname = TeacherLname;
            teacher.teacherHireDate = TeacherHireDate;
            teacher.teacherEmployeeNumber = TeacherEmployeeNumber;
            teacher.salary = Convert.ToDecimal(TeacherSalary);

            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacherData(id, teacher);

            return Redirect(Url.Action("Show", new { id = id }));
        }
    }
}

