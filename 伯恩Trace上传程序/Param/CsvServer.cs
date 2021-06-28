
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Global
{
    public class CsvServer
    {
        private Thread _thread;
        private ConcurrentQueue<CsvInfo> queue = new ConcurrentQueue<CsvInfo>();
        private readonly static CsvServer instance = new CsvServer();
        public object obj = new object();
        CsvServer() { Start(); }
        public static CsvServer Instance
        {
            get { return instance; }
        }

        public void Start()
        {
            Stop();
            _thread = new Thread(new ThreadStart(ProcessEventQueue));
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void Stop()
        {
            if (this._thread != null)
            {
                this._thread.Abort();
            }
        }
        private void Kill()//20170327 XSF
        {
            Process[] process = Process.GetProcesses();
            foreach (Process p in process)
            {
                if (p.ProcessName.ToUpper() == "ET")
                {
                    p.CloseMainWindow();
                    p.WaitForExit();
                }
            }
        }
        private void ProcessEventQueue()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    CsvInfo csvInfo;
                    queue.TryDequeue(out csvInfo);
                    try
                    {
                        lock (obj)
                        {
                            //Kill();//20170327 XSF
                            //StreamWriter sw = File.AppendText(csvInfo.Path);
                            FileInfo fi = new FileInfo(csvInfo.Path);
                            if (!fi.Directory.Exists)
                            {
                                fi.Directory.Create();
                            }
                            FileStream fs = new FileStream(csvInfo.Path, FileMode.Append, FileAccess.Write);
                            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                            sw.WriteLine(csvInfo.Line);
                            sw.Dispose();
                            fs.Dispose();                            
                        }
                    }
                    catch
                    {
                        queue.Enqueue(csvInfo);
                    }
                }
                Thread.Sleep(20);
            }
        }

        public void WriteLine(string path, string line)
        {
            CsvInfo csvInfo = new CsvInfo();
            csvInfo.Path = path;
            csvInfo.Line = line;
            queue.Enqueue(csvInfo);
        }
    }

    public class CsvInfo
    {
        public string Path { get; set; }
        public string Line { get; set; }
    }
}
