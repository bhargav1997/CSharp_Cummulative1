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
        /// <example>GET Teacher/ListTeachers</example>
        /// <returns>
        /// A list of Teachers
        /// </returns>
        [HttpGet]
        [Route("api/Teacher/ListTeachers/{SearchKey?}")]
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
        public Teacher FindTeacher(int id)
        {
            Teacher newTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select t.teacherid, t.teacherfname, t.teacherlname, t.employeenumber, t.salary, cls.classcode, cls.startdate,  cls.finishdate, cls.classname from teachers AS t JOIN classes AS cls ON t.teacherid = cls.teacherid WHERE t.teacherid = " + id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                newTeacher.teacherId = (int)ResultSet["teacherid"];
                newTeacher.teacherFname = ResultSet["teacherfname"].ToString();
                newTeacher.teacherLname = ResultSet["teacherlname"].ToString();
                newTeacher.teacherEmployeeNumber = ResultSet["employeenumber"].ToString();
                newTeacher.teacherHireDate = ResultSet["hiredate"].ToString();
                newTeacher.salary = (decimal)ResultSet["salary"];
            }


            return newTeacher;
        }
    }
}

