using System;
using System.Security.Permissions;
using System.Windows.Forms;
using Breakthrough.Client.Forms;

[assembly : FileIOPermission(SecurityAction.RequestMinimum)]
[assembly : CLSCompliant(false)]

namespace Breakthrough.Client
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IsRunning = true;
            MainForm mainForm = new MainForm();

            Application.Run(mainForm);
        }

        internal static bool IsRunning = false;
    }
}