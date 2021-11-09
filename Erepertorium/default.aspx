<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" EnableEventValidation="false" Inherits="Erepertorium._default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="Scripts/jquery-3.6.0.min.js"></script>

    <script src="Scripts/jquery.signalR-2.4.2.min.js"></script>

    <script src="signalr/hubs"></script>

    <!--Add script to update the page and send messages.-->
    <script type="text/javascript">
        $(function () {
            // Declare a proxy to reference the hub. 
            var chat = $.connection.chatHub;
            // Create a function that the hub can call to broadcast messages.
            chat.client.broadcastMessage = function (name, message) {
                // Html encode display name and message. 
                var encodedName = $('<div />').text(name).html();
                var encodedMsg = $('<div />').text(message).html();
                // Add the message to the page. 
                //$('#discussion').append('<li><strong>' + encodedName
                    //+ '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
               // $('Button1').click();

                document.getElementById('ContentPlaceHolder1_Button1').click();
            };
            // Get the user name and store it to prepend to messages.
           // $('#displayname').val(prompt('Enter your name:', ''));
            // Set initial focus to message input box.  
            //$('#message').focus();
            // Start the connection.
            $.connection.hub.start().done(function () {
                ////$('#sendmessage').click(function () {
                ////    // Call the Send method on the hub. 
                ////    chat.server.send($('#displayname').val(), $('#message').val());
                ////    // Clear text box and reset focus for next comment. 
                ////    $('#message').val('').focus();
                ////});
                //$('#btnEditModalDelte').click(function () {
                //    // Call the Send method on the hub. 
                //    chat.server.send($('#displayname').val(), $('#message').val());
                //    // Clear text box and reset focus for next comment. 
                //    //$('#message').val('').focus();
                //});
                //$('#btnEditModalCances').click(function () {
                //    // Call the Send method on the hub. 
                //    chat.server.send($('#displayname').val(), $('#message').val());
                //    // Clear text box and reset focus for next comment. 
                //    //$('#message').val('').focus();
                //});
                //$('#btnEditModaSave').click(function () {
                //    // Call the Send method on the hub. 
                //    chat.server.send($('#displayname').val(), $('#message').val());
                //    // Clear text box and reset focus for next comment. 
                //    //$('#message').val('').focus();
                //});
            });
        });
        

        // Odświeża wszystkich klientów
        function send() {
            var chat = $.connection.chatHub;
            $.connection.hub.start().done(function () {

                chat.server.send(1, 1);

            });
        };

    
    </script>

    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate></ContentTemplate>
    </asp:UpdatePanel>

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


    <!-- The Modal -->
    <asp:Panel runat="server" class="modaltlo" ID="myModal" Visible="false" Style="opacity: 1;">
        <div class="modal-dialog modal-xl">
            <div class="modal-content" style="background-color: silver; height: 80vh;">

                <!-- Modal Header -->
                <div class="modal-header">
                    <h4 class="modal-title">Nowe pozycje repertorium</h4>
                    <div>
                        <input Id="ccall" runat="server" type="color" title="Ustaw kolor wszystkich pozycji" style="padding: 0px;border-style: none;" name="favcolor"  />
                    </div>
                    
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

                                     <asp:Label ID="Label3" runat="server" Style="font-size: 20px; font-weight: 600;" Text='<%#Eval("number") %>'></asp:Label>
                                     <input Id="cc" title="Ustaw kolor" runat="server" type="color" style="padding: 0px;border-style: none;" name="favcolor" value='<%#Eval("color") %>' />                               
                                     
                                    <br />
                                    <asp:TextBox ID="tx1" runat="server" MaxLength="1000" TextMode="MultiLine" Style="width: 100%; height: 90%;" Text='<%#Eval("content") %>'></asp:TextBox>
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

                    <asp:Button ID="btnDeletemodal" runat="server" OnClick="btnDeletemodal_Click" type="button" class="btn btn-danger" data-bs-dismiss="modal" OnClientClick="return confirm('Potwierdzasz operacje?')" Text="Usuń wpisy"></asp:Button>
                    <asp:Button ID="btncancelmodal" runat="server" OnClick="btncancelmodal_Click" type="button" class="btn btn-danger" data-bs-dismiss="modal" OnClientClick="return confirm('Potwierdzasz operacje? \n Zmiany nie zostaną zapisane, pozycje nie zostaną usunięte.')" Text="Anuluj"></asp:Button>
                    <asp:Button ID="btnSaveModal" runat="server" class="btn btn-success" OnClick="btnSaveModal_Click" Text="Zatwierdź"></asp:Button>

                </div>

            </div>
        </div>
    </asp:Panel>




    <div class="container" style="width: 100%!important; max-width: 100%;">
        <div class="row" style="width: 100%!important;">

            <nav class="navbar navbar-expand-sm navbar-light bg-light">
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
                                            <button type="button" class="btn btn-outline-success dropdown-toggle" data-bs-toggle="dropdown">
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
                                            <span class="input-group-text btn-outline-success" style="background-color: transparent;">Dane z dnia:</span>
                                            <asp:TextBox ID="txDate" CssClass="btn btn-outline-success" Style="z-index: 1;" runat="server" TextMode="Date" OnTextChanged="txDate_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                        </div>




                                    </td>
                                    <td>
                                        <asp:CheckBox ID="ckdeleted" CssClass="btn btn-outline-success" runat="server" Text="Wyświetl usunięte pozycje" OnCheckedChanged="ckdeleted_CheckedChanged" AutoPostBack="true" />

                                    </td>
                                    <td>
                                        <asp:CheckBox ID="ckmy" CssClass="btn btn-outline-success" runat="server" Text="Wyświetl tylko moje pozycje" OnCheckedChanged="ckmy_CheckedChanged" AutoPostBack="true" />

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
                                                     <input Id="cce" runat="server" type="color" title="Ustaw kolor wszystkich pozycji" style="padding: 0px;border-style: none;" name="favcolor"  />
                                                </div>

                                                <!-- Modal body -->
                                                <div class="modal-body" style="overflow: auto; background-color: whitesmoke;">
                                                    <asp:GridView ID="dg4" CssClass="table table-light table-hover" runat="server" AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label1" runat="server" Text="Numer"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>                         
                                                                    <asp:Label ID="Label3" runat="server" Style="font-size: 20px; font-weight: 600;" Text='<%#Eval("number") %>'></asp:Label>
                                                                    <input Id="cc" title="Ustaw kolor" runat="server" type="color" style="padding: 0px;border-style: none;" name="favcolor" value='<%#Eval("color") %>' />
                                                                    
                                                                    <br />
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

                                                    <asp:Button ID="btnEditModalDelte" runat="server" OnClick="btnEditModalDelte_Click" type="button" class="btn btn-danger" data-bs-dismiss="modal" OnClientClick="return confirm('Potwierdzasz operacje?')" Text="Usuń wpisy"></asp:Button>
                                                    <asp:Button ID="btnEditModalCances" runat="server" OnClick="btnEditModalCances_Click" type="button" class="btn btn-danger" data-bs-dismiss="modal" OnClientClick="return confirm('Potwierdzasz operacje? \n Zmiany nie zostaną zapisane, pozycje nie zostaną usunięte.')" Text="Anuluj"></asp:Button>
                                                    <asp:Button ID="btnEditModaSave" runat="server" class="btn btn-success" OnClick="btnEditModaSave_Click" Text="Zatwierdź" ></asp:Button>

                                                </div>

                                            </div>
                                        </div>
                                    </asp:Panel>


                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <script>

                                function Mark(gid) {
                                    var elems = document.getElementsByName(gid);

                                    for (var i = 0; i < elems.length; i++) {
                                        elems[i].style.borderColor = "red";
                                    }
                                }
                                function MarkOut(gid) {
                                    var elems = document.getElementsByName(gid);

                                    for (var i = 0; i < elems.length; i++) {
                                        elems[i].style.borderColor = "inherit";
                                    }
                                }
                            </script>

                            <asp:GridView ID="dg3" CssClass="table table-hover table-light " runat="server" Style="z-index: 10;" ShowHeader="false" AutoGenerateColumns="false" OnRowDataBound="dg3_RowDataBound"
                                OnRowCommand="dg3_RowCommand" OnSelectedIndexChanged="dg3_SelectedIndexChanged">


                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="180" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label1" runat="server" Text="Numer"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                 

                                            <asp:Panel ID="pnc" name='<%#Eval("groupid") %>'  runat="server" ToolTip='<%#Eval("color") %>' style="border-radius:0%;box-shadow: 2px 2px 10px 1px rgba(66, 68, 90, 1);">
                                                <div onmouseover="Mark('<%#Eval("groupid") %>')" onmouseout="MarkOut('<%#Eval("groupid") %>')">
                                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("number") %>' ToolTip='<%#Eval("id") %>' Style="font-size: 40px; font-weight: 700;"></asp:Label><br />
                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("user") %>' Style="font-size: 10px;"></asp:Label>
                                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("date", " {0:HH:mm}") %>' Style="font-size: 10px;"></asp:Label>
                                                <asp:Label ID="lbgroup" runat="server" Text='<%#Eval("groupid") %>' Style="font-size: 1px;visibility:hidden;"></asp:Label>
                                                </div>
                                            </asp:Panel>
                                           
                                              

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                                        <HeaderTemplate>
                                            <asp:Label ID="Label2" runat="server" Text="Treść"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="txContent" runat="server" Text='<%#Eval("content") %>' Style="margin-left:10px; max-width: 100%; word-break: break-all;font-size:20px;" rows="10"></asp:Label>
                                            <asp:Panel ID="pnspin" runat="server" Visible="false" Style="align-items: center; display: flex;">

                                                <div class="spinner-grow text-success" style="opacity: 0.2; width: 10px; height: 10px;"></div>
                                                <div class="spinner-grow text-success" style="opacity: 0.4; width: 15px; height: 15px;"></div>
                                                <div class="spinner-grow text-success" style="opacity: 0.6; width: 20px; height: 20px;"></div>
                                                <div class="spinner-grow text-success" style="opacity: 0.8; width: 25px; height: 25px;"></div>
                                                <div class="spinner-grow text-success" style="flex; opacity: 1"></div>
                                                <asp:Label ID="Label44" runat="server" Style="font-size: 25px;"></asp:Label>

                                            </asp:Panel>
                                            <asp:Panel ID="Panel1" runat="server" Visible="false" Style="align-items: center; display: flex;">
                                                <p>Wpis usunięty</p>
                                            </asp:Panel>


                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-Width="130" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <style>
                                                .btedit{
                                                    border-style:solid;
                                                    border-width:1px;
                                                    border-radius:30%;
                                                    padding:8px;
                                                    background-color:white;
                                                    transition:all 1s;
                                                    margin:0px;
                                                    box-shadow: 6px 7px 20px -12px rgba(66, 68, 90, 1);
                                                }
                                                .btedit:hover{
                                                    border-color:green;
                                                    background-color:silver;
                                                    transition:all 1s;
                                                    padding:2px;
                                                }
                                            </style>
                                            <div style="display:inline-block;">
                                            <asp:ImageButton ID="ImageButton1" CssClass="btedit" ImageUrl="~/images/edit.png" CommandArgument='<%#Eval("status") %>' ToolTip="Edytuj pozycje" CommandName='<%#Eval("id") %>' Style="width: 50px; height: 50px; cursor:pointer;" runat="server" />
                                            
                                                <div onmouseover="Mark('<%#Eval("groupid") %>')" onmouseout="MarkOut('<%#Eval("groupid") %>')">
                                            <asp:ImageButton ID="ImageButton2"    CssClass="btedit" ImageUrl="~/images/eg.png" CommandArgument='<%#Eval("id") %>' ToolTip="Edytuj grupę pozycji"  CommandName="group"  AlternateText='<%#Eval("groupid") %>' Style="width: 50px; height: 50px; cursor:pointer;" runat="server" />

                                                   </div>
                                                </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>


                            </asp:GridView>
                            <asp:Timer ID="Timer2" runat="server" Interval="10000" OnTick="Timer2_Tick"></asp:Timer>


                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>


    <asp:Button ID="Button1" runat="server" Width="1" Height="1" Text="Button" OnClick="Button1_Click" style="visibility:hidden;" />


</asp:Content>
