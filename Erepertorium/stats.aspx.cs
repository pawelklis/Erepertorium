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
    public partial class stats : System.Web.UI.Page
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
        this.ReportViewer1.LocalReport.DataSources.Clear();
            DataTable dt = new DataTable();
            dt = MysqlCore.DB_Main().FillDatatable(
                "select user,count(content) as registered from registrys where year(date)=" + ddlyear.SelectedValue + " and month(date)=" + ddlmonth.SelectedValue + " group by user;");

            ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
            this.ReportViewer1.LocalReport.DataSources.Add(datasource);


            dt = MysqlCore.DB_Main().FillDatatable(
    "select day(date) as day,count(content) as registered from registrys where year(date)=" + ddlyear.SelectedValue + " and month(date)=" + ddlmonth.SelectedValue + " group by day(date);");

            datasource = new ReportDataSource("DataSet2", dt);
            this.ReportViewer1.LocalReport.DataSources.Add(datasource);


            dt = MysqlCore.DB_Main().FillDatatable(
    "select concat(hour(date),':00') as hour,count(content) as registered from registrys where year(date)=" + ddlyear.SelectedValue + " and month(date)=" + ddlmonth.SelectedValue + " group by hour(date) order by hour(date) asc;");

            datasource = new ReportDataSource("DataSet3", dt);
            this.ReportViewer1.LocalReport.DataSources.Add(datasource);




            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report2.rdlc");

            string fullMonthName = new DateTime(int.Parse(ddlyear.SelectedValue), int.Parse(ddlmonth.SelectedValue), 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("pl"));

            ReportParameter p1 = new ReportParameter("p1", ddlyear.SelectedValue + " " + fullMonthName);
            this.ReportViewer1.LocalReport.SetParameters(p1);

    

            this.ReportViewer1.DataBind();

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            BindReport();
        }
    }
}