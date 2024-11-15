using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslan.DotNetUtils.Collections;



namespace DotNetUtils.TestNet8._0 {



    public class Testlol {


        public void Test1() {

            var obs = new ObservableNotifyCollection<int>();

            var rO = new ReadOnlyObservableNotifyCollection<int>(obs);

        }
    }
}
