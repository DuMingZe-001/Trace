using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProFile;
using com.tlsvision.soft;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Global;
using GlobalServer;
using CSVParam;
using VisionSystem;
using System.Net;

namespace 伯恩Trace上传程序
{
    public partial class frm_main : Form
    {

        Frm_TopviewConn frm_TopviewConn = null;
        frm_BarCodeSetting frm_barcode = null;
        string str_Path2 = System.Environment.CurrentDirectory + @"\TraceSetting.ini";
        public ProFile.CProfIniFile m_ProfINIFile;
        //   public static string Path = Application.StartupPath + "\\";
        public static string Path = Application.StartupPath + "\\LOG" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
        public static string LogPath = Path + "\\" + "Log.log";

        public static string Path2 = Application.StartupPath + "\\CSV" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
        public static string DataPath = Path2 + "\\" + "Data.csv";

        public static string Path3 = Application.StartupPath + "\\计数清零记录" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
        public static string DataPath3 = Path3 + "\\" + "ZeroCount.csv";
        private static object objLock = new object();
        private string json;//Post请求返回Json数据
        Root root = new Root();
        private AsyncTcpClient client; //TCP客户端对象
        private AsyncTcpServer tcpServer;//TC服务端对象
        private bool m_tcp = false;//TCP客户端、服务端开关选择


        public frm_main()
        {
            InitializeComponent();

        }

        private void btn_TopViewConn_Click(object sender, EventArgs e)
        {
            if (frm_TopviewConn == null || frm_TopviewConn.IsDisposed)
            {
                frm_TopviewConn = new Frm_TopviewConn();
                frm_TopviewConn.Show();
            }
            else
            {
                frm_TopviewConn.Activate();
            }
        }

        private void btn_BarCodeSetting_Click(object sender, EventArgs e)
        {

            if (frm_barcode == null || frm_barcode.IsDisposed)
            {

                frm_barcode = new frm_BarCodeSetting();
                frm_barcode.Show();
            }
            else
            {
                frm_barcode.Activate();
            }
        }

        private void btn_Stat_Click(object sender, EventArgs e)
        {
            if (GlobalVar.IsAutoRunning == false)
            {
                if (GlobalVar.contain == true)
                {
                    try
                    {
                        if (m_tcp == true)  //Trace做客户端
                        {
                            m_ProfINIFile = new CProfIniFile(str_Path2);
                            IPAddress ip;
                            bool b = IPAddress.TryParse(m_ProfINIFile.Read("网路通信", "服务器IP", ""), out ip);
                            client = new AsyncTcpClient(ip, Convert.ToInt32(m_ProfINIFile.Read("网路通信", "服务器端口号", "")));
                            client.PlaintextReceived += Client_PlaintextReceived;
                            client.ServerConnected += Client_ServerConnected;
                            client.ServerDisconnected += Client_ServerDisconnected;
                            client.Connect();
                        }
                        else //Trace做服务器端
                        {
                            m_ProfINIFile = new CProfIniFile(str_Path2);
                            IPAddress ipAddress;
                            IPAddress.TryParse(m_ProfINIFile.Read("网路通信", "服务器IP", ""), out ipAddress);
                            int port;
                            port = Convert.ToInt32(m_ProfINIFile.Read("网路通信", "服务器端口号", ""));
                            tcpServer = new AsyncTcpServer(ipAddress, port);
                            tcpServer.PlaintextReceived += TcpServer_PlaintextReceived;
                            tcpServer.ClientConnected += TcpServer_ClientConnected; ;
                            tcpServer.ClientDisconnected += TcpServer_ClientDisconnected;
                            tcpServer.Start();
                        }

                    }
                    catch (Exception ex)
                    {
                        AddToLsbLog(ex.ToString());
                    }

                    GlobalVar.IsAutoRunning = true;
                    btn_Close.Enabled = true;
                    btn_Stat.Enabled = false;
                }
                else
                {
                    MessageBox.Show("用户等级低");
                }

            }

        }



        #region  网络通信   

