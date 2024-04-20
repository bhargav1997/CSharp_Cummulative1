using System.Diagnostics;
using System.Web.Http;
using Cumulative_Project.Models;
using MySql.Data.MySqlClient;
using System.Web.Http.Cors;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using System.Reflection.Metadata;
using System.Net;

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
        [Route("api/TeacherData/ListTeachers/{name?}/{hireDate?}/{salary?}")]
        public IEnumerable<Teacher> ListTeachers(string name = null, string hireDate = null, string salary = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            string Query = "SELECT * FROM teachers ";

            if (name != null || hireDate != null || salary != null)
            {
                Query += "WHERE ";
                if (name != null)
                {
                    Query += "(lower(teacherfname) LIKE lower(@key) " +
                             "OR lower(teacherlname) LIKE lower(@key) " +
                             "OR lower(concat(teacherfname, ' ', teacherlname)) LIKE lower(@key)) ";
                }

                if (hireDate != null)
                {
                    if (name != null)
                    {
                        Query += "AND ";
                    }
                    Query += "lower(hiredate) LIKE lower(@hiredate) ";
                }

                if (salary != null)
                {
                    if (name != null || hireDate != null)
                    {
                        Query += "AND ";
                    }
                    Query += "lower(salary) LIKE lower(@salary) ";
                }
            }

            // SQL QUERY
            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@key", name != null ? "%" + name + "%" : DBNull.Value);
            cmd.Parameters.AddWithValue("@hiredate", hireDate != null ? "%" + hireDate + "%" : DBNull.Value);
            cmd.Parameters.AddWithValue("@salary", salary != null ? "%" + salary + "%" : DBNull.Value);
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
                decimal salaryData = (decimal)ResultSet["salary"];

                Teacher newTeacher = new Teacher();
                newTeacher.teacherId = teacherId;
                newTeacher.teacherFname = teacherFname;
                newTeacher.teacherLname = teacherLname;
                newTeacher.teacherEmployeeNumber = teacherEmployeeNumber;
                newTeacher.teacherHireDate = hiredate;
                newTeacher.salary = salaryData;


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
            cmd.CommandText = "Select t.teacherid, t.teacherfname, t.hiredate, t.teacherlname, t.employeenumber, t.salary, cls.classcode, cls.startdate,  cls.finishdate, cls.classname from teachers AS t " +
                "LEFT JOIN classes AS cls ON t.teacherid = cls.teacherid WHERE t.teacherid = " + id;

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

        // <summary>
        /// Finds a Teacher in the system given an ID
        /// </summary>
        /// <param name="id">The Teacher ID primary key</param>
        /// <returns>Teacher object</returns>
        [HttpGet]
        public Teacher SearchTeacherById(int id)
        {
            Teacher newTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where teacherid = " + id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int teacherId = (int)ResultSet["teacherid"];
                string teacherFname = ResultSet["teacherfname"].ToString();
                string teacherLname = ResultSet["teacherlname"].ToString();
                string teacherEmployeeNumber = ResultSet["employeenumber"].ToString();
                string hiredate = ResultSet["hiredate"].ToString();
                decimal salary = Convert.ToDecimal(ResultSet["salary"]);

                newTeacher.teacherId = teacherId;

                newTeacher.teacherFname = teacherFname;
                newTeacher.teacherLname = teacherLname;
                newTeacher.teacherEmployeeNumber = teacherEmployeeNumber;
                newTeacher.teacherHireDate = hiredate;
                newTeacher.salary = salary;

            }


            return newTeacher;
        }

        /// <summary>
        /// Adds an Teacher to the MySQL Database.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the teacher's table. Non-Deterministic.</param>
        /// <example>
        /// POST api/TeacherData/AddTeacher 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Bhargav",
        ///	"TeacherLname":"Suthar",
        ///	"TeacherEmployeeNumber":"T703",
        ///	"TeacherHireDate":"2024-07-03",
        ///	"TeacherSalary": "73"
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            Debug.WriteLine(NewTeacher.teacherFname);
            // Check for missing information
            if (string.IsNullOrEmpty(NewTeacher.teacherFname) ||
                string.IsNullOrEmpty(NewTeacher.teacherLname) ||
                string.IsNullOrEmpty(NewTeacher.teacherEmployeeNumber) ||
                string.IsNullOrEmpty(NewTeacher.teacherHireDate) ||
                NewTeacher.salary <= 0)
            {
                // Handle missing information
                throw new ArgumentException("Missing information. Please provide all required fields.");
            }

            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            Debug.WriteLine(NewTeacher.teacherFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "INSERT INTO teachers(teacherfname, teacherlname, employeenumber, hiredate, salary) VALUES (@TeacherFname,@TeacherLname,@TeacherEmployeeNumber,@TeacherHireDate,@Salary);";

            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.teacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.teacherLname);
            cmd.Parameters.AddWithValue("@TeacherEmployeeNumber", NewTeacher.teacherEmployeeNumber);
            cmd.Parameters.AddWithValue("@TeacherHireDate", NewTeacher.teacherHireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Deletes an Teacher from the connected MySQL Database if the ID of that teacher exists. Does NOT maintain relational integrity.
        /// Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the Teacher.</param>
        /// <example>POST /api/TeacherData/DeleteTeacher/3</example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Updates an Teacher on the MySQL Database. 
        /// </summary>
        /// <param name="TeacherInfo">An object with fields that map to the columns of the Teacher's table.</param>
        /// <example>
        /// POST api/TeacherData/UpdateTeacherData/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Bhargav",
        ///	"TeacherLname":"Suthar",
        ///	"TeacherEmployeeNumber":"T703",
        ///	"TeacherHireDate":"2024-07-03",
        ///	"TeacherSalary": "73"
        /// }
        /// </example>
        [HttpPost]
        [Route("/TeacherData/UpdateTeacherData")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateTeacherData(int id, [FromBody] Teacher teacher)
        {
            // Check if essential information is missing
            if (string.IsNullOrEmpty(teacher.teacherFname) ||
                string.IsNullOrEmpty(teacher.teacherLname) ||
                string.IsNullOrEmpty(teacher.teacherEmployeeNumber) ||
                string.IsNullOrEmpty(teacher.teacherHireDate) ||
                teacher.salary <= 0)
            {
                // Return an appropriate response indicating missing information
                int StatusCode = (int)HttpStatusCode.BadRequest;

                // Log the exception
                Console.WriteLine("Error: " + StatusCode);
                // Handle missing information
                throw new ArgumentException("Missing information. Please provide all required fields.");
            }

            try
            {
                //Create an instance of a connection
                MySqlConnection Conn = school.AccessDatabase();

                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "UPDATE teachers SET teacherfname=@TeacherFname, teacherlname=@TeacherLname, employeenumber=@TeacherEmployeeNumber, hiredate=@TeacherHireDate, salary=@Salary WHERE teacherid=@TeacherId";
                cmd.Parameters.AddWithValue("@TeacherFname", teacher.teacherFname);
                cmd.Parameters.AddWithValue("@TeacherLname", teacher.teacherLname);
                cmd.Parameters.AddWithValue("@TeacherEmployeeNumber", teacher.teacherEmployeeNumber);
                cmd.Parameters.AddWithValue("@TeacherHireDate", teacher.teacherHireDate);
                cmd.Parameters.AddWithValue("@Salary", teacher.salary);
                cmd.Parameters.AddWithValue("@TeacherId", id);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                Conn.Close();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, or return an appropriate response
                int StatusCode = (int)HttpStatusCode.InternalServerError;
                // Log the exception
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Status Code:" + StatusCode);
            }
        }
    }
}

