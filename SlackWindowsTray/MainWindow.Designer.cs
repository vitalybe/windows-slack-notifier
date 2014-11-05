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
            this.lblSlackStatus = new System.Windows.Forms.Label();
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
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 126);
            this.Controls.Add(this.lblSlackStatus);
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSlackStatus;

    }
}

