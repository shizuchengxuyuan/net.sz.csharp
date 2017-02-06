using Net.Sz.Framework.ORMDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMTest
{
    class Program
    {


        static void Main(string[] args)
        {
            SqliteImpl impl = new SqliteImpl("", "test.db3", "", "", false);

            //impl.createTable(typeof(Model.user_table));

            //Model.user_table user = new Model.user_table() { ID = 1, LoginName = "s", LoginSys = "1", LoginPwa = "s", loginREName = " s", Logintime = "ss" };

            //Console.WriteLine(impl.Insert(user));

            //Console.WriteLine("删除行：" + impl.delete<Model.user_table>(6));


            Console.ReadLine();
        }


        //static void init()
        //{

        //    try
        //    {
        //        SqliteImpl impl = new SqliteImpl("", "haociyiqi.db3", "", "", true);
        //        impl.createTable(typeof(LTP.Model.cgsj));
        //        impl.createTable(typeof(Model.log_table));
        //        impl.createTable(typeof(Model.product));
        //        impl.createTable(typeof(Model.user_table));

        //        {
        //            Console.WriteLine("处理所有产品数据");
        //            List<Model.product> ps = DAL.product.GetModels();

        //            impl.Insert(10, ps.ToArray());
        //        }
        //        {
        //            Console.WriteLine("处理所有用户数据");
        //            List<Model.user_table> us = DAL.user_table.GetModels();

        //            impl.Insert(us.ToArray());
        //        }

        //        {
        //            Console.WriteLine("处理所有日志数据");
        //            List<Model.log_table> us = DAL.log_table.GetModels();

        //            impl.Insert(us.ToArray());
        //        }

        //        {
        //            Console.WriteLine("处理所有商家数据");
        //            List<LTP.Model.cgsj> cs = DAL.cgsj.GetModels();

        //            impl.Insert(cs.ToArray());
        //        }

        //        Console.WriteLine("处理完成所有数据");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //    Console.ReadLine();
        //}
    }
}
