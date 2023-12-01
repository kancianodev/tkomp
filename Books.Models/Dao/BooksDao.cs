using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Models.ViewModels;
using Dapper;

namespace Books.Models.Dao
{
    // TODO: add/include this file to project
    public class BooksDao
    {
        private readonly DbConnectionHolder _dbConnectionHolder;

        public BooksDao(DbConnectionHolder dbConnectionHolder)
        {
            _dbConnectionHolder = dbConnectionHolder;
        }

        public List<Book> GetBooks()
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var books = connection.Query<Book>("SELECT * FROM dbo.Books INNER JOIN dbo.Authors ON dbo.Authors.AuthorId = dbo.Books.AuthorId").ToList();
                return books;
            }
        }

        public List<Book> GetBooks(string title, DateTime releaseDate)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                // TODO: fill with query parameters
                var books = connection.Query<Book>("SELECT * FROM dbo.Books INNER JOIN dbo.Authors ON dbo.Authors.AuthorId = dbo.Books.AuthorId Where releaseDate = @rdate, title = @rtitle",
                    new
                    {
                        rdate = releaseDate,
                        rtitle = title
                    }).ToList();

                return books;
            }
            
        }

        public Book GetBook(int id)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                // TODO:fill with query parameters
                var book = connection.Query<Book>("SELECT * FROM dbo.Books INNER JOIN dbo.Authors ON dbo.Authors.AuthorId = dbo.Books.AuthorId WHERE dbo.Books.BookID = @bookId",
                    new
                    {
                        bookId = id
                    }).FirstOrDefault();

                return book;
            }
        }

        public Book DeleteBook(int id)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                // TODO: complete this method
                    var book = connection.Query<Book>("DELETE FROM dbo.Books WHERE dbo.Books.BookID = @bookId",
                        new
                        {
                            bookId = id
                        }).FirstOrDefault();
                    return book;
            }
        }

        public Book UpdateBook(string firstName, string lastName, string title, string releaseDate, int id)
        {

            using (var connection = _dbConnectionHolder.GetConnection())
            {
                // TODO: complete this method
                var book = connection.Query<Book>("UPDATE dbo.Books SET dbo.Books.Title = @title, dbo.Books.ReleaseDate = @relDate, dbo.Books.AuthorId = (SELECT dbo.Authors.AuthorId FROM dbo.Authors WHERE dbo.Authors.FirstName = @fname AND dbo.Authors.LastName = @lname) Where dbo.Books.BookID = @bookId",
                    new
                    {
                        title,
                        relDate = DateTime.Parse(releaseDate),
                        fname = firstName,
                        lname = lastName,
                        bookId = id
                    }).FirstOrDefault();
                return book;
            }
        }

        public Book InsertBook(string firstName, string lastName, string title, string releaseDate)
        {

            using (var connection = _dbConnectionHolder.GetConnection())
            {
                // TODO: complete this method
                var authorId = connection.Query<int>("SELECT AuthorId FROM dbo.Authors WHERE FirstName = @fname AND LastName = @lname",
                    new 
                    { 
                        fname = firstName, 
                        lname = lastName
                    }).FirstOrDefault();

                if (authorId == 0)
                {
                    //Exception error
                    throw new InvalidOperationException("Author not found.");
                }


                var book = connection.Query<Book>("INSERT INTO dbo.Books (Title, ReleaseDate, AuthorId) VALUES (@title, @relDate, @authorId); SELECT SCOPE_IDENTITY()",
                    new 
                    { 
                        title, 
                        relDate = DateTime.Parse(releaseDate), 
                        authorId 
                    }).FirstOrDefault();

                return book;
            }
        }


        public List<BookViewModel> GetBooksViewModel(List<Book> books)
        {
            var vm = new List<BookViewModel>();

            foreach (var book in books)
            {
                var bookVm = new BookViewModel
                {
                    AuthorId = book.Author.AuthorId,
                    BookId = book.BookId,
                    LastName = book.LastName, 
                    FirstName = book.FirstName,
                    Title = book.Title,
                    BookCount = book.Author.Books.Count,
                    ReleaseDateOfFirstBook = book.ReleaseDate
                };

                vm.Add(bookVm);
            }

            return vm;
        }
    }
}
