using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    public abstract class OdeDebug : IDisposable
    {
        [Header("Print")]
        public bool showInfo = false;

        public bool Disposed { get { return disposed; } }

        private bool disposed = false;
        private uint counter = 0;
        private object sync = new object();

        protected enum LogType { info, warning, error };
        protected Encoding encoding = Encoding.UTF8;

        public void LogInfo(string message)
        {
            if (showInfo)
                Debug.Log(Log(LogType.info, message));
            else
                Log(LogType.info, message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(Log(LogType.warning, message));
        }

        public void LogError(string message)
        {
            Debug.LogError(Log(LogType.error, message));
        }

        public void Dispose()
        {
            lock (sync)
            {
                if (disposed)
                    return;

                disposed = true;
                Free();
            }
        }

        protected abstract void LogEcho(string message);
        protected virtual void Free() { }

        private string Log(LogType tp, string message)
        {
            string text = "";
            lock (sync)
            {
                if (disposed)
                    return "Class has already been disposed!";

                counter++;
                text = string.Format("[{0}. {1:MM/dd HH:mm:ss.fff}] [{2}] [{3}]\r\n", counter, DateTime.Now, tp, message);
                LogEcho(text);
            }

            return text;
        }
    }

    public class OdeDebugNone : OdeDebug { protected override void LogEcho(string message) { } }

    public class OdeDebugRemote : OdeDebug
    {
        private const int timeOut = 1000;

        private TcpClient client;
        private string ip;
        private UInt16 port;
        private bool fail = false;

        public OdeDebugRemote(string address = "127.0.0.1:10137")
        {
            client = new TcpClient();
            client.SendTimeout = timeOut;
            client.ReceiveTimeout = timeOut;

            try
            {
                var addr = address.Trim().Split(new char[] { ':' });
                if (addr.Length != 2 || UInt16.TryParse(addr[1], out port) == false)
                    throw new Exception("Invalid address.");
                ip = addr[0];

                IAsyncResult ar = client.BeginConnect(ip, port, null, null);
                System.Threading.WaitHandle wh = ar.AsyncWaitHandle;
                try
                {
                    if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeOut), false))
                    {
                        client.Close();
                        throw new TimeoutException();
                    }
                    client.EndConnect(ar);
                }
                finally
                {
                    wh.Close();
                }
            }
            catch (Exception ex)
            {
                fail = true;
                Debug.LogWarning(string.Format("OdeDebug: \"{0}\"", ex.Message));
            }
        }

        protected override void LogEcho(string message)
        {
            if (fail)
                return;

            try
            {
                client.Client.Send(encoding.GetBytes(message));
            }
            catch (Exception ex)
            {
                fail = true;
                Debug.LogWarning(string.Format("OdeDebug: \"{0}\"", ex.Message));
            }
        }

        protected override void Free()
        {
            try
            {
                if (client != null && client.Connected)
                    client.Close();
            }
            finally { }
        }

    }

    public class OdeDebugFile : OdeDebug
    {
        private FileStream fs;
        private StreamWriter writer;
        private bool fail = false;

        public OdeDebugFile(string file = "ode.log")
        {
            try
            {
                fs = new FileStream(file, FileMode.Create);
                writer = new StreamWriter(fs, encoding);
            }
            catch (Exception ex)
            {
                fail = true;
                Debug.LogWarning(string.Format("OdeDebug: \"{0}\"", ex.Message));
            }
        }

        protected override void LogEcho(string message)
        {
            if (fail)
                return;

            try
            {
                writer.Write(message);
            }
            catch (Exception ex)
            {
                fail = true;
                Debug.LogWarning(string.Format("OdeDebug: \"{0}\"", ex.Message));
            }
        }

        protected override void Free()
        {
            try
            {
                if (writer != null)
                {
                    writer.Dispose();
                    writer = null;
                }
                if (fs != null)
                {
                    fs.Dispose();
                    fs = null;
                }
            }
            finally { }
        }
    }
}