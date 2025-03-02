using Project.BLL;

namespace Project.PL
{
    internal static class Program
    {
        // The Main method is the entry point of the application.
        private static void Main()
        {// The main entry point of the application
            while (true)
            {
                try
                {
                    // Display the main menu options for the user
                    Console.Clear();

                    // ASCII logo
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" _       _ ______  ______  _______ ______  _     _ ");
                    Console.WriteLine("| |     | (____  \\(_____ \\(_______|_____ \\| |   | |");
                    Console.WriteLine("| |     | |____)  )_____) )_______ _____) ) |___| |");
                    Console.WriteLine("| |     | |  __  (|  __  /|  ___  |  __  /|_____  |");
                    Console.WriteLine("| |_____| | |__)  ) |  \\ \\| |   | | |  \\ \\ _____| |");
                    Console.WriteLine("|_______)_|______/|_|   |_|_|   |_|_|   |_(_______|");

                    Console.WriteLine();

                    // Main menu with better structure and colors
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" ********************************************");
                    Console.WriteLine("*  Welcome to the Library Management System  *");
                    Console.WriteLine(" ********************************************");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("  ===========================================");
                    Console.WriteLine("  |  0. Exit                                |");
                    Console.WriteLine("  |  1. Login                               |");
                    Console.WriteLine("  |  2. Sign Up                             |");
                    Console.WriteLine("  ===========================================");
                    Console.WriteLine();

                    // User prompt for selection
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Enter your choice: ");
                    string? choice = Console.ReadLine();
                    Console.Clear();  // Clear the screen for a cleaner user experience

                    // Based on user input, navigate to the appropriate action
                    switch (choice)
                    {
                        case "0":  // Exit the application
                            return;

                        case "1":  // Call the login screen method
                            User.DisplayLoginScreen();
                            break;

                        case "2":  // Call the registration screen method
                            User.DisplayRegisterScreen();
                            break;

                        default:  // Handle invalid choices
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid choice!");
                            WaitForKeyPress();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // If an error occurs, display an error message
                    Console.WriteLine($"Error: {ex.Message}");
                    WaitForKeyPress();
                }
            }  // Keep looping until the user exits the application
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