using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Erepertorium
{
    public class UserType:DataBaseStorageHelper
    {
        public int Id {get;set;}
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Level { get; set; }
        public string localpwd;



        public void SetLocalPassword()
        {
            byte[] lb = System.Text.Encoding.UTF8.GetBytes(this.localpwd);
            UpdateByte(lb);           
        }

        public void UpdateByte(byte[] buffer)
        {
            try
            {
                MySqlConnection con = MysqlCore.DB_Main().Connection();

                MySqlCommand cmd = new MySqlCommand("update users set localpwd=@p1 where id=@p2",con);

                cmd.Parameters.Add(new MySqlParameter("@p1", buffer));
                cmd.Parameters.Add(new MySqlParameter("@p2", this.Id));

                cmd.ExecuteNonQuery();
                con.Clone();
            }
            catch (Exception)
            {

            }

        }

        public void GetLocalPassword()
        {
            byte[] lb = MysqlCore.DB_Main().GetByte("select localpwd from users where id=" + this.Id);
            this.localpwd = System.Text.Encoding.UTF8.GetString(lb,0,lb.Length);
        }

    }
}