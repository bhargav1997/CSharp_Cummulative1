using System;
using Cumulative_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative_Project.Controllers
{
	public class ClassController : Controller
    {
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Class/List
        public ActionResult List()
        {
            ClassListDataController controller = new ClassListDataController();
            IEnumerable<Class> classessList = controller.ListClasses();
            return View(classessList);
        }

        // GET: /Class/UpdateTeacher/{id}
        public ActionResult UpdateTeacher(int id)
        {
            ViewBag.TeacherId = id;

            TeacherDataController teacherController = new TeacherDataController();
            Teacher teacherData = teacherController.SearchTeacherById(id);

            if (teacherData != null)
            {
                ViewBag.TeacherName = teacherData.teacherFname + " " + teacherData.teacherLname;
            }


            ClassListDataController controller = new ClassListDataController();
            IEnumerable<Class> classesList = controller.ListClasses();

            return View(classesList);
        }

        /// <summary>
        /// Update Teacher Id to class
        /// </summary>
        /// <param name="classId">The ID of the class to update the teacher for.</param>
        /// <param name="teacherId">The ID of the teacher to assign to the class.</param>
        /// <returns>A dynamic webpage which provides the current information of the Teacher.</returns>
        /// <example>
        /// POST : /Class/Edit
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///    "classId":10,
        ///    "&teacherId":20
        /// }
        /// </example>
        [HttpPost]
        public ActionResult Edit(int classId, int teacherId)
        {
            ClassListDataController controller = new ClassListDataController();
            controller.UpdateClassTeacher(teacherId, classId);

            return Redirect(Url.Action("Show", "Teacher", new { id = teacherId }));
        }
    }
}

