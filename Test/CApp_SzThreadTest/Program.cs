using Net.Sz.Framework.Szlog;
using Net.Sz.Framework.SzThreading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CApp_SzThreadTest
{
    public class Program
    {

        static SzLogger log = SzLogger.getLogger();

        static void Main(string[] args)
        {
            long testthread = ThreadPool.GetThreadModel("测试线程");


            ThreadPool.AddTask(testthread, new ActionTask((task) =>
            {
                log.Error("指定线程执行一次");
            }));

            /*指定间隔时间无限执行，但是我只让他执行一次就取消*/
            ThreadPool.AddGlobTimerTask(new CancelTest());


            ThreadPool.AddGlobTask(new ActionTask((task) =>
            {
                log.Error("全局线程执行一次");
            }));

            System.Threading.Thread.Sleep(1000);

            /*指定间隔时间无限执行，全局线程*/
            ThreadPool.AddGlobTimerTask(new ActionTimerTask(100, (task) =>
            {
                log.Error("全局线程，指定间隔时间无限执行");
            }));


        }

        class CancelTest : TimerTaskModel
        {

            public CancelTest() : base(100) { }

            int runCount;


            public override void Run()
            {
                log.Error("CancelTest" + runCount);

                if (++runCount > 5)
                {
                    this.Cancel = true;
                    log.Error("CancelTest取消执行");
                }

            }

        }
    }
}
