using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslan.DotNetUtils.Text;



namespace DotNetUtils.TestNet8._0 {
    public class StringUtilsTest {



        [Fact]
        public void Test1() {
            var list = new List<string>() { "--test=Value", "Yolo" };

            string result = StringUtils.GetCommandLineOption(list, "test");


        }
    }
}
