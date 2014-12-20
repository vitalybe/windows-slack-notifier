using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SlackWindowsTray.Default.SnoozeAllTimeMinutes = int.Parse(txtSnoozeAllMinutes.Text);
                SlackWindowsTray.Default.SnoozeChatTimeMinutes = int.Parse(txtSnoozeChatMinutes.Text);

                SlackWindowsTray.Default.ToBlinkOnUnread = chkToBlinkUnread.Checked;
                SlackWindowsTray.Default.ToBlinkOnMention = chkToBlinkMention.Checked;

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
    }
}
