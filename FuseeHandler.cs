using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Process = System.Diagnostics.Process;

namespace FuseeGUI {
    public class FuseeHandler {
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

        private static readonly ProcessStartInfo PYTHON_PROCESS = new ProcessStartInfo {
            FileName = @"python3",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        public static bool checkAndDownloadFusee(MainWindow window) {
            var success = true;
            if (!Directory.Exists(FUSEE_FOLDER)) {
                Directory.CreateDirectory(FUSEE_FOLDER);
                window.sendMessage("Missing fusee folder. Please wait while we download Fusée Launcher!");
            }

            foreach (var fuseeFile in FUSEE_LAUNCHER_FILES) {
                if (!File.Exists(Path.Combine(FUSEE_FOLDER, fuseeFile))) {
                    window.sendMessage(string.Format("Missing {0}. Downloading {0} from GitHub!", fuseeFile));
                    
                    var download = new DownloadFile(window, FUSEE_RAW_GITHUB_LINK + fuseeFile);
                    download.download(Path.Combine(FUSEE_FOLDER, fuseeFile));

                    if (download.downloadFailed) {
                        window.sendMessage(string.Format("Failed to download {0}!", fuseeFile));
                        success = false;
                        break;
                    }

                    window.sendMessage(string.Format("Finished downloading {0}!", fuseeFile));
                }
            }

            return success;
        }

        public static void smashPayload(MainWindow window, string path) {
            PYTHON_PROCESS.Arguments = Path.Combine(FUSEE_FOLDER, FUSEE_LAUNCHER_FILES[0]) + " " + path;
            using (var process = Process.Start(PYTHON_PROCESS)) {
                using (var reader = process.StandardOutput) {
                    var result = reader.ReadToEnd().Trim();
                    if (!string.IsNullOrWhiteSpace(result))
                        window.sendMessage(result);
                }

                using (var reader = process.StandardError) {
                    var result = reader.ReadToEnd().Trim();
                    if (!string.IsNullOrWhiteSpace(result))
                        window.sendMessage(result);
                }
            }
        }

        private static void downloadFile(string file) {
            var client = new WebClient();
            var filePath = Path.Combine(FUSEE_FOLDER, file);
            var fileURI = new Uri(FUSEE_RAW_GITHUB_LINK + file);


            /*await new Task(() => {
                var filePath = Path.Combine(FUSEE_FOLDER, file);
                var fileURL = FUSEE_RAW_GITHUB_LINK + file;
                var net = new WebClient();
                var data = net.DownloadData(fileURL);
                File.WriteAllBytes(filePath, data);
            });*/
        }

        public static bool hasPython3() {
            PYTHON_PROCESS.Arguments = "--version";
            using (var process = Process.Start(PYTHON_PROCESS)) {
                using (var reader = process.StandardOutput) {
                    var result = reader.ReadToEnd();
                    if (result.StartsWith("Python 3"))
                        return true;
                }
            }

            return false;
        }
    }
}