using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace RemoteWebServer
{
    public partial class HttpServer
    {
        public static List<string> files = new List<string>();
        public static string DownloadPath = null;
        private HttpListener m_listener;

        public HttpServer(int port = 8080)
        {
            try
            {
                string directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
                string start = Path.Combine(directory, "Website");
                if (Directory.Exists(start))
                {
                    DownloadPath = start;
                    LoadWebsiteFiles(start);
                    m_listener = new HttpListener();
                    m_listener.Prefixes.Add("http://*:" + port + "/");
                }
                else MainActivity.handler.AddText("'Download/Website' folder does not exist!");
            }
            catch
            {
                return;
            }
        }

        public void LoadWebsiteFiles(string start)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(start);
                DirectoryInfo[] dirs = dir.GetDirectories();
                FileInfo[] fs = dir.GetFiles();
                foreach (FileInfo f in fs) files.Add(f.ToString());
                if (dirs.Length > 0) foreach (DirectoryInfo d in dirs) LoadWebsiteFiles(d.ToString());
            }
            catch
            {
                return;
            }
        }

        public void Start()
        {
            m_listener.Start();
            MainActivity.handler.AddText("Server started");
            m_listener.BeginGetContext(OnGetContext, null);
        }

        public void Stop()
        {
            m_listener.Stop();
            MainActivity.handler.AddText("Server stopped");
        }

        private void OnGetContext(IAsyncResult result)
        {
            try
            {
                var context = m_listener.EndGetContext(result);
                ThreadPool.QueueUserWorkItem(HandleRequest, context);
            }
            finally
            {
                m_listener.BeginGetContext(OnGetContext, null);
            }
        }

        private void HandleRequest(object oContext)
        {
            HttpListenerContext context = (HttpListenerContext)oContext;
            try
            {
                string[] raw = context.Request.RawUrl.Split('&');
                if (raw[0] == "/favicon.ico") return;
                context.Response.ContentEncoding = context.Request.ContentEncoding;
                context.Response.ContentType = MIME.GetMimeType(Path.GetExtension(raw[0]));
                MainActivity.handler.AddText($"Request: '{raw[0]}' from '{context.Request.RemoteEndPoint.Address.ToString()}'");
                string path = files.Find(f => f.ToString().Substring(DownloadPath.Length) == raw[0]);
                int numb = (int)(new FileInfo(path).Length);
                FileStream fstream = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fstream);
                byte[] output = br.ReadBytes(numb);
                context.Response.OutputStream.Write(output, 0, numb);
            }
            catch
            {
                return;
            }
            finally
            {
                context.Response.Close();
            }
        }
    }
}
