// =====COPYRIGHT=====
// Code originally retrieved from http://www.vbforums.com/showthread.php?t=547778 - no license information supplied
// =====COPYRIGHT=====
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Windows.Forms;

namespace ToastNotifications
{
    public partial class Notification : Form
    {
        private static List<Notification> openNotifications = new List<Notification>();
        private bool _allowFocus;
        private readonly FormAnimator _animator;
        private IntPtr _currentForegroundWindow;
        private List<string> messages = new List<string>();
        private string _channelId;

        public event EventHandler<string> OnQuickReply = delegate { };


        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="body"></param>
        /// <param name="durationSeconds"></param>
        /// <param name="animation"></param>
        /// <param name="direction"></param>
        public Notification(string channelId, string channelName, int durationSeconds, FormAnimator.AnimationMethod animation, FormAnimator.AnimationDirection direction)
        {
            InitializeComponent();

            if (durationSeconds < 0)
                durationSeconds = int.MaxValue;
            else
                durationSeconds = durationSeconds * 1000;

            lifeTimer.Interval = durationSeconds;
            _channelId = channelId;
            labelTitle.Text = channelName;

            _animator = new FormAnimator(this, animation, direction, 500);

            Region = Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, Width - 5, Height - 5, 20, 20));

            htmlBody.Text = "<head>" +
                            "<style>" +
                            "   p {margin: 0}" +
                            "   .username {font-weight: bold; }" +
                            "   .incoming {color: yellow; }" +
                            "   .outgoing {color: white; }" +
                            "</style>" +
                            "</head>";

            MouseMove += AllControls_MouseMove;
            foreach (Control control in this.Controls)
            {
                control.MouseMove += AllControls_MouseMove;
            }
        }

        public string ChannelId
        {
            get { return _channelId; }
        }

        #region Methods

        /// <summary>
        /// Displays the form
        /// </summary>
        /// <remarks>
        /// Required to allow the form to determine the current foreground window before being displayed
        /// </remarks>
        public new void Show()
        {
            // Determine the current foreground window so it can be reactivated each time this form tries to get the focus
            _currentForegroundWindow = NativeMethods.GetForegroundWindow();

            base.Show();
        }

        private int _messageCounter = 0;
        public void AddMessage(string username, string message, bool isIncoming)
        {
            string messageId = "message" + _messageCounter;
            _messageCounter++;
            
            // Add the newest message
            messages.Add(message);
            string direction = isIncoming ? "incoming" : "outgoing";
            htmlBody.Text += string.Format("<p id=\"{0}\" class=\"{1}\"><span class=\"username\">{2}</span>: {3}</p>", 
                                            messageId, direction, username, HttpUtility.HtmlEncode(message));
            htmlBody.ScrollToElement(messageId);

            lifeTimer.Stop();
            lifeTimer.Start();
        }

        #endregion // Methods

        #region Event Handlers

        private void Notification_Load(object sender, EventArgs e)
        {
            var totalOpenHeight = openNotifications.Count * Height;

            // Display the form just above the system tray.
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width,
                                      Screen.PrimaryScreen.WorkingArea.Height - Height - totalOpenHeight);

            // Move each open form upwards to make room for this one
            //foreach (Notification openForm in openNotifications)
            //{
            //    openForm.Top -= Height;
            //}

            openNotifications.Add(this);
            lifeTimer.Start();
        }

        private void Notification_Activated(object sender, EventArgs e)
        {
            // Prevent the form taking focus when it is initially shown
            if (!_allowFocus)
            {
                // Activate the window that previously had focus
                NativeMethods.SetForegroundWindow(_currentForegroundWindow);
            }
        }

        private void Notification_Shown(object sender, EventArgs e)
        {
            // Once the animation has completed the form can receive focus
            _allowFocus = true;

            // Close the form by sliding down.
            _animator.Duration = 0;
            _animator.Direction = FormAnimator.AnimationDirection.Right;
        }

        private void Notification_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Move down any open forms above this one
            var isAbove = false;
            foreach (Notification openForm in openNotifications)
            {
                if (openForm == this)
                {
                    // Remaining forms are above this one
                    isAbove = true;
                }
                if (isAbove)
                {
                    openForm.Top += Height;
                }
            }

            openNotifications.Remove(this);
        }

        private void lifeTimer_Tick(object sender, EventArgs e)
        {
            Close();
        }

        private void Notification_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void labelTitle_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void labelRO_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion // Event Handlers

        private void txtQuickReply_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtQuickReply.Text))
            {
                OnQuickReply(this, txtQuickReply.Text);
                txtQuickReply.Text = "";
            }
        }

        private void AllControls_MouseMove(object sender, EventArgs e)
        {
            lifeTimer.Stop();
            lifeTimer.Start();
        }
    }
}