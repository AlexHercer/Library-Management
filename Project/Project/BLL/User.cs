using Project.DAL;
using Project.PL;
using System.Text.RegularExpressions;

namespace Project.BLL
{
    public static class User
    {
        // Method to handle user login
        public static void DisplayLoginScreen()
        {
            try
            {
                // Clear the console for a fresh screen
                Console.Clear();

                // Add BookName and header with color
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           Welcome to the Library System      ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                // Login screen with colorized prompts
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n           === Login Screen ===           ");

                Console.WriteLine("==============================================");
                Console.WriteLine("       === Type 'exit' to return ==\n");

                // Prompt for username with green color
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Enter username: ");
                Console.ResetColor();
                string? username = Console.ReadLine();
                if (!string.IsNullOrEmpty(username) && username.Equals("exit", StringComparison.OrdinalIgnoreCase))

                {
                    return;
                }

                // Clear the screen before password input
                Console.Clear();

                // Add BookName and header with color
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           Welcome to the Library System      ");
                Console.WriteLine("==============================================\n");
                Console.ResetColor();

                // Display login screen again after clearing
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("           === Login Screen ===             ");
                Console.ResetColor();
                Console.WriteLine("==============================================");

                // Prompt for password with green color
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Enter password: ");
                Console.ResetColor();
                string? password = "";

                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter)
                        break;

                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password[..^1];
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        password = $"{password}{key.KeyChar}";
                        Console.Write("*");
                    }
                }

                // Clear the console for result display
                Console.Clear();

                // Create a new user and attempt authentication
                bool isAuthenticated = User.AuthenticateUser(username!, password);

                // Display authentication result
                if (isAuthenticated)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("|              Login Successful!             |");
                    Console.WriteLine("|         Welcome back to the system!        |");
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                    WaitForKeyPress();

                    // Check user role (librarian or student)
                    bool role = User.RoleVerify(username!);
                    if (role)
                    {
                        // If librarian, show librarian menu
                        LibraryApp.MenuLibrarian();
                    }
                    else
                    {
                        // If student, show student menu
                        LibraryApp.MenuStudents(username!);
                    }
                }
                else
                {
                    // If authentication fails
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("|        Invalid username or password!       |");
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                    WaitForKeyPress();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions during login
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Login error: {ex.Message}");
                Console.ResetColor();
                WaitForKeyPress();
            }
        }

        // Method to handle user registration
        public static void DisplayRegisterScreen()
        {
            try
            {
                // Add BookName with color and border
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           Welcome to the Library System      ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                // Registration screen with color and clear instructions
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n           === Register Screen ===           ");
                Console.ResetColor();
                Console.WriteLine("==============================================");
                Console.WriteLine("           === Type 'exit' to return ==\n");

                string? username, password, email;

                // Get user input
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter username: ");
                    Console.ResetColor();
                    username = Console.ReadLine();

                    if (!string.IsNullOrEmpty(username) && username.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    if (UserRepository.IsUserExist(username!))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("This user already exists. Please choose a different name.        ");
                        Console.ResetColor();

                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        continue;
                    }

                    if (!Regex.IsMatch(username!, "^[a-zA-Z0-9]"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid username! It can only contain letters and numbers.           ");
                        Console.ResetColor();

                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);

                        continue;
                    }

                    if (!Regex.IsMatch(username!, "^.{4,}$"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid username! It must be at least 4 characters long.         ");
                        Console.ResetColor();

                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);

                        continue;
                    }

                    break;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           Welcome to the Library System      ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                // Registration screen with color and clear instructions
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n           === Register Screen ===           ");
                Console.ResetColor();
                Console.WriteLine("==============================================");
                Console.WriteLine("           === Type 'exit' to return ==\n");

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter password: ");
                    Console.ResetColor();
                    password = Console.ReadLine();

                    if (!string.IsNullOrEmpty(username) && username.Equals("exit", StringComparison.OrdinalIgnoreCase))

                    {
                        return;
                    }

                    if (password!.Length < 6)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid password! It must be at least 6 characters long.");
                        Console.ResetColor();

                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);

                        continue;
                    }

                    break;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           Welcome to the Library System      ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                // Registration screen with color and clear instructions
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n           === Register Screen ===           ");
                Console.ResetColor();
                Console.WriteLine("==============================================");
                Console.WriteLine("           === Type 'exit' to return ==\n");

                do
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter email: ");
                    Console.ResetColor();
                    email = Console.ReadLine();

                    // Validate email format

                    if (!string.IsNullOrEmpty(username) && username.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    if (!IsValidEmail(email!))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid email format. Please try again.");
                        Console.ResetColor();
                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                    }
                } while (!IsValidEmail(email!));

                // Create a new user and attempt registration
                bool isRegistered = User.Register(username!, email!, password);

                // Display success or failure messages
                if (isRegistered)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("|          Registration successful!          |");
                    Console.WriteLine("==============================================");
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("|         Error during registration.         |");
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                }

                // Wait for user to press a key
                WaitForKeyPress();
            }
            catch (Exception ex)
            {
                Console.Clear();
                // Handle any exception that occurs
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("==============================================");
                Console.WriteLine($" Error during registration: {ex.Message}");
                Console.WriteLine("==============================================");
                Console.ResetColor();
                WaitForKeyPress();
            }
        }

        // Authenticate the user by verifying the password
        public static bool AuthenticateUser(string username, string enteredPassword)
        {
            // Check if the user exists
            if (UserRepository.IsUserExist(username))
            {
                // Get the stored hashed password from the database
                string? storedHashedPassword = UserRepository.GetStoredPassword(username);

                // Verify the entered password with the stored hashed password using bcrypt
                return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
            }
            else
            {
                return false;
            }
        }

        // Verify the user's role
        public static bool RoleVerify(string username)
        {
            return UserRepository.VerifyRole(username);
        }

        private static bool IsValidEmail(string email)
        {
            const string? pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        // Register a new user
        public static bool Register(string username, string email, string password)
        {
            // Check if the username already exists
            if (UserRepository.IsUserExist(username))
            {
                Console.WriteLine("Username already exists.");
                return false;
            }

            // Add the user to the database with bcrypt hashed password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return UserRepository.AddUser(username, email, hashedPassword);
        }

        // Method to pause and wait for the user to press any key
        public static void WaitForKeyPress()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();

            // Wait for the user to press any key
            Console.ReadKey(true);

            // Optionally clear the screen after the key press
            Console.Clear();
        }
    }
}