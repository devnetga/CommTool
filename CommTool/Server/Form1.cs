using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogFile.WriteLog("OnStart");

            //  Create the sub working thread...
            Thread thread_handler = new Thread(this.StartSocket);
            thread_handler.IsBackground = true;
            thread_handler.Start();
        }

        static TcpListener server = null;
        #region StartSocket
        public void StartSocket()
        {
            LogFile.WriteLog("StartSocket");

            try
            {
                //   set the TcpListener on port = 14000
                Int32 port = 14000;

                //  create the server socket & start
                server = new TcpListener(IPAddress.Any, port);
                server.Start();

                //  buffer for reading data
                byte[] bytes = new byte[2014];
                string data = string.Empty;

                //  waiting for a client connection
                while (true)
                {
                    LogFile.WriteLog("Waiting for a connection....");

                    TcpClient client = server.AcceptTcpClient();
                    LogFile.WriteLog("Connected!");

                    NetworkStream stream = client.GetStream();

                    // read the message from client and send the message to client
                    int bytesRead = 0;
                    while ((bytesRead = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                        LogFile.WriteLog(string.Format("Received : {0}", data));

                        data = data.ToUpper();

                        byte[] msg = Encoding.UTF8.GetBytes(data);

                        stream.Write(msg, 0, msg.Length);
                        LogFile.WriteLog(string.Format("Sent : {0}", data));
                    }

                    client.Close();
                }

            }
            catch (SocketException se)
            {
                LogFile.WriteLog(se.Message, LogFile.LogCode.Error);
            }
            catch (Exception ex)
            {
                LogFile.WriteLog(ex.Message, LogFile.LogCode.Error);
            }
            finally
            {
                server.Stop();
            }
        }
        #endregion

        #region StopSocket
        public void StopSocket()
        {
            try
            {
                if (server != null)
                    server.Stop();
            }
            catch (SocketException se)
            {
                LogFile.WriteLog(se.Message, LogFile.LogCode.Error);
            }
            catch (Exception ex)
            {
                LogFile.WriteLog(ex.Message, LogFile.LogCode.Error);
            }
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogFile.WriteLog("OnStop");

            this.StopSocket();
        }
    }
}
