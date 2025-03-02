using Project.BLL;
using Project.DAL;

namespace Project.PL
{
    // The main class for handling the library application logic
    public static class LibraryApp
    {
        // Method for displaying the librarian menu
        public static void MenuLibrarian()
        {
            while (true)
            {
                try
                {
                    // Display librarian menu options with colors and a cleaner layout
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("           Welcome to the Librarian Menu      ");
                    Console.WriteLine("==============================================");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n           === Librarian Menu ===            ");
                    Console.ResetColor();
                    Console.WriteLine("==============================================");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  Please choose an option from the menu below:");
                    Console.WriteLine();

                    // Display the menu options with colors to make each option stand out
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  ===========================================");
                    Console.WriteLine("  |   1. Book List                          |");
                    Console.WriteLine("  |   2. Add Book                           |");
                    Console.WriteLine("  |   3. Search Book                        |");
                    Console.WriteLine("  |   4. Student List                       |");
                    Console.WriteLine("  |   5. Borrow Record List                 |");
                    Console.WriteLine("  |   6. Return Book                        |");
                    Console.WriteLine("  |   7. Update Book                        |");
                    Console.WriteLine("  |                                         |");
                    Console.WriteLine("  |   0. Logout                             |");
                    Console.WriteLine("  ===========================================\n");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Enter your choice: ");

                    // Read user choice for librarian menu
                    string? choice = Console.ReadLine();
                    Console.Clear();  // Clear the screen for the next menu

                    // Handle each librarian menu option
                    switch (choice)
                    {
                        case "0":
                            // Display a visually appealing logout message
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("==============================================");
                            Console.WriteLine("           You have successfully logged out!  ");
                            Console.WriteLine("==============================================");
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nGoodbye and have a great day!");
                            Console.ResetColor();

                            WaitForKeyPress();
                            return;  // Log out and return to the main menu

                        case "1":
                            Book.DisplayBookList();  // Display list of books
                            WaitForKeyPress();
                            break;

                        case "2":
                            Book.AddBook();  // Add a new book
                            break;

                        case "3":
                            Book.SearchBooks();  // Search for a book
                            break;

                        case "4":
                            Student.DisplayStudentList();  // Display list of students
                            break;

                        case "5":
                            BorrowedReturn.BookRecordList();  // View borrowing/return records
                            break;

                        case "6":
                            BorrowedReturn.ReturnBook();  // Return a borrowed book
                            break;

                        case "7":
                            Book.UpdateBook();
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red; 
                            Console.WriteLine("Invalid choice!");
                            WaitForKeyPress();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exception that occurs in the librarian menu
                    Console.WriteLine($"Error in Librarian Menu: {ex.Message}");
                    WaitForKeyPress();
                }
            }
        }

        // Method for displaying the student menu
        public static void MenuStudents(string userName)
        {
            int studentId = StudentRepository.MyId(userName);
            while (true)
            {
                try
                {
                    // Display student menu options with colors and a cleaner layout
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("           Welcome to the Student Menu       ");
                    Console.WriteLine("==============================================");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n           === Student Menu ===             ");
                    Console.WriteLine("Your ID: " + studentId);
                    Console.ResetColor();
                    Console.WriteLine("==============================================");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  Please choose an option from the menu below:");
                    Console.WriteLine();

                    // Display the menu options with colors
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" ==============================================");
                    Console.WriteLine(" | 1. Book List                               |");
                    Console.WriteLine(" | 2. Search Book                             |");
                    Console.WriteLine(" | 3. Borrow Book                             |");
                    Console.WriteLine(" | 4. My Book Record List                     |");
                    Console.WriteLine(" |                                            |");
                    Console.WriteLine(" | 0. Logout                                  |");
                    Console.WriteLine(" ==============================================");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Enter your choice: ");

                    // Read user choice for student menu
                    string? choice = Console.ReadLine();
                    Console.Clear();  // Clear the screen for the next menu

                    // Handle each student menu option
                    switch (choice)
                    {
                        case "0":
                            // Display a visually appealing logout message
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("==============================================");
                            Console.WriteLine("           You have successfully logged out!  ");
                            Console.WriteLine("==============================================");
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nGoodbye and have a great day!");
                            Console.ResetColor();

                            WaitForKeyPress();
                            return;  // Log out and return to the main menu

                        case "1":
                            Book.DisplayBookList();  // Display list of books
                            WaitForKeyPress();
                            break;
                        case "2":
                            Book.SearchBooks();  // Search for a book
                            break;

                        case "3":
                            BorrowedReturn.BorrowBook(studentId, userName);  // Borrow a book
                            break;

                        case "4":
                            Student.StudentBookRecordList(studentId);  // View student's borrowing records
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid choice!");
                            WaitForKeyPress();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exception that occurs in the student menu
                    Console.WriteLine($"Error in Student Menu: {ex.Message}");
                    WaitForKeyPress();
                }
            }
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