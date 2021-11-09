using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            MysqlCore.DB_Main().ExecuteNonQuery("delete from registrys where user='testunit'");

            Erepertorium.RegistryType.CreateNewEntries(100, "testunit");

            int ct = MysqlCore.DB_Main().GetCount("SELECT count(distinct(number)) FROM erepdb.registrys where user = 'testunit';");

            st.Stop();
            Console.WriteLine(st.ElapsedMilliseconds.ToString());
            Debug.Print(st.ElapsedMilliseconds.ToString());
            Assert.IsTrue(ct == 100);
            

        }
    }
}
