using System;
using System.Windows.Forms;
using WebSocketSharp.Server;

namespace SlackWindowsTray
{
    public partial class MainWindow : Form
    {
        private WebSocketServer _wssv = new WebSocketServer(4649);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            UpdateSlackState(SlackNotifierStates.Disconnected);
                
            _wssv.AddWebSocketService<SlackEndpoint>("/Slack");
            SlackEndpoint.OnSlackStateChanged += (o, state) => this.UIThread(delegate { UpdateSlackState(state); }); 

            _wssv.Start();
            if (_wssv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", _wssv.Port);
                foreach (var path in _wssv.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }
        }

        private void UpdateSlackState(SlackNotifierStates slackNotifierStates)
        {
            lblSlackStatus.Text = slackNotifierStates.ToString();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _wssv.Stop();
        }
    }
}