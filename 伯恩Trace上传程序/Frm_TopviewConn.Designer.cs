namespace 伯恩Trace上传程序
{
    partial class Frm_TopviewConn
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_SendData = new System.Windows.Forms.Button();
            this.txt_SendData = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Received = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_ServerIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_ConnSave = new System.Windows.Forms.Button();
            this.btn_ConnRead = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Start = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_SendData);
            this.groupBox1.Controls.Add(this.txt_SendData);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txt_Received);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtServerPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txt_ServerIP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 397);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "网络通信";
            // 
            // btn_SendData
            // 
            this.btn_SendData.Location = new System.Drawing.Point(319, 362);
            this.btn_SendData.Name = "btn_SendData";
            this.btn_SendData.Size = new System.Drawing.Size(75, 29);
            this.btn_SendData.TabIndex = 24;
            this.btn_SendData.Text = "发送";
            this.btn_SendData.UseVisualStyleBackColor = true;
            this.btn_SendData.Visible = false;
            this.btn_SendData.Click += new System.EventHandler(this.btn_SendData_Click);
            // 
            // txt_SendData
            // 
            this.txt_SendData.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_SendData.ForeColor = System.Drawing.Color.Red;
            this.txt_SendData.Location = new System.Drawing.Point(15, 145);
            this.txt_SendData.Multiline = true;
            this.txt_SendData.Name = "txt_SendData";
            this.txt_SendData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_SendData.Size = new System.Drawing.Size(285, 104);
            this.txt_SendData.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 268);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 22;
            this.label4.Text = "接收区:";
            // 
            // txt_Received
            // 
            this.txt_Received.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_Received.ForeColor = System.Drawing.Color.Red;
            this.txt_Received.Location = new System.Drawing.Point(15, 286);
            this.txt_Received.Multiline = true;
            this.txt_Received.Name = "txt_Received";
            this.txt_Received.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Received.Size = new System.Drawing.Size(285, 104);
            this.txt_Received.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "发送区:";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(123, 77);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(177, 25);
            this.txtServerPort.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "服务器端口号:";
            // 
            // txt_ServerIP
            // 
            this.txt_ServerIP.Location = new System.Drawing.Point(123, 32);
            this.txt_ServerIP.Name = "txt_ServerIP";
            this.txt_ServerIP.Size = new System.Drawing.Size(177, 25);
            this.txt_ServerIP.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器IP：";
            // 
            // btn_ConnSave
            // 
            this.btn_ConnSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ConnSave.Location = new System.Drawing.Point(430, 246);
            this.btn_ConnSave.Name = "btn_ConnSave";
            this.btn_ConnSave.Size = new System.Drawing.Size(118, 33);
            this.btn_ConnSave.TabIndex = 32;
            this.btn_ConnSave.Text = "保存";
            this.btn_ConnSave.UseVisualStyleBackColor = true;
            this.btn_ConnSave.Click += new System.EventHandler(this.btn_ConnSave_Click);
            // 
            // btn_ConnRead
            // 
            this.btn_ConnRead.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ConnRead.Location = new System.Drawing.Point(430, 194);
            this.btn_ConnRead.Name = "btn_ConnRead";
            this.btn_ConnRead.Size = new System.Drawing.Size(118, 33);
            this.btn_ConnRead.TabIndex = 31;
            this.btn_ConnRead.Text = "读出";
            this.btn_ConnRead.UseVisualStyleBackColor = true;
            this.btn_ConnRead.Click += new System.EventHandler(this.btn_ConnRead_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(411, 25);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 19);
            this.label5.TabIndex = 30;
            this.label5.Text = "通信状态：未连接";
            // 
            // btn_Stop
            // 
            this.btn_Stop.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Stop.Location = new System.Drawing.Point(430, 121);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(118, 33);
            this.btn_Stop.TabIndex = 29;
            this.btn_Stop.Text = "关闭服务";
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Visible = false;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Start
            // 
            this.btn_Start.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Start.Location = new System.Drawing.Point(430, 74);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(118, 33);
            this.btn_Start.TabIndex = 28;
            this.btn_Start.Text = "启动服务";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Visible = false;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // Frm_TopviewConn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 407);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_ConnSave);
            this.Controls.Add(this.btn_ConnRead);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_Stop);
            this.Controls.Add(this.btn_Start);
            this.MaximizeBox = false;
            this.Name = "Frm_TopviewConn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "网络通信";
            this.Load += new System.EventHandler(this.Frm_TopviewConn_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_SendData;
        private System.Windows.Forms.TextBox txt_SendData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_Received;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_ServerIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_ConnSave;
        private System.Windows.Forms.Button btn_ConnRead;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Start;
    }
}