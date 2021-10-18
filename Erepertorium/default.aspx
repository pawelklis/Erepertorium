﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Erepertorium._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate></ContentTemplate>
    </asp:UpdatePanel>

    <style>
        .modaltlo {
            width: 100%;
            height: 100vh;
            background-color: #212529;
            position: absolute;
            top: 0px;
            left: 0px;
            z-index: 2;
        }
    </style>


    <!-- The Modal -->
    <asp:Panel runat="server" class="modaltlo" ID="myModal" Visible="false" Style="opacity: 1;">
        <div class="modal-dialog modal-xl">
            <div class="modal-content" style="background-color: silver; height: 80vh;">

                <!-- Modal Header -->
                <div class="modal-header">
                    <h4 class="modal-title">Nowe pozycje repertorium</h4>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <!-- Modal body -->
                <div class="modal-body" style="overflow: auto; background-color: whitesmoke;">
                    <asp:GridView ID="dg2" CssClass="table table-light table-hover" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="Label1" runat="server" Text="Numer"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Style="font-size: 20px; font-weight: 600;" Text='<%#Eval("number") %>'></asp:Label><br />
                                    <asp:TextBox ID="tx1" runat="server"  MaxLength="1000" TextMode="MultiLine" Style="width: 100%; height: 90%;" Text='<%#Eval("content") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-Width="60" Visible="false">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/images/edit.png" Style="width: 30px; height: 30px;" CommandArgument='<%#Eval("id") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>


                </div>

                <!-- Modal footer -->
                <div class="modal-footer">

                    <asp:Button ID="btnDeletemodal" runat="server" OnClick="btnDeletemodal_Click" type="button" class="btn btn-danger" data-bs-dismiss="modal" Text="Usuń wpisy"></asp:Button>
                    <asp:Button ID="btncancelmodal" runat="server" OnClick="btncancelmodal_Click" type="button" class="btn btn-danger" data-bs-dismiss="modal" Text="Anuluj"></asp:Button>
                    <asp:Button ID="btnSaveModal" runat="server" class="btn btn-success" OnClick="btnSaveModal_Click" Text="Zatwierdź"></asp:Button>

                </div>

            </div>
        </div>
    </asp:Panel>

 


    <div class="container" style="width: 100%!important; max-width: 100%;">
        <div class="row" style="width: 100%!important;">

            <nav class="navbar navbar-expand-sm navbar-dark bg-dark">
                <div class="container-fluid">
                      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#mynavbar">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="mynavbar">
                    <div class="" style="min-width: 450px;">


                        <table>
                            <tr>
                                <td>
                                    <div class="dropdown" style="position: relative; top: 10%; left: 0%;">
                                        <button type="button" class="btn btn-outline-danger dropdown-toggle" data-bs-toggle="dropdown">
                                            Dodaj pozycje
                                        </button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:Button ID="bnt1" runat="server" CssClass="dropdown-item" OnClick="btn1_Click" Text="1" />
                                            </li>
                                            <li>
                                                <asp:Button ID="btn2" runat="server" CssClass="dropdown-item" OnClick="btn2_Click" Text="2" />
                                            </li>
                                            <li>
                                                <asp:Button ID="btn3" runat="server" CssClass="dropdown-item" OnClick="btn3_Click" Text="3" />
                                            </li>
                                            <li>
                                                <asp:Button ID="btn4" runat="server" CssClass="dropdown-item" OnClick="btn4_Click" Text="4" />
                                            </li>
                                            <li>
                                                <asp:Button ID="btn5" runat="server" CssClass="dropdown-item" OnClick="btn5_Click" Text="5" />
                                            </li>
                                            <li>
                                                <asp:Button ID="btn6" runat="server" CssClass="dropdown-item" OnClick="btn6_Click" Text="6" />
                                            </li>
                                            <li>
                                                <asp:Button ID="btn7" runat="server" CssClass="dropdown-item" OnClick="btn7_Click" Text="7" />
                                            </li>
                                            <li>
                                                <asp:Button ID="btn8" runat="server" CssClass="dropdown-item" OnClick="btn8_Click" Text="8" />
                                            </li>
                                            <li>
                                                <asp:Button ID="btn9" runat="server" CssClass="dropdown-item" OnClick="btn9_Click" Text="9" />
                                            </li>
                                            <li>
                                                <asp:Button ID="btn10" runat="server" CssClass="dropdown-item" OnClick="btn10_Click" Text="10" /></li>
                                        </ul>
                                    </div>
                                </td>
                                <td>
                                    <div class="input-group">
                                        <span class="input-group-text btn-outline-danger" style="background-color: transparent;">Dane z dnia:</span>
                                        <asp:TextBox ID="txDate" CssClass="btn btn-outline-danger" Style="z-index: 1;" runat="server" TextMode="Date"></asp:TextBox>
                                    </div>




                                </td>
                                <td>
                                    <asp:CheckBox ID="ckdeleted" CssClass="btn btn-outline-danger" runat="server" Text="Wyświetl usunięte pozycje" />

                                </td>
                                <td>
                                    <asp:CheckBox ID="ckmy" CssClass="btn btn-outline-danger" runat="server" Text="Wyświetl tylko moje pozycje" />

                                </td>
                            </tr>
                        </table>







                        <style>
                            .hidden-mobile {
                                opacity: 1;
                                transition: all 2s;
                                display: none;
                            }

                            @media only screen and (max-width: 1309px) {
                                .hidden-mobile {
                                    opacity: 0;
                                    transition: all 2s;
                                }
                            }

                            .ftl {
                                background-color: red;
                            }

                            .hd {
                                z-index: 1 !important;
                            }
                        </style>

                        <div class="hidden-mobile" style="border-style: solid; border-color: white; z-index: 0; border-radius: 50%; width: 400px; height: 400px; padding: 35px; border-width: 1px; position: fixed; bottom: 10%; left: 5%;">
                            <img src="images/logo.svg" style="width: 320px; height: 320px;" />
                        </div>

                    </div>
        </div>
                   
                </div>
            </nav>

            <div class="" style="">
                

        <div style="overflow: auto; height: 80vh;">

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    
                <ContentTemplate>
                         <!-- The Modal -->
                    <asp:UpdatePanel ID="upm" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>


    <asp:Panel runat="server" class="modaltlo" ID="editModal" Visible="false" Style="opacity: 1;">
        <div class="modal-dialog modal-xl">
            <div class="modal-content" style="background-color: silver; height: 80vh;">

                <!-- Modal Header -->
                <div class="modal-header">
                    <h4 class="modal-title">Edycja pozycji repertorium</h4>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <!-- Modal body -->
                <div class="modal-body" style="overflow: auto; background-color: whitesmoke;">
                    <asp:GridView ID="dg4" CssClass="table table-light table-hover"  runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="Label1" runat="server" Text="Numer"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Style="font-size: 20px; font-weight: 600;" Text='<%#Eval("number") %>'></asp:Label><br />
                                    <asp:TextBox ID="tx1" runat="server" TextMode="MultiLine" MaxLength="1000" Style="width: 100%; height: 90%;" Text='<%#Eval("content") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>



                            <asp:TemplateField ItemStyle-Width="60" Visible="false">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/images/edit.png" Style="width: 30px; height: 30px;" CommandArgument='<%#Eval("Id") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>


                </div>

                <!-- Modal footer -->
                <div class="modal-footer">

                    <asp:Button ID="btnEditModalDelte" runat="server" OnClick="btnEditModalDelte_Click" type="button" class="btn btn-danger" data-bs-dismiss="modal" Text="Usuń wpisy"></asp:Button>
                    <asp:Button ID="btnEditModalCances" runat="server" OnClick="btnEditModalCances_Click" type="button" class="btn btn-danger" data-bs-dismiss="modal" Text="Anuluj"></asp:Button>
                    <asp:Button ID="btnEditModaSave" runat="server" class="btn btn-success" OnClick="btnEditModaSave_Click" Text="Zatwierdź"></asp:Button>

                </div>

            </div>
        </div>
    </asp:Panel>

                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    <asp:GridView ID="dg3" CssClass="table table-hover table-dark "  runat="server" style="z-index: 10;" AutoGenerateColumns="false" OnRowDataBound="dg3_RowDataBound" OnRowCommand="dg3_RowCommand">


                        <Columns>
                            <asp:TemplateField ItemStyle-Width="180" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:Label ID="Label1" runat="server" Text="Numer"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%#Eval("number") %>' ToolTip='<%#Eval("id") %>' Style="font-size: 40px; font-weight: 700;"></asp:Label><br />
                                    <asp:Label ID="Label4" runat="server" Text='<%#Eval("user") %>'  Style="font-size: 10px;"></asp:Label>
                                    <asp:Label ID="Label5" runat="server" Text='<%#Eval("date", " {0:hh:mm}") %>'  Style="font-size: 10px;"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                                <HeaderTemplate>
                                    <asp:Label ID="Label2" runat="server" Text="Treść"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txContent" runat="server" Text='<%#Eval("content") %>' style="max-width:100%;word-break:break-all;" rows="10"></asp:Label>
                                    <asp:Panel ID="pnspin" runat="server" Visible="false" Style="align-items: center; display: flex;">

                                        <div class="spinner-grow text-danger" style="opacity: 0.2; width: 10px; height: 10px;"></div>
                                        <div class="spinner-grow text-danger" style="opacity: 0.4; width: 15px; height: 15px;"></div>
                                        <div class="spinner-grow text-danger" style="opacity: 0.6; width: 20px; height: 20px;"></div>
                                        <div class="spinner-grow text-danger" style="opacity: 0.8; width: 25px; height: 25px;"></div>
                                        <div class="spinner-grow text-danger" style="flex; opacity: 1"></div>
                                        <asp:Label ID="Label44" runat="server" Text='<%#" Edytowany przez: " + Eval("user") %>' Style="font-size: 25px;"></asp:Label>

                                    </asp:Panel>
                                    <asp:Panel ID="Panel1" runat="server" Visible="false" Style="align-items: center; display: flex;">
                                        <p>Wpis usunięty</p>
                                    </asp:Panel>


                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center"  ItemStyle-VerticalAlign="Middle">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/images/edit.png" CommandArgument='<%#Eval("status") %>' CommandName='<%#Eval("id") %>' Style="width: 30px; height: 30px;" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>


                    </asp:GridView>
                    <asp:Timer ID="Timer2" runat="server" Interval="2000" OnTick="Timer2_Tick"></asp:Timer>


                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
            </div>
        </div>
    </div>




</asp:Content>