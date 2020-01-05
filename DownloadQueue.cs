using System.Collections.Generic;

namespace FuseeGUI {
    public class DownloadQueue {
        private IList<FileDownloader> filesToDownload;

        public DownloadQueue() {
            this.filesToDownload = new List<FileDownloader>();
        }

        public bool downloadFiles() {
            foreach (var downloader in filesToDownload) {
                if (!downloader.downloadFile())
                    return false;
            }

            return true;
        }

        public void addToQueue(FileDownloader fileDownloader) {
            if (!filesToDownload.Contains(fileDownloader))
                filesToDownload.Add(fileDownloader);
        }

        public void clearQueue() {
            filesToDownload.Clear();
        }

        public bool isEmpty() {
            return filesToDownload.Count == 0;
        }
    }
}