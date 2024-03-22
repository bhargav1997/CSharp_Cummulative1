using System;
using System.Web.Http;
using Cumulative_Project.Models;
using MySql.Data.MySqlClient;

namespace Cumulative_Project.Controllers
{
	public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext school = new SchoolDbContext();

        //This Controller Will access the Teachers table of our school database.
        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example>GET Teacher/ListTeachers/1</example>
        /// <returns>
        /// A list of Teachers
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();


            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int teacherId = (int)ResultSet["teacherid"];

                string teacherFname = ResultSet["teacherfname"].ToString();
                string teacherLname = ResultSet["teacherlname"].ToString();
                string teacherEmployeeNumber = ResultSet["employeenumber"].ToString();
                string hiredate = ResultSet["hiredate"].ToString();
                decimal salary = (decimal)ResultSet["salary"];

                Teacher newTeacher = new Teacher();
                newTeacher.teacherId = teacherId;
                newTeacher.teacherFname = teacherFname;
                newTeacher.teacherLname = teacherLname;
                newTeacher.teacherEmployeeNumber = teacherEmployeeNumber;
                newTeacher.teacherHireDate = hiredate;
                newTeacher.salary = salary;


                //Add the Teacher to the List
                teachers.Add(newTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of author names
            return teachers;
        }

        /// <summary>
        /// Finds an teacher in the system given an ID
        /// </summary>
        /// <param name="id">The teacher primary key</param>
        /// <returns>An teacher object</returns>
        [HttpGet]
        public IEnumerable<TeacherLecturesClass> FindTeacher(int id)
        {
            List<TeacherLecturesClass> teachersLecturesList = new List<TeacherLecturesClass> { };

            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select t.teacherid, t.teacherfname, t.hiredate, t.teacherlname, t.employeenumber, t.salary, cls.classcode, cls.startdate,  cls.finishdate, cls.classname from teachers AS t JOIN classes AS cls ON t.teacherid = cls.teacherid WHERE t.teacherid = " + id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                TeacherLecturesClass teacherLectures = new TeacherLecturesClass();
                //Access Column information by the DB column name as an index
                teacherLectures.teacherId = (int)ResultSet["teacherid"];
                teacherLectures.teacherFname = ResultSet["teacherfname"].ToString();
                teacherLectures.teacherLname = ResultSet["teacherlname"].ToString();
                teacherLectures.teacherEmployeeNumber = ResultSet["employeenumber"].ToString();
                teacherLectures.hiredate = ResultSet["hiredate"].ToString();
                teacherLectures.salary = (decimal)ResultSet["salary"];
                teacherLectures.classcode = ResultSet["classcode"].ToString();
                teacherLectures.startdate = ResultSet["startdate"].ToString();
                teacherLectures.finishdate = ResultSet["finishdate"].ToString();
                teacherLectures.classname = ResultSet["classname"].ToString();

                teachersLecturesList.Add(teacherLectures);
            }


            return teachersLecturesList;
        }
    }
}

