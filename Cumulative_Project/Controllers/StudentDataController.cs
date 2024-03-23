using System;
using System.Web.Http;
using Cumulative_Project.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace Cumulative_Project.Controllers
{
	public class StudentDataController
	{
		
		// The database context class which allows us to access our MySQL Database.
        private SchoolDbContext school = new SchoolDbContext();

        //This Controller Will access the Students table of our school database.
        /// <summary>
        /// Returns a list of students in the system
        /// </summary>
        /// <example>GET /api/student/liststudents</example>
        /// <returns>
        /// A list of Students
        /// </returns>
        [HttpGet]
        [Route("api/student/liststudents")]
        public IEnumerable<Student> ListStudents()
        {
            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from students";


            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Students
            List<Student> students = new List<Student> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int studentId = Convert.ToInt32(ResultSet["studentid"]) ;

                string studentFname = ResultSet["studentfname"].ToString();
                string studentLname = ResultSet["studentlname"].ToString();
                string studentnumber = ResultSet["studentnumber"].ToString();
                string enroldate = ResultSet["enroldate"].ToString();

                Student stuedent = new Student();
                stuedent.studentId = studentId;
                stuedent.studentfname = studentFname;
                stuedent.studentlname = studentLname;
                stuedent.studentnumber = studentnumber;
                stuedent.enroldate = enroldate;


                //Add the Teacher to the List
                students.Add(stuedent);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of author names
            return students;
        }

        // <summary>
        /// Finds a Student in the system given an ID
        /// </summary>
        /// <param name="id">The Student ID primary key</param>
        /// <returns>Student object</returns>
        [HttpGet]
        public Student FindStudent(int id)
        {
            Student newStudent = new Student();

            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from students where studentid = " + id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int StudentId = (int)ResultSet["studentid"];
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                string EnrolDate = ResultSet["enroldate"].ToString();

                newStudent.studentId = StudentId;

                newStudent.studentfname = StudentFname;
                newStudent.studentlname = StudentLname;
                newStudent.studentnumber = StudentNumber;
                newStudent.enroldate = EnrolDate;


            }


            return newStudent;
        }
    }
}

