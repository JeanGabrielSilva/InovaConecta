using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace InovaConecta {
    internal static class Program {
        private static Mutex mutex = new Mutex(true, "InovaConecta_UniqueMutexName");

        // Importar funções do Windows para ativar janela
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
                // Procura a janela pelo título do seu Form (ajuste para o título correto da sua janela)
                IntPtr hWnd = FindWindow(null, "InovaConecta");

                if (hWnd != IntPtr.Zero) {
                    ShowWindow(hWnd, SW_RESTORE);   // Restaura a janela se estiver minimizada
                    SetForegroundWindow(hWnd);      // Traz a janela para frente
                }

                return; // Sai da segunda instância
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            mutex.ReleaseMutex();
        }
    }
}
