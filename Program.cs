using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace ShutdownWarning4Germignon
{
    class Program
    {
        static void OnSessionEnding(object sender, SessionEndingEventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Haben Sie wirklich alles erledigt?", "Erinnerung", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                switch (result)
                {
                    case DialogResult.Yes:
                        ProcessStartInfo psi = new ProcessStartInfo("shutdown", "/s /t 0")
                        {
                            CreateNoWindow = true,
                            UseShellExecute = false
                        };
                        Process.Start(psi);
                        break;
                    case DialogResult.No:
                        MessageBox.Show("Es wird jetzt ein leeres Fenster im Editor geöffnet. Sobald Sie erledigt haben, was Sie noch tun wollten, schließen Sie dieses bitte. Erst dann wird der Computer herunterfahren.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process process = new Process();
                        process.StartInfo.FileName = "notepad.exe";
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                        process.Start();
                        process.WaitForExit();
                        break;
                    default:
                        MessageBox.Show("Unbekannter Fehler.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }
            }
            catch (Exception fehler)
            {
                MessageBox.Show("Fehler:" + Environment.NewLine + fehler.Message + Environment.NewLine + "Bitte wenden Sie sich an Ihren Systemadministrator.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                SystemEvents.SessionEnding += new SessionEndingEventHandler(OnSessionEnding);
            }
            catch (Exception fehler)
            {
                MessageBox.Show("Fehler:" + Environment.NewLine + fehler.Message + Environment.NewLine + "Bitte wenden Sie sich an Ihren Systemadministrator.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //this is only required, if SessionEndingEventHandlers need the app
            //running, which i assume is the case
            while (true)
            {
                try
                {
                    Thread.Sleep(int.MaxValue);
                }
                catch { } //ignore all errors while sleeping
            }
        }
    }
}
