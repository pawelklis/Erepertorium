<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Erepertorium.admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <style>
        .modaltlo {
            width: 100%;
            height: 100vh;
            background-color: whitesmoke;
            position: absolute;
            top: 0px;
            left: 0px;
            z-index: 2;
        }
    </style>


            <nav class="navbar navbar-expand-sm navbar-light bg-light" >
 
                <div style="float:left;width:100%">
                    <asp:Button ID="btnAdd" CssClass="btn btn-outline-success" runat="server" Text="Dodaj użytkownika" OnClick="btnAdd_Click" />       
                    <asp:Button ID="btnSave" CssClass="btn btn-outline-success" runat="server" Text="Zapisz" OnClick="btnSave_Click" />
                    
                    <asp:Button ID="btnReorder" style="float:right;" CssClass="btn btn-outline-danger" runat="server" ToolTip="Operacja spowoduje przenumerowanie wszystkich pozycji w bieżacym roku." Text="Przenumeruj bazę" OnClick="btnReorder_Click" OnClientClick="return confirm('Operacja spowoduje przenumerowanie wszystkich pozycji w bieżacym roku. Operacji nie można cofnąć. Czy chcesz kontynuować?')" />
                </div>


            </nav>





    <div>
        <asp:GridView ID="dg1" CssClass="table table-hover table-light" runat="server" AutoGenerateColumns="false" OnRowDataBound="dg1_RowDataBound" OnRowCommand="dg1_RowCommand">
            <columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="Label1" runat="server" Text="Użytkownik"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:TextBox ID="txlogin" runat="server"  Style="width: 100%; height: 90%;" Text='<%#Eval("login") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="Label2" runat="server" Text="Imie"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:TextBox ID="txname" runat="server"  Style="width: 100%; height: 90%;" Text='<%#Eval("name") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="Label3" runat="server" Text="Nazwisko"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:TextBox ID="txsurname" runat="server"  Style="width: 100%; height: 90%;" Text='<%#Eval("surname") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
 
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="Label4" runat="server" Text="Poziom uprawnień"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:DropDownList id="ddllevel" runat="server" CssClass="btn btn-outline-success" ToolTip='<%#Eval("level") %>'>
                                        <asp:ListItem Text="Użytkownik" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Administrator" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                           <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                                    
                                    <asp:Button ID="Button1" CssClass="btn btn-outline-warning" runat="server" Text="Wygeneruj hasło" OnClientClick="return confirm('Potwierdzasz operacje?')" CommandName="gen" CommandArgument='<%#Eval("id") %>' />
                                    <asp:Button ID="btnDel" CssClass="btn btn-outline-danger" runat="server" Text="Usuń" OnClientClick="return confirm('Potwierdzasz operacje?')" CommandName="del" CommandArgument='<%#Eval("id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </columns>
        </asp:GridView>
    </div>




</asp:Content>
