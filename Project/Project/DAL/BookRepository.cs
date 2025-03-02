using MySql.Data.MySqlClient;
using Project.BLL;

namespace Project.DAL
{
    public static class BookRepository
    {
        // Connection string for connecting to the MySQL database.It contains necessary information like server, database name, username, and password.
        private static readonly DatabaseHelper _dbHelper = new();

        // Method to retrieve all books from the database.
        public static List<Book> Books
        {
            get
            {
                List<Book> books = [];

                try
                {
                    const string query = "SELECT * FROM Books";
                    using var reader = _dbHelper.ExecuteReader(query);
                    while (reader.Read())
                    {
                        Book book = new()
                        {
                            BookId = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : 0, // Book ID
                            BookName = reader["BookName"] as string ?? string.Empty, // Book Name
                            Author = reader["AuthorName"] as string ?? string.Empty, // Author Name
                            Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt32(reader["Quantity"]) : 0, // Quantity available
                        };
                        books.Add(book);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while fetching books: {ex.Message}");
                }

                return books;
            }
        }

        public static bool IsBookExist(string bookname)
        {
            try
            {
                const string query = "SELECT COUNT(*) FROM Books WHERE BookName = @BookName";
                MySqlParameter[] parameters = [new("@BookName", bookname)];

                int count = Convert.ToInt32(_dbHelper.ExecuteScalar(query, parameters));
                return count > 0;
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

        public static bool AddBook(string bookName, string author, int quantity)
        {
            try
            {
                const string query = "INSERT INTO Books (BookName, AuthorName, Quantity) VALUES (@Name, @Author, @Quantity)";
                var parameters = new MySqlParameter[]
                {
                    new ("@Name", bookName),
                    new ("@Author", author),
                    new ("@Quantity", quantity)
                };

                _dbHelper.ExecuteQuery(query, parameters);
                return true; // Indicate success
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false; // Indicate failure
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false; // Indicate failure
            }
        }

        public static bool UpdateBookQuantity(int bookId, int quantity)
        {
            try
            {
                const string query = "UPDATE Books SET Quantity=@Quantity WHERE ID=@BookId";
                var parameters = new MySqlParameter[]
                {
                    new("@Quantity", quantity),
                    new("@BookId", bookId)
                };

                _dbHelper.ExecuteQuery(query, parameters);
                return true; // Indicate success
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false; // Indicate failure
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false; // Indicate failure
            }
        }

        // Method to search books by name
        public static List<Book> SearchBooksByName(string bookName)
        {
            var books = new List<Book>();
            const string query = "SELECT * FROM Books WHERE BookName LIKE @BookName";
            var parameters = new MySqlParameter[]
            {
            new("@BookName", "%" + bookName + "%")
            };

            using (var reader = _dbHelper.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        BookId = Convert.ToInt32(reader["ID"]),
                        BookName = reader["BookName"].ToString(),
                        Author = reader["AuthorName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
            }
            return books;
        }

        // Method to search books by ID
        public static List<Book> SearchBooksById(string bookId)
        {
            var books = new List<Book>();
            const string query = "SELECT * FROM Books WHERE ID = @BookID";
            var parameters = new MySqlParameter[]
            {
            new("@BookID", bookId)
            };

            using (var reader = _dbHelper.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        BookId = Convert.ToInt32(reader["ID"]),
                        BookName = reader["BookName"].ToString(),
                        Author = reader["AuthorName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
            }
            return books;
        }

        // Method to search books by author name
        public static List<Book> SearchBooksByAuthor(string authorName)
        {
            var books = new List<Book>();
            const string query = "SELECT * FROM Books WHERE AuthorName LIKE @AuthorName";
            var parameters = new MySqlParameter[]
            {
            new("@AuthorName", "%" + authorName + "%")
            };

            using (var reader = _dbHelper.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        BookId = Convert.ToInt32(reader["ID"]),
                        BookName = reader["BookName"].ToString(),
                        Author = reader["AuthorName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
            }
            return books;
        }
    }
}