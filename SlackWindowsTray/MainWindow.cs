using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WebSocketSharp.Server;
using Timer = System.Windows.Forms.Timer;

namespace SlackWindowsTray
{
    public partial class MainWindow : Form
    {
        private WebSocketServer _wssv = new WebSocketServer(4649);

        private bool _animationIconBlink = true;
        private Timer _animationTimer = new Timer(); 
        private SlackNotifierStates _lastState = SlackNotifierStates.DisconnectedFromExtension;

        public MainWindow()
        {
            InitializeComponent();
            slackTrayIcon.ContextMenuStrip = trayContextMenu;

            _animationTimer.Interval = 500;
            _animationTimer.Tick += AnimationTimerOnTick;
            _animationTimer.Enabled = false;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            UpdateSlackState(SlackNotifierStates.DisconnectedFromExtension);
                
            _wssv.AddWebSocketService<SlackEndpoint>("/Slack");
            SlackEndpoint.OnSlackStateChanged += (o, state) => this.UIThread(delegate { UpdateSlackState(state); }); 

            _wssv.Start();
            if (_wssv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", _wssv.Port);
                foreach (var path in _wssv.WebSocketServices.Paths)
                {
                    Console.WriteLine("- {0}", path);
                }
            }
        }

        private void UpdateSlackState(SlackNotifierStates newState)
        {
            _lastState = newState;
            
            // Change the icon and the tooltip
            slackTrayIcon.Text = newState.ToString();
            ChangeTrayIcon(newState);

            // Start the animation if possible and enabled
            var canAnimateIcon = newState == SlackNotifierStates.ImportantUnread ||
                                 newState == SlackNotifierStates.Unread;
            _animationTimer.Enabled = canAnimateIcon;
        }

        private void ChangeTrayIcon(SlackNotifierStates state)
        {
            slackTrayIcon.Icon = new Icon(@"Icons\" + state.ToString() + ".ico");
        }

        private void AnimationTimerOnTick(object sender, EventArgs eventArgs)
        {
            ChangeTrayIcon(_animationIconBlink ? SlackNotifierStates.AllRead : _lastState);

            _animationIconBlink = !_animationIconBlink;
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _wssv.Stop();
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
    }
}