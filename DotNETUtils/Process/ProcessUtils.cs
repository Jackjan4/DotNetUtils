using System.Diagnostics;

namespace Roslan.DotNetUtils.Process {
    public static class ProcessUtils {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="processPath"></param>
        /// <param name="arguments"></param>
        /// <param name="useShellExecute"></param>
        public static void StartProcess(string processPath, string arguments = "", bool useShellExecute = false) {
            var startInfo = new ProcessStartInfo(processPath, arguments) {
                UseShellExecute = useShellExecute,
            };

            System.Diagnostics.Process.Start(startInfo);
        }
    }
}
