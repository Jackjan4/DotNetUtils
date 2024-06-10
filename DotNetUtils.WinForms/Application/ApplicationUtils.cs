using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace DotNetUtils.WinForms.Application {
    public class ApplicationUtils {


        public static void RestartApplicationWithoutArgs() {
            var currentStartInfo = new ProcessStartInfo() {
                FileName = System.Windows.Forms.Application.ExecutablePath,
                Arguments = ""
            };

            System.Windows.Forms.Application.Exit();
            Process.Start(currentStartInfo);
        }
    }
}

