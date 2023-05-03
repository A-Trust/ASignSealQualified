namespace DemoClientCSharp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.path_load_auth_cert = new System.Windows.Forms.TextBox();
            this.auth_cert_pwd = new System.Windows.Forms.TextBox();
            this.targetsystem = new System.Windows.Forms.ComboBox();
            this.button_load_auth_cert = new System.Windows.Forms.Button();
            this.buttonGetCertificate = new System.Windows.Forms.Button();
            this.show_seal_cert = new System.Windows.Forms.Button();
            this.buttonSignTest = new System.Windows.Forms.Button();
            this.buttonBatchSign = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Authentication certificate PKCS#12";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Authentication certificate password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Target system:";
            // 
            // path_load_auth_cert
            // 
            this.path_load_auth_cert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.path_load_auth_cert.Location = new System.Drawing.Point(210, 12);
            this.path_load_auth_cert.Name = "path_load_auth_cert";
            this.path_load_auth_cert.Size = new System.Drawing.Size(393, 23);
            this.path_load_auth_cert.TabIndex = 3;
            // 
            // auth_cert_pwd
            // 
            this.auth_cert_pwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.auth_cert_pwd.Location = new System.Drawing.Point(210, 50);
            this.auth_cert_pwd.Name = "auth_cert_pwd";
            this.auth_cert_pwd.Size = new System.Drawing.Size(393, 23);
            this.auth_cert_pwd.TabIndex = 4;
            this.auth_cert_pwd.Text = "testpwd";
            // 
            // targetsystem
            // 
            this.targetsystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetsystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetsystem.FormattingEnabled = true;
            this.targetsystem.Location = new System.Drawing.Point(210, 89);
            this.targetsystem.Name = "targetsystem";
            this.targetsystem.Size = new System.Drawing.Size(393, 23);
            this.targetsystem.TabIndex = 5;
            // 
            // button_load_auth_cert
            // 
            this.button_load_auth_cert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_load_auth_cert.Location = new System.Drawing.Point(622, 12);
            this.button_load_auth_cert.Name = "button_load_auth_cert";
            this.button_load_auth_cert.Size = new System.Drawing.Size(166, 23);
            this.button_load_auth_cert.TabIndex = 6;
            this.button_load_auth_cert.Text = "load";
            this.button_load_auth_cert.UseVisualStyleBackColor = true;
            this.button_load_auth_cert.Click += new System.EventHandler(this.button_load_auth_cert_Click);
            // 
            // buttonGetCertificate
            // 
            this.buttonGetCertificate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetCertificate.Location = new System.Drawing.Point(12, 144);
            this.buttonGetCertificate.Name = "buttonGetCertificate";
            this.buttonGetCertificate.Size = new System.Drawing.Size(166, 23);
            this.buttonGetCertificate.TabIndex = 7;
            this.buttonGetCertificate.Text = "Get Certificate";
            this.buttonGetCertificate.UseVisualStyleBackColor = true;
            this.buttonGetCertificate.Click += new System.EventHandler(this.buttonGetCertificate_Click);
            // 
            // show_seal_cert
            // 
            this.show_seal_cert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.show_seal_cert.Location = new System.Drawing.Point(184, 144);
            this.show_seal_cert.Name = "show_seal_cert";
            this.show_seal_cert.Size = new System.Drawing.Size(166, 23);
            this.show_seal_cert.TabIndex = 8;
            this.show_seal_cert.Text = "Show Certificate";
            this.show_seal_cert.UseVisualStyleBackColor = true;
            this.show_seal_cert.Click += new System.EventHandler(this.show_seal_cert_Click);
            // 
            // buttonSignTest
            // 
            this.buttonSignTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSignTest.Location = new System.Drawing.Point(356, 144);
            this.buttonSignTest.Name = "buttonSignTest";
            this.buttonSignTest.Size = new System.Drawing.Size(166, 23);
            this.buttonSignTest.TabIndex = 9;
            this.buttonSignTest.Text = "Sign Test";
            this.buttonSignTest.UseVisualStyleBackColor = true;
            this.buttonSignTest.Click += new System.EventHandler(this.buttonSignTest_Click);
            // 
            // buttonBatchSign
            // 
            this.buttonBatchSign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBatchSign.Location = new System.Drawing.Point(528, 144);
            this.buttonBatchSign.Name = "buttonBatchSign";
            this.buttonBatchSign.Size = new System.Drawing.Size(166, 23);
            this.buttonBatchSign.TabIndex = 10;
            this.buttonBatchSign.Text = "Batch Sign Test";
            this.buttonBatchSign.UseVisualStyleBackColor = true;
            this.buttonBatchSign.Click += new System.EventHandler(this.buttonBatchSign_Click);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResult.Location = new System.Drawing.Point(12, 184);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(776, 254);
            this.textBoxResult.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.buttonBatchSign);
            this.Controls.Add(this.buttonSignTest);
            this.Controls.Add(this.show_seal_cert);
            this.Controls.Add(this.buttonGetCertificate);
            this.Controls.Add(this.button_load_auth_cert);
            this.Controls.Add(this.targetsystem);
            this.Controls.Add(this.auth_cert_pwd);
            this.Controls.Add(this.path_load_auth_cert);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "a.sign seal qualified - demo client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox path_load_auth_cert;
        private TextBox auth_cert_pwd;
        private ComboBox targetsystem;
        private Button button_load_auth_cert;
        private Button buttonGetCertificate;
        private Button show_seal_cert;
        private Button buttonSignTest;
        private Button buttonBatchSign;
        private TextBox textBoxResult;
    }
}