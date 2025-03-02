using MySql.Data.MySqlClient;
using Project.BLL;

namespace Project.DAL
{
    public static class BorrowedReturnRepository
    {
        private static readonly DatabaseHelper _dbHelper = new();

        public static List<BorrowedReturn> GetBookRecords(string query, MySqlParameter[] parameters)
        {
            List<BorrowedReturn> bookRecords = [];

            try
            {
                using var reader = _dbHelper.ExecuteReader(query, parameters);
                while (reader.Read())
                {
                    var bookRecord = new BorrowedReturn
                    {
                        StudentId = Convert.ToInt32(reader["StudentID"]),
                        Name = reader["Name"] as string ?? string.Empty,
                        BookId = Convert.ToInt32(reader["BookID"]),
                        BookName = reader["BookName"] as string ?? string.Empty,
                        Date = reader.GetDateTime("ActionDate"),
                        Status = reader["Status"] as string ?? string.Empty,
                    };
                    bookRecords.Add(bookRecord);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Database error: " + ex.Message);
                Console.ResetColor();
            }

            return bookRecords;
        }

        public static bool IsStudentBorrowed(int studentId)
        {
            try
            {
                const string query = "SELECT COUNT(*) FROM BorrowRecords WHERE StudentID = @StudentID AND Status = 'Borrowed'";

                MySqlParameter[] parameters = [new MySqlParameter("@StudentID", studentId)];

                var result = _dbHelper.ExecuteScalar(query, parameters);

                if (result == null || !int.TryParse(result.ToString(), out int count))
                {
                    return false;
                }

                return count > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return false;
        }

        public static bool IsBookBorrowed(string username, int bookId)
        {
            try
            {
                const string query = "SELECT COUNT(*) FROM BorrowRecords WHERE Name = @Name AND BookID = @BookID AND Status = 'Borrowed'";

                MySqlParameter[] parameters =
                [new("@Name", username), new("@BookID", bookId)];

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

        public static void BorrowBook(int studentId, string name, int bookId)
        {
            try
            {
                const string checkBookQuery = "SELECT BookName, Quantity FROM Books WHERE ID = @BookId";
                var parameters = new MySqlParameter[] { new("@BookId", bookId) };

                using (var reader = _dbHelper.ExecuteReader(checkBookQuery, parameters))
                {
                    if (!reader.Read())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Book not found.");
                        Console.ResetColor();
                        return;
                    }

                    string? bookName = reader["BookName"].ToString();
                    int availableQuantity = Convert.ToInt32(reader["Quantity"]);

                    if (availableQuantity <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"The book '{bookName}' is not available for borrowing.");
                        Console.ResetColor();
                        return;
                    }
                }

                const string borrowBookQuery = @"
                    INSERT INTO BorrowRecords (StudentID ,Name, BookID, BookName, Status)
                    VALUES (@StudentID, @Name, @BookID, @BookName, 'Borrowed')";
                var borrowParams = new MySqlParameter[]
                {
                    new ("@StudentID", studentId),
                    new ("@Name", name),
                    new ("@BookID", bookId),
                    new ("@BookName", GetBookNameById(bookId))
                };
                _dbHelper.ExecuteQuery(borrowBookQuery, borrowParams);

                const string updateBookQuery = "UPDATE Books SET Quantity = Quantity - 1 WHERE ID = @BookID";
                _dbHelper.ExecuteQuery(updateBookQuery, parameters);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==============================================");
                Console.WriteLine("|        Borrowed Book successfully!         |");
                Console.WriteLine("==============================================");
                Console.ResetColor();
                WaitForKeyPress();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.ResetColor();
            }
        }

        public static void ReturnBook(int studentID, int bookId)
        {
            try
            {
                const string checkQuery = @"
                    SELECT COUNT(*) FROM BorrowRecords
                    WHERE StudentID = @StudentID AND BookID = @BookID AND Status = 'Borrowed'";
                var parameters = new MySqlParameter[]
                {
                    new ("@@StudentID", studentID),
                    new ("@BookID", bookId)
                };

                int borrowedCount = Convert.ToInt32(_dbHelper.ExecuteScalar(checkQuery, parameters));
                if (borrowedCount == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No borrowed record found for this book.");
                    Console.ResetColor();
                    return;
                }

                const string updateBorrowQuery = @"
                    UPDATE BorrowRecords
                    SET Status = 'Returned'
                    WHERE StudentID = @StudentID AND BookID = @BookID AND Status = 'Borrowed'";
                _dbHelper.ExecuteQuery(updateBorrowQuery, parameters);

                const string updateBooksQuery = "UPDATE Books SET Quantity = Quantity + 1 WHERE ID = @BookID";
                _dbHelper.ExecuteQuery(updateBooksQuery, [new("@BookID", bookId)]);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==============================================");
                Console.WriteLine("|        Book returned successfully!         |");
                Console.WriteLine("==============================================");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.ResetColor();
            }
        }

        public static void ListBorrowedBooks(int studentId)
        {
            const string query = "SELECT StudentID, Name, BookID, BookName, ActionDate, Status FROM BorrowRecords WHERE StudentID = @StudentID AND Status = 'Borrowed' ORDER BY ActionDate DESC";
            var parameters = new MySqlParameter[] { new("@StudentID", studentId) };

            BorrowedReturn.DisplayBookRecordsMini(query, parameters, "No borrowed books found!");
        }

        public static void ListReturnedBooks(int studentId)
        {
            const string query = "SELECT StudentID, Name, BookID, BookName, ActionDate, Status FROM BorrowRecords WHERE StudentID = @StudentID AND Status = 'Returned' ORDER BY ActionDate DESC";
            var parameters = new MySqlParameter[] { new("@StudentID", studentId) };

            BorrowedReturn.DisplayBookRecordsMini(query, parameters, "No returned books found!");
        }

        public static void ListWhoBorrowedBooks()
        {
            const string query = "SELECT StudentID, Name, BookID, BookName, ActionDate, Status FROM BorrowRecords WHERE Status = 'Borrowed' ORDER BY Name";
            BorrowedReturn.DisplayWhoBorrowedBook(query, null!, "No students borrowed books found!");
        }

        public static void ListAllBorrowedBooks()
        {
            const string query = "SELECT StudentID, Name, BookID, BookName, ActionDate, Status FROM BorrowRecords WHERE Status = 'Borrowed' ORDER BY ActionDate DESC";
            BorrowedReturn.DisplayBookRecords(query, null!, "No borrowed books found!");
        }

        public static void ListAllReturnedBooks()
        {
            const string query = "SELECT StudentID, Name, BookID, BookName, ActionDate, Status FROM BorrowRecords WHERE Status = 'Returned' ORDER BY ActionDate DESC";
            BorrowedReturn.DisplayBookRecords(query, null!, "No returned books found!");
        }

        private static string GetBookNameById(int bookId)
        {
            const string query = "SELECT BookName FROM Books WHERE ID = @BookId";
            var parameters = new MySqlParameter[] { new("@BookId", bookId) };
            return _dbHelper.ExecuteScalar(query, parameters)?.ToString() ?? "Unknown";
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