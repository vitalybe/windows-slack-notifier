using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            _wssv.AddWebSocketService<Slack>("/Slack");

            _wssv.Start();
            if (_wssv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", _wssv.Port);
                foreach (var path in _wssv.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _wssv.Stop();
        }
    }
}
