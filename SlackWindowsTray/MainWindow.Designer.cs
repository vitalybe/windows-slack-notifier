namespace SlackWindowsTray
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.lblSlackStatus = new System.Windows.Forms.Label();
            this.slackTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.snoozeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unsnoozeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSlackStatus
            // 
            this.lblSlackStatus.AutoSize = true;
            this.lblSlackStatus.Location = new System.Drawing.Point(106, 60);
            this.lblSlackStatus.Name = "lblSlackStatus";
            this.lblSlackStatus.Size = new System.Drawing.Size(35, 13);
            this.lblSlackStatus.TabIndex = 0;
            this.lblSlackStatus.Text = "label1";
            // 
            // slackTrayIcon
            // 
            this.slackTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("slackTrayIcon.Icon")));
            this.slackTrayIcon.Visible = true;
            this.slackTrayIcon.DoubleClick += new System.EventHandler(this.slackTrayIcon_DoubleClick);
            // 
            // trayContextMenu
            // 
            this.trayContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.snoozeStripMenuItem,
            this.unsnoozeStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.trayContextMenu.Name = "trayContextMenu";
            this.trayContextMenu.Size = new System.Drawing.Size(153, 98);
            this.trayContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.trayContextMenu_Opening);
            // 
            // snoozeStripMenuItem
            // 
            this.snoozeStripMenuItem.Name = "snoozeStripMenuItem";
            this.snoozeStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.snoozeStripMenuItem.Text = "Snooze all";
            this.snoozeStripMenuItem.ToolTipText = "Don\'t show unread/mention notifications for a while";
            this.snoozeStripMenuItem.Click += new System.EventHandler(this.snoozeStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // unsnoozeStripMenuItem
            // 
            this.unsnoozeStripMenuItem.Name = "unsnoozeStripMenuItem";
            this.unsnoozeStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.unsnoozeStripMenuItem.Text = "Unsooze all";
            this.unsnoozeStripMenuItem.Visible = false;
            this.unsnoozeStripMenuItem.Click += new System.EventHandler(this.unsnoozeStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 126);
            this.Controls.Add(this.lblSlackStatus);
            this.Name = "MainWindow";
            this.ShowInTaskbar = false;
            this.Text = "Form1";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.trayContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSlackStatus;
        private System.Windows.Forms.NotifyIcon slackTrayIcon;
        private System.Windows.Forms.ContextMenuStrip trayContextMenu;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem snoozeStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem unsnoozeStripMenuItem;

    }
}

