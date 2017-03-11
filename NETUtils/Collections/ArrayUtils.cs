using System;

namespace De.JanRoslan.NETUtils.Collections
{
    public static class ArrayUtils
    {
       

        public static T[] Concat<T>(params T[][] arrays)
        {

            int length = 0;

            foreach (T[] arr in arrays)
            {
                length += arr.Length;
            }

            T[] result = new T[length];

            return result;
        }

        public static T[] SubArray<T>(T[] array, int start, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, start, result, 0, length);
            return result;
        }

    }
}
