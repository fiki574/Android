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
            m_listener.BeginGetContext(OnGetContext, null);
        }

        public void Stop()
        {
            m_listener.Stop();
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
                string path = files.Find(f => f.ToString().Substring(DownloadPath.Length) == raw[0]);
                using (StreamReader sr = new StreamReader(File.OpenRead(path)))
                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, context.Response.ContentEncoding)) writer.Write(sr.ReadToEnd());
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
