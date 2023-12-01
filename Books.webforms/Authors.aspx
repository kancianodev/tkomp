<%@ Page Title="Autorzy" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Authors.aspx.vb" Inherits="Books.webforms.Authors" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Autorzy</h2>

    <asp:Button runat="server" ID="btnAddNew" CssClass="btn btn-default" OnCommand="AddNewAuthor" Text="Dodaj nowego autora"/>

    <asp:Repeater runat="server" Id="AuthorsRepeater">
        <HeaderTemplate>
            <table class="table" style="margin-top:15px">
            <!-- TODO: Style this table using bootstrap CSS classes http://getbootstrap.com/docs/3.3/components/ -->
            <tr>
                <th style="width:100px">Nazwisko</th>
                <th style="width:100px">Imię</th>
                <th style="width:30px"></th>
                <th style="width:30px"></th>
            </tr>
            <tr>
                <td style="width:100px"><asp:TextBox runat="server" ID="FindBox_LastName" name="FindBox_LastName" CssClass="form-control"></asp:TextBox></td>
                <td style="width:100px"><asp:TextBox runat="server" Id="FindBox_FirstName" name="FindBox_FirstName" CssClass="form-control"></asp:TextBox></td>
                <td style="width:100px"></td>
                <td style="width:100px"></td>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("LastName") %></td>
                <td><%# Eval("FirstName") %></td>
                <td><asp:LinkButton ID="lkbEdit" runat="server" OnCommand="EditAuthor" CommandArgument='<%# Eval("AuthorId") %>'> E </asp:LinkButton></td>
                <td><asp:LinkButton ID="lkbDelete" runat="server" OnCommand="DeleteAuthor" CommandArgument='<%# Eval("AuthorId") %>' OnClientClick="return confirm('Are You sure to delete?')"> X </asp:LinkButton></td> 
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <% if IsEditMode then %>
       <hr />
        <div class="form-group">
            <label for="Editbox_firstName">Imię</label>
            <input type="text" class="form-control" id="Editbox_firstName" name="Editbox_firstName" placeholder="Imię" value="<%=EditedAuthor.FirstName%>">
        </div>
        <div class="form-group">
            <label for="Editbox_lastName">Nazwisko</label>
            <input type="text" class="form-control" id="Editbox_lastName" name="Editbox_lastName" placeholder="Nazwisko"value="<%=EditedAuthor.LastName%>">
        </div>
        <input type="hidden" id="Editbox_id" name="Editbox_id" placeholder="Nazwisko"value="<%=EditedAuthor.AuthorId%>">
        <asp:Button runat="server" ID="btnEditSave" CssClass="btn btn-default" OnCommand="SaveAuthor" Text="Zapisz" OnClientClick="return validateAuthors();"/>
    <% end if%>
</asp:Content>
