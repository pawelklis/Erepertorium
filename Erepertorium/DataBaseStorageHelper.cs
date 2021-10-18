using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Erepertorium
{
    public class DataBaseStorageHelper
    {

        public async void SaveAsync()
        {
            await Task.Run(new Action(async () =>
            {
                MysqlCore m = MysqlCore.DB_Main();
                string tb = this.ToString().Replace("Erepertorium.", "").Replace("Type", "s");
                await Task.Run(new Action(() => m.NewInsertOrUpdate(this, tb)));
            }));
        }

        public void Save()
        {
            string tb = this.ToString().Replace("Erepertorium.", "").Replace("Type", "s");
            MysqlCore.DB_Main().NewInsertOrUpdate(this, tb);
        }
        public static void CreateTable(object ExampleObject)
        {
            string tb = ExampleObject.ToString().Replace("Erepertorium.", "").Replace("Type", "s");
            MysqlCore.DB_Main().CreateTable(ExampleObject, "erepdb", "id", tb);
        }

        public static T Load<T>(int objectID) where T : new()
        {

            string tb = typeof(T).ToString().Replace("Erepertorium.", "").Replace("Type", "s");
            return MysqlCore.DB_Main().NewGetSingleObject<T>("Select * from `" + tb + "` where id=" + objectID);
        }

        public static List<T> LoadAll<T>() where T : new()
        {
            string tb = typeof(T).ToString().Replace("Erepertorium.", "").Replace("Type", "s");
            return MysqlCore.DB_Main().NewGetObjects<T>("Select * from `" + tb + "`;");
        }
        public static List<T> LoadWhere<T>(string where) where T : new()
        {
            string tb = typeof(T).ToString().Replace("Erepertorium.", "").Replace("Type", "s");
            return MysqlCore.DB_Main().NewGetObjects<T>("Select * from `" + tb + "` WHERE " + where + ";");
        }







    }
}
