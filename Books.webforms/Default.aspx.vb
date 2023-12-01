Imports Books.Models
Imports Books.Models.Dao
Imports Books.Models.ViewModels

Public Class _Default
    'I changed Page to AppPage
    Inherits AppPage

    Public IsEditMode As Boolean = False
    Public EditedBook As Book = New Book()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    'I modified
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim firstname_findValue As String = ""
        Dim lastname_findValue As String = ""

        Dim booksDao As BooksDao = New BooksDao(Me.DbConnectionHolder)
        Dim books As List(Of Book)

        If Me.IsPostBack Then
            Dim authorsDao As AuthorsDAO = New AuthorsDAO(Me.DbConnectionHolder)
            Dim authors As List(Of Author)

            lastname_findValue = Request.Form("FindBox_LastName")
            firstname_findValue = Request.Form("FindBox_FirstName")
            'TODO: use this values to find authors by firstname and lastname

            authors = authorsDao.GetAuthors(firstname_findValue, lastname_findValue)

            For Each author In authors
                Dim authorBooks = author.Books
                books.AddRange(authorBooks)
            Next
        Else
            ' if http method is GET
            books = booksDao.GetBooks()
        End If

        Dim booksVMs As List(Of BookViewModel) = GetViewModel(books)

        AuthorsRepeater.DataSource = booksVMs
        ' TODO: use authorsVMs instead authors
        AuthorsRepeater.DataBind()
    End Sub

    Private Function GetViewModel(ByVal books As List(Of Book)) As List(Of BookViewModel)

        ' TODO: map Author to AuthorViewModel, and fill BookCount and ReleaseDateOfFirstBook properties
        ' use Books property of Author, first fill this collection in AuthorsDAO

        Dim booksDao As New BooksDao(Me.DbConnectionHolder)

        Dim vm As List(Of BookViewModel) = BooksDao.GetBooksViewModel(books)

        ' you can do it in C# in Books.Models project
        Return vm
    End Function

    Protected Sub AddNewBook(sender As Object, e As CommandEventArgs)
        IsEditMode = True
    End Sub
    Protected Sub EditBook(sender As Object, e As CommandEventArgs)
        IsEditMode = True
        Dim editedBookId As Integer = Integer.Parse(e.CommandArgument)

        Dim booksDao As New BooksDao(Me.DbConnectionHolder)
        EditedBook = booksDao.GetBook(editedBookId)
    End Sub

    Protected Sub DeleteBook(sender As Object, e As CommandEventArgs)
        'TODO: add method to delete author from DB. Use AuthorsDAO class, add method to remove data
        Dim deletedBookId As Integer = Integer.Parse(e.CommandArgument)

        Dim booksDao As New BooksDao(Me.DbConnectionHolder)
        EditedBook = booksDao.DeleteBook(deletedBookId)

        Dim books As List(Of Book) = booksDao.GetBooks()
        Dim booksVMs As List(Of BookViewModel) = GetViewModel(books)

        AuthorsRepeater.DataSource = booksVMs
        AuthorsRepeater.DataBind()
    End Sub

    Protected Sub SaveBook(sender As Object, e As CommandEventArgs)
        Dim title As String = Request.Form("Editbox_title")
        Dim lastName As String = Request.Form("Editbox_lastName")
        Dim firstName As String = Request.Form("Editbox_firstName")
        Dim releaseDate As String = Request.Form("Editbox_date")
        Dim editedBookId As Integer = Integer.Parse(Request.Form("Editbox_id"))

        Dim booksDao As New BooksDao(Me.DbConnectionHolder)

        'TODO: add method to update/insert author. Use AuthorsDAO class, add method to update/insert data

        If editedBookId = 0 Then
            ' INSERT 
            EditedBook = booksDao.InsertBook(firstName, lastName, title, releaseDate)
        Else
            ' UPDATE
            EditedBook = booksDao.UpdateBook(firstName, lastName, title, releaseDate, editedBookId)
        End If

        Dim books As List(Of Book) = booksDao.GetBooks()
        Dim booksVMs As List(Of BookViewModel) = GetViewModel(books)

        AuthorsRepeater.DataSource = booksVMs
        AuthorsRepeater.DataBind()

    End Sub
End Class