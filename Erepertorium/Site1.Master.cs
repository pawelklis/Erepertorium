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
    }
}
