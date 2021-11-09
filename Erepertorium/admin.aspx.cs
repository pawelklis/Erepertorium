using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Erepertorium
{
    public partial class admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Bind();
        }
        void Bind()
        {
            List<UserType> users = UserType.LoadAll<UserType>();
            dg1.DataSource = users;
            dg1.DataBind();
        }
        protected void dg1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddllevel");
                    ddl.SelectedValue = ddl.ToolTip;
                }
            }
            catch (Exception)
            {


            }
        }

        protected void dg1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                
                int id = int.Parse((string)e.CommandArgument);

                if (e.CommandName == "del")
                {
                    MysqlCore.DB_Main().ExecuteNonQuery("delete from users where id=" + id);
                    Bind();
                }

                if (e.CommandName == "gen")
                {
                    string pwd = Guid.NewGuid().ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Hasło:" + pwd + "')", true);
                    UserType u = UserType.Load<UserType>(id);
                    u.localpwd = pwd;
                    u.SetLocalPassword();
                    u.Save();
                    Bind();

                }
            }
            catch (Exception)
            {


            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in dg1.Rows)
                {
                    int id = int.Parse(((Button)row.FindControl("btndel")).CommandArgument.ToString());
                    string name = ((TextBox)row.FindControl("txname")).Text;
                    string surname = ((TextBox)row.FindControl("txsurname")).Text;
                    string login = ((TextBox)row.FindControl("txlogin")).Text;
                    DropDownList ddl = (DropDownList)row.FindControl("ddllevel");

                    UserType u = UserType.Load<UserType>(id);
                    u.Name = name;
                    u.Surname = surname;
                    u.Login = login;
                    u.Level = int.Parse(ddl.SelectedValue);
                    u.Save();
                    
                }

                Bind();
            }
            catch (Exception)
            {


            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string pwd = Guid.NewGuid().ToString(); 

            ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Hasło:" + pwd +"')", true);

            UserType u = new UserType();
            u.Level = 0;
            u.localpwd = pwd;
            u.Save();
            u.SetLocalPassword();
            Bind();
        }

        protected void btnReorder_Click(object sender, EventArgs e)
        {
            
            RegistryType.ReindexBaseOAll(DateTime.Now.Year);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "al", "alert('Zakończono numeracje.')", true);
        }
    }
}