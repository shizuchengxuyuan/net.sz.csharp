using Net.Sz.Framework.Szlog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Netty.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpSession
    {

        private static SzLogger log = SzLogger.getLogger();

        private const string HTTP_CONTENT_All = "*/*; charset=UTF-8";
        private const string HTTP_CONTENT_TEXT = "text/plain; charset=UTF-8";
        private const string HTTP_CONTENT_JSON = "applicaton/x-json; charset=UTF-8";
        private const string HTTP_CONTENT_HTML = "text/html; charset=UTF-8";
        private const string HTTP_CONTENT_XML = "text/xml; charset=UTF-8";
        private const string HTTP_CONTENT_JAVACRIPT = "application/javascript; charset=UTF-8";


        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB
        private const int BUF_SIZE = 4096;
        private Socket _Socket;
        private BufferedStream inputStream;
        private StreamWriter OutputStream;
        internal bool IsRespons = false;

        private StringBuilder builder = new StringBuilder();

        /// <summary>
        /// get or post
        /// </summary>
        public String Http_Method;
        /// <summary>
        /// 访问的本地绑定连接地址
        /// </summary>
        public String Http_Url;
        /// <summary>
        /// 提交的所有参数
        /// </summary>
        public String Http_Data_Body;
        /// <summary>
        /// 
        /// </summary>
        public String Http_Protocol_Versionstring;
        /// <summary>
        /// 
        /// </summary>
        public Hashtable HttpHeaders;
        /// <summary>
        /// 键值对形式的参数解析
        /// </summary>
        private Dictionary<string, string> _Parms;

        /// <summary>
        /// 获取远程终结点
        /// </summary>
        public IPEndPoint RemoteEndPoint { get { return (IPEndPoint)_Socket.RemoteEndPoint; } }

        /// <summary>
        /// 获取本地终结点
        /// </summary>
        public IPEndPoint LocalEndPoint { get { return (IPEndPoint)_Socket.LocalEndPoint; } }

        /// <summary>
        /// 可以用于临时变量
        /// </summary>
        private ObjectAttribute _RunAttribute;

        /// <summary>
        /// 
        /// </summary>
        public ObjectAttribute RunAttribute
        {
            get
            {
                /*未考虑并发*/
                if (_RunAttribute == null) _RunAttribute = new ObjectAttribute();
                return _RunAttribute;
            }
            set { this._RunAttribute = value; }
        }

        public String Parms(string key)
        {
            if (_Parms.ContainsKey(key))
            {
                return _Parms[key];
            }
            return null;
        }

        public HttpSession(Socket s)
        {
            this._Socket = s;
            HttpHeaders = new Hashtable();
            inputStream = new BufferedStream(new NetworkStream(_Socket, FileAccess.Read));
            OutputStream = new StreamWriter(new NetworkStream(_Socket, FileAccess.Write));

        }

        internal void _Action()
        {
            ParseRequest();
            ReadHeaders();
            _Parms = GetRequestExec();
        }

        private string StreamReadLine()
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }

        internal void Process(IHttpHandler httphandler)
        {
            try
            {
                httphandler.Run(this);
                if (!this.IsRespons)
                {
                    log.Error("实现类：" + httphandler.GetType().Name + "，未调用 Respons 函数回复请求");
                    if (builder == null || builder.Length < 1) Respons404();
                    else Respons();
                }
            }
            catch (Exception e)
            {
                log.Error("ActiveHttp Exception: ", e);
                this.Close();
            }
        }

        public void Close()
        {
            IsRespons = true;
            OutputStream.Flush();
            OutputStream.Dispose();
            OutputStream = null;
            inputStream.Dispose();
            inputStream = null;
            this._Socket.Close();
            this.builder = null;
        }

        private void ParseRequest()
        {
            String request = StreamReadLine();
            string[] tokens = request.Split(' ');
            Http_Method = tokens[0];
            int length = tokens[1].IndexOf("?");
            if (length >= 0)
            {
                Http_Data_Body = tokens[1].Substring(length + 1);
                Http_Url = tokens[1].Substring(0, length).ToLower();
            }
            else
            {
                Http_Data_Body = "";
                Http_Url = tokens[1].ToLower();
            }
            if (Http_Url.StartsWith("/"))
            {
                Http_Url = Http_Url.Substring(1);
            }
            if (Http_Url.EndsWith("/"))
            {
                Http_Url = Http_Url.Substring(0, Http_Url.Length - 1);
            }
            Http_Protocol_Versionstring = tokens[2];
        }

        private void ReadHeaders()
        {
            String line;
            while (!String.IsNullOrWhiteSpace(line = StreamReadLine()))
            {
                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++;//过滤键值对的空格
                }
                string value = line.Substring(pos, line.Length - pos);
                HttpHeaders[name] = value;
            }
        }

        #region 读取数据 private Dictionary<string, string> GetRequestExec()
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetRequestExec()
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            string data = "";
            if ("GET".Equals(Http_Method))
            {
                data = Http_Data_Body;
            }
            else if ("POST".Equals(Http_Method))
            {
                int content_len = 0;
                using (MemoryStream ms = new MemoryStream())
                {
                    if (this.HttpHeaders.ContainsKey("Content-Length"))
                    {
                        //内容的长度
                        content_len = Convert.ToInt32(this.HttpHeaders["Content-Length"]);
                        if (content_len > MAX_POST_SIZE) { throw new Exception(String.Format("POST Content-Length({0}) 对于这个简单的服务器太大", content_len)); }
                        byte[] buf = new byte[BUF_SIZE];
                        int to_read = content_len;
                        while (to_read > 0)
                        {
                            int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                            if (numread == 0)
                            {
                                if (to_read == 0) { break; }
                                else { throw new Exception("client disconnected during post"); }
                            }
                            to_read -= numread;
                            ms.Write(buf, 0, numread);
                        }
                        ms.Seek(0, SeekOrigin.Begin);
                    }
                    using (StreamReader inputData = new StreamReader(ms))
                    {
                        data = inputData.ReadToEnd();
                    }
                }
            }
            this.Http_Data_Body = data;
            datas = getData(data);
            return datas;
        }

        /// <summary>
        /// 分析http提交数据分割
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private static Dictionary<string, string> getData(string rawData)
        {
            var rets = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(rawData))
            {
                string[] rawParams = rawData.Split('&');
                foreach (string param in rawParams)
                {
                    if (!string.IsNullOrWhiteSpace(param))
                    {
                        string[] kvPair = param.Split('=');
                        if (kvPair.Length > 1)
                        {
                            string key = kvPair[0];
                            string value = HttpUtility.UrlDecode(kvPair[1]);
                            rets[key] = value;
                        }
                    }
                }
            }
            return rets;
        }

        #endregion

        #region 输出数据 public void WriteFlush()

        public void AddContent(string body)
        {
            builder.Append(body);
        }

        public void AddContent(string formatbody, params object[] values)
        {
            builder.AppendFormat(formatbody, values);
        }

        public void AddContentLine(string body)
        {
            builder.AppendLine(body);
        }

        public void AddContentLine(string formatbody, params object[] values)
        {
            builder.AppendFormat(formatbody, values);
            builder.AppendLine();
        }
        #endregion

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="content_type">HttpSession 常量字符调用</param>
        public void Respons(string content_type = HttpSession.HTTP_CONTENT_All)
        {
            OutputStream.WriteLine("HTTP/1.0 200 OK");
            OutputStream.WriteLine("Content-Type: " + content_type);
            OutputStream.WriteLine("Connection: close");
            OutputStream.WriteLine("");
            OutputStream.Write(builder.ToString());
            this.Close();
        }

        /// <summary>
        /// 发送404错误
        /// </summary>
        /// <param name="msg">输出错误信息</param>
        /// <param name="content_type">HttpSession 常量字符调用</param>
        public void Respons404(string msg = "路由错误", string content_type = HttpSession.HTTP_CONTENT_All)
        {
            OutputStream.WriteLine("HTTP/1.0 200 OK");
            OutputStream.WriteLine("Content-Type: " + content_type);
            OutputStream.WriteLine("Connection: close");
            OutputStream.WriteLine("");
            OutputStream.Write(msg);
            this.Close();
        }
    }
}
