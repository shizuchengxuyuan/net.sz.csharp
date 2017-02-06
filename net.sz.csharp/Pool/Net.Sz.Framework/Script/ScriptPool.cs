using Microsoft.CSharp;
using Net.Sz.Framework.Log;
using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Script
{
    /// <summary>
    /// 加载脚本文件
    /// </summary>
    public class ScriptPool
    {
        //TestMain.TestString = "我是第二次";
        //LoadScriptManager.Instance.LoadCSharpFile(new string[] { @"F:\javatest\ConsoleApplication6\CodeDomeCode" }, new List<String>() { ".cs" });
        //Console.ReadLine();
        //{
        //    var rets = LoadScriptManager.Instance.Instances<ITestScript>();
        //    foreach (var item in rets)
        //    {
        //        item.TestShowString("加载");
        //    }
        //}

        HashSet<String> ddlNames = new HashSet<string>();

        //                        接口名称                 类全称命名空间
        private Dictionary<string, Dictionary<string, object>> ScriptInstances = new Dictionary<string, Dictionary<string, object>>();

        public ScriptPool() { }

        #region 返回查找的脚本实例 public IEnumerable<T> Instances<T>()
        /// <summary>
        /// 返回查找的脚本实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetScripts<T>()
        {
            //使用枚举迭代器，避免了再一次创建对象
            string typeName = typeof(T).Name;
            if (ScriptInstances.ContainsKey(typeName))
            {
                var items = ScriptInstances[typeName];
                foreach (var item in items)
                {
                    if (item.Value is T)
                    {
                        yield return (T)item.Value;
                    }
                }
            }
        }
        #endregion

        #region 根据指定的文件动态编译获取实例 public void LoadCSharpFile(string[] paths, List<String> extensionNames, params string[] dllName)
        /// <summary>
        /// 根据指定的文件动态编译获取实例
        /// <para>如果需要加入调试信息，加入代码 System.Diagnostics.Debugger.Break();</para>
        /// <para>如果传入的是目录。默认只会加载目录中后缀“.cs”文件</para>
        /// </summary>
        /// <param name="paths">
        /// 可以是目录也可以是文件路径
        /// </param>
        /// <param name="dllName">加载的附加DLL文件的路径，绝对路径</param>
        public List<String> LoadCSharpFile(string[] paths, params string[] dllName)
        {
            return LoadCSharpFile(paths, null, dllName);
        }

        List<String> csExtensionNames = new List<String>() { ".cs" };
        string exts = ".dll,.exe,";

        /// <summary>
        /// 根据指定的文件动态编译获取实例
        /// <para>如果需要加入调试信息，加入代码 System.Diagnostics.Debugger.Break();</para>
        /// <para>如果传入的是目录。默认只会加载目录中后缀“.cs”文件</para>
        /// </summary>
        /// <param name="paths">
        /// 可以是目录也可以是文件路径
        /// </param>
        /// <param name="extensionNames">需要加载目录中的文件后缀</param>
        /// <param name="dllName">加载的附加DLL文件的路径，绝对路径</param>
        public List<String> LoadCSharpFile(String[] paths, List<String> extensionNames, params String[] dllName)
        {
            List<String> retStrs = new List<String>();
            if (extensionNames == null)
            {
                extensionNames = csExtensionNames;
            }


            var asss = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in asss)
            {
                try
                {
                    if (!item.ManifestModule.IsResource() && !"<未知>".Equals(item.ManifestModule.FullyQualifiedName))
                    {
                        String ext = System.IO.Path.GetExtension(item.ManifestModule.FullyQualifiedName).ToLower();
                        if (exts.Contains(ext))
                        {
                            ddlNames.Add(item.ManifestModule.FullyQualifiedName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("查找需要的dll路径错误1：" + item.ManifestModule.FullyQualifiedName, ex);
                }
            }

            foreach (var item in dllName)
            {
                try
                {
                    String ext = System.IO.Path.GetExtension(item).ToLower();
                    if (exts.Contains(ext))
                    {
                        ddlNames.Add(item);
                    }
                }
                catch (Exception ex) { Logger.Error("查找需要的dll路径错误2：" + item, ex); }
            }
            foreach (var item in ddlNames)
            {
                try
                {
                    Logger.Debug("加载依赖程序集：" + System.IO.Path.GetFileName(item));
                }
                catch (Exception) { Logger.Error("加载依赖程序集：" + item); }
            }

            List<String> fileNames = new List<String>();
            ActionPath(paths, extensionNames, ref fileNames);
            if (fileNames.Count == 0) { retStrs.Add("目录不存在任何脚本文件"); return retStrs; }

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameter = new CompilerParameters();
            //不输出编译文件
            parameter.GenerateExecutable = false;
            //生成调试信息
            parameter.IncludeDebugInformation = true;
            //需要调试必须输出到内存
            parameter.GenerateInMemory = true;
            //添加需要的程序集
            parameter.ReferencedAssemblies.AddRange(ddlNames.ToArray());
            //System.Environment.CurrentDirectory            
            CompilerResults result = provider.CompileAssemblyFromFile(parameter, fileNames.ToArray());//根据制定的文件加载脚本
            if (result.Errors.HasErrors)
            {
                var item = result.Errors.GetEnumerator();
                while (item.MoveNext())
                {
                    Logger.Error("动态加载文件出错了！" + item.Current.ToString());
                }
            }
            else
            {
                Dictionary<string, Dictionary<string, object>> tempInstances = new Dictionary<string, Dictionary<string, object>>();
                ActionAssembly(result.CompiledAssembly, tempInstances, retStrs);
                if (retStrs.Count == 0 && tempInstances.Count > 0)
                {
                    this.ScriptInstances = tempInstances;
                }
            }
            return retStrs;
        }
        #endregion

        #region 根据指定的文件动态编译获取实例 public void LoadDll(string[] paths)

        List<String> dllExtensionNames = new List<String>() { ".dll", ".DLL" };

        /// <summary>
        /// 根据指定的文件动态编译获取实例
        /// <para>如果需要加入调试信息，加入代码 System.Diagnostics.Debugger.Break();</para>
        /// </summary>
        /// <param name="paths">
        /// 可以是目录也可以是文件路径
        /// <para>如果传入的是目录。只会加载目录中后缀“.dll”,“.DLL”文件</para>
        /// </param>
        public List<String> LoadDllFromBinary(params string[] paths)
        {
            List<String> retStrs = new List<String>();
            List<String> fileNames = new List<String>();
            ActionPath(paths, dllExtensionNames, ref fileNames);
            if (fileNames.Count == 0) {retStrs.Add("目录不存在任何脚本文件"); return retStrs;}
            try
            {
                List<byte> bFile = new List<byte>();
                Dictionary<string, Dictionary<string, object>> tempInstances = new Dictionary<string, Dictionary<string, object>>();
                foreach (var path in fileNames)
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            bFile.AddRange(br.ReadBytes((int)fs.Length));
                        }
                    }
                }
                ActionAssembly(Assembly.Load(bFile.ToArray()), tempInstances, retStrs);
                if (retStrs.Count == 0 && tempInstances.Count > 0)
                {
                    this.ScriptInstances = tempInstances;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("动态加载文件", ex);
            }
            return retStrs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        public List<String> LoadDllFromFile(params string[] paths)
        {
            List<String> retStrs = new List<String>();
            List<String> fileNames = new List<String>();
            ActionPath(paths, dllExtensionNames, ref fileNames);
            if (fileNames.Count == 0) { retStrs.Add("目录不存在任何脚本文件"); return retStrs; }
            Dictionary<string, Dictionary<string, object>> tempInstances = new Dictionary<string, Dictionary<string, object>>();
            foreach (var path in fileNames)
            {
                try
                {
                    ActionAssembly(Assembly.LoadFrom(path), tempInstances, retStrs);
                }
                catch (Exception ex)
                {
                    Logger.Error("动态加载文件", ex);
                }
            }
            
            if (retStrs.Count == 0 && tempInstances.Count > 0)
            {
                this.ScriptInstances = tempInstances;
            }
            return retStrs;
        }
        #endregion

        #region 处理加载出来的实例 void ActionAssembly(Assembly assembly)
        string baseScriptName = typeof(IBaseScript).Name;
        /// <summary>
        /// 处理加载出来的实例
        /// </summary>
        /// <param name="assembly"></param>
        void ActionAssembly(Assembly assembly, Dictionary<string, Dictionary<string, object>> tempInstances, List<String> retStrs)
        {
            //获取加载的所有对象模型
            Type[] instances = assembly.GetExportedTypes();
            foreach (var itemType in instances)
            {
                if (!itemType.IsClass || itemType.IsAbstract)
                {
                    continue;
                }
                try
                {
                    //生成实例
                    object obj = assembly.CreateInstance(itemType.FullName);
                    if (obj is IInitBaseScript)
                    {
                        (obj as IInitBaseScript).InitScript();
                    }
                    //获取单个模型的所有继承关系和接口关系
                    Type[] interfaces = itemType.GetInterfaces();
                    if (interfaces == null || interfaces.Length == 0)
                    {
                        Logger.Debug("动态加载 " + itemType.FullName + " 没有实现IBaseScript接口");
                        continue;
                    }
                    if (obj is IBaseScript)
                    {
                        foreach (var iteminterface in interfaces)
                        {
                            if (iteminterface.Name == baseScriptName)
                            {
                                continue;
                            }
                            Logger.Debug("动态加载实例：" + itemType.FullName + " : " + iteminterface.FullName);
                            //加入对象集合
                            if (!tempInstances.ContainsKey(iteminterface.Name))
                            {
                                tempInstances[iteminterface.Name] = new Dictionary<string, object>();
                            }
                            //基于对象的命名空间区分不同的文件；
                            tempInstances[iteminterface.Name][itemType.FullName] = obj;
                        }
                    }
                    else
                    {
                        Logger.Debug("动态加载 " + itemType.FullName + " 没有实现IBaseScript接口");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("动态加载Error：" + ex.Message);
                    retStrs.Add("动态加载Error：" + ex.Message);
                }
            }
        }
        #endregion

        #region 处理传入的路径 void ActionPath(string[] paths, List<String> extensionNames, ref List<String> fileNames)
        /// <summary>
        /// 处理传入的路径，
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="extensionNames"></param>
        /// <param name="fileNames"></param>
        void ActionPath(string[] paths, List<String> extensionNames, ref List<String> fileNames)
        {
            foreach (var tempPath in paths)
            {
                string path = Path.GetFullPath(tempPath);
                Logger.Debug(path);
                if (System.IO.Path.HasExtension(path))
                {
                    if (System.IO.File.Exists(path))
                    {
                        fileNames.Add(path);
                        //编译文件
                        Logger.Debug("动态加载文件：" + path);
                    }
                    else
                    {
                        Logger.Debug("动态加载文件 无法找到文件：" + path);
                    }
                }
                else
                {
                    GetFiles(path, extensionNames, ref fileNames);
                }
            }
        }
        #endregion

        #region 根据指定文件夹获取指定路径里面全部文件 void GetFiles(string sourceDirectory, List<String> extensionNames, ref  List<String> fileNames)
        /// <summary>
        /// 根据指定文件夹获取指定路径里面全部文件
        /// </summary>
        /// <param name="sourceDirectory">目录</param>
        /// <param name="extensionNames">需要获取的文件扩展名</param>
        /// <param name="fileNames">返回文件名</param>
        void GetFiles(string sourceDirectory, List<String> extensionNames, ref  List<String> fileNames)
        {
            if (!Directory.Exists(sourceDirectory))
            {
                Logger.Error("动态加载文件", new Exception("目录 " + sourceDirectory + " 未能找到需要加载的脚本文件"));
                return;
            }
            {
                //获取所有文件名称
                string[] fileName = Directory.GetFiles(sourceDirectory);
                foreach (string path in fileName)
                {
                    if (System.IO.File.Exists(path))
                    {
                        string extName = System.IO.Path.GetExtension(path);
                        if (extensionNames.Contains(extName))
                        {
                            fileNames.Add(path);
                            //编译文件
                            Logger.Debug("动态加载文件：" + path);
                        }
                    }
                    else
                    {
                        Logger.Debug("动态加载 无法找到文件：" + path);
                    }
                }
            }
            //获取所有子目录名称
            string[] directionName = Directory.GetDirectories(sourceDirectory);
            foreach (string directionPath in directionName)
            {
                //递归下去
                GetFiles(directionPath, extensionNames, ref fileNames);
            }
        }
        #endregion
    }
}
