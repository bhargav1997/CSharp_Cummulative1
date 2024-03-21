using System;


using MySql.Data.MySqlClient;


namespace Cumulative_Project.Models
{
	public class SchoolDbContext
	{
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "school"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "8889"; } }

        // ConnectionString is a series of credentials used to connect to the database.
        protected static string ConnectionString
        {
            get {

                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }

        //This is the method is use to get the database!
        /// <summary>
        /// Returns a connection to the School database.
        /// </summary>
        /// <example>
        /// private SchoolDbContext school = new SchoolDbContext();
        /// MySqlConnection Conn = school.AccessDatabase();
        /// </example>
        /// <returns>A MySqlConnection Object</returns>
        public MySqlConnection AccessDatabase()
        {
            //We are instantiating the MySqlConnection Class to create an object
            //the object is a specific connection to our school database on port 8888 of localhost
            return new MySqlConnection(ConnectionString);
        }
    }
}

