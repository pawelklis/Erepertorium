
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Erepertorium
{
    public partial class reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindYear();
            }
        }

        void BindYear()
        {

            DataTable dt = MysqlCore.DB_Main().FillDatatable("select distinct(YEAR(date)) as yer from registrys;");
            ddlyear.DataSource = dt;
            ddlyear.DataTextField = "yer";
            ddlyear.DataValueField = "yer";
            ddlyear.DataBind();

            ddlyear.SelectedIndex = 0;
            BindMonth();
        }

        void BindMonth()
        {
            DataTable dt = MysqlCore.DB_Main().FillDatatable("select distinct(MONTH(date)) as mon from registrys where YEAR(date)=" + ddlyear.SelectedValue + ";");
            ddlmonth.DataSource = dt;
            ddlmonth.DataValueField = "mon";
            ddlmonth.DataTextField = "mon";
            ddlmonth.DataBind();
        }
        protected void ddlyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMonth();
        }

        void BindReport()
        {

            DataTable dt = new DataTable();
            dt = MysqlCore.DB_Main().FillDatatable("SELECT id,number,user,date_format(date,' %Y-%m-%d') as date,content FROM erepdb.registrys where year(date)=" + ddlyear.SelectedValue + " and month(date)=" + ddlmonth.SelectedValue + ";");

          



            ReportDataSource datasource = new ReportDataSource("DataSet1", dt);

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report1.rdlc");

            string fullMonthName = new DateTime(int.Parse(ddlyear.SelectedValue), int.Parse(ddlmonth.SelectedValue), 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("pl"));

            ReportParameter p1 = new ReportParameter("p1", ddlyear.SelectedValue + " " + fullMonthName);
            this.ReportViewer1.LocalReport.SetParameters(p1);

            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(datasource);
            this.ReportViewer1.DataBind();

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            BindReport();
        }
    }
}