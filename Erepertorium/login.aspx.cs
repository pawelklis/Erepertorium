using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Erepertorium
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            UserType user;
            if (A_Directory.ValidateActiveDirectoryLogin(txlogin.Text, txpassword.Text) == true)
            {
                List<UserType> ul = UserType.LoadWhere<UserType>("login ='" + txlogin.Text + "' ");
                if (ul.Count > 0)
                {
                    //logowanie ad istnieje w bazie
                    user = ul[0];
                }
                else
                {
                    //logowanie ad nie istnieje w bazie
                    user = new UserType();
                    user.Login = txlogin.Text;
                    user.Name = txlogin.Text;
                    user.Surname = txlogin.Text;
                    user.Save();

                    user.localpwd = txpassword.Text;
                    user.SetLocalPassword();
                }

                user.GetLocalPassword();

                //if (txpassword.Text == user.localpwd)
                //{
                //logowanie ad hasło poprawne
                user.localpwd = txpassword.Text;
                user.SetLocalPassword();
                    Session["user"] = user;
                    Response.Redirect("~/default.aspx");
                //}
                //else
                //{
                //    //logowanie ad błędne hasło
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Podano błędne dane logowania!')", true);
                //}
            }
            else
            {
                //błędne logowanie ad

                List<UserType> ul = UserType.LoadWhere<UserType>("login ='" + txlogin.Text + "' ");
                if (ul.Count > 0)
                {
                    //logowanie lokalne istnieje w bazie
                    user = ul[0];
                    user.GetLocalPassword();

                    if (txpassword.Text == user.localpwd)
                    {
                        //logowanie lokalne hasło poprawne
                        Session["user"] = user;
                        Response.Redirect("~/default.aspx");
                    }
                    else
                    {
                        //logowanie lokalne błędne hasło
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Podano błędne dane logowania!')", true);
                    }

                }
                else
                {
                    //błedne logowanie ad, brak lokalnego użytkownika w bazie
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Podano błędne dane logowania lub użytkownik nie istnieje.')", true);
                }


            }




        }
    }
}