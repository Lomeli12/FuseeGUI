using System.IO;
using System.Net;

namespace FuseeGUI {
    public class FuseeDownload {
        public static readonly string FUSEE_FOLDER = Path.Combine(Directory.GetCurrentDirectory(), "fusee");

        private static readonly string FUSEE_RAW_GITHUB_LINK =
            "https://raw.githubusercontent.com/Qyriad/fusee-launcher/master/";
        public static readonly string[] FUSEE_LAUNCHER_FILES = {
            "fusee-launcher.py",
            "intermezzo.S",
            "intermezzo.bin",
            "intermezzo.lds",
            "libusbK.py",
            "LICENSE"
        };

        public static void checkAndDownloadFusee() {
            if (!Directory.Exists(FUSEE_FOLDER))
                Directory.CreateDirectory(FUSEE_FOLDER);

            foreach (var fuseeFiles in FUSEE_LAUNCHER_FILES) {
                if (!File.Exists(fuseeFiles)) {
                    downloadFile(fuseeFiles);
                }
            }
        }

        private static void downloadFile(string file) {
            var filePath = Path.Combine(FUSEE_FOLDER, file);
            var fileURL = FUSEE_RAW_GITHUB_LINK + file;
            var net = new WebClient();
            var data = net.DownloadData(fileURL);
            File.WriteAllBytes(filePath, data);
        }
    }
}