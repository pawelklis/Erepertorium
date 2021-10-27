using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Erepertorium
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                Response.Redirect("~/login.aspx");
            else
            {
                UserType user = (UserType)Session["user"];
                ImageButton2.ToolTip = user.Name + " " + user.Surname;

                if (user.Level == 0)
                {
                    a3.Visible = false;
                }
                else
                {
                    a3.Visible = true;
                }
            }


        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txSearch.Text))
                return;

            List<RegistryType> l = new List<RegistryType>();
            l = RegistryType.LoadWhere<RegistryType>(" number='" + txSearch.Text +"' or content like '%" + txSearch.Text +"%' or date like'" + txSearch.Text + "%' order by number desc limit 1000;");

            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("number");
            dt.Columns.Add("user");
            dt.Columns.Add("date");
            dt.Columns.Add("hist");
            dt.Columns.Add("content");

            foreach(var o in l)
            {
                dt.Rows.Add(o.Id, o.Number, o.User, o.Date, o.HistoryToString(),o.Content);
            }


            dg1.DataSource = dt;
            dg1.DataBind();

            if (l.Count < 999)
            {
                des.Text = "Znaleziono rekordów: " + l.Count;
            }
            else
            {
                des.Text = "Znaleziono rekordów: >" + l.Count + " Liczba wyników przekracza 1000 pozycji, proszę użyć bardziej szczegółowego wyszukiwania";
            }

            myModal.Visible = true;

        }

        protected void ModalClose_Click(object sender, EventArgs e)
        {
            myModal.Visible = false;
        }

        protected void dg1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if(e.Row.RowType== DataControlRowType.DataRow)
                {

                }
            }
            catch (Exception)
            {


            }
        }

        protected void a1_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }

        protected void a2_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/reports.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("~/login.aspx");
        }

        protected void a3_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/admin.aspx");
        }

        protected void a4_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/stats.aspx");
        }

        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {
            txOldPWD.Text = "";
            txNewPWD.Text = "";
            txConfirmPWD.Text = "";
            pnChangePassword.Visible = true;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void btnSaveModal_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txNewPWD.Text))
            {
                //haslo nie moze byc puste
               this.Page.ClientScript.RegisterStartupScript( this.GetType(), "akl", "alert('Nowe hasło nie może być puste.');", true);
                return;
            }
            if (string.IsNullOrEmpty(txConfirmPWD.Text))
            {
                //powtórz nowe hasło
                ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Potwierdź nowe hasło.');", true);
                return;
            }
            if (string.IsNullOrEmpty(txOldPWD.Text))
            {
                //podaj biezace hasło
                ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Wprowadź bieżące hasło.');", true);
                return;
            }
            if (txNewPWD.Text.Length < 8)
            {
                //nowe haslo jest za krótkie
                ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Nowe hasło jest zbyt krótkie, podaj minimum 8 znaków.');", true);
                return;
            }

            UserType user = (UserType)Session["user"];

            if (txOldPWD.Text == user.localpwd)
            {
                if (txNewPWD.Text == txConfirmPWD.Text)
                {
                    user.localpwd = txNewPWD.Text;
                    user.SetLocalPassword();

                    txOldPWD.Text = "";
                    txNewPWD.Text = "";
                    txConfirmPWD.Text = "";
                    pnChangePassword.Visible = false;
                    //hasło zostało zmienione
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Sukces! Hasło zostało zmienione.');", true);
                }
                else
                {
                    //nowe hasła różnią się od siebie
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Wprowadzone hasła różnią się od siebie.');", true);
                    return;
                }
            }
            else
            {
                //biezace hasło jest nierpawidłowe
                ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Bieżące hasło jest nieprawidłowe.');", true);
                return;
            }

            txOldPWD.Text = "";
            txNewPWD.Text = "";
            txConfirmPWD.Text = "";
            pnChangePassword.Visible = false;
        }

        protected void btncancelmodal_Click(object sender, EventArgs e)
        {
            txOldPWD.Text = "";
            txNewPWD.Text = "";
            txConfirmPWD.Text = "";
            pnChangePassword.Visible = false;
        }
    }
}
