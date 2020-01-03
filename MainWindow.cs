using System;
using System.Diagnostics;
using System.IO;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace FuseeGUI {
    class MainWindow : Window {
        [UI] private Button smashPayloadBtn = null;

        public MainWindow() : this(new Builder("MainWindow.glade")) {
            /*
            var start = new ProcessStartInfo();
            start.FileName = @"python3";
            start.Arguments = "--vesrsion";
            start.UseShellExecute = false;
            start.RedirectStandardError = true;
            start.CreateNoWindow = true;

            using (Process process = Process.Start(start)) {
                using (StreamReader reader = process.StandardError) {
                    string result = reader.ReadToEnd();
                    if (result.Contains("Command \'python3\' not found.")) {
                        
                    }
                }
            }*/
        }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle) {
            builder.Autoconnect(this);

            DeleteEvent += windowDeleteEvent;
            smashPayloadBtn.ButtonPressEvent += smashPayloadBtnEvent;
        }

        private void smashPayloadBtnEvent(object sender, EventArgs args) {
            smashPayloadBtn.Label = "Smashed!";
        }

        private void windowDeleteEvent(object sender, DeleteEventArgs a) {
            Application.Quit();
        }
    }
}