using System;
using Gtk;

namespace FuseeGUI {
    class Program {
        [STAThread]
        public static void Main(string[] args) {
            FuseeDownload.checkAndDownloadFusee();
            Application.Init();

            var app = new Application("net.lomeli.FuseeGUI", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
    }
}