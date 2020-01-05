using System;
using GLib;
using Application = Gtk.Application;

namespace FuseeGUI {
    class Program {
        [STAThread]
        public static void Main(string[] args) {
            Application.Init();

            var app = new Application("net.lomeli.FuseeGUI", ApplicationFlags.None);
            app.Register(Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
    }
}