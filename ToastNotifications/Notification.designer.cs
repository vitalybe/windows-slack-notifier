namespace ToastNotifications
{
    partial class Notification
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notification));
            this.lifeTimer = new System.Windows.Forms.Timer(this.components);
            this.labelTitle = new System.Windows.Forms.Label();
            this.txtQuickReply = new System.Windows.Forms.TextBox();
            this.htmlBody = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
            this.btnSnooze = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lifeTimer
            // 
            this.lifeTimer.Tick += new System.EventHandler(this.lifeTimer_Tick);
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(49)))), ((int)(((byte)(60)))));
            this.labelTitle.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(0, 1);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(294, 24);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "title goes here";
            this.labelTitle.Click += new System.EventHandler(this.labelTitle_Click);
            // 
            // txtQuickReply
            // 
            this.txtQuickReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuickReply.Location = new System.Drawing.Point(8, 128);
            this.txtQuickReply.Name = "txtQuickReply";
            this.txtQuickReply.Size = new System.Drawing.Size(278, 20);
            this.txtQuickReply.TabIndex = 1;
            this.txtQuickReply.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuickReply_KeyDown);
            // 
            // htmlBody
            // 
            this.htmlBody.AutoScroll = true;
            this.htmlBody.AutoScrollMinSize = new System.Drawing.Size(278, 20);
            this.htmlBody.BackColor = System.Drawing.Color.Transparent;
            this.htmlBody.BaseStylesheet = null;
            this.htmlBody.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.htmlBody.Location = new System.Drawing.Point(8, 25);
            this.htmlBody.Name = "htmlBody";
            this.htmlBody.Size = new System.Drawing.Size(278, 96);
            this.htmlBody.TabIndex = 2;
            this.htmlBody.Text = "<p style=\"margin: 0; color: yellow; font-weight: bold\">hi</p>";
            // 
            // btnSnooze
            // 
            this.btnSnooze.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(57)))), ((int)(((byte)(75)))));
            this.btnSnooze.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSnooze.ForeColor = System.Drawing.Color.White;
            this.btnSnooze.Location = new System.Drawing.Point(224, 1);
            this.btnSnooze.Name = "btnSnooze";
            this.btnSnooze.Size = new System.Drawing.Size(53, 22);
            this.btnSnooze.TabIndex = 3;
            this.btnSnooze.Text = "Snooze";
            this.btnSnooze.UseVisualStyleBackColor = false;
            this.btnSnooze.Click += new System.EventHandler(this.btnSnooze_Click);
            // 
            // Notification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(57)))), ((int)(((byte)(75)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(294, 159);
            this.ControlBox = false;
            this.Controls.Add(this.btnSnooze);
            this.Controls.Add(this.htmlBody);
            this.Controls.Add(this.txtQuickReply);
            this.Controls.Add(this.labelTitle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Notification";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "EDGE Shop Flag Notification";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.Notification_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Notification_FormClosed);
            this.Load += new System.EventHandler(this.Notification_Load);
            this.Shown += new System.EventHandler(this.Notification_Shown);
            this.Click += new System.EventHandler(this.Notification_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer lifeTimer;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox txtQuickReply;
        private TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel htmlBody;
        private System.Windows.Forms.Button btnSnooze;
    }
}