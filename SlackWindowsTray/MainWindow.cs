using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace SlackWindowsTray
{
    public partial class MainWindow : Form
    {
        private StateService _stateService = StateService.Instance;

        public MainWindow()
        {
            InitializeComponent();
            slackTrayIcon.ContextMenuStrip = trayContextMenu;

            _stateService.OnStateChange += (o, state) => this.UIThread(delegate { ChangeSlackState(state); });
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // Add the notifier to Windows startup:
            try
            {
                RegistryKey currentVersionRunRegKey = Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                string startPath = Assembly.GetExecutingAssembly().Location;
                currentVersionRunRegKey.SetValue("SlackWindowsTray", '"' + startPath + '"');
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add SlackWindowsTray to run on startup: " + ex.Message);
            }
        }

        private void ChangeSlackState(SlackNotifierStates newState)
        {
            // Change the icon and the tooltip
            slackTrayIcon.Text = newState.ToString();

            var appDir = Path.GetDirectoryName(Application.ExecutablePath);
            var iconPath = Path.Combine(appDir, "Icons", newState.ToString() + ".ico");
            slackTrayIcon.Icon = new Icon(iconPath);
        }

        private void slackTrayIcon_DoubleClick(object sender, EventArgs e)
        {
            var activated = ChromeActivator.ActivateChromeWindowByTitle(window => window.Title.EndsWith(" Slack"));
            if (!activated)
            {
                MessageBox.Show("Couldn't find Slack window");
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void snoozeStripMenuItem_Click(object sender, EventArgs e)
        {
            snoozeStripMenuItem.Visible = false;
            await _stateService.Snooze();
            snoozeStripMenuItem.Visible = true;
        }
    }
}