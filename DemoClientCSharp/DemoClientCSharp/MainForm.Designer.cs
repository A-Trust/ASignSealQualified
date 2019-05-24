namespace DemoClientCSharp
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.path_load_auth_cert = new System.Windows.Forms.TextBox();
            this.button_load_auth_cert = new System.Windows.Forms.Button();
            this.auth_cert_pwd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.targetsystem = new System.Windows.Forms.ComboBox();
            this.buttonGetCertificate = new System.Windows.Forms.Button();
            this.buttonSignTest = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.show_seal_cert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Authentication Certificate PKCS#12";
            // 
            // path_load_auth_cert
            // 
            this.path_load_auth_cert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.path_load_auth_cert.Location = new System.Drawing.Point(193, 12);
            this.path_load_auth_cert.Name = "path_load_auth_cert";
            this.path_load_auth_cert.Size = new System.Drawing.Size(337, 20);
            this.path_load_auth_cert.TabIndex = 1;
            // 
            // button_load_auth_cert
            // 
            this.button_load_auth_cert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_load_auth_cert.Location = new System.Drawing.Point(536, 10);
            this.button_load_auth_cert.Name = "button_load_auth_cert";
            this.button_load_auth_cert.Size = new System.Drawing.Size(97, 23);
            this.button_load_auth_cert.TabIndex = 2;
            this.button_load_auth_cert.Text = "load";
            this.button_load_auth_cert.UseVisualStyleBackColor = true;
            this.button_load_auth_cert.Click += new System.EventHandler(this.button_load_auth_cert_Click);
            // 
            // auth_cert_pwd
            // 
            this.auth_cert_pwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.auth_cert_pwd.Location = new System.Drawing.Point(193, 38);
            this.auth_cert_pwd.Name = "auth_cert_pwd";
            this.auth_cert_pwd.Size = new System.Drawing.Size(337, 20);
            this.auth_cert_pwd.TabIndex = 4;
            this.auth_cert_pwd.Text = "testpwd";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Authentication Certificate Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Target System:";
            // 
            // targetsystem
            // 
            this.targetsystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetsystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetsystem.FormattingEnabled = true;
            this.targetsystem.Location = new System.Drawing.Point(193, 64);
            this.targetsystem.Name = "targetsystem";
            this.targetsystem.Size = new System.Drawing.Size(337, 21);
            this.targetsystem.TabIndex = 6;
            // 
            // buttonGetCertificate
            // 
            this.buttonGetCertificate.Location = new System.Drawing.Point(15, 107);
            this.buttonGetCertificate.Name = "buttonGetCertificate";
            this.buttonGetCertificate.Size = new System.Drawing.Size(97, 23);
            this.buttonGetCertificate.TabIndex = 7;
            this.buttonGetCertificate.Text = "Get Certificate";
            this.buttonGetCertificate.UseVisualStyleBackColor = true;
            this.buttonGetCertificate.Click += new System.EventHandler(this.buttonGetCertificate_Click);
            // 
            // buttonSignTest
            // 
            this.buttonSignTest.Location = new System.Drawing.Point(228, 107);
            this.buttonSignTest.Name = "buttonSignTest";
            this.buttonSignTest.Size = new System.Drawing.Size(97, 23);
            this.buttonSignTest.TabIndex = 8;
            this.buttonSignTest.Text = "Sign Test";
            this.buttonSignTest.UseVisualStyleBackColor = true;
            this.buttonSignTest.Click += new System.EventHandler(this.buttonSignTest_Click);
            // 
            // textBoxResult
            // 
            this.textBoxResult.AcceptsReturn = true;
            this.textBoxResult.AcceptsTab = true;
            this.textBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResult.Location = new System.Drawing.Point(15, 136);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(618, 174);
            this.textBoxResult.TabIndex = 9;
            // 
            // show_seal_cert
            // 
            this.show_seal_cert.Enabled = false;
            this.show_seal_cert.Location = new System.Drawing.Point(118, 107);
            this.show_seal_cert.Name = "show_seal_cert";
            this.show_seal_cert.Size = new System.Drawing.Size(104, 23);
            this.show_seal_cert.TabIndex = 10;
            this.show_seal_cert.Text = "Show Certificate";
            this.show_seal_cert.UseVisualStyleBackColor = true;
            this.show_seal_cert.Click += new System.EventHandler(this.show_seal_cert_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 322);
            this.Controls.Add(this.show_seal_cert);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.buttonSignTest);
            this.Controls.Add(this.buttonGetCertificate);
            this.Controls.Add(this.targetsystem);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.auth_cert_pwd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_load_auth_cert);
            this.Controls.Add(this.path_load_auth_cert);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "a.sign premium seal qualified  - demo client";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox path_load_auth_cert;
        private System.Windows.Forms.Button button_load_auth_cert;
        private System.Windows.Forms.TextBox auth_cert_pwd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox targetsystem;
        private System.Windows.Forms.Button buttonGetCertificate;
        private System.Windows.Forms.Button buttonSignTest;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button show_seal_cert;
    }
}

