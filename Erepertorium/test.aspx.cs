using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Erepertorium
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                bind();
        }

        void bind()
        {
            dg1.DataSource = RegistryType.LoadAll<RegistryType>();
            dg1.DataBind();
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            bind();
        }
    }
}