        //tcp客户端连接
        private void TcpServer_ClientConnected(object sender, GlobalServer.TcpClientConnectedEventArgs e)
        {
            AddToTxt4Log("TopView通讯正常！" + "远程主机：" + e.TcpClient.Client.LocalEndPoint.ToString() + "连接成功！");
            label4.BackColor = Color.Green;
        }
        //tcp接收到信息
        private void TcpServer_PlaintextReceived(object sender, GlobalServer.TcpDatagramReceivedEventArgs<string> e)
        {

            Thread GetTraceTest = new Thread(() =>
            {
                try
                {
                    m_ProfINIFile = new CProfIniFile(str_Path2);
                    //获取当前系统时间
                    GlobalVar.uutStarTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //进行Biel服务器请求
                    if (Convert.ToBoolean(m_ProfINIFile.Read("参数设置", "checkBox2.Checked", "")) == true)
                    {

                        if (e.Datagram != null && e.Datagram != "" && e.Datagram.Substring(0, 3) == "GET")
                        {

                            AddToTxt3Log("进行条码自校验++++");
                            string m_bielcodeflag = e.Datagram.Substring(0, 3);//解析Biel系统Get请求白片码标识符
                            string m_bielcoderesult = e.Datagram.Substring(4, 17);//解析Brel系统Get请求白片码
                            if (m_bielcoderesult.Length == 17 && m_bielcodeflag == "GET")  //获取到白片码信息(Get请求)
                            {

                                AddToTxt3Log("自校验接收到白片码信息-->" + m_bielcoderesult);
                                //Get请求后服务器返回数据
                                GlobalVar.m_getresultdata = GetBielQuest(m_bielcoderesult);
                                if (GlobalVar.m_getresultdata != "404" && GlobalVar.m_getresultdata != "400" && GlobalVar.m_getresultdata != "504" && GlobalVar.m_getresultdata == "0")
                                {
                                    // tcpServer.SendAll("B" + m_getresultdata + "C");
                                    AddToTxt3Log("自校验结果成功+" + "返回结果-->" + GlobalVar.m_getresultdata);
                                    this.BeginInvoke(new Action(() =>
                                    {
                                        label2.Text = "自校验成功";
                                        label2.BackColor = Color.Green;
                                        label3.BackColor = Color.Green;
                                    }));
                                }
                                else if (GlobalVar.m_getresultdata == "1" || GlobalVar.m_getresultdata == "-1")
                                {

                                    this.BeginInvoke(new Action(() =>
                                    {
                                        label3.BackColor = Color.Red;
                                        label2.Text = "自校验失败";
                                        label2.BackColor = Color.Red;
                                        AddToTxt3Log("自校验返回结果：-->" + GlobalVar.m_getresultdata);
                                        tcpServer.SendAll("NG" + "," + "none" + ",");//biel请求校验结果NG
                                        GlobalVar.NGNum++;
                                        txt_NgNum.Text = Convert.ToString(GlobalVar.NGNum);
                                        GlobalVar.AllNum = GlobalVar.OKNum + GlobalVar.NGNum;
                                        txt_AllNum.Text = Convert.ToString(GlobalVar.OKNum + GlobalVar.NGNum);
                                    }));

                                }
                                else
                                {

                                    this.BeginInvoke(new Action(() =>
                                    {
                                        label3.BackColor = Color.Red;
                                        label2.Text = "自校验失败";
                                        label2.BackColor = Color.Red;
                                        AddToTxt3Log("自校验返回结果：-->" + GlobalVar.m_getresultdata);
                                        tcpServer.SendAll("ER" + "," + "none" + ",");//biel请求校验结果报错
                                        GlobalVar.NGNum++;
                                        txt_NgNum.Text = Convert.ToString(GlobalVar.NGNum);
                                        GlobalVar.AllNum = GlobalVar.OKNum + GlobalVar.NGNum;
                                        txt_AllNum.Text = Convert.ToString(GlobalVar.OKNum + GlobalVar.NGNum);
                                    }));
                                }
                            }
                            //if (GlobalVar.m_getresultdata == "1" || GlobalVar.m_getresultdata == "-1")
                            //{
                            //    this.BeginInvoke(new Action(() =>
                            //    {
                            //        label3.BackColor = Color.Red;
                            //        label2.Text = "Biel上传失败";
                            //        label2.BackColor = Color.Red;
                            //        AddToTxt3Log("Biel上传返回结果：-->" + GlobalVar.m_getresultdata);
                            //        tcpServer.SendAll("NG" + "none" + ",");//biel请求校验结果NG
                            //        GlobalVar.NGNum++;
                            //        txt_NgNum.Text = Convert.ToString(GlobalVar.NGNum);
                            //        GlobalVar.AllNum = GlobalVar.OKNum + GlobalVar.NGNum;
                            //        txt_AllNum.Text = Convert.ToString(GlobalVar.OKNum + GlobalVar.NGNum);
                            //    }));

                            //}
                        }
                    }
                    else
                    {
                        HttpUitls.m_getstatusdescription = "OK";
                        GlobalVar.m_getresultdata = "0";
                    }
                    if (Convert.ToBoolean(m_ProfINIFile.Read("参数设置", "checkBox1.Checked", "")) == true)
                    {
                        if (HttpUitls.m_getstatusdescription == "OK" && GlobalVar.m_getresultdata == "0" && e.Datagram.Substring(0, 3) == "GET")//Biel服务器上传成功，O为非F料
                        {
                            AddToTxt3Log("进行trace服务器GET请求++++");
                            //进行trace服务器请求
                            label3.BackColor = Color.Green;
                            string m_getresultdata = GetTraceQuest(e.Datagram.Substring(4, 17));
                            //对Trace服务器数据进行解析
                            if (m_getresultdata != "404" && m_getresultdata != "400" && m_getresultdata != "504" && m_getresultdata != "502" && m_getresultdata != "ER")
                            {
                                Root2 rt = JsonConvert.DeserializeObject<Root2>(m_getresultdata);
                                GlobalVar.getTraceJson = rt.pass;//解析true或者false
                            }
                            if (GlobalVar.getTraceJson == true && HttpUitls.m_getstatusdescription == "OK")
                            {
                                // label2.Text = "Trace卡站成功";
                                AddToLbl2("Trace卡站成功", Color.Green);
                                tcpServer.SendAll("OK" + "," + GlobalVar.uutStarTime + ",");//返回结果发送机器人
                                AddToTxt3Log("Trace卡站成功" + "," + "Trace卡站结果发送机器人成功++++");
                                AddToTxt3Log("Trace卡站返回结果：-->" + m_getresultdata);
                            }
                            else if (GlobalVar.getTraceJson == false && HttpUitls.m_getstatusdescription == "OK")
                            {
                                //label2.Text = "Trace卡站失败";
                                AddToLbl2("Trace卡站失败", Color.Red);
                                AddToTxt3Log("Trace卡站失败");
                                AddToTxt3Log("Trace卡站返回结果：-->" + m_getresultdata);
                                label3.BackColor = Color.Red;
                                tcpServer.SendAll("NG" + "," + "none" + ",");
                                this.BeginInvoke(new Action(() =>
                                {
                                    GlobalVar.NGNum++;
                                    txt_NgNum.Text = Convert.ToString(GlobalVar.NGNum);
                                    GlobalVar.AllNum = GlobalVar.OKNum + GlobalVar.NGNum;
                                    txt_AllNum.Text = Convert.ToString(GlobalVar.OKNum + GlobalVar.NGNum);
                                }));
                            }
                            else if (m_getresultdata == "404" || m_getresultdata == "400" || m_getresultdata == "504" || m_getresultdata == "502" || m_getresultdata == "ER")
                            {
                                AddToLbl2("Trace卡站失败", Color.Red);
                                AddToTxt3Log("Trace卡站失败");
                                AddToTxt3Log("Trace卡站返回结果：-->" + m_getresultdata);
                                label3.BackColor = Color.Red;
                                tcpServer.SendAll("ER" + "," + "none" + ",");
                                this.BeginInvoke(new Action(() =>
                                {
                                    GlobalVar.NGNum++;
                                    txt_NgNum.Text = Convert.ToString(GlobalVar.NGNum);
                                    GlobalVar.AllNum = GlobalVar.OKNum + GlobalVar.NGNum;
                                    txt_AllNum.Text = Convert.ToString(GlobalVar.OKNum + GlobalVar.NGNum);
                                }));

                            }
                            else { }

                        }
                    }
                    else
                    {
                        if (e.Datagram.Substring(0, 3) == "GET")
                        {
                            AddToTxt3Log("Trace卡站成功++++");
                            AddToLbl2("Trace卡站成功", Color.Green);
                            label3.BackColor = Color.Green;
                            tcpServer.SendAll("OK" + "," + GlobalVar.uutStarTime + ",");//返回结果发送机器人
                        }

                    }
                }
                catch (Exception ex)
                {
                    AddToLsbLog(ex.ToString());
                }
            });
            GetTraceTest.IsBackground = true;
            GetTraceTest.Start();

            Thread PostTest = new Thread(() =>
            {
                try
                {
                    if (Convert.ToBoolean(m_ProfINIFile.Read("参数设置", "checkBox1.Checked", "")) == true)
                    {

                        if (e.Datagram != null && e.Datagram != "")
                        {
                            //解析Post请求
                            if (e.Datagram.Length > 18 && e.Datagram.Substring(0, 4) == "POST")  //获取到架子码及两白片码信息，Post请求
                            {

                                AddToTxt3Log("进行trace服务器POST请求++++");
                                //解析Psot系统请求白片码标识符
                                //string postCodeFlag = e.Datagram.Substring(0, 4);

                                root.data = new Data();
                                root.serials = new List<string>();
                                root.data.insight = new Insight();
                                root.data.insight.test_attributes = new TestAttributes();
                                root.data.insight.test_station_attributes = new TestStationAttributes();
                                root.data.insight.uut_attributes = new UutAttributes();
                                root.data.properties = new Properties2();
                                //解析白片码
                                string[] strs = e.Datagram.Split(new string[] { ",", "(", ")", "POST", "bg_ss", "rack_id", "uut_start", }, StringSplitOptions.RemoveEmptyEntries);
                                string m_strs1 = strs[0];//bg_ss码1
                                string m_strs2 = strs[1];//bg_ss码2
                                string m_strs3 = strs[2];//rack_id码
                                string m_str4 = strs[3];//uut_start时间
                                string m_str5 = strs[4];//区分AB左右框

                                if (GlobalVar.resultUnitSerialNumber == 0)
                                {
                                    GlobalVar.unitSerialNumber = m_strs1;//将第一片玻璃的条码信息赋值
                                    GlobalVar.resultUnitSerialNumber = 2;
                                }
                                if (GlobalVar.resultUnitSerialNumberrSec == 0)
                                {
                                    GlobalVar.unitSerialNumberSec = m_strs2;//将第二片玻璃的条码信息赋值
                                    GlobalVar.resultUnitSerialNumberrSec = 2;
                                }
                                if (m_strs1 != "NG" && m_strs2 != "NG")
                                {
                                    //root.serials.bg_ss = m_strs1 + "," + m_strs2;//白片码属性
                                    root.serials.AddRange(new string[2] { m_strs1, m_strs2 });
                                 
                                }
                                else if (m_strs1 == "NG" && m_strs2 != "NG")
                                {
                                    //root.serials.bg_ss = "" + "," + "m_strs2";
                                    root.serials.AddRange(new string[1] { m_strs2 });
                                 
                                }
                                else if (m_strs1 != "NG" && m_strs2 == "NG")
                                {
                                    // root.serials.bg_ss = "m_strs1" + "," + "";
                                    root.serials.AddRange(new string[1] { m_strs1 });
                             
                                }
                                else
                                {
                                    // root.serials.bg_ss = "" + "," + "";
                                    // root.serials.AddRange(new string[2] { "", "" });
                                }

                                AddToTxt3Log("POST接收到机器人信号-->" + e.Datagram);

                                // m_ProfINIFile = new CProfIniFile(str_Path2);
                                root.data.insight.test_attributes.test_result = "pass";//测试结果，发送                   
                                root.serial_type = "bg_ss";
                                if (GlobalVar.unitSerialNumber != "NG")
                                {
                                    root.data.insight.test_attributes.unit_serial_number = GlobalVar.unitSerialNumber;
                                }
                                if (GlobalVar.unitSerialNumberSec != "NG")
                                {
                                    root.data.insight.test_attributes.unit_serial_number = GlobalVar.unitSerialNumberSec;
                                }
                                root.data.insight.test_attributes.uut_start = m_str4;//条码扫码开始时间                                                                                              
                                string uutStopTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");  //获取系统当前时间
                                root.data.insight.test_attributes.uut_stop = uutStopTime;//条码扫码结束时间
                                root.data.insight.test_station_attributes.fixture_id = m_ProfINIFile.Read("参数设置", "fixture_id", "");//夹具号可配置
                                root.data.insight.test_station_attributes.head_id = m_ProfINIFile.Read("参数设置", "head_id", "");
                                root.data.insight.test_station_attributes.line_id = m_ProfINIFile.Read("参数设置", "line_id", "");//可配置
                                root.data.insight.test_station_attributes.software_name = m_ProfINIFile.Read("参数设置", "software_name", "");
                                root.data.insight.test_station_attributes.software_version = m_ProfINIFile.Read("参数设置", "software_version", "");
                                root.data.insight.test_station_attributes.station_id = m_ProfINIFile.Read("参数设置", "station_id", "");//可配置
                                root.data.insight.uut_attributes.STATION_STRING = m_ProfINIFile.Read("参数设置", "STATION_STRING", "");
                                root.data.insight.uut_attributes.rack_id = m_strs3;//架子码
                                root.data.properties.Factory = m_ProfINIFile.Read("参数设置", "Factory", "");//可配置

                                json = JsonConvert.SerializeObject(root);
                                //string url = "http://10.227.102.15:8765/v2/log?";//Post请求url----单片
                                string url = "http://localhost:8765/v2/log_batch?";//Post请求url----两片
                                GlobalVar.getJson = HttpUitls.Post(url, json, "");

                                //解析Json
                                //Root2 rt = JsonConvert.DeserializeObject<Root2>(GlobalVar.getJson);
                                //bool getJson2 = rt.pass;//解析true或者false
                                //发送解析的数据
                                if (HttpUitls.m_poststatusDescription == "OK")
                                {
                                    label3.BackColor = Color.Green;
                                    tcpServer.SendAll("OK");
                                    AddToTxt3DebugModeLog("Post上传成功：-->" + "OK" + ",SN:" + m_strs1 + "," + m_strs2 + ",Machine info:" + m_ProfINIFile.Read("网路通信", "Trace服务器IP", "") + ",Result:" + "PASS" + ",Log Id:" + HttpUitls.m_poststatusDescription);
                                    AddToTxt3Log("接收POST返回结果-->" + GlobalVar.getJson);
                                    AddToLbl2("Post上传成功", Color.Green);
                                    this.BeginInvoke(new Action(() =>
                                    {

                                        if (m_strs1 != "NG" && m_strs2 != "NG")
                                        {
                                            switch (m_str5)
                                            {
                                                case "A":
                                                    GlobalVar.insertNumA += 2;
                                                    txt_InsertNumA.Text = Convert.ToString(GlobalVar.insertNumA);
                                                    break;
                                                case "B":
                                                    GlobalVar.insertNumB += 2;
                                                    txt_InsertNumB.Text = Convert.ToString(GlobalVar.insertNumB);
                                                    break;
                                                default:
                                                    break;
                                            }
                                            GlobalVar.insertNum += 2;
                                            txt_InsertNum.Text = Convert.ToString(GlobalVar.insertNum);
                                        }
                                        else if (m_strs1 == "NG" && m_strs2 != "NG")
                                        {
                                            switch (m_str5)
                                            {
                                                case "A":
                                                    GlobalVar.insertNumA += 1;
                                                    txt_InsertNumA.Text = Convert.ToString(GlobalVar.insertNumA);
                                                    break;
                                                case "B":
                                                    GlobalVar.insertNumB += 1;
                                                    txt_InsertNumB.Text = Convert.ToString(GlobalVar.insertNumB);
                                                    break;
                                                default:
                                                    break;
                                            }
                                            GlobalVar.insertNum += 1;
                                            txt_InsertNum.Text = Convert.ToString(GlobalVar.insertNum);
                                        }
                                        else if (m_strs1 != "NG" && m_strs2 == "NG")
                                        {
                                            switch (m_str5)
                                            {
                                                case "A":
                                                    GlobalVar.insertNumA += 1;
                                                    txt_InsertNumA.Text = Convert.ToString(GlobalVar.insertNumA);
                                                    break;
                                                case "B":
                                                    GlobalVar.insertNumB += 1;
                                                    txt_InsertNumB.Text = Convert.ToString(GlobalVar.insertNumB);
                                                    break;
                                                default:
                                                    break;
                                            }
                                            GlobalVar.insertNum += 1;
                                            txt_InsertNum.Text = Convert.ToString(GlobalVar.insertNum);
                                        }

                                        label3.BackColor = Color.Green;
                                        GlobalVar.OKNum++;
                                        txt_OkNum.Text = Convert.ToString(GlobalVar.OKNum);
                                        GlobalVar.AllNum = GlobalVar.OKNum + GlobalVar.NGNum;
                                        txt_AllNum.Text = Convert.ToString(GlobalVar.OKNum + GlobalVar.NGNum);
                                    }));
                                    //插入数据表格
                                    DataGridViewRow row = new DataGridViewRow();
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells[0].Value = GlobalVar.startNumber++;
                                    row.Cells[1].Value = m_str4;
                                    row.Cells[2].Value = m_strs1;
                                    row.Cells[3].Value = m_strs2;
                                    row.Cells[4].Value = m_strs3;
                                    row.Cells[5].Value = m_ProfINIFile.Read("网路通信", "Trace服务器IP", "");
                                    row.Cells[6].Value = "PASS";
                                    row.Cells[7].Value = "OK";
                                    AddToDgvData(row);

                                    // "序号", "时间", "SN1", "SN2", "rack_id", "Machine info", "Result", "Error/Logid"
                                    string[] head = new string[8];//标题
                                    string[] ls = new string[8];//内容
                                    head[0] = "序号";
                                    head[1] = "时间";
                                    head[2] = "SN1";
                                    head[3] = "SN2";
                                    head[4] = "rack_id";
                                    head[5] = "Machine info";
                                    head[6] = "Result";
                                    head[7] = "Error/Logid";

                                    ls[0] = (GlobalVar.startNumber - 1).ToString();//
                                    ls[1] = m_str4;
                                    ls[2] = m_strs1;
                                    ls[3] = m_strs2;
                                    ls[4] = m_strs3;
                                    ls[5] = m_ProfINIFile.Read("网路通信", "Trace服务器IP", "");
                                    ls[6] = "PASS";
                                    ls[7] = "OK";

                                    Path2 = Application.StartupPath + "\\CSV" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
                                    DataPath = Path2 + "\\" + "Data.csv";
                                    if (!Directory.Exists(Path2))
                                    {
                                        Directory.CreateDirectory(Path2);
                                    }
                                    CSVUtil.WriteCSV(DataPath, ls, head);

                                }
                                else if (GlobalVar.getJson == "404" || GlobalVar.getJson == "400" || GlobalVar.getJson == "504" || GlobalVar.getJson == "502")
                                {
                                    label3.BackColor = Color.Red;
                                    tcpServer.SendAll("ER");
                                    AddToTxt3DebugModeLog("Post上传失败！+" + "NG" + ",SN" + m_strs1 + "," + m_strs2 + ",Machine info:" + m_ProfINIFile.Read("网路通信", "Trace服务器IP", "") + ",Result" + "Fail" + ",Error:" + GlobalVar.getJson);
                                    AddToTxt3Log("接收POST返回结果-->" + GlobalVar.getJson);
                                    AddToLbl2("Post上传失败", Color.Red);
                                    this.BeginInvoke(new Action(() =>
                                    {
                                        //label2.Text = "Post上传失败";
                                        //label2.BackColor = Color.Red;
                                        label3.BackColor = Color.Red;
                                        GlobalVar.NGNum++;
                                        txt_NgNum.Text = Convert.ToString(GlobalVar.NGNum);
                                        GlobalVar.AllNum = GlobalVar.OKNum + GlobalVar.NGNum;
                                        txt_AllNum.Text = Convert.ToString(GlobalVar.OKNum + GlobalVar.NGNum);
                                    }));

                                    //插入数据表格
                                    DataGridViewRow row = new DataGridViewRow();
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells.Add(new DataGridViewTextBoxCell());
                                    row.Cells[0].Value = GlobalVar.startNumber++;
                                    row.Cells[1].Value = m_str4;
                                    row.Cells[2].Value = m_strs1;
                                    row.Cells[3].Value = m_strs2;
                                    row.Cells[4].Value = m_strs3;
                                    row.Cells[5].Value = m_ProfINIFile.Read("网路通信", "Trace服务器IP", "");
                                    row.Cells[6].Value = "Fail";
                                    row.Cells[7].Value = GlobalVar.getJson;
                                    AddToDgvData(row);

                                    string[] head = new string[8];//标题
                                    string[] ls = new string[8];//内容
                                    head[0] = "序号";
                                    head[1] = "时间";
                                    head[2] = "SN1";
                                    head[3] = "SN2";
                                    head[4] = "rack_id";
                                    head[5] = "Machine info";
                                    head[6] = "Result";
                                    head[7] = "Error/Logid";

                                    ls[0] = (GlobalVar.startNumber - 1).ToString();//
                                    ls[1] = m_str4;
                                    ls[2] = m_strs1;
                                    ls[3] = m_strs2;
                                    ls[4] = m_strs3;
                                    ls[5] = m_ProfINIFile.Read("网路通信", "Trace服务器IP", "");
                                    ls[6] = "Fail";
                                    ls[7] = GlobalVar.getJson;

                                    Path2 = Application.StartupPath + "\\CSV" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
                                    DataPath = Path2 + "\\" + "Data.csv";
                                    if (!Directory.Exists(Path2))
                                    {
                                        Directory.CreateDirectory(Path2);
                                    }
                                    CSVUtil.WriteCSV(DataPath, ls, head);

                                }
                            }
                        }
                    }
                    else
                    {
                        if (e.Datagram.Substring(0, 4) == "POST")
                        {
                            tcpServer.SendAll("OK" + ",");
                            AddToTxt3Log("Post上传成功++++");
                            AddToLbl2("Post上传成功", Color.Green);
                            this.BeginInvoke(new Action(() =>
                            {
                                label3.BackColor = Color.Green;
                                GlobalVar.OKNum++;
                                txt_OkNum.Text = Convert.ToString(GlobalVar.OKNum);
                                GlobalVar.AllNum = GlobalVar.OKNum + GlobalVar.NGNum;
                                txt_AllNum.Text = Convert.ToString(GlobalVar.OKNum + GlobalVar.NGNum);
                            }));
                        }

                    }

                }
                catch (Exception ex)
                {
                    AddToLsbLog("Post上传数据解析失败-->" + ex.Message);
                }
            });
            PostTest.IsBackground = true;
            PostTest.Start();

            //Thread ClearNum = new Thread(()=>
            //{
            //    if (e.Datagram != null && e.Datagram != ""&& e.Datagram.Substring(0, 5) == "CLEAR")
            //    {
            //        string[] strs = e.Datagram.Split(new string[] { ",", }, StringSplitOptions.RemoveEmptyEntries);
            //        string m_strs1 = strs[0];//区分AB框
            //        string m_strs2 = strs[1];//C清空计数
            //        if (m_strs1=="A"&&m_strs2=="C")
            //        {
            //            GlobalVar.insertNumA = 0;
            //        }
            //        else if (m_strs1=="B"&&m_strs2=="C")
            //        {
            //            GlobalVar.insertNumB = 0;
            //        }
            //    }
            //});
        }
        //tcp客户端断开
        private void TcpServer_ClientDisconnected(object sender, GlobalServer.TcpClientDisconnectedEventArgs e)
        {
            AddToTxt4Log("TopView通讯断开连接");
            label4.BackColor = Color.Red;
        }


