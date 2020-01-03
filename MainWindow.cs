using System;
using System.IO;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace FuseeGUI {
    public class MainWindow : Window {
        [UI] private Button smashPayloadBtn = null;
        [UI] private Button clearLogBtn = null;
        [UI] private TextView smashOutput = null;
        [UI] private FileChooserButton payloadBtn = null;
        [UI] private ScrolledWindow scrolledArea = null;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle) {
            this.SetIconFromFile(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "nintendo-switch.png")); 
            builder.Autoconnect(this);

            // I had added this in glade, but it wouldn't take affect.
            var binFilter = new FileFilter();
            binFilter.AddPattern("*.bin");
            payloadBtn.AddFilter(binFilter);

            DeleteEvent += windowDeleteEvent;
            smashPayloadBtn.ButtonReleaseEvent += smashPayloadBtnEvent;
            clearLogBtn.ButtonReleaseEvent += clearLogBtnEvent;
        }

        private void smashPayloadBtnEvent(object sender, EventArgs args) {
            if (payloadBtn.File == null) {
                sendMessage("Please choose a payload first!");
                return;
            }

            sendMessage(string.Format("Attempting to smash with {0}.", payloadBtn.File.Basename));

            // Checking prerequisites
            if (!FuseeHandler.checkAndDownloadFusee(this)) {
                sendMessage("Could not download Fusée Launcher. Maybe your connection got interrupted?");
                return;
            }
            if (!FuseeHandler.hasPython3()) {
                sendMessage("Please install Python 3 before continuing! Canceling smash...");
                return;
            }

            FuseeHandler.smashPayload(this, payloadBtn.File.Path);
        }

        private void clearLogBtnEvent(object sender, EventArgs args) {
            smashOutput.Buffer.Text = "";
        }

        private void windowDeleteEvent(object sender, DeleteEventArgs a) {
            Application.Quit();
        }

        public void sendMessage(string msg) {
            smashOutput.Buffer.Text += msg + "\n";
            scrolledArea.Vadjustment.Value = scrolledArea.Vadjustment.Lower;
        }
    }
}