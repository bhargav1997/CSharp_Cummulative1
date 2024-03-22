﻿using System;
using Cumulative_Project.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative_Project.Controllers
{
	public class ClassListDataController : Controller
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

            //SQL QUERY
            cmd.CommandText = "Select * from classes";


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
    }
}
