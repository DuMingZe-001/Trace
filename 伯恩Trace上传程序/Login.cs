using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 伯恩Trace上传程序
{
    public partial class Login : Form
    {
        List<User> Users = new List<User>();
        public static User CurrentUser = new User("操作员", "123", 0);//default user
       
        public Login()
        {
            InitializeComponent();
        }
        /*
                private void btn_login_Click(object sender, EventArgs e)
                {
                    foreach (User user in Users)
                    {
                        if (user.Equal(cb_username.Text, tb_code.Text, cb_username.SelectedIndex))
                        {
                            CurrentUser = user;
                            GlobalVar.contain = true;
                            GlobalVar.cbresult = cb_username.SelectedIndex;
                        }

                    }

                    if (!GlobalVar.contain)
                        MessageBox.Show("用户名或密码错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    else

                    this.Close();
                }
        */
        private void btn_login_Click(object sender, EventArgs e)
        {
            if (((cb_username.Text == "操作员") && (tb_code.Text == "123456"))
                || ((cb_username.Text == "技术员") && (tb_code.Text == "888888")) || ((cb_username.Text == "管理员") && (tb_code.Text == "101010")))
            {

                GlobalVar.contain = true;
                GlobalVar.cbresult = cb_username.SelectedIndex;
                this.Close();
            }
            else 
            {
                GlobalVar.contain = false;
                MessageBox.Show("用户名或密码错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 

         }
        private void Login_Load(object sender, EventArgs e)
        {
            Users.Add(new User("操作员", "123456", 0));
            Users.Add(new User("技术员", "888888", 1));
            Users.Add(new User("管理员", "101010", 2));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
