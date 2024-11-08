using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslan.DotNetUtils.IO;
using Xunit.Abstractions;

namespace DotNetUtils.TestNet8._0 {
	public class FileUtilsTest {

		private readonly ITestOutputHelper _output;

		public FileUtilsTest(ITestOutputHelper output) {
			_output = output;
		}


		[Fact]
		public async void TestBigFileCopyAsync() {
			var sourceFile = @"C:\Program Files\brand\Starter3\Starter3.exe";
			var destFile = @"\\srvfbapp\brand_tools$\Starter3\setup\Starter3.exe";

			var progress = new Progress<long>(x => {
				//_output.WriteLine("Copied: {0} of {1} bytes {2}", x.Item1, x.Item2, x.Item3);
			});

			var sw = new Stopwatch();

			sw.Restart();
			await FileUtils.CopyFileDeltaAsync(sourceFile, destFile, new FileCopyDeltaOptions(1028 * 128, 1028 * 128, FileCompareMethod.LastWriteTime, true), progress);
			sw.Stop();

			_output.WriteLine("Time: {0}", sw.ElapsedMilliseconds);
		}
	}
}
