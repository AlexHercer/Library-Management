using MySql.Data.MySqlClient;
using Project.DAL;

namespace Project.BLL
{
    public class BorrowedReturn
    {
        public int StudentId { get; set; }
        public string? Name { get; set; }
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string? Status { get; set; }

        public override string ToString()
        {
            return $"StudentID: {StudentId}, Name: {Name}, BookID: {BookId}, Book Name: {BookName}, Quantity: {Quantity}, Status: {Status}";
        }

        // Method for borrowing a book
        public static void BorrowBook(int studentId, string username)
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           Borrow Book Management             ");
                Console.WriteLine("==============================================");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("           === Type 'exit' to return ==\n");
                Console.ResetColor();

                // Display the available books for borrowing
                Book.DisplayBookList();
                Console.WriteLine();

                int bookId;
                string? bookIdInput;


                while (true)
                {
                    // Prompt the user to enter the Book ID
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter Book ID to Borrow: ");
                    bookIdInput = Console.ReadLine();
                    Console.ResetColor();

                    // Allow user to exit the borrowing process
                    if (!string.IsNullOrEmpty(bookIdInput) && bookIdInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    // Validate the book ID input (must be a positive integer)
                    if (!int.TryParse(bookIdInput, out bookId) || bookId <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Only numbers are allowed. Please enter again!");
                        Console.ResetColor();

                        // Clear the previous input line
                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        continue;
                    }

                    // Check if the book is already borrowed by the user
                    if (BorrowedReturnRepository.IsBookBorrowed(username, bookId))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("This book is already borrowed. Please choose another one.");
                        Console.ResetColor();

                        // Clear the previous input line
                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        continue;
                    }

                    break; // Exit loop if valid input is provided
                }

