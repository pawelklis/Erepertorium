<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Erepertorium.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">


    <title>E-repertorium</title>

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <script src="Scripts/jquery-3.6.0.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.bundle.min.js"></script>
    <%--<link href="declares.css" rel="stylesheet" />--%>
</head>
<body>
    <form id="form1" runat="server">
        <div>


            <style>
                .lgn {
                    position: absolute;
                    width: 100%;
                    height: 99vh;
                    z-index: 100;
                    background-color: white;
                    top: 0px;
                    
                }

                .lgn_hide {
                    position: absolute;
                    width: 100%;
                    height: 99vh;
                    z-index: 100;
                    background-color: var(--darkbg);
                    top: -2000px;
                    
                }
                .hdr {
                    position: absolute;
                    top: 9%;
                    left: 5%;
                    font-size: 100px;
                    color: black;
                    display:none;
                }
                .immctn {
                    display:none;
                    border-style: solid;
                    border-color: transparent;
                    z-index: 0;
                    border-radius: 50%;
                    width: 400px;
                    height: 400px;
                    padding: 35px;
                    border-width: 1px;
                    position: absolute;
                    bottom: 30%;
                    left: 15%;
                    background-color:dimgray;
                }

                .inps {
                    display:none;
                    position: absolute;
                    top: 48%;
                    left: 60%
                }

                @media only screen and (max-width: 790px) {
                    .hdr {
                        top:0%;
                        font-size:50px;
                    }
                    .inps{
                        top:10%;
                        left:30%;
                    }
                    .immctn{
                        bottom:20%;
                        left:20%;
                    }
                }
                @media only screen and (max-height: 600px) {
                .immctn{
                    width:100px;
                    height:100px;
                    bottom:0px;
                }
                .hdr{
                    top:-100px;
                }
                
                
                }
            </style>

            <!-- Logowanie -->
            <div class="lgn" id="lg">
                <div class="container">
                    <div class="" >

                        <div>
                            <h1 class="hdr mb-3" >E-repertorium</h1>

                        </div>

                        <div class="immctn" style="">
                            <img src="images/logo.svg" style="width: 320px; height: 320px;" />
                            </div>

                            <div class="inps" style="">

                                <asp:TextBox ID="txlogine" CssClass="btn btn-outline-success" runat="server" placeholder="Login" Width="230"></asp:TextBox>
                                <br />
                                <asp:TextBox ID="txpassworde" CssClass="btn btn-outline-success" runat="server" TextMode="Password" Width="230" placeholder="Hasło"></asp:TextBox>
                                <br />
                                <asp:Button ID="btnLogine" runat="server" Text="Zaloguj" class="btn btn-outline-success" Style="width: 230px;" OnClick="btnLogin_Click" value="Zaloguj" />


                        </div>
                    </div>
                </div>
            </div>





            <script>
                function hide() {
                    document.getElementById('lg').className = "lgn_hide";
                }
                function show() {
                    document.getElementById('lg').className = "lgn";
                }
            </script>


        </div>


        <div class="container" style="position: absolute; top: 50%; left: 50%; width: 50%; height: 50%; z-index: 100; transform: translate(-50%, -50%); border-style: none; border-color: black; border-radius: 15px; border-width: 1px;display:grid;">

            <div class="row d-flex align-items-center justify-content-center h-100" style="" >

                <div style="background-color: #393f47;border-radius:20px;box-shadow: 8px 8px 24px 0px rgba(66, 68, 90, 1);">
                    <h1 class="mb-3" style="color:white;font-size:50px;" >E-repertorium</h1>
                    <table style="border: none;">
                        <tr>
                            <td style="width:75%;">

                                <img src="images/logo.svg" style="width: 320px; height: 320px;" />

                            </td>
                            <td style="text-align:right;">
                                <div style="background-color: #393f47;">
                                    <asp:TextBox ID="txlogin" CssClass="form-control form-control-lg" runat="server" placeholder="Login" Width="230"></asp:TextBox>
                                    <br />
                                        <asp:TextBox ID="txpassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" Width="230" placeholder="Hasło"></asp:TextBox>                       
                                    <br />
                                    <asp:Button ID="btnLogin" runat="server" Text="Zaloguj" class="btn btn-success btn-lg btn-block" Style="width: 230px;" OnClick="btnLogin_Click" value="Zaloguj" />
                                </div>
                            </td>
                        </tr>
                    </table>


                </div>
    



            </div>


        </div>

    </form>
</body>
</html>
