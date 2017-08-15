using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace WebAlbumViewer
{
    public partial class HttpServer
    {
        public List<string> files;
        public string directory;
        private HttpListener m_listener;

        public HttpServer(int port = 8080)
        {
            try
            {
                files = new List<string>();
                m_listener = new HttpListener();
                m_listener.Prefixes.Add("http://*:" + port + "/");
                directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDcim);
                if (Directory.Exists(directory + "/100ANDRO")) directory += "/100ANDRO";
                DirectoryInfo dir = new DirectoryInfo(directory);
                FileInfo[] fs = dir.GetFiles();
                foreach (FileInfo f in fs) files.Add(f.FullName);
                MainActivity.handler.AddText($"Found {files.Count} media files");
            }
            catch
            {
                return;
            }
        }

        public void Start()
        {
            m_listener.Start();
            MainActivity.handler.AddText("HTTP server started");
            m_listener.BeginGetContext(OnGetContext, null);
        }

        public void Stop()
        {
            m_listener.Stop();
            files.Clear();
            MainActivity.handler.AddText("HTTP server stopped");
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
                MainActivity.handler.AddText($"Request: '{ RemFirstCh(raw[0].Substring(raw[0].LastIndexOf('/'), raw[0].Length - raw[0].LastIndexOf('/'))) }' from {context.Request.RemoteEndPoint.Address.ToString()}");
                context.Response.ContentEncoding = context.Request.ContentEncoding;
                context.Response.ContentType = MIME.GetMimeType(Path.GetExtension(raw[0]));
                string path = files.Find(f => f.ToString().Contains(raw[0]));
                FileStream fstream = new FileStream(path, FileMode.Open, FileAccess.Read);
                int numb = (int)(new FileInfo(path).Length);
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

        private string RemFirstCh(string str)
        {
            string newstr = null;
            foreach (char c in str) if (c != '/') newstr += c;
            return newstr;
        }
    }
}
