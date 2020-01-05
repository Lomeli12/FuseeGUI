using System.Diagnostics;
using System.IO;

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

        public static void checkAndDownloadFusee(MainWindow window, DownloadQueue queue) {
            if (!Directory.Exists(FUSEE_FOLDER)) {
                Directory.CreateDirectory(FUSEE_FOLDER);
                window.sendMessage("Missing fusee folder. Please wait while we download Fusée Launcher!");
            }

            foreach (var fuseeFile in FUSEE_LAUNCHER_FILES) {
                if (File.Exists(Path.Combine(FUSEE_FOLDER, fuseeFile))) continue;

                window.sendMessage($"Missing {fuseeFile}. Adding to download queue");
                var url = FUSEE_RAW_GITHUB_LINK + fuseeFile;
                var file = Path.Combine(FUSEE_FOLDER, fuseeFile);

                queue.addToQueue(new FileDownloader(window, fuseeFile, url, file));
            }
        }

        public static void smashPayload(MainWindow window, string path) {
            PYTHON_PROCESS.Arguments = Path.Combine(FUSEE_FOLDER, FUSEE_LAUNCHER_FILES[0]) + " " + path;
            using var process = Process.Start(PYTHON_PROCESS);
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

        public static bool hasPython3() {
            PYTHON_PROCESS.Arguments = "--version";
            using (var process = Process.Start(PYTHON_PROCESS)) {
                if (process == null) return false;
                using var reader = process.StandardOutput;
                var result = reader.ReadToEnd();
                if (result.StartsWith("Python 3"))
                    return true;
            }

            return false;
        }
    }
}