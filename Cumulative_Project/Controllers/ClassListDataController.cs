﻿using System.Diagnostics;
using System.Web.Http;
using Cumulative_Project.Models;
using MySql.Data.MySqlClient;
using System.Web.Http.Cors;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using System.Reflection.Metadata;
using System.Net;

namespace Cumulative_Project.Controllers
{
	public class ClassListDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext school = new SchoolDbContext();

        //This Controller Will access the classes table of our school database.
        /// <summary>
        /// Returns a list of classes in the system
        /// </summary>
        /// <example>GET /api/classes/classlist</example>
        /// <returns>
        /// A list of classess
        /// </returns>
        [HttpGet]
        [Route("api/classes/classlist")]
        public IEnumerable<Class> ListClasses()
        {
            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //Only courses in the classes table which is no longer pointing to a teacher who no longer exists
            cmd.CommandText = "SELECT * FROM classes WHERE teacherid IN (SELECT teacherid FROM teachers);\n";


            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of classes
            List<Class> classes = new List<Class> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int classId = (int)ResultSet["classid"];

                string classCode = ResultSet["classcode"].ToString();
                int teacherid = Convert.ToInt32(ResultSet["teacherid"]);
                string startdate = ResultSet["startdate"].ToString();
                string finishdate = ResultSet["finishdate"].ToString();
                string classname = ResultSet["classname"].ToString();

                Class classData = new Class();
                classData.classid = classId;
                classData.classname = classname;
                classData.classcode = classCode;
                classData.startdate = startdate;
                classData.finishdate = finishdate;
                classData.teacherid = teacherid;


                //Add the Teacher to the List
                classes.Add(classData);
            }
            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of author names
            return classes;
        }

        /// <summary>
        /// Finds an class in the system given an Class ID
        /// </summary>
        /// <param name="id">The classId primary key</param>
        /// <returns>An class object</returns>
        [HttpGet]
        public Class FindClass(int id)
        {
            Class classData = new Class();
            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
               cmd.CommandText = "Select * from classes where classid = " + id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {

                int ClassId = (int)ResultSet["classid"];
                string ClassCode = ResultSet["classcode"].ToString();
                int TeacherId = (int)ResultSet["teacherid"];
                string StartDate = ResultSet["startdate"].ToString();
                string FinishDate = ResultSet["finishdate"].ToString();
                string ClassName = ResultSet["classname"].ToString();


                classData.classid = ClassId;

                classData.classcode = ClassCode;
                classData.teacherid = TeacherId;
                classData.startdate = StartDate;
                classData.finishdate = FinishDate;
                classData.classname = ClassName;
            }
            return classData;
        }

        /// <summary>
        /// Updates the teacher assigned to a specific class in the MySQL Database. 
        /// </summary>
        /// <param name="teacherId">The ID of the teacher to assign to the class.</param>
        /// <param name="classId">The ID of the class to update.</param>
        /// <remarks>
        /// If both teacherId and classId are provided, the function updates the class with the specified teacher. 
        /// If either teacherId or classId is not provided, the function returns an error.
        /// </remarks>
        /// <example>
        /// POST api/TeacherData/UpdateClassTeacher
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"teacherId":1,
        ///	"classId":2,
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateClassTeacher(int teacherId, int classId)
        {
            try
            {
                // Check if both teacherId and classId are provided
                if (teacherId == 0 || classId == 0)
                {
                    throw new ArgumentException("Both teacherId and classId are required.");
                }

                // Create an instance of a connection
                MySqlConnection Conn = school.AccessDatabase();

                // Open the connection between the web server and database
                Conn.Open();

                // Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                // SQL QUERY
                cmd.CommandText = "UPDATE classes SET teacherid=@TeacherId WHERE classid=@ClassId;";
                cmd.Parameters.AddWithValue("@TeacherId", teacherId);
                cmd.Parameters.AddWithValue("@ClassId", classId);

                cmd.Prepare();

                cmd.ExecuteNonQuery();

                Conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

                // internal server error
                throw new ArgumentException(ex.Message);
            }
        }

    }
}

