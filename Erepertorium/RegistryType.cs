using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Erepertorium
{
    public class RegistryType:DataBaseStorageHelper
    {

        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Content { get;set; }
        public int Status { get; private set; }
        public List<RegistryHistoryType> History { get; set; }

        public string hs;

        public RegistryType()
        {
            this.History = new List<RegistryHistoryType>();
        }

        public string HistoryToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var h in this.History)
            {
                sb.AppendLine(h.Time + " " + h.Description + " " + h.User + "<br/> ");
            }
            return sb.ToString();
        }

        public static int GetCount(DateTime date, bool ShowDeleted, bool onlymy, string user = "")
        {
            int ct = 0;
            
            if (onlymy == false)
            {
                if (ShowDeleted == true)
                    ct = MysqlCore.DB_Main().GetCount("select count(id) from registrys where  date like'%" + date.ToShortDateString() + "%';");
                else
                    ct = MysqlCore.DB_Main().GetCount("select count(id) from registrys where (status =1) and date like'%" + date.ToShortDateString() + "%' and status <> 5 ;");
            }
            else
            {
                if (ShowDeleted == true)
                    ct = MysqlCore.DB_Main().GetCount("select count(id) from registrys where  date like'%" + date.ToShortDateString() + "%'  and user='" + user + "'  ;");
                else
                    ct = MysqlCore.DB_Main().GetCount("select count(id) from registrys where (status =1) and date like'%" + date.ToShortDateString() + "%' and status <> 5  and user='" + user + "'  ;");
            }


            return ct;
        }
        public static List<RegistryType>LoadByDate(DateTime date, bool ShowDeleted, bool onlymy, string user="")
        {
            List<RegistryType> l = new List<RegistryType>();
            if (onlymy == false)
            {
                if(ShowDeleted==true)
                    l = RegistryType.LoadWhere<RegistryType>("date like'%" + date.ToShortDateString() + "%' order by number desc");
                else
                    l = RegistryType.LoadWhere<RegistryType>("date like'%" + date.ToShortDateString() + "%' and status <> 5 order by number desc");
            }
            else
            {
                if (ShowDeleted == true)
                    l = RegistryType.LoadWhere<RegistryType>("date like'%" + date.ToShortDateString() + "%' and user='" + user + "' order by number desc");
                else
                    l = RegistryType.LoadWhere<RegistryType>("date like'%" + date.ToShortDateString() + "%' and status <> 5 and user='" + user + "' order by number desc");
            }


            foreach (var o in l)
                o.hs = o.HistoryToString();

            return l;
        }

        public enum HistoryDescriptions
        {
            Utworzono,
            Edytowano,
            Usunięto,
            Anulowano_zmiany
        }

        private void AddHistoryEntry(string user, HistoryDescriptions description)
        {
            RegistryHistoryType h = new RegistryHistoryType();
            h.Time = DateTime.Now;
            h.User = user;
            h.Description = description.ToString();
            this.History.Add(h);
        }

        public static void ChangeContent(string content,string user,int id)
        {
            RegistryType r = RegistryType.Load<RegistryType>(id);
            r.ChangeContent(content, user);
        }

        public static void CancelEdit(string user,int id)
        {
            RegistryType r = RegistryType.Load<RegistryType>(id);
            r.Status = 0;
            r.AddHistoryEntry(user, HistoryDescriptions.Anulowano_zmiany);
            r.Save();
        }
        public static void DeleteRegistry(string user, int id)
        {
            RegistryType r = RegistryType.Load<RegistryType>(id);
            r.Status = 5;
            r.AddHistoryEntry(user, HistoryDescriptions.Usunięto);
            r.Save();
        }

        public void ChangeContent(string content, string user)
        {
            this.Content = content;

            if (this.Content.Length > 1000)
                this.Content = this.Content.Substring(0, 1000);

            this.Status = 0;
            this.AddHistoryEntry(user, HistoryDescriptions.Edytowano);
            this.Save();
        }

        public void BeginEdit(string user)
        {
            this.Status = 1;
            this.AddHistoryEntry(user, HistoryDescriptions.Edytowano);
            this.Save();
        }

        public void DeleteEmptys()
        {
            string sq = "delete from registrys where ordered is null and status = 5 and date <= NOW() - INTERVAL 1 DAY;";
            MysqlCore.DB_Main().ExecuteNonQuery(sq);
        }
        public void ReindexBaseOnlyNoOrdered()
        {
            string sq = "" +
                "SELECT @i:= max(number) from registrys where ordered is not null;" +
                "UPDATE registrys SET number = @i:= @i + 1, ordered = 1 where ordered is null;" +
                "";
            MysqlCore.DB_Main().ExecuteNonQuery(sq);           
                        
        }
        public void ReindexBaseOAll()
        {
            string sq = "" +
                "SELECT @i:=0;" +
                "UPDATE registrys SET number = @i:= @i + 1, ordered = 1;" +
                "";
            MysqlCore.DB_Main().ExecuteNonQuery(sq);

        }

        public static List<RegistryType> CreateNewEntries(int count, string user)
        {
            List<RegistryType> l = new List<RegistryType>();

            int firstnumber = GetNextNumber();

            for (int i = 0; i < count; i++)
            {
                RegistryType r = new RegistryType();
                r.Date = DateTime.Now;
                r.Number = firstnumber;
                r.User = user;
                r.Content = "";
                r.Status = 1;
                r.AddHistoryEntry(user, HistoryDescriptions.Utworzono);
                r.Save();
                l.Add(r);

                firstnumber += 1;
            }

            return l;
        }

        public static int GetNextNumber()
        {
            int nn = 0;
            nn = int.Parse(MysqlCore.DB_Main().GetString("select max(number) from erepdb.registrys;", "0"));
            nn += 1;
            return nn;
        }

    }
}