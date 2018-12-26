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
        public string[] valid_extensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif" }; //NOTE: in future maybe support videos?
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
                foreach (FileInfo f in fs) if(IsValidExtension(f.Extension)) files.Add(f.FullName);
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
                string recvip = context.Request.RemoteEndPoint.Address.ToString();
                if (raw[0] == "/favicon.ico") return;
                MainActivity.handler.AddText($"Request: '{ RemFirstCh(raw[0].Substring(raw[0].LastIndexOf('/'), raw[0].Length - raw[0].LastIndexOf('/'))) }' from { recvip }");
                context.Response.ContentEncoding = context.Request.ContentEncoding;
                if (raw[0] == "/album.view")
                {
                    string html_part1 = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"></head><body style=\"width:100%;height:100%;margin: 0 auto;\">";
                    string html_part2 = "</body></html>";
                    string site = html_part1;
                    foreach(string f in files)
                    {
                        string ip = Utilities.GetLocalIP();
                        if (!AreIPsLocal(ip, recvip)) ip = Utilities.GetPublicIP();
                        string url = "http://" + ip + ":8080" + f;
                        string pic = "<div style=\"height:80%;width:80%;margin: 0 auto;text-align:center;\"><img src=\"" + url + "\" style=\"height:100%;width:100%;margin: 0 auto;\"></div>";
                        site += pic;
                    }
                    site += html_part2;
                    byte[] output = System.Text.Encoding.ASCII.GetBytes(site);
                    context.Response.ContentType = MIME.GetMimeType(".html");
                    context.Response.OutputStream.Write(output, 0, output.Length);
                }
                else
                {
                    string path = files.Find(f => f.ToString().Contains(raw[0]));
                    FileStream fstream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    int numb = (int)(new FileInfo(path).Length);
                    BinaryReader br = new BinaryReader(fstream);
                    byte[] output = br.ReadBytes(numb);
                    context.Response.ContentType = MIME.GetMimeType(Path.GetExtension(raw[0]));
                    context.Response.OutputStream.Write(output, 0, numb);
                }
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

        private bool IsValidExtension(string extension)
        {
            for (int i = 0; i < valid_extensions.Length; i++) if (valid_extensions[i].ToLowerInvariant() == extension.ToLowerInvariant()) return true;
            return false;
        }

        private bool AreIPsLocal(string ip1, string ip2)
        {
            string iprange1 = ip1.Split('.')[0] + "." + ip1.Split('.')[1];
            string iprange2 = ip2.Split('.')[0] + "." + ip2.Split('.')[1];
            if (iprange1 == iprange2) return true;
            else return false;
        }
    }
}
