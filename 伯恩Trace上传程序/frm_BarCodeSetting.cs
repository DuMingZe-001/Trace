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
using System.IO;

namespace 伯恩Trace上传程序
{
    public partial class frm_BarCodeSetting : Form
    {
        string str_Path = System.Environment.CurrentDirectory + @"\TraceSetting.ini";
        public ProFile.CProfIniFile m_ProfINIFile;
        Login login = new Login();

        public frm_BarCodeSetting()
        {
            InitializeComponent();
        }

        private void btn_BarcodeRead_Click(object sender, EventArgs e)
        {          
            m_ProfINIFile = new CProfIniFile(str_Path);
            txt_unit_serial_number.Text = m_ProfINIFile.Read("参数设置", "unit_serial_number", "");
            txt_fixtureid.Text = m_ProfINIFile.Read("参数设置", "fixture_id", "");
            txt_head_id.Text = m_ProfINIFile.Read("参数设置", " head_id", "");
            txt_lineid.Text = m_ProfINIFile.Read("参数设置", "line_id", "");
            txt_software_name.Text = m_ProfINIFile.Read("参数设置", "software_name", "");
            txt_stationid.Text = m_ProfINIFile.Read("参数设置", "station_id", "");
            txt_STATION_STRING.Text = m_ProfINIFile.Read("参数设置", "STATION_STRING", "");
            txt_factory.Text = m_ProfINIFile.Read("参数设置", "Factory", "");
        }

        private void btn_BarCodeSave_Click(object sender, EventArgs e)
        {           
            m_ProfINIFile = new CProfIniFile(str_Path);
            m_ProfINIFile.Write("参数设置", "unit_serial_number", txt_unit_serial_number.Text);
            m_ProfINIFile.Write("参数设置", "fixture_id", txt_fixtureid.Text);
            m_ProfINIFile.Write("参数设置", "head_id", txt_head_id.Text);
            m_ProfINIFile.Write("参数设置", "line_id", txt_lineid.Text);
            m_ProfINIFile.Write("参数设置", "software_name", txt_software_name.Text);
            m_ProfINIFile.Write("参数设置", "station_id", txt_stationid.Text);
            m_ProfINIFile.Write("参数设置", "STATION_STRING", txt_STATION_STRING.Text);
            m_ProfINIFile.Write("参数设置", "Factory", txt_factory.Text);
            if (checkBox2.Checked == true)
            {
                m_ProfINIFile.Write("参数设置", "checkBox2.Checked", "True");
            }
            else
            {
                m_ProfINIFile.Write("参数设置", "checkBox2.Checked", "False");
            }
            if (checkBox1.Checked == true)
            {
                m_ProfINIFile.Write("参数设置", "checkBox1.Checked", "True");
            }
            else
            {
                m_ProfINIFile.Write("参数设置", "checkBox1.Checked", "False");
            }
            if (checkBox3.Checked == true)
            {
                m_ProfINIFile.Write("参数设置", "checkBox3.Checked", "True");
            }
            else
            {
                m_ProfINIFile.Write("参数设置", "checkBox3.Checked", "False");
            }

        }

        private void btn_regular_Click(object sender, EventArgs e)
        {
            //frm_regular _frmregular = new frm_regular();
            //_frmregular.ShowDialog();
        }

        private void frm_BarCodeSetting_Load(object sender, EventArgs e)
        {   
           /*
            //判断路径是否存在
            if(!Directory.Exists(str_Path))
            {
                Directory.CreateDirectory(str_Path);
            }
            */
            //判断文件是否存在，不存在则创建文件
            if (!File.Exists(str_Path))
            {
                File.Create(str_Path);
                MessageBox.Show("文件不存在，重新创建！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            m_ProfINIFile = new CProfIniFile(str_Path);//需要加载文件判断，否则会崩溃
            txt_unit_serial_number.Text = m_ProfINIFile.Read("参数设置", "unit_serial_number", "");
            txt_fixtureid.Text = m_ProfINIFile.Read("参数设置", "fixture_id", "");
            txt_head_id.Text = m_ProfINIFile.Read("参数设置", " head_id", "");
            txt_lineid.Text = m_ProfINIFile.Read("参数设置", "line_id", "");
            txt_software_name.Text = m_ProfINIFile.Read("参数设置", "software_name", "");
            txt_stationid.Text = m_ProfINIFile.Read("参数设置", "station_id", "");
            txt_STATION_STRING.Text = m_ProfINIFile.Read("参数设置", "STATION_STRING", "");
            txt_factory.Text = m_ProfINIFile.Read("参数设置", "Factory", "");
            checkBox1.Checked = Convert.ToBoolean(m_ProfINIFile.Read("参数设置", "checkBox1.Checked", ""));
            checkBox2.Checked = Convert.ToBoolean(m_ProfINIFile.Read("参数设置", "checkBox2.Checked", ""));
            checkBox3.Checked = Convert.ToBoolean(m_ProfINIFile.Read("参数设置", "checkBox3.Checked", ""));
            if (GlobalVar.contain == true&&(GlobalVar.cbresult==1||GlobalVar.cbresult==2))
            {
                foreach (Control ctrl in tabControl1.Controls)
                {
                    ctrl.Enabled = true;
                    //checkBox2.Checked = true;
                }
            }
            else
            {
                foreach (Control ctrl in tabControl1.Controls)
                {
                    ctrl.Enabled = false;
                   
                }
            }
         

        }
    }
}
