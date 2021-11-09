using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
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
                txDate.Text = FromMinusDate(DateTime.Today.ToShortDateString()).ToShortDateString();
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
                    r.Number = RegistryType.GetNextNumber(DateTime.Now.Year);
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
            

            List<RegistryType> L = RegistryType.CreateNewEntries(ir,user, DateTime.Now.Year);

            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("number");
            dt.Columns.Add("content");
            dt.Columns.Add("color");
            dt.Columns.Add("groupid");

            foreach(var r in L)
            {
                dt.Rows.Add(r.Id, r.Number, r.Content, r.color, r.GroupId);
            }

            dg2.DataSource = dt;
            dg2.DataBind();
        }



     


        protected void btnSaveModal_Click(object sender, EventArgs e)
        {

            string allcolor = ccall.Value;



            foreach(GridViewRow row in dg2.Rows)
            {
                int id = int.Parse(((ImageButton)row.FindControl("ImageButton1")).CommandArgument);
                string content = ((TextBox)row.FindControl("tx1")).Text;

                System.Web.UI.HtmlControls.HtmlInputGenericControl c = ((System.Web.UI.HtmlControls.HtmlInputGenericControl)row.FindControl("cc"));
                string color = "#ffffff";
                if (allcolor != "#ffffff")
                    color = allcolor;
                else
                    color = c.Value;


                RegistryType.ChangeContent(content, user, id, color);
            }
            myModal.Visible = false;
            BindDG();
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
            BindDG();
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
            BindDG();
        }

        DateTime FromDotsDate(string DateWithDots)
        {
            String result = DateTime.Today.ToString();

            try
            {
                result = DateTime
     .ParseExact(DateWithDots, "dd.MM.yyyy", CultureInfo.InvariantCulture)
     .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                return DateTime.Parse(result);
            }
            catch (Exception)
            {
                return DateTime.Parse(DateWithDots);
            }

        } 
        
        DateTime FromMinusDate(string DateWithoutDots)
        {
            String result = DateTime.Today.ToString();

            try
            {
                result = DateTime
     .ParseExact(DateWithoutDots, "yyyy-MM-dd", CultureInfo.InvariantCulture)
     .ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

                DateTime dt = DateTime.ParseExact(result,"dd.MM.yyyy", CultureInfo.InvariantCulture);

                return dt;
            }
            catch (Exception)
            {
                return DateTime.Parse(DateWithoutDots);
            }

        }

        void BindDG()
        {

            DateTime date = DateTime.Today;
            try
            {

                date = FromDotsDate(txDate.Text);
            }
            catch (Exception ex)
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
            //Check();
            //Timer2.Enabled = false;
        }

        void Check()
        {
            if (Session["ct"] == null)
                Session["ct"] = 0;

            DateTime date = DateTime.Today;
            try
            {
                date = DateTime.Parse(txDate.Text);
            }
            catch (Exception)
            {
            }
            int ct = RegistryType.GetCount(date, ckdeleted.Checked, ckmy.Checked, user);

            int storedCt =int.Parse( Session["ct"].ToString());

            if (ct != storedCt)
            {
                Session["ct"] = ct;
                BindDG();
            }

        }
        protected void dg3_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dg3.Rows)
            {
                if (row.RowIndex == dg3.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#20c997");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select this row.";
                }
            }
        }
        protected void dg3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dg3, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";

                Label txcontext = (Label)e.Row.FindControl("txContent");
                Label lbuser = (Label)e.Row.FindControl("Label4");
                Label lb44 = (Label)e.Row.FindControl("Label44");
                Label lbgroup = (Label)e.Row.FindControl("lbgroup");
                Panel pnspin = (Panel)e.Row.FindControl("pnspin");
                Panel panel1 = (Panel)e.Row.FindControl("Panel1");
                Panel pnc = (Panel)e.Row.FindControl("pnc");
                ImageButton imgbtn = (ImageButton)e.Row.FindControl("ImageButton1");
                ImageButton imgbtn2 = (ImageButton)e.Row.FindControl("ImageButton2");
                int status = int.Parse(imgbtn.CommandArgument);

                e.Row.Attributes.Add("Name", lbgroup.Text);
                

                if (pnc.ToolTip == "#ffffff")
                    pnc.Style.Add("background-color", "transparent");
                else
                    pnc.Style.Add("background-color", pnc.ToolTip);

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

                    RegistryType r = RegistryType.Load<RegistryType>(int.Parse(imgbtn.CommandName));

                    string cu = r.History.Last().User.ToString();

                    lb44.Text ="Edytowany przez: " + cu;

                    //if (lbuser.Text != cu)
                    //{
                        //imgbtn.Visible = false;

                        imgbtn.Visible = false;
                        imgbtn2.Visible = false;
                    //}
                    pnspin.Visible = true;
                    txcontext.Visible = false;
                    panel1.Visible = false;
                }
                if (status == 5)
                {
                    //if (lbuser.Text != user)
                        //imgbtn.Visible = false;
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
                UserType userObject = (UserType)Session["user"];
                if (userObject != null)
                    user = userObject.Name + " " + userObject.Surname;

                Timer2.Enabled = false;

                if (e.CommandName == "group")
                {
                    int id = int.Parse(e.CommandArgument.ToString());

                    ImageButton btn = e.CommandSource as ImageButton;

                    string groupid = btn.AlternateText;
                    List<RegistryType> l = RegistryType.LoadWhere<RegistryType>(" groupid='" + groupid + "' ");

                    foreach(var r in l)
                    {
                        r.BeginEdit(user);
                    }

                    dg4.DataSource = l;
                    dg4.DataBind();

                    cce.Value = "#ffffff";
                    editModal.Visible = true;

                }
                else
                {
                    int id = int.Parse(e.CommandName.ToString());
                    RegistryType r = RegistryType.Load<RegistryType>(id);
                    r.BeginEdit(user);
                    List<RegistryType> l = new List<RegistryType>();
                    l.Add(r);
                    dg4.DataSource = l;
                    dg4.DataBind();

                    cce.Value = "#ffffff";
                    editModal.Visible = true;
                }



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

            BindDG();
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
            
            BindDG();
        }

        protected void btnEditModaSave_Click(object sender, EventArgs e)
        {
            string allcolor = cce.Value;

            foreach (GridViewRow row in dg4.Rows)
            {
                int id = int.Parse(((ImageButton)row.FindControl("ImageButton1")).CommandArgument);
                string content = ((TextBox)row.FindControl("tx1")).Text;

                System.Web.UI.HtmlControls.HtmlInputGenericControl c = ((System.Web.UI.HtmlControls.HtmlInputGenericControl)row.FindControl("cc"));
                string color = "#ffffff";
                if (allcolor != "#ffffff")
                    color = allcolor;
                else
                    color = c.Value;

                RegistryType.ChangeContent(content,user, id,color);
            }
            editModal.Visible = false;
            Timer2.Enabled = true;

            BindDG();
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            Binddg2(1);
            ccall.Value = "#ffffff";
            myModal.Visible = true;
        }


        protected void btn2_Click(object sender, EventArgs e)
        {
            Binddg2(2);
            ccall.Value = "#ffffff";
            myModal.Visible = true;
        }

        protected void btn3_Click(object sender, EventArgs e)
        {
            Binddg2(3);
            ccall.Value = "#ffffff";
            myModal.Visible = true;
        }

        protected void btn4_Click(object sender, EventArgs e)
        {
            Binddg2(4);
            ccall.Value = "#ffffff";
            myModal.Visible = true;
        }

        protected void btn5_Click(object sender, EventArgs e)
        {
            Binddg2(5);
            ccall.Value = "#ffffff";
            myModal.Visible = true;
        }

        protected void btn6_Click(object sender, EventArgs e)
        {
            Binddg2(6);
            ccall.Value = "#ffffff";
            myModal.Visible = true;
        }

        protected void btn7_Click(object sender, EventArgs e)
        {
            Binddg2(7);
            ccall.Value = "#ffffff";
            myModal.Visible = true;
        }

        protected void btn8_Click(object sender, EventArgs e)
        {
            Binddg2(8);
            ccall.Value = "#ffffff";
            myModal.Visible = true;
        }

        protected void btn9_Click(object sender, EventArgs e)
        {
            Binddg2(9);
            ccall.Value = "#ffffff";
            myModal.Visible = true;
        }
        protected void btn10_Click(object sender, EventArgs e)
        {
            Binddg2(10);
            ccall.Value = "#ffffff";
            myModal.Visible = true;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            BindDG();
        }

        protected void ckdeleted_CheckedChanged(object sender, EventArgs e)
        {
            BindDG();
        }

        protected void ckmy_CheckedChanged(object sender, EventArgs e)
        {
            BindDG();
        }

        protected void txDate_TextChanged(object sender, EventArgs e)
        {
            BindDG();
        }
    }
}