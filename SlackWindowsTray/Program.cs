using System;
using System.Windows.Forms;

namespace SlackWindowsTray
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (string.IsNullOrWhiteSpace(SlackWindowsTray.Default.SlackToken))
            {
                var form = new OptionsForm();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ShowDialog();
            }

            if (string.IsNullOrWhiteSpace(SlackWindowsTray.Default.SlackToken))
            {
                MessageBox.Show("Valid slack token must be set to continue. Application will now quit",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Application.Run(new MainWindow());
            }
        }
    }
}
