using System;
using System.Windows.Forms;

namespace SlackWindowsTray
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            txtSnoozeAllMinutes.Text = SlackWindowsTray.Default.SnoozeAllTimeMinutes.ToString();
            txtSnoozeChatMinutes.Text = SlackWindowsTray.Default.SnoozeChatTimeMinutes.ToString();

            chkToBlinkUnread.Checked = SlackWindowsTray.Default.ToBlinkOnUnread;
            chkToBlinkMention.Checked = SlackWindowsTray.Default.ToBlinkOnMention;

            txtSlackToken.Text = SlackWindowsTray.Default.SlackToken;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SlackWindowsTray.Default.SnoozeAllTimeMinutes = int.Parse(txtSnoozeAllMinutes.Text);
                SlackWindowsTray.Default.SnoozeChatTimeMinutes = int.Parse(txtSnoozeChatMinutes.Text);

                SlackWindowsTray.Default.ToBlinkOnUnread = chkToBlinkUnread.Checked;
                SlackWindowsTray.Default.ToBlinkOnMention = chkToBlinkMention.Checked;

                SlackWindowsTray.Default.SlackToken = txtSlackToken.Text;

                SlackWindowsTray.Default.Save();

                this.Close();
            }
            catch (FormatException exception)
            {
                MessageBox.Show("All snooze times must be integers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkGenerateToken_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
