using Net.Sz.Framework.Szlog;
using Net.Sz.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CApp_SzlogTest
{
    class Program
    {

        static SzLogger log = null;

        static void Main(string[] args)
        {
            CommUtil.LOG_PRINT_CONSOLE = false;
            CommUtil.LOG_PRINT_FILE = true;
            //CommUtil.LOG_PRINT_FILE_BUFFER = false;
            log = SzLogger.getLogger();

            Console.WriteLine("准备好测试了请敲回车");
            Console.ReadLine();
            List<System.Threading.Thread> ths = new List<System.Threading.Thread>();
            long time = TimeUtil.CurrentTimeMillis();
            for (int k = 0; k < 5; k++)
            {
                /*5个线程*/
                System.Threading.Thread t = new System.Threading.Thread(() =>
                    {
                        /*每个线程 10万 条日志*/
                        for (int i = 0; i < 1000000; i++)
                        {
                            Program.log.Error(i + " ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss我测ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss");
                        }
                    });
                t.Start();
                ths.Add(t);
            }

            ths.ForEach((t) => t.Join());
            Console.WriteLine("并发结束，等待写入结束" + (TimeUtil.CurrentTimeMillis() - time));
            while (Program.log.Count > 0)
            {

            }
            Console.WriteLine("500万条日志并发写入结束" + (TimeUtil.CurrentTimeMillis() - time));
            Console.ReadLine();
        }
    }
}
