namespace NETUtils_Test {
    [TestClass]
    public class NetworkTest {


        [TestMethod]
        public void TestMethod1() {

            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

        }

        [TestMethod]
        public void TestMethod2() {

            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            string ip1;
            List<string> test = NetworkUtils.GetAllIpAddresses();

            Trace.WriteLine(test);
            Debug.WriteLine(test);
        }
    }
}