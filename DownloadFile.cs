using System;
using System.ComponentModel;
using System.Net;

namespace FuseeGUI {
    public class DownloadFile {
        private MainWindow window;
        private Uri fileUri;
        private bool downloadCompleted;
        private bool failed;

        public DownloadFile(MainWindow window, string url) {
            this.window = window;
            this.fileUri = new Uri(url);
        }

        public void download(string location) {
            var client = new WebClient();
            client.DownloadFileCompleted += downloadComplete;
            client.DownloadProgressChanged += downloadProgress;
            client.DownloadFileAsync(fileUri, location);
        }

        private void downloadProgress(object sender, DownloadProgressChangedEventArgs args) {
            window.sendMessage(string.Format("{0} downloaded {1} of {2} bytes. {3} % complete...",
                (string) args.UserState,
                args.BytesReceived,
                args.TotalBytesToReceive,
                args.ProgressPercentage));
        }

        private void downloadComplete(object sender, AsyncCompletedEventArgs args) {
            if (args.Cancelled)
                failed = true;
            downloadCompleted = true;
        }

        public bool finishedDownload {
            get => downloadCompleted;
        }

        public bool downloadFailed {
            get => failed;
        }
    }
}