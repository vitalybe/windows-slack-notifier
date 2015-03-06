namespace SlackWindowsTray
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSnoozeChatMinutes = new System.Windows.Forms.TextBox();
            this.txtSnoozeAllMinutes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.chkToBlinkMention = new System.Windows.Forms.CheckBox();
            this.chkToBlinkUnread = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.linkGenerateToken = new System.Windows.Forms.LinkLabel();
            this.txtSlackToken = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSnoozeChatMinutes);
            this.groupBox1.Controls.Add(this.txtSnoozeAllMinutes);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Snoozing timeout";
            // 
            // txtSnoozeChatMinutes
            // 
            this.txtSnoozeChatMinutes.Location = new System.Drawing.Point(154, 46);
            this.txtSnoozeChatMinutes.Name = "txtSnoozeChatMinutes";
            this.txtSnoozeChatMinutes.Size = new System.Drawing.Size(58, 20);
            this.txtSnoozeChatMinutes.TabIndex = 3;
            // 
            // txtSnoozeAllMinutes
            // 
            this.txtSnoozeAllMinutes.Location = new System.Drawing.Point(154, 20);
            this.txtSnoozeAllMinutes.Name = "txtSnoozeAllMinutes";
            this.txtSnoozeAllMinutes.Size = new System.Drawing.Size(58, 20);
            this.txtSnoozeAllMinutes.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Snooze chat (minutes)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Snooze everything (minutes)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox2);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.chkToBlinkMention);
            this.groupBox2.Controls.Add(this.chkToBlinkUnread);
            this.groupBox2.Location = new System.Drawing.Point(14, 204);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 80);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Notification blinking";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(115, 45);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(31, 24);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(115, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(31, 24);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // chkToBlinkMention
            // 
            this.chkToBlinkMention.AutoSize = true;
            this.chkToBlinkMention.Location = new System.Drawing.Point(9, 45);
            this.chkToBlinkMention.Name = "chkToBlinkMention";
            this.chkToBlinkMention.Size = new System.Drawing.Size(104, 17);
            this.chkToBlinkMention.TabIndex = 5;
            this.chkToBlinkMention.Text = "Blink on mention";
            this.chkToBlinkMention.UseVisualStyleBackColor = true;
            // 
            // chkToBlinkUnread
            // 
            this.chkToBlinkUnread.AutoSize = true;
            this.chkToBlinkUnread.Location = new System.Drawing.Point(9, 22);
            this.chkToBlinkUnread.Name = "chkToBlinkUnread";
            this.chkToBlinkUnread.Size = new System.Drawing.Size(100, 17);
            this.chkToBlinkUnread.TabIndex = 4;
            this.chkToBlinkUnread.Text = "Blink on unread";
            this.chkToBlinkUnread.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(81, 296);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(162, 296);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.linkGenerateToken);
            this.groupBox3.Controls.Add(this.txtSlackToken);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(14, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(223, 88);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Slack token";
            // 
            // linkGenerateToken
            // 
            this.linkGenerateToken.AutoSize = true;
            this.linkGenerateToken.LinkArea = new System.Windows.Forms.LinkArea(17, 4);
            this.linkGenerateToken.Location = new System.Drawing.Point(10, 68);
            this.linkGenerateToken.Name = "linkGenerateToken";
            this.linkGenerateToken.Size = new System.Drawing.Size(120, 17);
            this.linkGenerateToken.TabIndex = 4;
            this.linkGenerateToken.TabStop = true;
            this.linkGenerateToken.Text = "Generate a token here.";
            this.linkGenerateToken.UseCompatibleTextRendering = true;
            this.linkGenerateToken.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGenerateToken_LinkClicked);
            // 
            // txtSlackToken
            // 
            this.txtSlackToken.Location = new System.Drawing.Point(10, 42);
            this.txtSlackToken.Name = "txtSlackToken";
            this.txtSlackToken.PasswordChar = '*';
            this.txtSlackToken.Size = new System.Drawing.Size(201, 20);
            this.txtSlackToken.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkRed;
            this.label4.Location = new System.Drawing.Point(7, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Valid Slack token:";
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 333);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "Slack Windows Tray - Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSnoozeChatMinutes;
        private System.Windows.Forms.TextBox txtSnoozeAllMinutes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox chkToBlinkMention;
        private System.Windows.Forms.CheckBox chkToBlinkUnread;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.LinkLabel linkGenerateToken;
        private System.Windows.Forms.TextBox txtSlackToken;
        private System.Windows.Forms.Label label4;
    }
}