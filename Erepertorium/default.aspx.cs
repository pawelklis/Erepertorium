using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Erepertorium
{
    public partial class _default : System.Web.UI.Page
    {
        private string user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txDate.Text = DateTime.Today.ToShortDateString();
                BindDG();
                //Generate();
            }

            UserType userObject = (UserType)Session["user"];
            if(userObject!=null)
                user = userObject.Name + " " + userObject.Surname;

            //Timer2.Enabled = true;

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "filter", publicMethods.ShowTableFilter("ContentPlaceHolder1_dg3", 0, false), false);
        }


        void Generate()
        {

            DateTime date = DateTime.Parse("2021-09-01");

            List<string> us = new List<string>();
            us.Add("Jan Kowalski");
            us.Add("Magdalena Nowak-Kowalska");
            us.Add("Jan Janowski");
            us.Add("Urszula Urszulska");
            us.Add("Amadeusz Mozart");


            List<string> conts = new List<string>();
            conts.Add("Czynność 1");
            conts.Add("Czynność 2");
            conts.Add("Czynność 3");
            conts.Add("Czynność 4");
            conts.Add("Czynność 5");
            conts.Add("Czynność 6");
            conts.Add("Czynność 7");
            conts.Add("Czynność 8");



            for (int i = 0; i < 30; i++)
            {
                for (int ii = 0; ii < 300; ii++)
                {
                    Random rnd = new Random();
                    string user = us[rnd.Next(0, us.Count)];
                    string ct = conts[rnd.Next(0, conts.Count)];

                    RegistryType r = new RegistryType();
                    r.Date = date;
                    r.User = user;
                    r.Number = RegistryType.GetNextNumber();
                    r.Content = ct;
                
                    r.Save();

                    
                }
date = date.AddDays(1);
            }


        }
     
        protected void a1_ServerClick(object sender, EventArgs e)
        {
            Binddg2(1);
            myModal.Visible = true;
        }

        void Binddg2(int ir)
        {


            List<RegistryType> L = RegistryType.CreateNewEntries(ir,user);

            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("number");
            dt.Columns.Add("content");

            foreach(var r in L)
            {
                dt.Rows.Add(r.Id, r.Number, r.Content);
            }

            dg2.DataSource = dt;
            dg2.DataBind();
        }



     


        protected void btnSaveModal_Click(object sender, EventArgs e)
        {
            foreach(GridViewRow row in dg2.Rows)
            {
                int id = int.Parse(((ImageButton)row.FindControl("ImageButton1")).CommandArgument);
                string content = ((TextBox)row.FindControl("tx1")).Text;

                RegistryType.ChangeContent(content, user, id);
            }
            myModal.Visible = false;
        }

        protected void btncancelmodal_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dg2.Rows)
            {
                int id = int.Parse(((ImageButton)row.FindControl("ImageButton1")).CommandArgument);
                string content = ((TextBox)row.FindControl("tx1")).Text;

                RegistryType.CancelEdit(user, id);
            }
            myModal.Visible = false;
        }

        protected void btnDeletemodal_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dg2.Rows)
            {
                int id = int.Parse(((ImageButton)row.FindControl("ImageButton1")).CommandArgument);
                string content = ((TextBox)row.FindControl("tx1")).Text;

                RegistryType.DeleteRegistry(user, id);
            }
            myModal.Visible = false;
        }

        void BindDG()
        {

            DateTime date = DateTime.Today;
            try
            {
               date = DateTime.Parse(txDate.Text);
            }
            catch (Exception)
            {

            }


            List<RegistryType> l = RegistryType.LoadByDate(date, ckdeleted.Checked,ckmy.Checked,user);

            l.ForEach(p =>
            {
                p.Content = p.Content.Replace("\n", "<br/>");
            });

            dg3.DataSource = l;
            dg3.DataBind();
        }

        protected void Timer2_Tick(object sender, EventArgs e)
        {
            BindDG();
        }

        protected void dg3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label txcontext = (Label)e.Row.FindControl("txContent");
                Label lbuser = (Label)e.Row.FindControl("Label4");
                Panel pnspin = (Panel)e.Row.FindControl("pnspin");
                Panel panel1 = (Panel)e.Row.FindControl("Panel1");
                ImageButton imgbtn = (ImageButton)e.Row.FindControl("ImageButton1");
                int status = int.Parse(imgbtn.CommandArgument);

                imgbtn.Visible = true;

                if (status == 0)
                {
                   
                    imgbtn.Visible = true;
                    pnspin.Visible = false;
                    txcontext.Visible = true;
                    panel1.Visible = false;
                }
                if (status == 1)
                {
                    if(lbuser.Text!=user)
                        imgbtn.Visible = false;
                    pnspin.Visible = true;
                    txcontext.Visible = false;
                    panel1.Visible = false;
                }
                if (status == 5)
                {
                    if (lbuser.Text != user)
                        imgbtn.Visible = false;
                    pnspin.Visible = false;
                    txcontext.Visible = false;
                    panel1.Visible = true;
                }

            }
        }

        protected void dg3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Timer2.Enabled = false;

                int id = int.Parse(e.CommandName.ToString());
                RegistryType r = RegistryType.Load<RegistryType>(id);
                r.BeginEdit();
                List<RegistryType> l = new List<RegistryType>();
                l.Add(r);
                dg4.DataSource = l;
                dg4.DataBind();

                editModal.Visible = true;

            }
            catch (Exception)
            {

                
            }
        }

        protected void btnEditModalDelte_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dg4.Rows)
            {
                int id = int.Parse(((ImageButton)row.FindControl("ImageButton1")).CommandArgument);
                string content = ((TextBox)row.FindControl("tx1")).Text;

                RegistryType.DeleteRegistry(user, id);
            }
            editModal.Visible = false;

            Timer2.Enabled = true;
        }

        protected void btnEditModalCances_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dg4.Rows)
            {
                int id = int.Parse(((ImageButton)row.FindControl("ImageButton1")).CommandArgument);
                string content = ((TextBox)row.FindControl("tx1")).Text;

                RegistryType.CancelEdit(user, id);
            }
            editModal.Visible = false;
            Timer2.Enabled = true;
        }

        protected void btnEditModaSave_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dg4.Rows)
            {
                int id = int.Parse(((ImageButton)row.FindControl("ImageButton1")).CommandArgument);
                string content = ((TextBox)row.FindControl("tx1")).Text;

                RegistryType.ChangeContent(content,user, id);
            }
            editModal.Visible = false;
            Timer2.Enabled = true;
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            Binddg2(1);
            myModal.Visible = true;
        }


        protected void btn2_Click(object sender, EventArgs e)
        {
            Binddg2(2);
            myModal.Visible = true;
        }

        protected void btn3_Click(object sender, EventArgs e)
        {
            Binddg2(3);
            myModal.Visible = true;
        }

        protected void btn4_Click(object sender, EventArgs e)
        {
            Binddg2(4);
            myModal.Visible = true;
        }

        protected void btn5_Click(object sender, EventArgs e)
        {
            Binddg2(5);
            myModal.Visible = true;
        }

        protected void btn6_Click(object sender, EventArgs e)
        {
            Binddg2(6);
            myModal.Visible = true;
        }

        protected void btn7_Click(object sender, EventArgs e)
        {
            Binddg2(7);
            myModal.Visible = true;
        }

        protected void btn8_Click(object sender, EventArgs e)
        {
            Binddg2(8);
            myModal.Visible = true;
        }

        protected void btn9_Click(object sender, EventArgs e)
        {
            Binddg2(9);
            myModal.Visible = true;
        }
        protected void btn10_Click(object sender, EventArgs e)
        {
            Binddg2(10);
            myModal.Visible = true;
        }
    }
}