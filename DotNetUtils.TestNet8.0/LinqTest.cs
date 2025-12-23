using Roslan.DotNetUtils.IO;
using Roslan.DotNetUtils.Linq;
using Xunit.Abstractions;

namespace DotNetUtils.TestNet8._0;

public class LinqTest {

    private readonly ITestOutputHelper _output;

    public LinqTest(ITestOutputHelper output) {
        _output = output;
    }

    [Fact]
    public void Test1() {
        List<string> lst =
        [
            "ABC",
            "Hallo hallo",
            "moin"
        ];

        var result = lst.Randomize();

        foreach (var item in result) {
            _output.WriteLine(item);
        }
    }
}