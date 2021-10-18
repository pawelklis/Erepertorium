<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="stats.aspx.cs" Inherits="Erepertorium.stats" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div class="container" style="width: 100%!important; max-width: 100%;">
        <div class="row" style="width: 100%!important; height: 80vh;">
            <div class="col-2">
                <asp:DropDownList ID="ddlyear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged" CssClass="btn btn-outline-danger"></asp:DropDownList>
                <asp:DropDownList ID="ddlmonth" runat="server" CssClass="btn btn-outline-danger"></asp:DropDownList>

                <asp:Button ID="Button1" runat="server" Text="Generuj dane" OnClick="Button1_Click" CssClass="btn btn-outline-danger" />

            </div>
            <div class="col-9" style="overflow: auto; background-color: white; width: 100%">

                <rsweb:ReportViewer Width="1200" Height="800" ID="ReportViewer1" runat="server"></rsweb:ReportViewer>

            </div>
        </div>
    </div>




</asp:Content>
