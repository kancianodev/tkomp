using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Books.Models.ViewModels;

namespace Books.Models.Dao
{
    public class AuthorsDAO
    {
        private readonly DbConnectionHolder _dbConnectionHolder;

        public AuthorsDAO(DbConnectionHolder dbConnectionHolder)
        {
            _dbConnectionHolder = dbConnectionHolder;
        }

        public List<Author> GetAuthors()
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {

                // TODO: map properties Books using JOIN

                var query = "SELECT Authors.*, Books.* FROM dbo.Authors LEFT JOIN dbo.Books ON Authors.AuthorID = Books.AuthorID";

                var authors = new List<Author>();

                connection.Query<Author, Book, Author>(query, (author, book) =>
                    {
                        var currentAuthor = authors.FirstOrDefault(a => a.AuthorId == author.AuthorId);

                        if (currentAuthor == null)
                        {
                            currentAuthor = author;
                            currentAuthor.Books = new List<Book>();
                            authors.Add(currentAuthor);
                        }

                        if (book != null)
                        {
                            currentAuthor.Books.Add(book);
                        }

                        return currentAuthor;
                    },  splitOn: "BookID"
                );

                return authors;
            }
        }

        public List<Author> GetAuthors(string firstName, string lastName)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {

                // TODO: map properties Books using JOIN

                var query = "SELECT Authors.*, Books.* FROM dbo.Authors LEFT JOIN dbo.Books ON Authors.AuthorID = Books.AuthorID WHERE Authors.LastName = @lname AND Authors.FirstName = @fname";

                var authors = new List<Author>();

                connection.Query<Author, Book, Author>(query, (author, book) =>
                    {
                        var currentAuthor = authors.FirstOrDefault(a => a.AuthorId == author.AuthorId);

                        if (currentAuthor == null)
                        {
                            currentAuthor = author;
                            currentAuthor.Books = new List<Book>();
                            authors.Add(currentAuthor);
                        }

                        if (book != null)
                        {
                            currentAuthor.Books.Add(book);
                        }

                        return currentAuthor;
                    },
                    new { 
                        lname = lastName, 
                        fname = firstName 
                    }, splitOn: "BookID"
                );

                return authors;
            }
        }

        public Author GetAuthor(int id)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {

                // TODO: map properties Books using JOIN

                var query = "SELECT Authors.*, Books.* FROM dbo.Authors LEFT JOIN dbo.Books ON Authors.AuthorID = Books.AuthorID WHERE Authors.AuthorID = @authorId";

                List<Author> authors = new List<Author>();

                connection.Query<Author, Book, Author>(
                    query,
                    (author, book) =>
                    {
                        var currentAuthor = authors.FirstOrDefault(a => a.AuthorId == author.AuthorId);

                        if (currentAuthor == null)
                        {
                            currentAuthor = author;
                            currentAuthor.Books = new List<Book>();
                            authors.Add(currentAuthor);
                        }

                        if (book != null)
                        {
                            currentAuthor.Books.Add(book);
                        }

                        return currentAuthor;
                    },
                    new { 
                        authorId = id 
                    }, splitOn: "BookID"
                );

                return authors.FirstOrDefault();
            }
        }

        // TODO: see BooksDao.cs - Add it to project
        public Author DeleteAuthor(int id)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {     
                var author = connection.Query<Author>("DELETE FROM dbo.Authors Where dbo.Authors.AuthorID = @authorId",
                    new
                    {
                        authorId = id
                    }).FirstOrDefault();
                return author;
            }
        }

        public Author UpdateAuthor(string firstName, string lastName, int id)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var author = connection.Query<Author>("UPDATE dbo.Authors SET dbo.Authors.FirstName = @fname, dbo.Authors.LastName = @lname Where dbo.Authors.AuthorID = @authorId",
                    new
                    {
                        lname = lastName,
                        fname = firstName,
                        authorId = id
                    }).FirstOrDefault();
                return author;
            }
        }

        public Author InsertAuthor(string firstName, string lastName)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var author = connection.Query<Author>("INSERT INTO dbo.Authors (dbo.Authors.FirstName, dbo.Authors.LastName) VALUES (@fname, @lname)",
                    new
                    {
                        lname = lastName,
                        fname = firstName,
                    }).FirstOrDefault();
                return author;
            }
        }

        public List<AuthorViewModel> GetAuthorsViewModel(List<Author> authors)
        {
            var vm = new List<AuthorViewModel>();

            foreach (var author in authors)
            {
                var authorVm = new AuthorViewModel
                {
                    AuthorId = author.AuthorId,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    BookCount = author.Books.Count
                };

                if (author.Books.Count > 0)
                {
                    authorVm.ReleaseDateOfFirstBook = author.Books.OrderBy(book => book.ReleaseDate).First().ReleaseDate;
                }

                vm.Add(authorVm);
            }

            return vm;
        }

    }
}
