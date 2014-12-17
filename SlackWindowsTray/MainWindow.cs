using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using WebSocketSharp.Server;
using System.IO;
using System.Threading.Tasks;

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

    class StateService
    {
        private WebSocketServer _wssv = new WebSocketServer(4649);
        private StateProcessorBase _stateProcessor;
        private SlackNotifierStates _lastSlackState;

        private StateService()
        {
            _stateProcessor = new StartProcessor();
            _stateProcessor.AddProcessor(new StateCallbackProcessor(state => OnStateChange(null, state)));
            _stateProcessor.AddProcessor(new StateAnimationProcessor());

            _stateProcessor.HandleState(SlackNotifierStates.DisconnectedFromExtension);

            ConnectionToExtension();
        }

        private void ConnectionToExtension()
        {
            _wssv.AddWebSocketService<SlackEndpoint>("/Slack");
            SlackEndpoint.OnSlackStateChanged += (o, state) => UpdateState(state);

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

        private void UpdateState(SlackNotifierStates state)
        {
            _lastSlackState = state;

            _stateProcessor.HandleState(state);
        }

        public static readonly StateService Instance = new StateService();

        public event EventHandler<SlackNotifierStates> OnStateChange = delegate { };

        public async Task Snooze()
        {
            var snoozingProcessor = new SnoozingProcessor(_lastSlackState);
            _stateProcessor.AddProcessor(snoozingProcessor);
            await Task.Delay(TimeSpan.FromSeconds(10));
            _stateProcessor.RemoveProcessor(snoozingProcessor);
        }
    }

    abstract class StateProcessorBase
    {
        protected enum StateProcessorPriorityEnum
        {
            Start,
            Snoozing,
            Animation,
            Callback,
        }

        protected StateProcessorBase _nextProcessor = null;

        protected abstract StateProcessorPriorityEnum Priority { get; }

        protected abstract bool HandleStateRaw(SlackNotifierStates state);

        protected virtual void OnAdd() { }
        protected virtual void OnRemove() { }

        public void AddProcessor(StateProcessorBase newProcessor)
        {
            if (_nextProcessor == null || _nextProcessor.Priority >= newProcessor.Priority)
            {
                newProcessor._nextProcessor = _nextProcessor;
                this._nextProcessor = newProcessor;

                newProcessor.OnAdd();
            }
            else
            {
                _nextProcessor.AddProcessor(newProcessor);
            }
        }

        public void RemoveProcessor(StateProcessorBase removedProcessor)
        {
            if (_nextProcessor == removedProcessor)
            {
                _nextProcessor.OnRemove();
                this._nextProcessor = removedProcessor._nextProcessor;
            }
            else if(_nextProcessor != null)
            {
                _nextProcessor.RemoveProcessor(removedProcessor);
            }

        }

        
        public void HandleState(SlackNotifierStates state)
        {
            HandleState(state, skipMyself: false);
        }

        protected void NextHandleState(SlackNotifierStates state)
        {
            HandleState(state, skipMyself: true);            
        }

        private  void HandleState(SlackNotifierStates state, bool skipMyself)
        {
            var continueToNextProcessor = true;
            if (!skipMyself)
            {
                continueToNextProcessor = this.HandleStateRaw(state);
            }

            if (continueToNextProcessor && _nextProcessor != null)
            {
                _nextProcessor.HandleState(state);
            }
        }

    }

    class SnoozingProcessor : StateProcessorBase
    {
        private SlackNotifierStates _lastState;
        private SlackNotifierStates _lastSlackState;

        public SnoozingProcessor(SlackNotifierStates _lastSlackState)
        {
            _lastState = _lastSlackState;
        }

        protected override StateProcessorPriorityEnum Priority
        {
            get { return StateProcessorPriorityEnum.Snoozing; }
        }

        protected override void OnAdd()
        {
            NextHandleState(SlackNotifierStates.AllRead);
        }

        protected override void OnRemove()
        {
            NextHandleState(_lastState);
        }

        protected override bool HandleStateRaw(SlackNotifierStates state)
        {
            _lastState = state;

            return false;
        }
    }
}