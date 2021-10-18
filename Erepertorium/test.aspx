<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Erepertorium.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DropDownList ID="ddl1" runat="server">
                <asp:ListItem>jeden</asp:ListItem>
                <asp:ListItem>jeden</asp:ListItem>
                <asp:ListItem>jeden</asp:ListItem>
                <asp:ListItem>jeden</asp:ListItem>
                <asp:ListItem>jeden</asp:ListItem>
                <asp:ListItem>cztery</asp:ListItem>
            </asp:DropDownList>

            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>



            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
            <asp:GridView ID="dg1" runat="server"></asp:GridView>
            <asp:Timer ID="Timer1" runat="server" Interval="2000" OnTick="Timer1_Tick"></asp:Timer>
                </ContentTemplate>
            </asp:UpdatePanel>











        </div>
    </form>
</body>
</html>
