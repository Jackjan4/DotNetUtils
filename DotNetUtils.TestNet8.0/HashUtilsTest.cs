using Roslan.DotNETUtils.Crypto;
using Roslan.DotNETUtils.Files;
using Xunit.Abstractions;

namespace DotNetUtils.TestNet8._0 {
    public class HashUtilsTest {

        private readonly ITestOutputHelper _output;

        public HashUtilsTest(ITestOutputHelper output)
        {
            _output = output;
        }   

        [Fact]
        public void Test1()
        {
            bool test = FileUtils.CompareMd5FileHashes("C:\\Users\\Jackj\\Desktop\\test1.txt",
                "C:\\Users\\Jackj\\Desktop\\test2.txt");

            _output.WriteLine(test.ToString());
        }
    }
}