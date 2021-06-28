namespace 伯恩Trace上传程序
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_code = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_login = new System.Windows.Forms.Button();
            this.cb_username = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_code
            // 
            this.tb_code.Location = new System.Drawing.Point(130, 71);
            this.tb_code.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_code.Name = "tb_code";
            this.tb_code.PasswordChar = '*';
            this.tb_code.Size = new System.Drawing.Size(132, 21);
            this.tb_code.TabIndex = 14;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(185, 151);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 36);
            this.button2.TabIndex = 13;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(34, 151);
            this.btn_login.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(76, 36);
            this.btn_login.TabIndex = 12;
            this.btn_login.Text = "登录";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // cb_username
            // 
            this.cb_username.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_username.FormattingEnabled = true;
            this.cb_username.Items.AddRange(new object[] {
            "操作员",
            "技术员",
            "管理员"});
            this.cb_username.Location = new System.Drawing.Point(130, 30);
            this.cb_username.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cb_username.Name = "cb_username";
            this.cb_username.Size = new System.Drawing.Size(132, 20);
            this.cb_username.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(28, 68);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "输入密码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(28, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 19);
            this.label2.TabIndex = 9;
            this.label2.Text = "用户名：";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 241);
            this.Controls.Add(this.tb_code);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.cb_username);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_code;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_login;
        public  System.Windows.Forms.ComboBox cb_username;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}