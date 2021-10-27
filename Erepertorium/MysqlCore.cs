using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;


/// <summary>

/// '''   ConnectionTest

/// '''   ExecuteNonQuery

/// '''   ExecuteScalar

/// '''   FillDatatable

/// '''   GetTablesList

/// '''   GetColumnsList

/// '''   Insert

/// '''   InsertList

/// '''   InsertOrUpdate

/// '''   Update

/// '''   UpdateList

/// '''   GetCount

/// '''   GetObject

/// '''   GetObject(list)

/// '''   Delete

/// '''   Truncate

/// '''   DropTable

/// '''   CreateTable

/// '''   AddColumn

/// '''   DeleteColumn

/// ''' </summary>
public class MysqlCore
{
    public static MysqlCore DB_Main()
    {

        try
        {
            EventHandler handler = OnConnection;
            handler?.Invoke(null, new EventArgs());
        }
        catch (Exception)
        {
        }

        return new MysqlCore("localhost", "erepdb", "mayday1");
    }



    public void CreateTable(object o, string dbname, string idcolumnName, string tablename)
    {
        //string a = "CREATE TABLE `sio_solution`.`test` (  `id` INT NOT NULL,  `alter` VARCHAR(255) NULL,  `dbl` DECIMAL NULL,  PRIMARY KEY(`id`)); ";
        StringBuilder sb = new StringBuilder();
        sb.Append("CREATE TABLE `" + dbname + "`.`" + tablename + "` ( ");
        string pk = "";

        PropertyInfo[] props = o.GetType().GetProperties();

        foreach (var p in props)
        {
            if (p.PropertyType.Name.ToLower().StartsWith("int"))
            {
                sb.Append("`" + p.Name + "` " + "INT");
                if (p.Name.ToLower() == idcolumnName.ToLower())
                {
                    sb.Append(" NOT NULL AUTO_INCREMENT");
                    pk = "PRIMARY KEY(`" + p.Name + "`)";
                }
                sb.Append(", ");
            }

            if (p.PropertyType.Name.ToLower() == "string")
            {
                string val = p.GetValue(o)?.ToString();

                int len = 255;
                if (val != null)
                {
                    if (val.Length > len)
                        len = val.Length;
                }


                sb.Append("`" + p.Name + "` " + "VARCHAR(" + len + ") NULL");
                sb.Append(", ");
            }
            if (p.PropertyType.Name.ToLower() == "double")
            {
                sb.Append("`" + p.Name + "` " + "DECIMAL NULL");
                sb.Append(", ");
            }

            if (p.PropertyType.Name.ToLower().Contains("date"))
            {
                sb.Append("`" + p.Name + "` " + "DATETIME NULL");
                sb.Append(", ");
            }

            if (p.PropertyType.Name.ToLower() != "double")
            {
                if (p.PropertyType.Name.ToLower() != "string")
                {
                    if (!p.PropertyType.Name.ToLower().StartsWith("int"))
                    {
                        if (!p.PropertyType.Name.ToLower().Contains("date"))
                        {
                            sb.Append("`" + p.Name + "` " + "JSON NULL");
                            sb.Append(", ");
                        }
                    }
                }
            }


        }

        sb.Append(pk);
        sb.Append(");");

        ExecuteNonQuery("DROP TABLE `" + dbname + "`.`" + tablename + "`;");

        string cmd = sb.ToString().Replace(", )", ")");

        ExecuteNonQuery(cmd);

    }

    public bool NewUpdate(object o, string tablename, string idcolumn = "id", MySqlConnection cn = null)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE `" + tablename + "` SET ");

            string idval = "";

            PropertyInfo[] props = o.GetType().GetProperties();

            foreach (var p in props)
            {
                if (p.Name.ToLower() == idcolumn.ToLower())
                {
                    idval = p.GetValue(o).ToString();
                }
                else
                {



                    if (p.PropertyType.Name.ToLower().StartsWith("int"))
                    {
                        sb.Append("`" + p.Name + "`='" + p.GetValue(o).ToString() + "', ");
                    }

                    if (p.PropertyType.Name.ToLower() == "string")
                    {
                        sb.Append("`" + p.Name + "`='" + p.GetValue(o)?.ToString() + "', ");
                    }
                    if (p.PropertyType.Name.ToLower() == "double")
                    {
                        sb.Append("`" + p.Name + "`='" + p.GetValue(o).ToString() + "', ");
                    }

                    if (p.PropertyType.Name.ToLower().Contains("date"))
                    {
                        sb.Append("`" + p.Name + "`='" + p.GetValue(o).ToString() + "', ");
                    }

                    if (p.PropertyType.Name.ToLower() != "double")
                    {
                        if (p.PropertyType.Name.ToLower() != "string")
                        {
                            if (!p.PropertyType.Name.ToLower().StartsWith("int"))
                            {
                                if (!p.PropertyType.Name.ToLower().Contains("date"))
                                {
                                    var val = p.GetValue(o);
                                    sb.Append("`" + p.Name + "`='" + JsonConvert.SerializeObject(val) + "', ");
                                }
                            }
                        }
                    }

                }
            }

