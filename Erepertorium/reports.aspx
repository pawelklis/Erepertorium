<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="reports.aspx.cs" Inherits="Erepertorium.reports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <style>
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

    <div class="container" style="width: 100%!important; max-width: 100%;">
        <div class="row" style="width: 100%!important;height:80vh;">
            <div class="col-2">
                <asp:DropDownList ID="ddlyear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged" CssClass="btn btn-outline-success"></asp:DropDownList>
                <asp:DropDownList ID="ddlmonth" runat="server"  CssClass="btn btn-outline-success"></asp:DropDownList>

                <asp:Button ID="Button1" runat="server" Text="Generuj dane" OnClick="Button1_Click"  CssClass="btn btn-outline-success" />

            </div>
            <div class="col-9" style="overflow: auto; background-color: white;width:100%">

                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="800" Width="1200" ShowParameterPrompts="true">
                </rsweb:ReportViewer>

            </div>
        </div>
    </div>

  

</asp:Content>

