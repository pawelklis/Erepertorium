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
                }
                .immctn {
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
                    <div class="">

                        <div>
                            <h1 class="hdr" >E-repertorium</h1>

                        </div>

                        <div class="immctn" style="">
                            <img src="images/logo.svg" style="width: 320px; height: 320px;" />
                            </div>

                            <div class="inps" style="">

                                <asp:TextBox ID="txlogin" CssClass="btn btn-outline-success" runat="server" placeholder="Login" Width="230"></asp:TextBox>
                                <br />
                                <asp:TextBox ID="txpassword" CssClass="btn btn-outline-success" runat="server" TextMode="Password" Width="230" placeholder="Hasło"></asp:TextBox>
                                <br />
                                <asp:Button ID="btnLogin" runat="server" Text="Zaloguj" class="btn btn-outline-success" Style="width: 230px;" OnClick="btnLogin_Click" value="Zaloguj" />


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
    </form>
</body>
</html>
