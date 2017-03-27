using Net.Sz.Framework.Utils;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz
{
    public static class SzExtensions
    {

        static LongId0Util ids = new LongId0Util();

        static public long GetId()
        {
            return ids.GetId();
        }

        /// <summary>
        /// 根据guid生成的无序的重复率非常小的id
        /// </summary>
        /// <returns></returns>
        public static long GetGUID()
        {
            return BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
        }

        static Mutex mutex;

        /// <summary>
        /// 单例模式
        /// </summary>
        /// <param name="strMutex">系统互斥名,任意设置标识就行</param>
        static public void Singleton(string strMutex = "SzExtensions")
        {
            bool createdNew;
            //系统能够识别有名称的互斥，因此可以使用它禁止应用程序启动两次
            //第二个参数可以设置为产品的名称:Application.ProductName
            //每次启动应用程序，都会验证名称为SingletonWinAppMutex的互斥是否存在
            mutex = new Mutex(true, strMutex, out createdNew);
            //如果已运行，则在前端显示
            //createdNew == false，说明程序已运行
            if (!createdNew)
            {
                Process instance = GetExistProcess();
                //如果程序已经启动，设置为前端显示
                if (instance != null) { SetForegroud(instance); }
                //退出当前程序
                System.Environment.Exit(0);
            }
        }

        /// <summary>
        /// 查看程序是否已经运行
        /// </summary>
        /// <returns></returns>
        static Process GetExistProcess()
        {
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if ((process.Id != currentProcess.Id) &&
                    (Assembly.GetExecutingAssembly().Location == currentProcess.MainModule.FileName))
                {
                    return process;
                }
            }
            return null;
        }

        /// <summary>
        /// 使程序前端显示
        /// </summary>
        /// <param name="instance"></param>
        static void SetForegroud(Process instance)
        {
            IntPtr mainFormHandle = instance.MainWindowHandle;
            if (mainFormHandle != IntPtr.Zero)
            {
                ShowWindowAsync(mainFormHandle, 1);
                SetForegroundWindow(mainFormHandle);
            }
        }

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [MethodImpl(MethodImplOptions.ForwardRef), DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [MethodImpl(MethodImplOptions.ForwardRef), DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
        [MethodImpl(MethodImplOptions.ForwardRef), DllImport("Kernel32.dll")]
        private static extern bool AllocConsole();
        [MethodImpl(MethodImplOptions.ForwardRef), DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [MethodImpl(MethodImplOptions.ForwardRef), DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hwind, int cmdShow);

        [MethodImpl(MethodImplOptions.ForwardRef), DllImport("user32.dll")]
        private static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        private static string ConsoleTitle = string.Empty;

        private static bool flag_console = false;

        private static System.Timers.Timer Console_Timer;

        /// <summary>
        /// 关闭/隐藏控制台 窗体
        /// </summary>
        static public void Console_Hide()
        {
            if (Console_Timer != null)
            {
                Console_Timer.Stop();
            }
            flag_console = false;
            FreeConsole();
        }

        /// <summary>
        /// 显示控制台,一旦显示就不会再调用第二次
        /// </summary>
        /// <param name="title">显示的标题</param>
        /// <param name="withCloseBox">是否移除关闭按钮，默认值：false 显示</param>
        /// <param name="isShowThreadCount">在控制台显示线程数量，默认值：true 显示</param>
        static public void Console_Show(string title, [MarshalAs(UnmanagedType.U1)] bool withCloseBox, [MarshalAs(UnmanagedType.U1)] bool isShowThreadCount)
        {
            //检测flag_console,防止调用两次
            //直接用win32api
            //顺便移除窗口
            if (!flag_console)
            {
                flag_console = true;
                AllocConsole();
                Console.Title = title;
                if (withCloseBox)
                {
                    RemoveMenu();
                }
                if (isShowThreadCount) { ShowThreadCount(title); }
                else { Console.Title = title; }
            }
        }

        /// <summary>
        /// 移除关闭按钮
        /// </summary>
        static void RemoveMenu()
        {
            IntPtr mainFormHandle = GetForegroundWindow();
            RemoveMenu(GetSystemMenu(mainFormHandle, IntPtr.Zero), 0xf060, 0);
        }

        /// <summary>
        /// 在控制台title显示线程量
        /// </summary>
        /// <param name="title"></param>
        public static void ShowThreadCount(string title)
        {
            ConsoleTitle = title;
            Console_Timer = new System.Timers.Timer(200);
            Console_Timer.Elapsed += (obj, e) =>
            {
                int thrads = Process.GetCurrentProcess().Threads.Count;
                int runthreads = 0;
                foreach (ProcessThread item in Process.GetCurrentProcess().Threads) if (item.ThreadState == System.Diagnostics.ThreadState.Running) runthreads++;
                long mem = System.Environment.WorkingSet / 1024 / 1024;
                Console.Title = string.Format("{0} -> 当前内存占用 -> {3} MB -> 当前线程量 -> {1} ->  当前活动线程： -> {2}", ConsoleTitle, thrads, runthreads, mem);
            };
            Console_Timer.Start();
        }
    }
}