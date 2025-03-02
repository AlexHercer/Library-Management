using Project.DAL;

namespace Project.BLL
{
    public class Student
    {
        public int StudentId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public override string ToString()
        {
            return $"ID: {StudentId}, Name: {Name}, Email: {Email}";
        }

        // Method for displaying the student list in a table format
        public static void DisplayStudentList()
        {
            try
            {
                // Clear the screen to present a clean output
                Console.Clear();

                // Create a repository to fetch the list of students
                var students = StudentRepository.Students;

                // Header with a clean design and cyan color
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("                Student List                  ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                // Table header with yellow color for distinction
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("+-----+----------------------+--------------------------+");
                Console.WriteLine("| ID  | Name                 | Email                    |");
                Console.WriteLine("+-----+----------------------+--------------------------+");
                Console.ResetColor();

                // Display each student's details in a clean and readable format
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var student in students)
                {
                    Console.WriteLine(
                        $"| {student.StudentId,-3} | {student.Name,-20} | {student.Email,-24} |");
                }
                Console.ResetColor();

                // Footer with cyan color to match the header design
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("+-----+----------------------+--------------------------+");
                Console.ResetColor();

                // Wait for key press after displaying the list
                WaitForKeyPress();
            }
            catch (Exception ex)
            {
                // Handle any exception while displaying the student list in red
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Error while displaying student list: {ex.Message}");
                Console.ResetColor();
                WaitForKeyPress();
            }
        }

        // Method for displaying student's borrowing and returning records
        public static void StudentBookRecordList(int studentId)
        {
            try
            {
                // Display book list options with colors for clarity
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           Book Record Management            ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n           === Choose a Book Record ===     ");
                Console.ResetColor();
                Console.WriteLine("==============================================\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  Please choose an option from the menu below:");
                Console.WriteLine();

                // Display options with color for better visibility
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==============================================");
                Console.WriteLine("| 1. Borrowed Book List                      |");
                Console.WriteLine("| 2. Returned Book List                      |");
                Console.WriteLine("|                                            |");
                Console.WriteLine("| 0. Return                                  |");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Enter your choice: ");
                // Read user choice for student record view
                string? choice = Console.ReadLine();
                Console.Clear();  // Clear the screen for the next view

                // Handle each option for viewing borrowed and returned books
                switch (choice)
                {
                    case "0":
                        break;

                    case "1":
                        BorrowedReturnRepository.ListBorrowedBooks(studentId);  // Display borrowed books for the student
                        WaitForKeyPress();
                        break;

                    case "2":
                        BorrowedReturnRepository.ListReturnedBooks(studentId);  // Display returned books for the student
                        WaitForKeyPress();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("==============================================");
                        Console.WriteLine("|               Invalid choice!              |");
                        Console.WriteLine("==============================================");
                        Console.ResetColor();
                        WaitForKeyPress();
                        break;
                }
            }
            catch (Exception ex)
            {
                // Handle any exception while fetching student's book records
                Console.WriteLine($"Error while fetching student book records: {ex.Message}");
                WaitForKeyPress();
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