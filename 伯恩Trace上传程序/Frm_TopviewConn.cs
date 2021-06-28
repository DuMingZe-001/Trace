using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.tlsvision.soft;
using System.IO;
using ProFile;
using System.Threading;

namespace 伯恩Trace上传程序
{
    public partial class Frm_TopviewConn : Form
    {
        #region 变量的定义
        public CSocketClient m_sockClient = null;
        private CSocketClient.ReceiveCallBack m_recvCallBack = null;
        string str_Path;
        public ProFile.CProfIniFile m_ProfINIFile;
     


        //定义回调
        private delegate void SetTextCallBack(string strValue);
        private SetTextCallBack setCallBack;


        #endregion

        public Frm_TopviewConn()
        {
            InitializeComponent(); 
            setCallBack = ReceiveMsg;
            m_sockClient = new CSocketClient();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (GlobalVar.m_Tcp == false)
            {
                m_recvCallBack = ClientRecv;
                //初始化服务并/注册回调
                m_sockClient.InitService(txt_ServerIP.Text, txtServerPort.Text, m_recvCallBack);
            }

            GlobalVar.m_Tcp = true;
        }
        private void ClientRecv(byte[] byteMsg, int nLen)
        {
            string strReceiveMsg = "";

            if (byteMsg[0] == 0)
            {
                string str = Encoding.Default.GetString(byteMsg, 1, nLen - 1);
                strReceiveMsg = "接收：服务端发送的消息:" + str;
                this.txt_Received.Invoke(setCallBack, strReceiveMsg);
            }
            else if (byteMsg[0] == 1)
            {
                SaveFile(byteMsg, nLen - 1);
            }
            else
            {
                string str = Encoding.Default.GetString(byteMsg, 0, nLen);
                str = Encoding.Default.GetString(byteMsg, 0, nLen);
                strReceiveMsg = "接收：服务端发送的消息:" + str;
                this.txt_Received.Invoke(setCallBack, strReceiveMsg);
            }
        }

        private void SaveFile(byte[] buffer, int nCount)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"";
            sfd.Title = "请选择要保存的文件";
            sfd.Filter = "所有文件|*.*";
            sfd.ShowDialog(this);

            string strPath = sfd.FileName;
            using (FileStream fsWrite = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fsWrite.Write(buffer, 1, nCount - 1);
            }

            MessageBox.Show("保存文件成功");
            this.txt_Received.Invoke(setCallBack, "接收远程服务器:" + m_sockClient.GetServerIP() + "发送的文件：" + sfd.FileName);
        }

        private void ReceiveMsg(string strMsg)
        {
            this.txt_Received.AppendText(strMsg + " \r \n");
        }

        private void btn_SendData_Click(object sender, EventArgs e)
        {
            try
            {
                string strMsg = this.txt_SendData.Text.Trim();
                m_sockClient.SendMsg(strMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送消息出错:" + ex.Message);

            }
        }

     

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            m_sockClient.CloseService();
        }

        private void btn_ConnSave_Click(object sender, EventArgs e)
        {
            str_Path = System.Environment.CurrentDirectory + @"\TraceSetting.ini";
            m_ProfINIFile = new CProfIniFile(str_Path);
            m_ProfINIFile.Write("网路通信","服务器IP", txt_ServerIP.Text);
            m_ProfINIFile.Write("网路通信", "服务器端口号",txtServerPort.Text);
        }

        private void btn_ConnRead_Click(object sender, EventArgs e)
        {
            str_Path = System.Environment.CurrentDirectory + @"\TraceSetting.ini";
            m_ProfINIFile = new CProfIniFile(str_Path);
            txt_ServerIP.Text= m_ProfINIFile.Read("网路通信", "服务器IP", "");
            txtServerPort.Text= m_ProfINIFile.Read("网路通信", "服务器端口号", "");
        }

        private void Frm_TopviewConn_Load(object sender, EventArgs e)
        {
            str_Path = System.Environment.CurrentDirectory + @"\TraceSetting.ini";
            m_ProfINIFile = new CProfIniFile(str_Path);
            txt_ServerIP.Text = m_ProfINIFile.Read("网路通信", "服务器IP", "");
            txtServerPort.Text = m_ProfINIFile.Read("网路通信", "服务器端口号", "");

        }
    }

    public sealed class XCGMotion
    {
        static Frm_TopviewConn instance = null;
        static readonly object padlock = new object();
        XCGMotion()
        {
        }

        public static Frm_TopviewConn Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Frm_TopviewConn();
                        }
                    }
                }
                return instance;
            }
        }
    }
}
