using MySql.Data.MySqlClient;

namespace Project.DAL
{
    public static class UserRepository
    {
        // Connection string for connecting to the MySQL database.It contains necessary information like server, database name, username, and password.
        private static readonly DatabaseHelper _dbHelper = new();

        public static bool IsUserExist(string username)
        {
            try
            {
                const string query = "SELECT COUNT(*) FROM Users WHERE Name = @Name";
                MySqlParameter[] parameters = [new("@Name", username)];

                return Convert.ToInt32(_dbHelper.ExecuteScalar(query, parameters)) > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool AddUser(string username, string email, string hashedPassword)
        {
            try
            {
                const string query = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                MySqlParameter[] parameters = [
            new ("@Name", username),
            new ("@Email", email),
            new ("@Password", hashedPassword)
        ];

                _dbHelper.ExecuteQuery(query, parameters);
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static string? GetStoredPassword(string username)
        {
            try
            {
                const string query = "SELECT Password FROM Users WHERE Name = @Name";
                MySqlParameter[] parameters = [new("@Name", username)];

                var result = _dbHelper.ExecuteScalar(query, parameters);
                return result?.ToString();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        // Method to verify the role of the user (whether they are a librarian or not)
        public static bool VerifyRole(string username)
        {
            try
            {
                const string query = "SELECT Role FROM Users WHERE Name = @Name";
                MySqlParameter[] parameters = [new("@Name", username)];

                string? role = _dbHelper.ExecuteScalar(query, parameters)?.ToString();
                return role == "librarian";
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}