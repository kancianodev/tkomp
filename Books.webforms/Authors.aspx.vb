Imports Books.Models
Imports Books.Models.Dao
Imports Books.Models.ViewModels

Public Class Authors
    Inherits AppPage

    Public IsEditMode as boolean = False
    Public EditedAuthor as Author = New Author()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim firstname_findValue as String =""
        Dim lastname_findValue As String = ""

        Dim authorsDao As AuthorsDAO = New AuthorsDAO(Me.DbConnectionHolder)
        Dim authors As List(Of Author)

        If Me.IsPostBack Then
            lastname_findValue = Request.Form("FindBox_LastName")
            firstname_findValue = Request.Form("FindBox_FirstName")

            'TODO: use this values to find authors by firstname and lastname
            authors = authorsDao.GetAuthors(firstname_findValue, lastname_findValue)
        Else
            ' if http method is GET
            authors = authorsDao.GetAuthors()
        End If

        ' TODO: use authorsVMs instead authors
        Dim authorsVMs As List(Of AuthorViewModel) = GetViewModel(authors)

        AuthorsRepeater.DataSource = authorsVMs
        AuthorsRepeater.DataBind()
    End Sub
    Protected Sub AddNewAuthor(sender As Object, e As CommandEventArgs)
        IsEditMode = True
    End Sub
    Protected Sub EditAuthor(sender As Object, e As CommandEventArgs)
        IsEditMode = True
        dim editedAuthorId as Integer = Integer.Parse(e.CommandArgument)

        Dim authorsDao As New AuthorsDAO(Me.DbConnectionHolder)
        EditedAuthor = authorsDao.GetAuthor(editedAuthorId)
    End Sub

    Protected Sub DeleteAuthor(sender As Object, e As CommandEventArgs)
        'TODO: add method to delete author from DB. Use uthorsDAO class, add method to remove data
        Dim deletedAuthorId As Integer = Integer.Parse(e.CommandArgument)

        Dim authorsDao As New AuthorsDAO(Me.DbConnectionHolder)
        EditedAuthor = authorsDao.DeleteAuthor(deletedAuthorId)

        Dim authors As List(Of Author) = authorsDao.GetAuthors()
        Dim authorsVMs As List(Of AuthorViewModel) = GetViewModel(authors)

        AuthorsRepeater.DataSource = authorsVMs
        AuthorsRepeater.DataBind()
    End Sub

    Protected Sub SaveAuthor(sender As Object, e As CommandEventArgs)
        dim firstName As String = Request.Form("Editbox_firstName")
        dim lastName As String = Request.Form("Editbox_lastName")
        dim editedAuthorId as Integer = Integer.Parse(Request.Form("Editbox_id"))

        Dim newAuthor As New Author With {
            .FirstName = firstName,
            .LastName = lastName,
            .AuthorId = editedAuthorId
        }

        'TODO: add method to update/insert author. Use AuthorsDAO class, add method to update/insert data

        Dim authorsDao As New AuthorsDAO(Me.DbConnectionHolder)

        If editedAuthorId = 0 Then
            ' INSERT new
            EditedAuthor = authorsDao.InsertAuthor(newAuthor.FirstName, newAuthor.LastName)
        Else
            ' UPDATE
            EditedAuthor = authorsDao.UpdateAuthor(newAuthor.FirstName, newAuthor.LastName, newAuthor.AuthorId)
        End If

        Dim authors As List(Of Author) = authorsDao.GetAuthors()
        Dim authorsVMs As List(Of AuthorViewModel) = GetViewModel(authors)

        AuthorsRepeater.DataSource = authorsVMs
        AuthorsRepeater.DataBind()
    End Sub


    Private Function GetViewModel(ByVal authors As List(Of Author)) As List(Of AuthorViewModel)

        ' TODO: map Author to AuthorViewModel, and fill BookCount and ReleaseDateOfFirstBook properties
        ' use Books property of Author, first fill this collection in AuthorsDAO

        Dim dao As New AuthorsDAO(Me.DbConnectionHolder)
        Dim vm As List(Of AuthorViewModel) = dao.GetAuthorsViewModel(authors)

        ' you can do it in C# in Books.Models project
        Return vm
    End Function

End Class