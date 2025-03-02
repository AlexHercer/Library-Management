using MySql.Data.MySqlClient;
using Project.BLL;

namespace Project.DAL
{
    // The 'StudentRepository' class is responsible for interacting with the database to retrieve or search student data.
    public static class StudentRepository
    {
        // Connection string for connecting to the MySQL database. It contains necessary information like server, database name, username, and password.
        private static readonly DatabaseHelper _dbHelper = new();

        public static List<Student> Students
        {
            get
            {
                List<Student> students = [];
                const string query = "SELECT * FROM Users WHERE Role = 'student'";

                try
                {
                    using var reader = _dbHelper.ExecuteReader(query);
                    while (reader.Read())
                    {
                        Student student = new()
                        {
                            StudentId = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : 0,
                            Name = reader["Name"] as string ?? string.Empty,
                            Email = reader["Email"] as string ?? string.Empty,
                        };
                        students.Add(student);
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Database error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                return students;
            }
        }

        public static int MyId(string name)
        {
            try
            {
                const string query = "SELECT ID FROM Users WHERE Role='student' AND Name LIKE @Name";
                MySqlParameter[] parameters = [new("@Name", "%" + name + "%")];

                using var reader = _dbHelper.ExecuteReader(query, parameters);
                if (reader.Read())
                {
                    return Convert.ToInt32(reader["ID"]);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return -1;
        }

        // Method to search for a student by their name. The search is case-insensitive and looks for partial matches.
        public static void Search(string name)
        {
            try
            {
                const string query = "SELECT * FROM Users WHERE Role='student' AND Name LIKE @Name";
                MySqlParameter[] parameters = [new("@Name", "%" + name + "%")];

                using var reader = _dbHelper.ExecuteReader(query, parameters);
                Console.WriteLine("Search Results:");
                if (!reader.HasRows)
                {
                    Console.WriteLine("No students found.");
                }

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["ID"]}, Name: {reader["Name"]}, Email: {reader["Email"]}");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}