                // Proceed with borrowing the book
                BorrowedReturnRepository.BorrowBook(studentId, username, bookId);
            }
            catch (FormatException ex)
            {
                // Handle invalid input for book ID
                Console.WriteLine($"Invalid input for book ID: {ex.Message}");
                WaitForKeyPress();
            }
            catch (Exception ex)
            {
                // Handle any unexpected error during the borrowing process
                Console.WriteLine($"Error while borrowing book: {ex.Message}");
                WaitForKeyPress();
            }
        }

        // Method for returning a book
        public static void ReturnBook()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           Return Book Management             ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                BorrowedReturnRepository.ListWhoBorrowedBooks();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("           === Type 'exit' to return ==\n");
                Console.ResetColor();

                // Ask the librarian to enter student details for returning a book
                int studentId = 0;
                string? studentIdInput;

                do
                {
                    // Ask the user to provide the book ID to return
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter Student ID to Return: ");
                    studentIdInput = Console.ReadLine();
                    Console.ResetColor();

                    if (string.IsNullOrWhiteSpace(studentIdInput) || !int.TryParse(studentIdInput, out studentId) || studentId < 0)
                    {
                        if (!string.IsNullOrEmpty(studentIdInput) && studentIdInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        {
                            return;
                        }

                        if (!BorrowedReturnRepository.IsStudentBorrowed(studentId))
                        {
                            // Display error message
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This student hasn't borrowed any books!");
                            Console.ResetColor();

                            Console.SetCursorPosition(0, Console.CursorTop - 2);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, Console.CursorTop);
                        }

                        // Display error message
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Invalid Student ID!");
                        Console.ResetColor();

                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                    }
                } while (string.IsNullOrWhiteSpace(studentIdInput) || !int.TryParse(studentIdInput, out studentId) || studentId < 0);

                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           Return Book Management             ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                BorrowedReturnRepository.ListBorrowedBooks(studentId);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("           === Type 'exit' to return ==\n");
                Console.ResetColor();

                int bookId = 0;
                string? bookIdInput;

                do
                {
                    // Ask the user to provide the book ID to return
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter Book ID to Return: ");
                    bookIdInput = Console.ReadLine();
                    Console.ResetColor();

                    if (string.IsNullOrWhiteSpace(bookIdInput) || !int.TryParse(bookIdInput, out bookId) || bookId < 0)
                    {
                        if (!string.IsNullOrEmpty(bookIdInput) && bookIdInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        {
                            return;
                        }

                        // Display error message
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Invalid Book ID");
                        Console.ResetColor();

                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                    }
                } while (string.IsNullOrWhiteSpace(bookIdInput) || !int.TryParse(bookIdInput, out bookId) || bookId < 0); // <-- Dấu ngoặc bị thiếu đã được thêm lại

                // Call the service to return the book
                BorrowedReturnRepository.ReturnBook(studentId, bookId);
                WaitForKeyPress();
            }
            catch (Exception ex)
            {
                // Handle any exception that occurs while returning a book
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Error while returning book: {ex.Message}");
                Console.ResetColor();
                WaitForKeyPress();
            }
        }

        // Method for displaying borrowing and returning records
        public static void BookRecordList()
        {
            try
            {
                // Display book list options with color and better formatting
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("           Book Record Options               ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n           === Choose a Record ===           ");
                Console.ResetColor();
                Console.WriteLine("==============================================");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  Please choose an option from the menu below:");
                Console.WriteLine();

                // Display the options with color
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
                // Read user choice for record view
                string? choice = Console.ReadLine();
                Console.Clear();  // Clear the screen for the next view

                // Handle each option for viewing borrowed and returned books
                switch (choice)
                {
                    case "0":
                        break;

                    case "1":
                        BorrowedReturnRepository.ListAllBorrowedBooks();  // Display all borrowed books
                        WaitForKeyPress();
                        break;

                    case "2":
                        BorrowedReturnRepository.ListAllReturnedBooks();  // Display all returned books
                        WaitForKeyPress();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("==============================================");
                        Console.WriteLine("|                Invalid choice!             |");
                        Console.WriteLine("==============================================");
                        WaitForKeyPress();
                        break;
                }
            }
            catch (Exception ex)
            {
                // Handle any exception while fetching book records
                Console.WriteLine($"Error while fetching book records: {ex.Message}");
                WaitForKeyPress();
            }
        }

        public static void DisplayWhoBorrowedBook(string query, MySqlParameter[] parameters, string emptyMessage)
        {
            var bookRecords = BorrowedReturnRepository.GetBookRecords(query, parameters);

            if (bookRecords.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("==============================================");
                Console.WriteLine($"|       {emptyMessage,-36} |");
                Console.WriteLine("==============================================");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("   List Who Borrowed Book");
            Console.WriteLine("+-----+----------------------+");
            Console.WriteLine("| ID  | User Name            |");
            Console.WriteLine("+-----+----------------------+");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var record in bookRecords.DistinctBy(r => r.Name))
            {
                Console.WriteLine($"| {record.StudentId,-3} | {record.Name,-20} |");
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+-----+----------------------+");
            Console.ResetColor();
        }

        public static void DisplayBookRecordsMini(string query, MySqlParameter[] parameters, string emptyMessage)
        {
            var bookRecords = BorrowedReturnRepository.GetBookRecords(query, parameters);

            if (bookRecords.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("==============================================");
                Console.WriteLine($"|       {emptyMessage,-36} |");
                Console.WriteLine("==============================================");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("+--------+---------------------------+----------------+----------+");
            Console.WriteLine("| BookID | Book Name                 | Date           | Status   |");
            Console.WriteLine("+--------+---------------------------+----------------+----------+");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var record in bookRecords)
            {
                Console.WriteLine($"| {record.BookId,-6} | {record.BookName,-25} | {record.Date,-10:dd - MM - yyyy} | {record.Status,-8} |");
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+--------+---------------------------+----------------+----------+");
            Console.ResetColor();
        }

        public static void DisplayBookRecords(string query, MySqlParameter[] parameters, string emptyMessage)
        {
            var bookRecords = BorrowedReturnRepository.GetBookRecords(query, parameters);

            if (bookRecords.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("==============================================");
                Console.WriteLine($"|       {emptyMessage,-36} |");
                Console.WriteLine("==============================================");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("+-----+----------------------+--------+---------------------------+----------------+----------+");
            Console.WriteLine("| ID  | User Name            | BookID | Book Name                 | Date           | Status   |");
            Console.WriteLine("+-----+----------------------+--------+---------------------------+----------------+----------+");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var record in bookRecords)
            {
                Console.WriteLine($"| {record.StudentId,-3} | {record.Name,-20} | {record.BookId,-6} | {record.BookName,-25} | {record.Date,-10:dd - MM - yyyy} | {record.Status,-8} |");
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+-----+----------------------+--------+---------------------------+----------------+----------+");
            Console.ResetColor();
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