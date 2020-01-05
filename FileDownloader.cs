using System;
using System.IO;
using System.Net;

namespace FuseeGUI {
    public class FileDownloader {
        private MainWindow window;
        private string fileName;
        private string fileURL;
        private string fileLocation;

        public FileDownloader(MainWindow window, string filename, string fileURL, string fileLocation) {
            this.window = window;
            this.fileName = filename;
            this.fileURL = fileURL;
            this.fileLocation = fileLocation;
        }

        // Returns true if download succeeded.
        public bool downloadFile() {
            //window.sendMessage($"Attempting to download {fileName} from GitHub");
            var bytesProcessed = 0;
            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;

            try {
                var request = WebRequest.Create(fileURL);
                if (request == null) return false;

                response = request.GetResponse();
                if (response == null) return false;

                remoteStream = response.GetResponseStream();
                if (remoteStream == null) return false;
                localStream = File.Create(fileLocation);

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                do {
                    bytesRead = remoteStream.Read(buffer, 0, buffer.Length);
                    localStream.Write(buffer, 0, bytesRead);
                    bytesProcessed += bytesRead;
                } while (bytesRead > 0);

                localStream.Close();
                remoteStream.Close();
                return true;
            } catch (Exception ex) {
                //F
            }

            return false;
        }
    }
}