            sb.Append(" WHERE `" + idcolumn + "` ='" + idval + "';");

            string cmd = sb.ToString().Replace("  WHERE", " WHERE ").Replace(", WHERE", " WHERE ");
            return ExecuteNonQuery(cmd, cn);
        }
        catch (Exception ex)
        {
            
            return false;
        }

    }

    public void NewInsertOrUpdate(object o, string tablename, string idcolumn = "id")
    {
        PropertyInfo[] props = o.GetType().GetProperties();
        try
        {
            foreach (var p in props)
            {
                if (p.Name.ToLower() == idcolumn.ToLower())
                {
                    string val = p.GetValue(o).ToString();
                    int id = int.Parse(val);
                    if (id == 0)
                    {
                        NewInsert(o, tablename, idcolumn);
                    }
                    else
                    {
                        NewUpdate(o, tablename, idcolumn);
                    }

                }


            }
        }
        catch (Exception ex)
        {
            
        }

    }

    public int NewInsert(object o, string tablename, string idcolumn = "id", MySqlConnection cn = null)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            sb.Append("INSERT INTO `" + tablename + "` (");

            PropertyInfo[] props = o.GetType().GetProperties();

            foreach (var p in props)
            {
                if (p.Name.ToLower() != idcolumn.ToLower())
                    sb.Append("`" + p.Name.ToLower() + "`, ");
            }
            sb.Append(") VALUES (");

            foreach (var p in props)
            {
                if (p.Name.ToLower() != idcolumn.ToLower())
                {
                    if (p.PropertyType.Name.ToLower().StartsWith("int"))
                    {
                        sb.Append("'" + p.GetValue(o)?.ToString() + "', ");
                    }

                    if (p.PropertyType.Name.ToLower() == "string")
                    {
                        sb.Append("'" + p.GetValue(o)?.ToString() + "', ");
                    }
                    if (p.PropertyType.Name.ToLower() == "double")
                    {
                        sb.Append("'" + p.GetValue(o)?.ToString() + "', ");
                    }

                    if (p.PropertyType.Name.ToLower().Contains("date"))
                    {
                        sb.Append("'" + p.GetValue(o)?.ToString() + "', ");
                    }

                    if (p.PropertyType.Name.ToLower() != "double")
                    {
                        if (p.PropertyType.Name.ToLower() != "string")
                        {
                            if (!p.PropertyType.Name.ToLower().StartsWith("int"))
                            {
                                if (!p.PropertyType.Name.ToLower().Contains("date"))
                                {
                                    var val = p.GetValue(o);
                                    sb.Append("'" + JsonConvert.SerializeObject(val) + "', ");
                                }
                            }
                        }
                    }
                }


            }
            sb.Append(");");
            sb.Append("select last_insert_id();");
            string cmd = sb.ToString().Replace(", )", ")");
            string oid = ExecuteScalar(cmd, cn);


            foreach (var p in props)
            {
                int id = int.Parse(oid);
                if (p.Name.ToLower() == idcolumn.ToLower())
                    p.SetValue(o, id);
            }

            return int.Parse(oid);
        }
        catch (Exception ex)
        {
            
            return (0);

        }




    }


    private void SetPropvalue(PropertyInfo p, object ob, object c)
    {
        try
        {
            if (p.PropertyType.Name.ToLower().StartsWith("int"))
            {
                p.SetValue(ob, int.Parse(c.ToString()));
            }

            if (p.PropertyType.Name.ToLower() == "string")
            {
                p.SetValue(ob, c.ToString());
            }
            if (p.PropertyType.Name.ToLower() == "double")
            {
                p.SetValue(ob, double.Parse(c.ToString()));
            }

            if (p.PropertyType.Name.ToLower().Contains("date"))
            {
                if (string.IsNullOrEmpty(c.ToString()))
                    c = "0001-01-01 00:00:00";

                p.SetValue(ob, DateTime.Parse(c.ToString()));
            }

            if (p.PropertyType.Name.ToLower() != "double")
            {
                if (p.PropertyType.Name.ToLower() != "string")
                {
                    if (!p.PropertyType.Name.ToLower().StartsWith("int"))
                    {
                        if (!p.PropertyType.Name.ToLower().Contains("date"))
                        {
                            try
                            {
                                string val = c.ToString();
                                p.SetValue(ob, JsonConvert.DeserializeObject(val, p.PropertyType));

                            }
                            catch (Exception ex)
                            {
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public T NewGetSingleObject<T>(string SQLcommand) where T : new()
    {
        List<T> l = new List<T>();

        var Props = typeof(T).GetProperties();
        DataTable dt = new DataTable();

        string command = "";
        command = SQLcommand;
        try
        {
            com = new MySqlCommand(command, Connection());
            adpt = new MySqlDataAdapter(command, con);
            adpt.Fill(dt);

            for (var i = 0; i <= dt.Rows.Count - 1; i++)
            {
                T ob = new T();

                for (var xx = 0; xx <= dt.Columns.Count - 1; xx++)
                {
                    foreach (var p in Props)
                    {
                        if (p.Name.ToLower() == dt.Columns[xx].ColumnName.ToLower())
                        {
                            var c = dt.Rows[i][xx];

                            SetPropvalue(p, ob, c);

                        }
                    }
                }
                l.Add(ob);
            }
        }
        catch (Exception ex)
        {

            return default(T);
        }

        finally
        {
            try
            {
                Connection().Close();
            }
            catch (Exception ex)
            {
            }

            con.Close();
            con.Dispose();
            con = null;
        }


        if (l == null)
            return default(T);
        if (l.Count == 0)
            return default(T);


        return l[0];
    }


    public List<T> NewGetObjects<T>(string SQLcommand) where T : new()
    {
        List<T> l = new List<T>();

        var Props = typeof(T).GetProperties();
        DataTable dt = new DataTable();

        string command = "";
        command = SQLcommand;
        try
        {
            com = new MySqlCommand(command, Connection());
            adpt = new MySqlDataAdapter(command, con);
            adpt.Fill(dt);

            for (var i = 0; i <= dt.Rows.Count - 1; i++)
            {
                T ob = new T();

                for (var xx = 0; xx <= dt.Columns.Count - 1; xx++)
                {
                    foreach (var p in Props)
                    {
                        if (p.Name.ToLower() == dt.Columns[xx].ColumnName.ToLower())
                        {
                            var c = dt.Rows[i][xx];
                            SetPropvalue(p, ob, c);



                        }
                    }
                }
                l.Add(ob);
            }
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            try
            {
                Connection().Close();
            }
            catch (Exception ex)
            {
            }

            con.Close();
            con.Dispose();
            con = null;
        }





        return l;
    }

    public static EventHandler OnConnection;

    public int OperationCount { get; set; }


    public static void KillSleepConnections(MysqlCore m, int timeout)
    {
        try
        {
            DataTable dt = m.FillDatatable("show processlist;");
            int a = 0;
            int i = 0;
            for (i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i]["Command"].ToString().ToLower() == "sleep")
                {
                    int ti = (int)dt.Rows[i]["time"];
                    if (ti > timeout)
                    {
                        m.ExecuteNonQuery("kill " + dt.Rows[i]["id"]);
                        a += 1;
                    }
                }
            }

        }
        catch (Exception ex)
        {
        }
    }



    public string ConnectionStr { get; }
    public Dictionary<DateTime, string> ErrorHistory { get; set; }
    public Dictionary<DateTime, string> EventHistory { get; set; }
    public string LastError { get; set; }

    public readonly Parameters ConParameters;
    public class Parameters
    {
        public string Database { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string SecondServer { get; set; }
        public string User { get; set; }
        public int Port { get; set; }
        public string CharacterSet { get; set; }
        public int ConnectionTimeout { get; set; }
        public bool ConvertZeroDate { get; set; }
        public bool UseCompression { get; set; }
    }
    public string ConString()
    {
        return this.ConnectionStr;
    }


    public MySqlConnection con { get; set; }
    public MySqlCommand com { get; set; }
    public MySqlDataAdapter adpt { get; set; }




    public MysqlCore(string server, string database, string password, string SecondServer = null, bool UseCompression = true, string CharacterSet = "utf8", string user = "root", int port = 3306, int ConnectionTimeout = 200, bool ConvertZeroDate = true)
    {
        this.ConParameters = new Parameters();
        {
            var withBlock = this.ConParameters;
            withBlock.Database = database;
            withBlock.Password = password;
            withBlock.Server = server;
            withBlock.User = user;
            withBlock.Port = port;
            withBlock.CharacterSet = CharacterSet;
            withBlock.ConnectionTimeout = ConnectionTimeout;
            withBlock.ConvertZeroDate = ConvertZeroDate;
            withBlock.UseCompression = UseCompression;
            withBlock.SecondServer = SecondServer;
        }



        string ConnectionString = "SERVER=" + server + ";UID=" + user + ";DATABASE=" + database + ";PORT=" + port + ";password=" + password + ";  Character Set=" + CharacterSet + "; Connect Timeout=" + ConnectionTimeout + ";Pooling=false;Convert Zero Datetime=" + ConvertZeroDate + ";";

        this.ConnectionStr = ConnectionString;
        if (SecondServer != null)
        {
            if (this.ConnectionTest() == false)
            {
                ConnectionString = "SERVER=" + SecondServer + ";UID=" + user + ";DATABASE=" + database + ";PORT=" + port + ";password=" + password + ";  Character Set=" + CharacterSet + "; Connect Timeout=" + ConnectionTimeout + ";Pooling=false;Convert Zero Datetime=" + ConvertZeroDate + ";";
                this.ConnectionStr = ConnectionString;
            }
        }
    }
    /// <summary>
    ///     ''' Test połączenia z bazą danych
    ///     ''' </summary>
    ///     ''' <returns></returns>
    public bool ConnectionTest()
    {
        MySqlConnection testcon = new MySqlConnection(this.ConnectionStr);
        try
        {
            testcon.Open();
            return true;
        }
        catch (Exception ex)
        {
            // ErrorHistoryAdd(ex)
            return false;
        }
        finally
        {
            testcon.Close();
        }

    }
    public MySqlConnection Connection()
    {
        OperationCount += 1;
        if (con != null)
        {
            try
            {
                con.Close();
            }
            catch (Exception ex)
            {
            }
        }

        con = new MySqlConnection(ConnectionStr);


        try
        {
            con.Open();
        }
        catch (Exception ex)
        {


        }

        return con;
    }
    public Dictionary<string, object> toDict(object ob)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        PropertyInfo[] info = ob.GetType().GetProperties();
        foreach (var p in info)
            dict.Add(p.Name, p.GetValue(ob, null));
        return dict;
    }

    public T GetSingleObject<T>(string SQLcommand) where T : new()
    {
        List<T> l = new List<T>();

        var Props = typeof(T).GetProperties();
        DataTable dt = new DataTable();

        string command = "";
        command = SQLcommand;
        try
        {
            com = new MySqlCommand(command, Connection());
            adpt = new MySqlDataAdapter(command, con);
            adpt.Fill(dt);

            for (var i = 0; i <= dt.Rows.Count - 1; i++)
            {
                T ob = new T();

                for (var xx = 0; xx <= dt.Columns.Count - 1; xx++)
                {
                    foreach (var p in Props)
                    {
                        if (p.Name.ToLower() == dt.Columns[xx].ColumnName.ToLower())
                        {
                            var c = dt.Rows[i][xx];

                            try
                            {
                                if (p.PropertyType.Name == "Boolean")
                                {
                                    if (c.ToString().ToLower() == "true")
                                    {
                                        p.SetValue(ob, true, null);
                                        break;
                                    }
                                    else
                                    {
                                        p.SetValue(ob, false, null);
                                        break;
                                    }
                                }

                                if (p.PropertyType.Name == "Double")
                                {
                                    p.SetValue(ob, double.Parse(c.ToString().Replace(".", ",")), null);
                                    break;
                                }

                                if (p.PropertyType.Name.StartsWith("Int"))
                                {
                                    p.SetValue(ob, int.Parse(c.ToString()), null);
                                    break;
                                }

                                if (p.PropertyType.Name == "DateTime")
                                {
                                    p.SetValue(ob, DateTime.Parse(c.ToString()), null);
                                    break;
                                }

                                if (p.PropertyType.Name == "String")
                                {
                                    p.SetValue(ob, c.ToString(), null);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                            }



                            try
                            {
                                if (dt.Columns[xx].DataType == c.GetType())
                                {
                                    p.SetValue(ob, dt.Rows[i][xx], null);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                l.Add(ob);
            }
        }
        catch (Exception ex)
        {

            return default(T);
        }

        finally
        {
            try
            {
                Connection().Close();
            }
            catch (Exception ex)
            {
            }

            con.Close();
            con.Dispose();
            con = null;
        }


        if (l == null)
            return default(T);
        if (l.Count == 0)
            return default(T);


        return l[0];
    }

    public bool ExecuteNonQuery(string sqlCommand, MySqlConnection cn = null)
    {
        try
        {
            if (cn != null)
            {
                com = new MySqlCommand(sqlCommand, cn);
            }
            else
            {
                com = new MySqlCommand(sqlCommand, Connection());
            }


            com.ExecuteNonQuery();
        }
        catch (Exception ex)
        {

            return false;
        }
        finally
        {
            if (cn == null)
                Connection().Close();
        }


        return true;
    }
    public string ExecuteScalar(string sqlCommand, MySqlConnection cn = null)
    {
        string ret = "";

        if (cn != null)
        {
            com = new MySqlCommand(sqlCommand, cn);
        }
        else
        {
            com = new MySqlCommand(sqlCommand, Connection());
        }

        try
        {
            ret = com.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {

            return "false";
        }
        finally
        {
            if (cn == null)
                Connection().Close();
        }

        return ret;
    }
    public DataTable FillDatatable(string sqlCommand)
    {
        DataTable dt = new DataTable();

        try
        {
            com = new MySqlCommand(sqlCommand, Connection());
            adpt = new MySqlDataAdapter(sqlCommand, con);
            adpt.Fill(dt);
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            Connection().Close();
        }



        return dt;
    }



    public string GetInsertString(string TableName, string IdColumn, Dictionary<string, object> PropertiesDict)
    {
        string command = "";
        string strNazwypol = "";
        string strParametry = "";
        TableName = "`" + TableName + "`";
        foreach (var p in PropertiesDict)
        {
            // If p.Key <> IdColumn Then
            strNazwypol = strNazwypol + "`" + p.Key.ToLower() + "`,";
            strParametry = strParametry + "@" + p.Key.ToLower() + ",";
        }
        strNazwypol = strNazwypol.Substring(0, strNazwypol.Length - 1);
        strParametry = strParametry.Substring(0, strParametry.Length - 1);
        command = "INSERT INTO " + TableName + " (" + strNazwypol + ") VALUES (" + strParametry + ");select LAST_INSERT_ID();";
        return command;
    }
    public string GetInsertStringWithoutID(string TableName, string IdColumn, Dictionary<string, object> PropertiesDict)
    {
        string command = "";
        string strNazwypol = "";
        string strParametry = "";
        foreach (var p in PropertiesDict)
        {
            if (p.Key != IdColumn)
            {
                strNazwypol = strNazwypol + p.Key + ",";
                strParametry = strParametry + "@" + p.Key + ",";
            }
        }
        strNazwypol = strNazwypol.Substring(0, strNazwypol.Length - 1);
        strParametry = strParametry.Substring(0, strParametry.Length - 1);
        command = "INSERT INTO " + TableName + " (" + strNazwypol + ") VALUES (" + strParametry + ");select LAST_INSERT_ID();";
        return command;
    }
    public bool Insert(string TableName, string IdColumn, object ob)
    {
        Dictionary<string, object> PropertiesDict = new Dictionary<string, object>();
        PropertiesDict = toDict(ob);
        string command = GetInsertString(TableName, IdColumn, PropertiesDict);

        try
        {
            com = new MySqlCommand(command, Connection())
            {
                CommandType = CommandType.Text
            };
            foreach (var p in PropertiesDict)
                com.Parameters.AddWithValue("@" + p.Key.ToLower(), p.Value);


            PropertyInfo[] info = ob.GetType().GetProperties();
            foreach (var p in info)
            {
                if (p.Name.ToLower() == IdColumn.ToLower())
                {
                    var aid = com.ExecuteScalar();
                    int id = int.Parse(aid.ToString());
                    p.SetValue(ob, id, null);
                }
            }
        }
        catch (Exception ex)
        {

            return false;
        }

        finally
        {
            con.Close();
            con.Dispose();
            con = null;
        }

        return true;
    }
    public void NewUpdateList<T>(List<T> L) where T : new()
    {
        this.con = this.Connection();
        MySqlCommand myCommand = this.con.CreateCommand();
        MySqlTransaction myTrans;

        // Start a local transaction
        myTrans = this.con.BeginTransaction();
        // Must assign both transaction object and connection
        // to Command object for a pending local transaction
        myCommand.Connection = this.con;
        myCommand.Transaction = myTrans;

        try
        {
            foreach (var o in L)
            {
                string tb = typeof(T).ToString().Replace("siosolution.", "").Replace("Type", "s");
                NewUpdate(o, tb, "id", this.con);
            }

            myTrans.Commit();

        }
        catch (Exception e)
        {
            try
            {
                myTrans.Rollback();
            }
            catch (MySqlException ex)
            {
                if (myTrans.Connection != null)
                {

                }
            }


        }
        finally
        {
            this.con.Close();
        }
    }

    public void NewInsertList<T>(List<T> L) where T : new()
    {
        this.con = this.Connection();
        MySqlCommand myCommand = this.con.CreateCommand();
        MySqlTransaction myTrans;

        // Start a local transaction
        myTrans = this.con.BeginTransaction();
        // Must assign both transaction object and connection
        // to Command object for a pending local transaction
        myCommand.Connection = this.con;
        myCommand.Transaction = myTrans;

        try
        {
            foreach (var o in L)
            {
                string tb = typeof(T).ToString().Replace("siosolution.", "").Replace("Type", "s");
                NewInsert(o, tb, "id", this.con);
            }

            myTrans.Commit();

        }
        catch (Exception e)
        {
            try
            {
                myTrans.Rollback();
            }
            catch (MySqlException ex)
            {
                if (myTrans.Connection != null)
                {

                }
            }


        }
        finally
        {
            this.con.Close();
        }
    }


    /// <summary>
    ///     ''' Insert Listy Obiektów za pomocą transakcji
    ///     ''' </summary>
    ///     ''' <param name="TableName"></param>
    ///     ''' <param name="ListOb"></param>
    ///     ''' <returns></returns>
    public bool InsertList(string TableName, string idColumn, List<object> ListOb, bool BezID = true)
    {
        if (ListOb.Count == 0)
        {
            LastError = "MysqlCore: InsertList: Lista nie posiada obiektów";
            return false;
        }

        Dictionary<string, object> PropertiesDict = new Dictionary<string, object>();
        PropertiesDict = toDict(ListOb[0]);
        string command = GetInsertString(TableName, idColumn, PropertiesDict);
        if (BezID == false)
            command = GetInsertStringWithoutID(TableName, idColumn, PropertiesDict);
        MySqlTransaction tr = null;
        try
        {
            com = new MySqlCommand(command, Connection());
            tr = con.BeginTransaction();
            com.Transaction = tr;

            foreach (var ob in ListOb)
            {
                PropertiesDict = new Dictionary<string, object>();
                PropertiesDict = toDict(ob);

                com.CommandType = CommandType.Text;
                com.Parameters.Clear();

                foreach (var p in PropertiesDict)
                    com.Parameters.AddWithValue("@" + p.Key, p.Value);

                PropertyInfo[] info = ob.GetType().GetProperties();
                bool sended = false;
                foreach (var p in info)
                {
                    if (p.Name == idColumn)
                    {
                        try
                        {
                            p.SetValue(ob, com.ExecuteNonQuery(), null);
                            sended = true;
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    }
                }
                try
                {
                    if (sended == false)
                        com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
            }

            tr.Commit();
        }

        catch (Exception ex)
        {
            tr.Rollback();

            return false;
        }

        finally
        {
            con.Close();
            con.Dispose();
            con = null;
        }

        return true;
    }

    public String InsertOnDuplicateUpdate(string id, string value)
    {
        MysqlCore m = MysqlCore.DB_Main();
        m.ExecuteNonQuery("INSERT INTO `stat` VALUES (id='" + id + "', val='" + value + "') ON DUPLICATE KEY UPDATE val='" + value + "' ;");

        return "";
    }

    public string GetUpdatetString(string TableName, string idColumnName, Dictionary<string, object> PropertiesDict)
    {
        string command = "";
        string strNazwypol = "";
        string strParametry = "";
        string strPOLE = "";
        TableName = "`" + TableName + "`";
        foreach (var p in PropertiesDict)
        {
            if (p.Key != idColumnName)
                strPOLE = strPOLE + "`" + p.Key + "`=" + "@" + p.Key + ",";
        }

        strPOLE = strPOLE.Substring(0, strPOLE.Length - 1);

        command = "UPDATE " + TableName + " SET " + strPOLE + " WHERE " + idColumnName + "=@" + idColumnName + " ;";

        return command;
    }
    public bool InsertOrUpdate(string TableName, string idColumnName, object ob)
    {
        Dictionary<string, object> PropertiesDict = new Dictionary<string, object>();
        PropertiesDict = toDict(ob);
        dynamic wId = null;
        foreach (var p in PropertiesDict)
        {
            if (p.Key == idColumnName)
            {
                wId = p.Value;
                break;
            }
        }
        int cont = GetCount("select count(*) from " + TableName + " WHERE " + idColumnName + "='" + (string)wId + "';");

        if (cont == 0)
            return Insert(TableName, idColumnName, ob);
        else
            return Update(TableName, idColumnName, ob);

    }
    public bool Update(string TableName, string idColumnName, object ob)
    {
        Dictionary<string, object> PropertiesDict = new Dictionary<string, object>();
        PropertiesDict = toDict(ob);
        string command = GetUpdatetString(TableName, idColumnName, PropertiesDict);

        try
        {
            com = new MySqlCommand(command, Connection())
            {
                CommandType = CommandType.Text
            };
            foreach (var p in PropertiesDict)
                com.Parameters.AddWithValue("@" + p.Key, p.Value);
            com.ExecuteNonQuery();
        }
        catch (Exception ex)
        {

            return false;
        }

        finally
        {
            con.Close();
            con.Dispose();
            con = null;
        }

        return true;
    }


    /// <summary>
    ///     ''' Update Listy Obiektów za pomocą transakcji
    ///     ''' </summary>
    ///     ''' <param name="TableName"></param>
    ///     ''' <param name="ListOb"></param>
    ///     ''' <returns></returns>
    public bool UpdateList(string TableName, string idColumnName, List<object> ListOb)
    {
        if (ListOb.Count == 0)
        {
            LastError = "Lista nie posiada obiektów";
            return false;
        }

        Dictionary<string, object> PropertiesDict = new Dictionary<string, object>();
        PropertiesDict = toDict(ListOb[0]);
        string command = GetUpdatetString(TableName, idColumnName, PropertiesDict);

        MySqlTransaction tr = null;
        try
        {
            com = new MySqlCommand(command, Connection());
            tr = con.BeginTransaction();
            com.Transaction = tr;

            foreach (var ob in ListOb)
            {
                PropertiesDict = new Dictionary<string, object>();
                PropertiesDict = toDict(ob);

                com.CommandType = CommandType.Text;
                com.Parameters.Clear();

                foreach (var p in PropertiesDict)
                    com.Parameters.AddWithValue("@" + p.Key, p.Value);
                com.ExecuteNonQuery();
            }

            tr.Commit();
        }

        catch (Exception ex)
        {
            tr.Rollback();

            return false;
        }

        finally
        {
            con.Close();
            con.Dispose();
            con = null;
        }

        return true;
    }
    /// <summary>
    ///     ''' Zwraca String
    ///     ''' </summary>
    ///     ''' <param name="sqlCommand"></param>
    ///     ''' <returns></returns>
    public string GetString(string sqlCommand, string ReturnValueIffNull = "")
    {
        string GS = ReturnValueIffNull;
        try
        {
            com = new MySqlCommand(sqlCommand, Connection());
            MySqlDataReader rd = com.ExecuteReader();
            while (rd.Read())
                GS = rd[0].ToString();
        }

        catch (Exception ex)
        {

            GS = ReturnValueIffNull;
        }

        finally
        {
            try
            {
                Connection().Close();
            }
            catch (Exception exx)
            {
            }
        }


        //GS = ReturnValueIffNull;
        return GS;


    }
    /// <summary>
    ///     ''' Zwraca Byte
    ///     ''' </summary>
    ///     ''' <param name="sqlCommand"></param>
    ///     ''' <returns></returns>
    public byte[] GetByte(string sqlCommand)
    {
        byte[] GB = null;
        try
        {
            com = new MySqlCommand(sqlCommand, Connection());
            MySqlDataReader rd = com.ExecuteReader();
            while (rd.Read())
                GB = (byte[])rd[0];

            
        }

        catch (Exception ex)
        {

            GB = null;
        }

        finally
        {
            Connection().Close();
        }
        return GB;
    }
    /// <summary>
    ///     ''' Zwraca liczbę rekordów Integer
    ///     ''' </summary>
    ///     ''' <param name="sqlCommand"></param>
    ///     ''' <returns></returns>
    public int GetCount(string sqlCommand)
    {
        int GC = 0;
        try
        {
            com = new MySqlCommand(sqlCommand, Connection());
            GC = int.Parse(com.ExecuteScalar().ToString());
        }
        catch (Exception ex)
        {
            GC = 0;
        }
        finally
        {
            if (GC < 0)
                GC = 0;
            Connection().Close();
        }
        return GC;
    }
    /// <summary>
    ///     ''' Tworzy dowolny obiekt na podstawie dowolnej klasy z pojedynczego rekordu z tabeli
    ///     ''' </summary>
    ///     ''' <typeparam name="T"></typeparam>
    ///     ''' <param name="TableName"></param>
    ///     ''' <param name="idColumnName"></param>
    ///     ''' <param name="id"></param>
    ///     ''' <returns></returns>
    public object GetObject<T>(string TableName, string idColumnName, string id) where T : new()
    {
        T ob = new T();
        var Props = typeof(T).GetProperties();
        DataTable dt = new DataTable();

        string command = "";
        command = "SELECT * FROM " + TableName + " WHERE " + idColumnName + "='" + id + "';";
        try
        {
            com = new MySqlCommand(command, Connection());
            adpt = new MySqlDataAdapter(command, con);
            adpt.Fill(dt);

            for (var i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (var xx = 0; xx <= dt.Columns.Count - 1; xx++)
                {
                    foreach (var p in Props)
                    {
                        if (p.Name == dt.Columns[xx].ColumnName)
                        {
                            var c = dt.Rows[i][xx];
                            try
                            {
                                if (dt.Columns[xx].DataType == c.GetType())
                                    p.SetValue(ob, dt.Rows[i][xx], null);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            Connection().Close();
            con.Close();
            con.Dispose();
            con = null;
        }


        return ob;
    }
    /// <summary>
    ///     ''' Tworzy listę obiektów na podstawie dowolnej klasy wg komendy mysQL
    ///     ''' </summary>
    ///     ''' <typeparam name="T"></typeparam>
    ///     ''' <param name="SQLcommand"></param>
    ///     ''' <returns></returns>
    public List<T> GetObject<T>(string SQLcommand) where T : new()
    {
        List<T> l = new List<T>();

        var Props = typeof(T).GetProperties();
        DataTable dt = new DataTable();

        string command = "";
        command = SQLcommand;
        try
        {
            com = new MySqlCommand(command, Connection());
            adpt = new MySqlDataAdapter(command, con);
            adpt.Fill(dt);

            for (var i = 0; i <= dt.Rows.Count - 1; i++)
            {
                T ob = new T();
                for (var xx = 0; xx <= dt.Columns.Count - 1; xx++)
                {
                    foreach (var p in Props)
                    {
                        if (p.Name.ToLower() == dt.Columns[xx].ColumnName.ToLower())
                        {
                            var c = dt.Rows[i][xx];
                            try
                            {
                                if (p.PropertyType.Name == "Boolean")
                                {
                                    if (c.ToString().ToLower() == "true")
                                    {
                                        p.SetValue(ob, true, null);
                                        break;
                                    }
                                    else
                                    {
                                        p.SetValue(ob, false, null);
                                        break;
                                    }
                                }

                                if (p.PropertyType.Name == "Double")
                                {
                                    p.SetValue(ob, double.Parse(c.ToString().Replace(".", ",")), null);
                                    break;
                                }

                                if (p.PropertyType.Name.StartsWith("Int"))
                                {
                                    p.SetValue(ob, int.Parse(c.ToString()), null);
                                    break;
                                }

                                if (p.PropertyType.Name == "DateTime")
                                {
                                    p.SetValue(ob, DateTime.Parse(c.ToString()), null);
                                    break;
                                }

                                if (p.PropertyType.Name == "String")
                                {
                                    p.SetValue(ob, c.ToString(), null);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                            }



                            try
                            {
                                if (dt.Columns[xx].DataType == c.GetType())
                                {
                                    p.SetValue(ob, dt.Rows[i][xx], null);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                l.Add(ob);
            }
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            Connection().Close();
            con.Close();
            con.Dispose();
            con = null;
        }


        return l;
    }
    public bool Delete(string TableName, string idColumnName, string id)
    {
        return this.ExecuteNonQuery("DELETE from " + TableName + " WHERE " + idColumnName + "='" + id + "';");
    }

    public bool Truncate(string TableName)
    {
        return this.ExecuteNonQuery("TRUNCATE " + TableName + ";");
    }

    public bool DropTable(string TableName)
    {
        return this.ExecuteNonQuery("Drop TABLE " + TableName + ";");
    }







    public string InsertUpdateDuplicateKey(string tablename, string columnid, object ob)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("INSERT  INTO " + tablename + " ");



        sb.Append(getinsertduplicateString(ob));

        this.ExecuteNonQuery(sb.ToString());


        return sb.ToString();
    }


    private string getinsertduplicateString(object ob)
    {
        PropertyInfo[] props = ob.GetType().GetProperties();

        StringBuilder sb = new StringBuilder();
        sb.Append(" (");

        foreach (var p in props)
        {
            sb.Append(p.Name);
            sb.Append(",");
        }
        if (sb.ToString().EndsWith(","))
            sb.Remove(sb.Length - 1, 1);

        sb.Append(") ");

        sb.AppendLine(" VALUES ");

        sb.Append(" (");

        foreach (var p in props)
        {
            sb.Append("'");
            if (p.PropertyType.Name == "Double")
            {
                string vv = p.GetValue(ob, null).ToString().Replace(",", ".");
                sb.Append(vv);
            }
            else
                sb.Append(p.GetValue(ob, null));

            sb.Append("'");
            sb.Append(",");
        }

        if (sb.ToString().EndsWith(","))
            sb.Remove(sb.Length - 1, 1);

        sb.Append(") ");
        sb.AppendLine("  ON DUPLICATE KEY UPDATE ");

        foreach (var p in props)
        {
            sb.Append("`");
            sb.Append(p.Name);
            sb.Append("` = ");
            sb.Append("'");
            if (p.PropertyType.Name == "Double")
            {
                string vv = p.GetValue(ob, null).ToString().Replace(",", ".");
                sb.Append(vv);
            }
            else
                sb.Append(p.GetValue(ob, null));
            sb.Append("',");
        }
        if (sb.ToString().EndsWith(","))
            sb.Remove(sb.Length - 1, 1);

        sb.Append(";");
        return sb.ToString();
    }
}
