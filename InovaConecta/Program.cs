using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace InovaConecta {
    internal static class Program {
        private static Mutex mutex = new Mutex(true, "InovaConecta_UniqueMutexName");

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private const int SW_RESTORE = 9;

        [STAThread]
        static void Main() {
            if (!mutex.WaitOne(TimeSpan.Zero, true)) {
                IntPtr hWnd = FindWindow(null, "InovaConecta");

                if (hWnd != IntPtr.Zero) {
                    ShowWindow(hWnd, SW_RESTORE);
                    SetForegroundWindow(hWnd);
                }

                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            mutex.ReleaseMutex();
        }
    }
}