        //服务器连接事件
        private void Client_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
            AddToTxt4Log("TopView通讯正常！" + "远程主机：" + e.Addresses[0] + ":" + e.Port + "连接成功！");
            label4.BackColor = Color.Green;
        }
        //服务器断开事件
        private void Client_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {
            AddToTxt4Log("TopView通讯断开连接");
            label4.BackColor = Color.Red;
        }

        //服务器接收事件
        private void Client_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<string> e)
        {

        }

        #endregion

        //Biel服务器数据请求Get
        private string GetBielQuest(string m_getbieldata)
        {
            //m_ProfINIFile = new CProfIniFile(str_Path);
            ////Get发送数据
            //// m_getdata = "serial=" + m_getdata + "&"+ "serial_type="+ "bg_ss";
            //string url = m_ProfINIFile.Read("TRACE通讯GET请求", "URL地址", "");
            string url = "http://10.217.255.173:8898/api/PreIOXCheck/checkSn?";
            string m_getdata2 = "sn=" + m_getbieldata;
            string getjson = HttpUitls.Get(url + m_getdata2);
            return getjson;
        }

        //Trace服务器数据请求Get
        private string GetTraceQuest(string getTraceQuest)
        {

            string url = "http://localhost:8765/v2/process_control?";
            string m_getdata2 = "serial=" + getTraceQuest + "&serial_type=" + "bg_ss";
            string getjson = HttpUitls.Get(url + m_getdata2);

            return getjson;
        }

        //Trace数据请求Post
        private string PostQuest(string m_postdata)
        {
            //Post发送数据
            m_ProfINIFile = new CProfIniFile(str_Path2);
            string url = m_ProfINIFile.Read("TRACE通讯POST请求", "URL地址", "");
            HttpUitls.Post(url, m_postdata, "");
            return "";
        }



        private void btn_Close_Click(object sender, EventArgs e)
        {
            // client.Close();//关闭服务器连接
            if (GlobalVar.IsAutoRunning == true)
            {
                tcpServer.Stop();
                GlobalVar.IsAutoRunning = false;
                btn_Close.Enabled = false;
                btn_Stat.Enabled = true;
                label4.BackColor = Color.Red;
            }

        }

        private void frm_main_Load(object sender, EventArgs e)
        {
            label4.BackColor = Color.White;
            btn_Close.Enabled = false;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.Columns.Add(new DataGridViewColumn());
            dataGridView1.Columns.Add(new DataGridViewColumn());
            dataGridView1.Columns.Add(new DataGridViewColumn());
            dataGridView1.Columns.Add(new DataGridViewColumn());
            dataGridView1.Columns.Add(new DataGridViewColumn());
            dataGridView1.Columns.Add(new DataGridViewColumn());
            dataGridView1.Columns.Add(new DataGridViewColumn());
            dataGridView1.Columns.Add(new DataGridViewColumn());
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 110;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[5].Width = 100;
            dataGridView1.Columns[6].Width = 50;
            dataGridView1.Columns[7].Width = 80;

            string[] colHead = new string[] { "序号", "时间", "SN1", "SN2", "rack_id", "Machine info", "Result", "Error/Logid" };
            for (int i = 0; i < 8; i++)
            {
                dataGridView1.Columns[i].HeaderText = colHead[i];
                //不可排序
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //记录插架计数清零
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToOrderColumns = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col3 = new DataGridViewTextBoxColumn();
            dataGridView2.Columns.Add(col);
            dataGridView2.Columns.Add(col1);
            dataGridView2.Columns.Add(col2);
            dataGridView2.Columns.Add(col3);
            dataGridView2.Columns[0].Width = 40;
            dataGridView2.Columns[1].Width = 150;
            dataGridView2.Columns[2].Width = 150;
            dataGridView2.Columns[3].Width = 150;
            //col.CellTemplate = col1.CellTemplate = col2.CellTemplate = col3.CellTemplate;//设置等宽
            col.HeaderText = "序号";
            col1.HeaderText = "时间";
            col2.HeaderText = "记录当前值";
            col3.HeaderText = "操作类型";

        }

        //添加消息到log列表
        void AddToLsbLog(string str)
        {
            this.BeginInvoke(new Action(() =>
            {
                string msg = "[" + DateTime.Now.ToString() + "]" + "   " + str;
                //lsb_Log.Items.Add(msg);
                textBox1.AppendText(msg + "\r\n");
                //自动下拉到最后
                // lsb_Log.TopIndex = lsb_Log.Items.Count - 1;

                Path = Application.StartupPath + "\\LOG" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
                LogPath = Path + "\\" + "Log.log";
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                CsvServer.Instance.WriteLine(LogPath, msg);
            }));
        }

        void AddToTxt3Log(string str)
        {
            this.BeginInvoke(new Action(() =>
            {
                string msg;
                msg = "[" + DateTime.Now.ToString() + "]" + "   " + str;
                //lsb_Log.Items.Add(msg);
                textBox3.AppendText(msg + "\r\n");
                //自动下拉到最后
                // lsb_Log.TopIndex = lsb_Log.Items.Count - 1;
                Path = Application.StartupPath + "\\LOG" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
                LogPath = Path + "\\" + "Log.log";
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                CsvServer.Instance.WriteLine(LogPath, msg);
            }));
        }
        void AddToTxt3DebugModeLog(string str)
        {
            this.BeginInvoke(new Action(() =>
            {
                string msg;
                if (Convert.ToBoolean(m_ProfINIFile.Read("参数设置", "checkBox3.Checked", "")))
                {
                    msg = "[" + DateTime.Now.ToString() + "]" + "   " + str + ",Input_parameter:" + json;
                }
                else
                {
                    msg = "[" + DateTime.Now.ToString() + "]" + "   " + str;
                }
                //lsb_Log.Items.Add(msg);
                textBox3.AppendText(msg + "\r\n");
                //自动下拉到最后
                // lsb_Log.TopIndex = lsb_Log.Items.Count - 1;
                Path = Application.StartupPath + "\\LOG" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
                LogPath = Path + "\\" + "Log.log";
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                CsvServer.Instance.WriteLine(LogPath, msg);
            }));
        }
        void AddToTxt4Log(string str)
        {
            this.BeginInvoke(new Action(() =>
            {
                string msg = "[" + DateTime.Now.ToString() + "]" + "   " + str;
                //lsb_Log.Items.Add(msg);
                textBox4.AppendText(msg + "\r\n");
                //自动下拉到最后
                // lsb_Log.TopIndex = lsb_Log.Items.Count - 1;
                Path = Application.StartupPath + "\\LOG" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
                LogPath = Path + "\\" + "Log.log";
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                CsvServer.Instance.WriteLine(LogPath, msg);
            }));
        }

        //插入表格数据
        void AddToDgvData(DataGridViewRow row)
        {
            this.BeginInvoke(new Action(() =>
            {
                //添加行
                dataGridView1.Rows.Add(row);
                //自动下拉到最后
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;

                string msg = "";
                foreach (DataGridViewCell cell in row.Cells)
                {
                    msg += cell.Value + ",";
                }
            }));
        }

        Color AddToLbl2(string str, Color backColor)
        {
            this.BeginInvoke(new Action(() =>
            {
                label2.Text = str;
                label2.BackColor = backColor;
            }));
            return backColor;

        }



        private void 用户登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();
        }

        private void btn_ClearInsertNum_Click(object sender, EventArgs e)//插架计数清空
        {
            /*  
              GlobalVar.insertNum = 0;
              txt_InsertNum.Text = Convert.ToString(GlobalVar.insertNum);
              AddToTxt3Log("插架计数清零++++");
              System.Diagnostics.Debug.WriteLine("插架计数清零++++\n");//只在Debug模式输出信息
              System.Diagnostics.Trace.WriteLine("插架计数清零++++\n");//Debug或者Release模式可输出信息
            */
            //获取当前系统时间
            GlobalVar.uutStarTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DataGridViewRow row = new DataGridViewRow();
            int index = dataGridView2.Rows.Add(row);
            dataGridView2.Rows[index].Cells[0].Value = GlobalVar.startNumber2++;//序号
            dataGridView2.Rows[index].Cells[1].Value = GlobalVar.uutStarTime;//时间
            dataGridView2.Rows[index].Cells[2].Value = txt_InsertNum.Text;//记录当前值
            dataGridView2.Rows[index].Cells[3].Value = "插架计数";//操作类型

            dataGridView2.Rows[index].Selected  = true;//设置从开始一直选中每行
            //下面三条效果一样，都可以让滚动条一直显示在最底部
            //this.dataGridView2.FirstDisplayedScrollingRowIndex = index;
            //this.dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows[index].Index;
            this.dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows[dataGridView2.Rows.Count-1].Index;
             dataGridView2.Rows[index].Selected = false;//取消选中每行

            // "序号", "时间", "记录清零值", "操作类型"
            string[] head = new string[4];//标题
            string[] ls = new string[4];//内容
            head[0] = "序号";
            head[1] = "时间";
            head[2] = "记录当前值";
            head[3] = "操作类型";
            ls[0] = (GlobalVar.startNumber2 - 1).ToString();//
            ls[1] = GlobalVar.uutStarTime;
            ls[2] = txt_InsertNum.Text;
            ls[3] = "插架计数";

            Path3 = Application.StartupPath + "\\计数清零记录" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
            DataPath3 = Path3 + "\\" + "ZeroCount.csv";
            if (!Directory.Exists(Path3))
            {
                Directory.CreateDirectory(Path3);
            }
            CSVUtil.WriteCSV(DataPath3, ls, head);
            
            string str1;
            str1 = txt_InsertNum.Text;
            str1 = "  " + str1 + "  " + "插架计数清零++++";
            AddToTxt3Log(str1);
            GlobalVar.insertNum = 0;
            txt_InsertNum.Text = Convert.ToString(GlobalVar.insertNum);
            str1 = str1 + "\n";
            System.Diagnostics.Debug.WriteLine(str1);//只在Debug模式输出信息
            System.Diagnostics.Trace.WriteLine(str1);//Debug或者Release模式可输出信息


        }

        private void btn_ClearInsertNumA_Click(object sender, EventArgs e)//左框插架
        {
            //获取当前系统时间
            GlobalVar.uutStarTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DataGridViewRow row = new DataGridViewRow();
            int index = dataGridView2.Rows.Add(row);
            dataGridView2.Rows[index].Cells[0].Value = GlobalVar.startNumber2++;//序号
            dataGridView2.Rows[index].Cells[1].Value = GlobalVar.uutStarTime;//时间
            dataGridView2.Rows[index].Cells[2].Value = txt_InsertNumA.Text;//记录当前值
            dataGridView2.Rows[index].Cells[3].Value = "左框插架";//操作类型

            dataGridView2.Rows[index].Selected = true;//设置从开始一直选中每行
            //下面三条效果一样，都可以让滚动条一直显示在最底部
            //this.dataGridView2.FirstDisplayedScrollingRowIndex = index;
            //this.dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows[index].Index;
            this.dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows[dataGridView2.Rows.Count - 1].Index;
            dataGridView2.Rows[index].Selected = false;//取消选中每行

            // "序号", "时间", "记录清零值", "操作类型"
            string[] head = new string[4];//标题
            string[] ls = new string[4];//内容
            head[0] = "序号";
            head[1] = "时间";
            head[2] = "记录当前值";
            head[3] = "操作类型";
            ls[0] = (GlobalVar.startNumber2 - 1).ToString();//
            ls[1] = GlobalVar.uutStarTime;
            ls[2] = txt_InsertNumA.Text;
            ls[3] = "左框插架";

            Path3 = Application.StartupPath + "\\计数清零记录" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
            DataPath3 = Path3 + "\\" + "ZeroCount.csv";
            if (!Directory.Exists(Path3))
            {
                Directory.CreateDirectory(Path3);
            }
            CSVUtil.WriteCSV(DataPath3, ls, head);

            string str1;
            str1 = txt_InsertNumA.Text;
            str1 = "  " + str1 + "  " + "左框计数清零++++";
            AddToTxt3Log(str1);
            GlobalVar.insertNumA = 0;
            txt_InsertNumA.Text = Convert.ToString(GlobalVar.insertNumA);
            str1 = str1 + "\n";
            System.Diagnostics.Debug.WriteLine(str1);//只在Debug模式输出信息
            System.Diagnostics.Trace.WriteLine(str1);//Debug或者Release模式可输出信息
        }

        private void btn_ClearInsertNumB_Click(object sender, EventArgs e)//右框插架
        {
            //获取当前系统时间
            GlobalVar.uutStarTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DataGridViewRow row = new DataGridViewRow();
            int index = dataGridView2.Rows.Add(row);
            dataGridView2.Rows[index].Cells[0].Value = GlobalVar.startNumber2++;//序号
            dataGridView2.Rows[index].Cells[1].Value = GlobalVar.uutStarTime;//时间
            dataGridView2.Rows[index].Cells[2].Value = txt_InsertNumB.Text;//记录当前值
            dataGridView2.Rows[index].Cells[3].Value = "右框插架";//操作类型

            dataGridView2.Rows[index].Selected = true;//设置从开始一直选中每行
            //下面三条效果一样，都可以让滚动条一直显示在最底部
            //this.dataGridView2.FirstDisplayedScrollingRowIndex = index;
            //this.dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows[index].Index;
            this.dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows[dataGridView2.Rows.Count - 1].Index;
            dataGridView2.Rows[index].Selected = false;//取消选中每行

            // "序号", "时间", "记录清零值", "操作类型"
            string[] head = new string[4];//标题
            string[] ls = new string[4];//内容
            head[0] = "序号";
            head[1] = "时间";
            head[2] = "记录当前值";
            head[3] = "操作类型";
            ls[0] = (GlobalVar.startNumber2 - 1).ToString();//
            ls[1] = GlobalVar.uutStarTime;
            ls[2] = txt_InsertNumB.Text;
            ls[3] = "右框插架";

            Path3 = Application.StartupPath + "\\计数清零记录" + "\\" + DateTime.Now.Date.ToString("yyyy-MM-dd");
            DataPath3 = Path3 + "\\" + "ZeroCount.csv";
            if (!Directory.Exists(Path3))
            {
                Directory.CreateDirectory(Path3);
            }
            CSVUtil.WriteCSV(DataPath3, ls, head);

            string str1;
            str1 = txt_InsertNumB.Text;
            str1 = "  " + str1 + "  " + "右框计数清零++++";
            AddToTxt3Log(str1);
            GlobalVar.insertNumB = 0;
            txt_InsertNumB.Text = Convert.ToString(GlobalVar.insertNumB);
            str1 = str1 + "\n";
            System.Diagnostics.Debug.WriteLine(str1);//只在Debug模式输出信息
            System.Diagnostics.Trace.WriteLine(str1);//Debug或者Release模式可输出信息
        }
    }
}
