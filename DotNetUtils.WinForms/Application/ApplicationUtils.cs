using System.Diagnostics;



namespace Roslan.DotNetUtils.WinForms.Application;



public class ApplicationUtils {



    /// <summary>
    /// Restarts the currently running WinForms application without any arguments.
    /// This function is an alternative to Application.Restart() since that will restart the application with the same arguments that it was already started with.
    /// </summary>
    public static void RestartApplicationNoArgs() {
        var currentStartInfo = new ProcessStartInfo {
            FileName = System.Windows.Forms.Application.ExecutablePath,
            Arguments = ""
        };

        System.Windows.Forms.Application.Exit();
        Process.Start(currentStartInfo);
    }
}