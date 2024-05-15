using System;
using System.Collections.Generic;
using System.Text;

namespace Roslan.DotNETUtils.Collections {
    public class LambdaEqualityComparer<TSource, TComparable> : IEqualityComparer<TSource> {
        Func<TSource, TSource, bool> _compareFunc;

        public LambdaEqualityComparer(Func<TSource, TSource, bool> compareFunc) {
            _compareFunc = compareFunc;
        }

        public bool Equals(TSource x, TSource y) {
            if (x == null || y == null) return (x == null && y == null);

            return _compareFunc(x,y);
        }

        public int GetHashCode(TSource obj) {
            if (obj == null) return int.MinValue;
            var k = obj.GetHashCode();
            if (k == null) return int.MaxValue;
            return k.GetHashCode();
        }
    }
}
