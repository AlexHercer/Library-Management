using MySql.Data.MySqlClient;
using Project.DAL;

namespace Project.BLL
{
    public class Book
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public int Quantity { get; set; }

        public bool IsAvailable
        {
            get
            {
                return Quantity > 0;
            }
        }

        public static bool IsbookExist(string bookName)
        {
            // Check if the username already exists
            if (BookRepository.IsBookExist(bookName))
            {
                Console.WriteLine("Book already exists.");
                return false;
            }

            return true;
        }


        public override string ToString()
        {
            return $"ID: {BookId}, BookName: {BookName}, Author: {Author}, Quantity: {Quantity}";
        }

        // Method for displaying the book list in a table format
        public static void DisplayBookList()
        {
            try
            {
                // Clear the screen for a fresh display
                Console.Clear();

                // Create a repository to fetch the list of books
                var books = BookRepository.Books;

                // Display header with cyan color and a clean table divider
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("                Book List                    ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                // Display the table header
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("+----+---------------------------+---------------------------+----------+");
                Console.WriteLine("| ID | BookName                  | Author                    | Quantity |");
                Console.WriteLine("+----+---------------------------+---------------------------+----------+");
                Console.ResetColor();

                // Display each book's details with green color for text
                foreach (var book in books)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(
                        $"| {book.BookId,-2} | {book.BookName,-25} | {book.Author,-25} | {book.Quantity,-8} |");
                }
                Console.ResetColor();

                // Display the table footer in cyan color
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("+----+---------------------------+---------------------------+----------+");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                // Handle any exception while displaying the book list with an error message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Error while displaying book list: {ex.Message}");
                Console.ResetColor();
                WaitForKeyPress();
            }
        }

