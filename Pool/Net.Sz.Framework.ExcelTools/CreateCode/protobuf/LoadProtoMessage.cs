using Microsoft.CSharp;
using Net.Sz.Framework.Szlog;
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
namespace Net.Sz.Framework.ExcelTools.CreateCode.protobuf
{
    /// <summary>
    /// 加载脚本文件
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"Login")]
    public class LoadProtoMessage : global::ProtoBuf.IExtensible
    {

        private static SzLogger log = SzLogger.getLogger();

        private static LoadProtoMessage instance = new LoadProtoMessage();
        /// <summary>
        /// 返回脚本管理器实例
        /// </summary>
        public static LoadProtoMessage Instance { get { return instance; } }

        HashSet<String> ddlNames = new HashSet<string>();

        LoadProtoMessage() { }

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
        public void LoadCSharpFile(string[] paths, params string[] dllName)
        {
            LoadCSharpFile(paths, null, dllName);
        }

        List<String> csExtensionNames = new List<String>() { ".cs" };


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
        public void LoadCSharpFile(string[] paths, List<String> extensionNames, params string[] dllName)
        {
            if (extensionNames == null)
            {
                extensionNames = csExtensionNames;
            }

            List<String> fileNames = new List<String>();
            ActionPath(paths, extensionNames, ref fileNames);
            if (fileNames.Count == 0)
            {
                return;
            }

            var asss = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in asss)
            {
                if (!item.ManifestModule.IsResource() && item.ManifestModule.FullyQualifiedName.EndsWith(".dll") || item.ManifestModule.FullyQualifiedName.EndsWith(".DLL"))
                {
                    ddlNames.Add(item.ManifestModule.FullyQualifiedName);
                }
            }

            foreach (var item in dllName)
            {
                if (item.EndsWith(".dll") || item.EndsWith(".DLL"))
                {
                    ddlNames.Add(item);
                }
            }
            foreach (var item in ddlNames)
            {
                try
                {
                    if (log.IsInfoEnabled()) log.Info("加载DLL：" + item);
                }
                catch { }
            }

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
            CompilerResults result = provider.CompileAssemblyFromFile(parameter, fileNames.ToArray());//根据制定的文件加载脚本
            if (result.Errors.HasErrors)
            {
                var item = result.Errors.GetEnumerator();
                while (item.MoveNext())
                {
                    if (log.IsErrorEnabled()) log.Error("动态加载文件出错了！" + item.Current.ToString());
                }
            }
            else
            {
                ActionAssembly(result.CompiledAssembly);
            }
        }
        #endregion

        Net.Sz.Framework.Script.ScriptPool scriptmanager = new Framework.Script.ScriptPool();

        #region 处理加载出来的实例 void ActionAssembly(Assembly assembly)
        /// <summary>
        /// 处理加载出来的实例
        /// </summary>
        /// <param name="assembly"></param>
        void ActionAssembly(Assembly assembly)
        {

            scriptmanager.LoadCSharpFile(new string[] { "Template/Scripts/" });

            IEnumerable<ICreateHandlerScript> scripts = scriptmanager.GetScripts<ICreateHandlerScript>();

            ConcurrentDictionary<string, ConcurrentDictionary<string, object>> tempInstances = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();
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
                    if (itemType.Name.EndsWith("Message"))
                    {
                        foreach (var member in itemType.GetMembers())
                        {
                            if (member.Name.EndsWith("Message"))
                            {
                                foreach (var script in scripts)
                                {
                                    script.Create(itemType, member);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled()) log.Error("根据类型模拟生成处理handler", ex);
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
            foreach (var path in paths)
            {
                if (System.IO.Path.HasExtension(path))
                {
                    if (System.IO.File.Exists(path))
                    {
                        fileNames.Add(path);
                        //编译文件
                        if (log.IsInfoEnabled()) log.Info("动态加载文件：" + path);
                    }
                    else
                    {
                        if (log.IsInfoEnabled()) log.Info("动态加载文件 无法找到文件：" + path);
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
                if (log.IsErrorEnabled()) log.Error("动态加载文件", new Exception("目录 " + sourceDirectory + " 未能找到需要加载的脚本文件"));
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
                        }
                    }
                    else
                    {
                        if (log.IsInfoEnabled()) log.Info("动态加载 无法找到文件：" + path);
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

        public ProtoBuf.IExtension GetExtensionObject(bool createIfMissing)
        {
            throw new NotImplementedException();
        }
    }
}
