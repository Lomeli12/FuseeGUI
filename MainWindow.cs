using System;
using System.ComponentModel;
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
        private BackgroundWorker backgroundWorker;
        private DownloadQueue queue = new DownloadQueue();
        private bool failed;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle) {
            this.SetIconFromFile(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "nintendo-switch.png"));
            backgroundWorker = new BackgroundWorker();
            builder.Autoconnect(this);

            // I had added this in glade, but it wouldn't take affect.
            var binFilter = new FileFilter();
            binFilter.AddPattern("*.bin");
            payloadBtn.AddFilter(binFilter);

            DeleteEvent += windowDeleteEvent;
            smashPayloadBtn.ButtonReleaseEvent += smashPayloadBtnEvent;
            clearLogBtn.ButtonReleaseEvent += clearLogBtnEvent;
            backgroundWorker.RunWorkerCompleted += backgroundWorkingComplete;
            backgroundWorker.DoWork += backgroundDoWork;
        }

        private void smashPayloadBtnEvent(object sender, EventArgs args) {
            if (payloadBtn.File == null) {
                sendMessage("Please choose a payload first!");
                return;
            }

            sendMessage($"Attempting to smash with {payloadBtn.File.Basename}.");

            // Checking prerequisites
            if (!FuseeHandler.hasPython3()) {
                sendMessage("Please install Python 3 before continuing! Canceling smash...");
                return;
            }

            //var queue = new DownloadQueueOld();
            FuseeHandler.checkAndDownloadFusee(this, queue);
            if (queue.isEmpty()) {
                FuseeHandler.smashPayload(this, payloadBtn.File.Path);
            } else {
                sendMessage("Attempting to download missing files");
                setStates(StateFlags.Insensitive);
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void backgroundDoWork(object sender, DoWorkEventArgs args) {
            if (queue.isEmpty()) return;
            if (!queue.downloadFiles()) {
                failed = true;
            }

            queue.clearQueue();
            backgroundWorker.CancelAsync();
        }

        private void backgroundWorkingComplete(object sender, RunWorkerCompletedEventArgs args) {
            if (failed) {
                failed = false;
                sendMessage("Failed to download missing files! Stopping...");
            } else
                FuseeHandler.smashPayload(this, payloadBtn.File.Path);

            setStates(StateFlags.Active);
        }

        private void clearLogBtnEvent(object sender, EventArgs args) {
            smashOutput.Buffer.Text = "";
        }

        private void windowDeleteEvent(object sender, DeleteEventArgs a) {
            Application.Quit();
        }

        public void sendMessage(string msg) {
            var endIter = smashOutput.Buffer.EndIter;
            smashOutput.Buffer.Insert(endIter, msg + "\n");
            scrolledArea.Vadjustment.Value = scrolledArea.Vadjustment.Upper;
        }

        private void setStates(StateFlags state) {
            smashPayloadBtn.SetStateFlags(state, true);
            clearLogBtn.SetStateFlags(state, true);
            payloadBtn.SetStateFlags(state, true);
        }
    }
}