        // Method to update a book
        public static void UpdateBook()
        {
            int bookId;
            int quantity;
            string? bookIdInput;

            while (true)
            {
                Console.Clear();
                Book.DisplayBookList(); // Display the list of books
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("                 Update Book                  ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                // Inform the user that they can type "exit" to return
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("           === Type 'exit' to return ==\n");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                // Prompt the user to enter the Book ID
                Console.Write("Enter Book ID to update: ");
                bookIdInput = Console.ReadLine()?.Trim();

                // If the user enters "exit", return to the previous menu
                if (bookIdInput?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
                    return;

                // Validate the input to ensure it's a positive integer
                if (int.TryParse(bookIdInput, out bookId) && bookId > 0)
                {
                    break; // Valid input, proceed to the next step
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Book ID! Please enter a valid positive integer.");
                Console.ResetColor();
            }

            while (true)
            {
                Console.Clear();
                var booksById = BookRepository.SearchBooksById(bookIdInput!);
                DisplayBooks(booksById);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("                 Update Book                  ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                // Inform the user that they can type "exit" to return
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("           === Type 'exit' to return ==\n");
                Console.ResetColor();

                // Prompt the user to enter the new quantity
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Enter New Quantity: ");
                string? quantityInput = Console.ReadLine()?.Trim();

                // If the user enters "exit", return to the previous menu
                if (quantityInput?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
                    return;

                // Validate the input to ensure it's a non-negative integer
                if (int.TryParse(quantityInput, out quantity) && quantity >= 0)
                {
                    break; // Valid input, proceed to update the book
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Quantity! Please enter a valid non-negative integer.");
                Console.ResetColor();
            }

            // Call the repository method to update the book quantity
            bool success = BookRepository.UpdateBookQuantity(bookId, quantity);

            // Display success or failure message based on repository response
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Book updated successfully!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to update the book. Please try again.");
                Console.ResetColor();
            }

            WaitForKeyPress(); // Wait for user input before returning to the menu
        }

        // Method to add a book
        public static void AddBook()
        {
            string? name, author;
            int quantity;

            while (true)
            {
                // Display the header for the book addition function
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("              Add Book Management             ");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                // Inform the user that they can type "exit" to return
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("           === Type 'exit' to return ==\n");

                // Prompt user to enter the book name
                Console.WriteLine("Enter Book Name: ");
                name = Console.ReadLine();

                // Check if the book name is empty
                if (string.IsNullOrEmpty(name))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("|         Book Name Cannot Be Empty.          |");
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                    WaitForKeyPress();
                    continue;
                }

                // If the user enters "exit", return to the previous menu
                if (name.Equals("exit", StringComparison.OrdinalIgnoreCase)) return;

                // Check if the book already exists in the repository
                if (BookRepository.IsBookExist(name))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("|            Book already exists.            |");
                    Console.WriteLine("==============================================");
                    Console.WriteLine(" Book Name   : " + name);
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                    WaitForKeyPress();
                    continue;
                }

                // Enter author name
                while (true)
                {
                    // Display the book name for reference
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("              Add Book Management             ");
                    Console.WriteLine("==============================================");
                    Console.WriteLine(" Book Name   : " + name);
                    Console.WriteLine("==============================================");
                    Console.ResetColor();

                    // Inform the user they can type "exit" to return
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("           === Type 'exit' to return ==\n");

                    // Prompt user to enter the author's name
                    Console.WriteLine("Enter Author Name: ");
                    author = Console.ReadLine();

                    // Check if the author name is empty
                    if (string.IsNullOrEmpty(author))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("==============================================");
                        Console.WriteLine("|       Author Name Cannot Be Empty.         |");
                        Console.WriteLine("==============================================");
                        Console.ResetColor();
                        WaitForKeyPress();
                        continue;
                    }
                    else
                    {
                        break;
                    }

                    // If the user enters "exit", return to the previous menu
                    if (author.Equals("exit", StringComparison.OrdinalIgnoreCase)) return;
                }

                // Enter book quantity
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("              Add Book Management             ");
                    Console.WriteLine("==============================================");
                    Console.WriteLine("Book Name   : " + name);
                    Console.WriteLine("Author Name : " + author);
                    Console.WriteLine("==============================================");
                    Console.ResetColor();

                    Console.WriteLine("           === Type 'exit' to return ==\n");
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    // Prompt user to enter the book quantity
                    Console.Write("Enter Quantity: ");
                    string? quantityInput = Console.ReadLine();
                    Console.ResetColor();

                    // If the user enters "exit", return to the previous menu
                    if (!string.IsNullOrEmpty(quantityInput) && quantityInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    // Validate that the input is a valid number and is not negative
                    if (int.TryParse(quantityInput, out quantity) && quantity >= 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Only numbers are allowed. Please enter again!");
                        Console.ResetColor();
                    }
                }

                // Attempt to add the book to the repository
                bool success = BookRepository.AddBook(name, author!, quantity);
                if (success)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("|          Book added successfully!          |");
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("==============================================");
                    Console.WriteLine("|        Error: Unable to add the book!       |");
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                }

                WaitForKeyPress();
                return;
            }
        }

        public static void SearchBooks()
        {
            while (true)
            {
                try
                {
                    // Display search options with a header
                    Console.Clear();
                    DisplaySearchOptions();

                    string? searchChoice = Console.ReadLine(); // Get the user's choice.
                    Console.Clear();
                    switch (searchChoice)
                    {
                        case "1": // Search by Book Name
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("==============================================");
                            Console.WriteLine("          Search Book By Book Name            ");
                            Console.WriteLine("==============================================");
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Enter Book Name: ");
                            string? bookName = Console.ReadLine()?.Trim();
                            var booksByName = BookRepository.SearchBooksByName(bookName!);
                            Console.ResetColor();
                            Console.Clear();
                            DisplayBooks(booksByName);
                            WaitForKeyPress();

                            break;

                        case "2": // Search by Book ID
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("==============================================");
                            Console.WriteLine("              Search Book By ID               ");
                            Console.WriteLine("==============================================");
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Enter Book ID: ");
                            string? bookId = Console.ReadLine()?.Trim();
                            var booksById = BookRepository.SearchBooksById(bookId!);
                            Console.ResetColor();
                            Console.Clear();
                            DisplayBooks(booksById);
                            WaitForKeyPress();

                            break;

                        case "3": // Search by Author Name
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("==============================================");
                            Console.WriteLine("         Search Book By Author Name           ");
                            Console.WriteLine("==============================================");
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Enter Author Name: ");
                            string? authorName = Console.ReadLine()?.Trim();
                            var booksByAuthor = BookRepository.SearchBooksByAuthor(authorName!);
                            Console.ResetColor();
                            Console.Clear();
                            DisplayBooks(booksByAuthor);
                            WaitForKeyPress();
                            break;

                        case "0": // Exit the search menu and return to the book list.
                            return;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("==============================================");
                            Console.WriteLine("|      Invalid choice! Please try again1     |");
                            Console.WriteLine("==============================================");
                            Console.ResetColor();
                            WaitForKeyPress();

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during search: {ex.Message}");
                }
            }
        }

        private static void DisplaySearchOptions()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==============================================");
            Console.WriteLine("                 Search Book                ");
            Console.WriteLine("==============================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  Please choose an option from the menu below:");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==============================================");
            Console.WriteLine("| 1.  Search by Book Name                    |");
            Console.WriteLine("| 2.  Search by Book ID                      |");
            Console.WriteLine("| 3.  Search by Author                       |");
            Console.WriteLine("|                                            |");
            Console.WriteLine("| 0.  Return                                 |");
            Console.WriteLine("==============================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nEnter your choice: ");
            Console.ResetColor();
        }

        // Method to display the list of books
        private static void DisplayBooks(List<Book> books)
        {
            // Check if the book list is empty
            if (books.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo books found!");
                Console.ResetColor();
                return; // Exit the method if no books are available
            }

            // Display the table header
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+----+---------------------------+---------------------------+----------+");
            Console.WriteLine("| ID | BookName                  | Author                    | Quantity |");
            Console.WriteLine("+----+---------------------------+---------------------------+----------+");
            Console.ResetColor();

            // Display each book in the list
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var book in books)
            {
                Console.WriteLine($"| {book.BookId,-2} | {book.BookName,-25} | {book.Author,-25} | {book.Quantity,-8} |");
            }
            Console.ResetColor();

            // Display the table footer
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+----+---------------------------+---------------------------+----------+");
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