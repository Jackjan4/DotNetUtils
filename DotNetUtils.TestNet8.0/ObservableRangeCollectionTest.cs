using System.Collections.Specialized;
using Roslan.DotNetUtils.Collections;
using Xunit.Abstractions;



namespace DotNetUtils.TestNet8._0 {



    public class ObservableRangeCollectionTest {

        private readonly ITestOutputHelper _output;

        public ObservableRangeCollectionTest(ITestOutputHelper output) {
            _output = output;
        }



        [Fact]
        public void TestBasic()
        {
            ObservableRangeCollection<int> collection = new ObservableRangeCollection<int>();

            collection.CollectionChanged += CollectionOnCollectionChanged;

            var range = Enumerable.Range(0, 10);

            collection.AddRange(range);

            collection.RemoveRange(Enumerable.Range(5,10));
        }

        private void CollectionOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _output.WriteLine("Called!");
        }
    }
}
